namespace LeadingEdge.Curator.Core
{
    public sealed class ReferenceType : Enumeration<ReferenceType, string>
    {
        public static readonly ReferenceType Customer = new ReferenceType("CUSTOMER", "Customer");
               

        /// <summary>
        /// Initializes a new instance of the <see cref="ReferenceType"/> class.
        /// </summary>
        private ReferenceType(string value, string displayName) : base(value, displayName)
        {
        }
    }
}
