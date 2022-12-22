using StructureMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LeadingEdge.Curator.Web.Infrastructure.DependencyInjection
{
    internal sealed class StructureMapDependencyResolver : IDependencyResolver
    {
        private const string NestedContainerKey = "Nested.Container.Key";

        private readonly IContainer _container;

        public StructureMapDependencyResolver(IContainer container)
        {
            if (container == null)
            {
                throw new ArgumentNullException("container");
            }

            _container = container;
        }

        public IContainer CurrentNestedContainer
        {
            get
            {
                return (IContainer)HttpContext.Items[NestedContainerKey];
            }

            set
            {
                HttpContext.Items[NestedContainerKey] = value;
            }
        }

        private HttpContextBase HttpContext
        {
            get
            {
                var ctx = _container.TryGetInstance<HttpContextBase>();
                return ctx ?? new HttpContextWrapper(System.Web.HttpContext.Current);
            }
        }

        public void CreateNestedContainer()
        {
            if (CurrentNestedContainer != null)
            {
                return;
            }

            CurrentNestedContainer = _container.GetNestedContainer();
        }

        public void DisposeNestedContainer()
        {
            if (CurrentNestedContainer != null)
            {
                CurrentNestedContainer.Dispose();
                CurrentNestedContainer = null;
            }
        }

        public void Dispose()
        {
            DisposeNestedContainer();
            _container.Dispose();
        }

        public object GetService(Type serviceType)
        {
            IContainer container = CurrentNestedContainer ?? _container;

            if (serviceType.IsAbstract || serviceType.IsInterface)
            {
                return container.TryGetInstance(serviceType);
            }

            return container.GetInstance(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return (CurrentNestedContainer ?? _container).GetAllInstances(serviceType).Cast<object>();
        }
    }
}