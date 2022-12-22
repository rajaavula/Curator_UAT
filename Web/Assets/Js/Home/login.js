$(function () {

    $('#terms-conditions').on('click', function (e) {
        e.preventDefault();
        ppTC.Show();
    });

    $('#forgotten-password').on('click', function (e) {
        e.preventDefault();
        txtEmail.SetText('');
        txtEmail.SetIsValid(true);
        ppForgottenPassword.Show();
    });
});

function Login(s, e) {
    if (!ASPxClientEdit.ValidateGroup("MAIN")) {
        e.processOnServer = false;
    }
}

function SendPassword() {
    if (txtEmail.GetIsValid() == false) return;

    $.ajax({
        cache: false,
        url: '/Home/Home/Password',
        data: { Email: txtEmail.GetText() },
        success: function (json) {
            if (json.Error && json.Error.length > 0) {

            }

            txtEmail.SetText('');
            ppForgottenPassword.Hide();
        }
    });
}