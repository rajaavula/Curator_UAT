using IdentityModel;
using LeadingEdge.Core.Claims;
using LeadingEdge.Core.Claims.Extensions;
using LeadingEdge.Curator.Core;
using LeadingEdge.Curator.Core.Configurations;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Owin;
using Microsoft.Owin.Extensions;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Notifications;
using Microsoft.Owin.Security.OpenIdConnect;
using Owin;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.SessionState;

namespace LeadingEdge.Curator.Web
{
    public sealed partial class Startup
    {
        public void ConfigureAuth(IAppBuilder app)
        {
            var settings = DependencyResolver.Current.GetService<OpenIdConnectSettings>();

            string scopes = null;

            if (settings.Scopes != null)
            {
                scopes = string.Join(" ", settings.Scopes);
            }

            App.BypassOpenIdConnect = settings.Bypass;

            // Setup session
            app.Use((context, next) =>
            {
                var httpContext = context.Get<HttpContextBase>(typeof(HttpContextBase).FullName);
                httpContext.SetSessionStateBehavior(SessionStateBehavior.Required);
                return next();
            });

            app.UseStageMarker(PipelineStage.MapHandler);

            // Setup authentication
            app.SetDefaultSignInAsAuthenticationType(CookieAuthenticationDefaults.AuthenticationType);

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                LoginPath = new PathString("/Login"),
                AuthenticationType = CookieAuthenticationDefaults.AuthenticationType,
                SlidingExpiration = true,
                ExpireTimeSpan = new TimeSpan(4, 0, 0),
                CookieSecure = CookieSecureOption.Always,
                AuthenticationMode = AuthenticationMode.Active
            });

            app.UseOpenIdConnectAuthentication(
                new OpenIdConnectAuthenticationOptions
                {
                    AuthenticationMode = App.BypassOpenIdConnect ? AuthenticationMode.Passive : AuthenticationMode.Active,

                    Authority = settings.Authority,
                    ClientId = settings.ClientId,
                    ClientSecret = settings.ClientSecret,

                    ResponseType = OpenIdConnectResponseType.Code,
                    ResponseMode = OpenIdConnectResponseMode.Query,
                    SaveTokens = true,
                    RedeemCode = true,

                    SignInAsAuthenticationType = CookieAuthenticationDefaults.AuthenticationType,

                    UseTokenLifetime = false,

                    Scope = $"openid profile email {scopes}",

                    Notifications = new OpenIdConnectAuthenticationNotifications
                    {
                        RedirectToIdentityProvider = notification =>
                        {
                            if (notification.ProtocolMessage.RequestType == OpenIdConnectRequestType.Authentication)
                            {
                                // generate code verifier and code challenge
                                var codeVerifier = CryptoRandom.CreateUniqueId(32);

                                string codeChallenge;
                                using (var sha256 = SHA256.Create())
                                {
                                    var challengeBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(codeVerifier));
                                    codeChallenge = Base64Url.Encode(challengeBytes);
                                }

                                // set code_challenge parameter on authorization request
                                notification.ProtocolMessage.SetParameter("code_challenge", codeChallenge);
                                notification.ProtocolMessage.SetParameter("code_challenge_method", "S256");

                                RememberCodeVerifier(notification, codeVerifier);

                                notification.ProtocolMessage.RedirectUri = $"{BuildBaseUri(notification.Request)}/signin-oidc";
                            }

                            if (notification.ProtocolMessage.RequestType == OpenIdConnectRequestType.Logout)
                            {
                                notification.ProtocolMessage.PostLogoutRedirectUri = BuildBaseUri(notification.Request);
                            }

                            return Task.CompletedTask;
                        },

                        AuthorizationCodeReceived = notification =>
                        {
                            var codeVerifier = RetrieveCodeVerifier(notification);

                            // attach code_verifier on token request
                            notification.TokenEndpointRequest.SetParameter("code_verifier", codeVerifier);

                            notification.TokenEndpointRequest.SetParameter("redirect_uri", $"{BuildBaseUri(notification.Request)}/signin-oidc");

                            return Task.CompletedTask;
                        },

                        AuthenticationFailed = context =>
                        {
                            context.HandleResponse();

                            if (context.Exception is OidcException ex)
                            {
                                context.Response.Redirect("/Callback/" + ex.Code);
                            }
                            else
                            {
                                Log.Error(context.Exception);
                                context.Response.Redirect("/Callback/000");
                            }

                            return Task.CompletedTask;
                        },

                        SecurityTokenValidated = context =>
                        {
                            var principal = new ClaimsPrincipal(context.AuthenticationTicket.Identity);

                            var id = principal.GetUserId();
                            if (id == null)
                            {
                                throw new OidcException("010");
                            }

                            var email = principal.GetEmailAddress();
                            if (string.IsNullOrEmpty(email))
                            {
                                throw new OidcException("010");
                            }

                            var user = UserManager.ValidateOidcUser(email);
                            if (user == null)
                            {
                                throw new OidcException("020");
                            }

                            if (user.OidcId == null)
                            {
                                user.OidcId = id;

                                var ex = UserManager.Save(user);
                                if (ex != null)
                                {
                                    throw new OidcException("030");
                                }
                            }

                            context.AuthenticationTicket.Identity.AddClaim(new Claim(JwtClaimNames.CortexId, user.UserID.ToString()));

                            context.AuthenticationTicket.Properties.IsPersistent = true;

                            if (!App.CreateSession(user, out string error))
                            {
                                throw new OidcException("040");
                            }

                            return Task.CompletedTask;
                        }
                    },

                    TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = true,
                        ValidIssuers = new List<string> { settings.Authority }
                    },
                });

            app.UseStageMarker(PipelineStage.PostAcquireState);
        }

        private string BuildBaseUri(IOwinRequest request)
        {
            return $"{request.Scheme}://{request.Host}{request.PathBase}";
        }

        private void RememberCodeVerifier(RedirectToIdentityProviderNotification<OpenIdConnectMessage, OpenIdConnectAuthenticationOptions> notification, string codeVerifier)
        {
            var properties = new AuthenticationProperties();
            properties.Dictionary.Add("cv", codeVerifier);
            notification.Options.CookieManager.AppendResponseCookie(
                notification.OwinContext,
                GetCodeVerifierKey(notification.ProtocolMessage.State),
                Convert.ToBase64String(Encoding.UTF8.GetBytes(notification.Options.StateDataFormat.Protect(properties))),
                new CookieOptions
                {
                    SameSite = Microsoft.Owin.SameSiteMode.None,
                    HttpOnly = true,
                    Secure = notification.Request.IsSecure,
                    Expires = DateTime.UtcNow + notification.Options.ProtocolValidator.NonceLifetime
                });
        }

        private string RetrieveCodeVerifier(AuthorizationCodeReceivedNotification notification)
        {
            string key = GetCodeVerifierKey(notification.ProtocolMessage.State);

            string codeVerifierCookie = notification.Options.CookieManager.GetRequestCookie(notification.OwinContext, key);
            if (codeVerifierCookie != null)
            {
                var cookieOptions = new CookieOptions
                {
                    SameSite = Microsoft.Owin.SameSiteMode.None,
                    HttpOnly = true,
                    Secure = notification.Request.IsSecure
                };

                notification.Options.CookieManager.DeleteCookie(notification.OwinContext, key, cookieOptions);
            }

            var cookieProperties = notification.Options.StateDataFormat.Unprotect(Encoding.UTF8.GetString(Convert.FromBase64String(codeVerifierCookie)));
            cookieProperties.Dictionary.TryGetValue("cv", out var codeVerifier);

            return codeVerifier;
        }

        private string GetCodeVerifierKey(string state)
        {
            using (var hash = SHA256.Create())
            {
                return OpenIdConnectAuthenticationDefaults.CookiePrefix + "cv." + Convert.ToBase64String(hash.ComputeHash(Encoding.UTF8.GetBytes(state)));
            }
        }
    }
}