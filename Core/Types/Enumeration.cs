using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace LeadingEdge.Curator.Core
{
    /// <summary>
    /// <para>
    /// Defines a thin language wrapper around an integral type like an enum, but with the ability to encapsulate
    /// business logic within the class without introducing many control flow statements in other classes to
    /// check the values of the enum.
    /// </para>
    /// <para>
    /// More information here:
    /// <see href="https://docs.microsoft.com/en-us/dotnet/standard/microservices-architecture/microservice-ddd-cqrs-patterns/enumeration-classes-over-enum-types"/>.
    /// </para>
    /// </summary>
    /// <typeparam name="TEnumeration">the type of the enumeration.</typeparam>
    /// <typeparam name="TValue">the type of the enumeration's value property.</typeparam>
    public abstract class Enumeration<TEnumeration, TValue> : IComparable<TEnumeration>, IEquatable<TEnumeration>
        where TEnumeration : Enumeration<TEnumeration, TValue>
        where TValue : IComparable
    {
        private static readonly Lazy<IEnumerable<TEnumeration>> Enumerations = new Lazy<IEnumerable<TEnumeration>>(GetEnumerations);

        /// <summary>
        /// Initializes a new instance of the <see cref="Enumeration{TEnumeration, TValue}"/> class.
        /// </summary>
        /// <param name="value">the value.</param>
        /// <param name="displayName">the display name.</param>
        protected Enumeration(TValue value, string displayName)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            Value = value;
            DisplayName = displayName;
        }

        public TValue Value { get; }

        public string DisplayName { get; }

        /// <summary>
        /// Overrides the equals operator to determine if two enumerations are equal to each other.
        /// </summary>
        /// <param name="left">the left side of the comparison.</param>
        /// <param name="right">the right side of the comparison.</param>
        /// <returns>true if the enumerations are equal, and false otherwise.</returns>
        public static bool operator ==(Enumeration<TEnumeration, TValue> left, Enumeration<TEnumeration, TValue> right)
        {
            if (left is null ^ right is null)
            {
                return false;
            }

            return left?.Equals(right) != false;
        }

        /// <summary>
        /// Overrides the not equals operator to determines if two enumerations are not equal to each other.
        /// </summary>
        /// <param name="left">the left side of the comparison.</param>
        /// <param name="right">the right side of the comparison.</param>
        /// <returns>true if the enumerations are not equal, and false otherwise.</returns>
        public static bool operator !=(Enumeration<TEnumeration, TValue> left, Enumeration<TEnumeration, TValue> right)
        {
            return !(left == right);
        }

        /// <summary>
        /// Gets all the enumerations declared in the class.
        /// </summary>
        /// <returns>the enumerations.</returns>
        public static IEnumerable<TEnumeration> GetAll()
        {
            return Enumerations.Value;
        }

        /// <summary>
        /// Indicates whether an enumeration with a specified value has been defined.
        /// </summary>
        /// <param name="value">the value of the enumeration.</param>
        /// <returns>true if the enumeration exists, and false otherwise.</returns>
        public static bool IsDefined(TValue value)
        {
            return GetAll().Any(e => e.ValueEquals(value));
        }

        /// <summary>
        /// Attempts to find an enumeration from its value.
        /// </summary>
        /// <param name="value">the value.</param>
        /// <param name="result">the resulting enumeration.</param>
        /// <returns>true if an enumeration was found successfully, and false otherwise.</returns>
        public static bool TryParse(TValue value, out TEnumeration result)
        {
            return TryParse(e => e.ValueEquals(value), out result);
        }

        /// <summary>
        /// Finds an enumeration from its value.
        /// </summary>
        /// <param name="value">the value.</param>
        /// <returns>the enumeration.</returns>
        public static TEnumeration FromValue(TValue value)
        {
            return Parse(value, "value", item => item.Value.Equals(value));
        }

        /// <summary>
        /// Finds an enumeration from its display name.
        /// </summary>
        /// <param name="displayName">the display name.</param>
        /// <returns>the enumeration.</returns>
        public static TEnumeration Parse(string displayName)
        {
            return Parse(displayName, "display name", item => item.DisplayName == displayName);
        }

        /// <summary>
        /// Compares the current enumeration with another enumeration and returns an integer that indicates
        /// whether the current enumeration precedes, follows, or occurs in the same position as the other
        /// enumeration.
        /// </summary>
        /// <param name="other">the other numeration.</param>
        /// <returns>
        /// Less than zero if this enumeration precedes the other, zero if they have the same sort order,
        /// and greater than zero if this enumeration follows the other.
        /// </returns>
        public int CompareTo(TEnumeration other)
        {
            return Value.CompareTo(other == default(TEnumeration) ? default : other.Value);
        }

        /// <summary>
        /// Gets a string representation of the enumeration.
        /// </summary>
        /// <returns>the string representation.</returns>
        public override sealed string ToString()
        {
            return DisplayName;
        }

        /// <summary>
        /// Determines whether the current enumeration is equal to another object.
        /// </summary>
        /// <param name="obj">the object being compared to this enumeration.</param>
        /// <returns>true if they are equal, and false otherwise.</returns>
        public override bool Equals(object obj)
        {
            return Equals(obj as TEnumeration);
        }

        /// <summary>
        /// Determines whether the current enumeration is equal to another enumeration.
        /// </summary>
        /// <param name="other">the enumeration being compared to this enumeration.</param>
        /// <returns>true if they are equal, and false otherwise.</returns>
        public bool Equals(TEnumeration other)
        {
            if (other == null)
            {
                return false;
            }

            // Check the objects are the same
            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return ValueEquals(other.Value);
        }

        /// <summary>
        /// Calculates a hash for the enumeration based on its value.
        /// </summary>
        /// <returns>the object's hash representation.</returns>
        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        /// <summary>
        /// Gets a collection of the enumerations declared in the class.
        /// </summary>
        /// <returns>a collection of enumerations.</returns>
        private static IEnumerable<TEnumeration> GetEnumerations()
        {
            var type = typeof(TEnumeration);
            var fields = type.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly);

            return fields.Where(x => type.IsAssignableFrom(x.FieldType)).Select(x => x.GetValue(null)).Cast<TEnumeration>();
        }

        /// <summary>
        /// Attempts to find an enumeration using the provided predicate.
        /// </summary>
        /// <param name="predicate">the predicate.</param>
        /// <param name="result">the resulting enumeration.</param>
        /// <returns>true if an enumeration was found successfully, and false otherwise.</returns>
        private static bool TryParse(Func<TEnumeration, bool> predicate, out TEnumeration result)
        {
            result = GetAll().FirstOrDefault(predicate);
            return result != null;
        }

        /// <summary>
        /// Finds an enumeration using the provided predicate.
        /// </summary>
        /// <param name="value">the value being parsed by the predicate.</param>
        /// <param name="description">the description of the field being compared by the predicate.</param>
        /// <param name="predicate">the predicate to find an enumeration.</param>
        /// <returns>the enumeration.</returns>
        private static TEnumeration Parse(object value, string description, Func<TEnumeration, bool> predicate)
        {
            TEnumeration result;

            if (!TryParse(predicate, out result))
            {
                string message = string.Format("'{0}' is not a valid {1} in {2}", value, description, typeof(TEnumeration));
                throw new ArgumentException(message, nameof(value));
            }

            return result;
        }

        /// <summary>
        /// Checks that the value of this enumeration is equal to another value.
        /// </summary>
        /// <param name="value">the value to compare against.</param>
        /// <returns>true if the values are equal, and false otherwise.</returns>
        private bool ValueEquals(TValue value)
        {
            return Value.Equals(value);
        }
    }
}
