using StructureMap;
using StructureMap.Graph;
using StructureMap.Graph.Scanning;
using StructureMap.Pipeline;
using StructureMap.TypeRules;
using System;
using System.Web.Mvc;

namespace LeadingEdge.Curator.Web.Infrastructure.DependencyInjection
{
    internal class AspNetMvcConvention : IRegistrationConvention
    {
        public void Process(Type type, Registry registry)
        {
            if (!type.IsAbstract && (typeof(IView).IsAssignableFrom(type) || type.CanBeCastTo<Controller>()))
            {
                registry.For(type).LifecycleIs(new UniquePerRequestLifecycle());
            }
        }

        public void ScanTypes(TypeSet types, Registry registry)
        {
            foreach (var type in types.AllTypes())
            {
                Process(type, registry);
            }
        }
    }
}