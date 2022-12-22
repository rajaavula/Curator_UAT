using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace LeadingEdge.Curator.Core
{
    public class ReferenceTypeProperty : Enumeration<ReferenceTypeProperty, string>
    {
        private static readonly Lazy<IEnumerable<ReferenceTypeProperty>> Enumerations = new Lazy<IEnumerable<ReferenceTypeProperty>>(GetProperties);

        /// <summary>
        /// Initializes a new instance of the <see cref="ReferenceTypeProperty"/> class.
        /// </summary>
        protected ReferenceTypeProperty(string value, string displayName, int placeholderId, ReferenceType referenceType) : base(value, displayName)
        {
            PlaceholderId = placeholderId;
            ReferenceType = referenceType;
        }

        public int PlaceholderId { get; }

        public ReferenceType ReferenceType { get; }

        public static int FindPlaceholderId(string referenceType, string propertyCode)
        {
            var property = Enumerations.Value.FirstOrDefault(x => x.ReferenceType.Value == referenceType && x.Value == propertyCode);

            return property?.PlaceholderId ?? 0;
        }

        private static IEnumerable<ReferenceTypeProperty> GetProperties()
        {
            var type = typeof(ReferenceTypeProperty);

            var fields = new List<FieldInfo>();

            foreach (var reference in type.GetNestedTypes())
            {
                fields.AddRange(reference.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly).ToList());
            }

            return fields.Where(x => type.IsAssignableFrom(x.FieldType)).Select(x => x.GetValue(null)).Cast<ReferenceTypeProperty>();
        }
    }
}
