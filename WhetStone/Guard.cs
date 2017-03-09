using System;
using WhetStone.SystemExtensions;

namespace WhetStone.Guard
{
    /// <summary>
    /// A generic, mutable wrapper, made for immutable objects.
    /// </summary>
    /// <typeparam name="T">The type to wrap.</typeparam>
    public interface IGuard<T> : ICloneable
    {
        /// <summary>
        /// The internal value of the <see cref="IGuard{T}"/>
        /// </summary>
        T value { get; set; }
    }
    /// <summary>
    /// Static container for <see cref="IGuard{T}"/> extension methods
    /// </summary>
    public static class Guard
    {
        /// <summary>
        /// Sets an <see cref="IGuard{T}"/>'s value only if it is not <see langword="null"/>.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="IGuard{T}"/>.</typeparam>
        /// <param name="this">The <see cref="IGuard{T}"/> whose value to set.</param>
        /// <param name="val">The value to set the <paramref name="this"/> to, if it is not <see langword="null"/>.</param>
        /// <returns>Whether the value was set or not.</returns>
        public static bool CondSet<T>(this IGuard<T> @this, T val)
        {
            if (@this == null)
                return false;
            @this.value = val;
            return true;
        }
        /// <summary>
        /// Gets an <see cref="IGuard{T}"/>'s value only if it is not <see langword="null"/>.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="IGuard{T}"/>.</typeparam>
        /// <param name="this">The <see cref="IGuard{T}"/> whose value to set.</param>
        /// <param name="defval">The value to return if <paramref name="this"/> is <see langword="null"/>.</param>
        /// <returns>The value of <paramref name="this"/> or <paramref name="defval"/> if <paramref name="this"/> is <see langword="null"/>.</returns>
        public static T CondGet<T>(this IGuard<T> @this, T defval = default(T))
        {
            return @this != null ? @this.value : defval;
        }
        /// <summary>
        /// Mutates and <see cref="IGuard{T}"/>'s value if it is not <see langword="null"/>.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="IGuard{T}"/>.</typeparam>
        /// <param name="this">The <see cref="IGuard{T}"/> whose value to mutate.</param>
        /// <param name="val">The mutator function for <paramref name="this"/>'s value, if it exists.</param>
        /// <returns>Whether the value was mutated.</returns>
        public static bool CondMutate<T>(this IGuard<T> @this, Func<T,T> val)
        {
            if (@this == null)
                return false;
            val.ThrowIfNull(nameof(val));
            @this.value = val(@this.value);
            return true;
        }
    }
    /// <summary>
    /// A simple <see cref="IGuard{T}"/>, storing only the internal value.
    /// </summary>
    /// <typeparam name="T">The type of the inner element.</typeparam>
    public class Guard<T> : IGuard<T>
    {
        /// <inheritdoc />
        public T value { get; set; }
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="load">The initial value.</param>
        public Guard(T load = default(T))
        {
            this.value = load;
        }
        /// <inheritdoc />
        public virtual object Clone()
        {
            var ret = new Guard<T>(value);
            return ret;
        }
        /// <inheritdoc />
        public override int GetHashCode()
        {
            return this.value.GetHashCode();
        }
        /// <summary>
        /// Converts a <see cref="Guard{T}"/> to a <typeparamref name="T"/> type.
        /// </summary>
        /// <param name="this">The <see cref="Guard{T}"/> to convert.</param>
        public static implicit operator T(Guard<T> @this)
        {
            return @this.value;
        }
        /// <inheritdoc />
        public override string ToString()
        {
            return value.ToString();
        }
    }
    /// <summary>
    /// An <see cref="IGuard{T}"/> that supports triggering event upon access.
    /// </summary>
    /// <typeparam name="T">The type of the inner element.</typeparam>
    /// <remarks>For any event to be fired upon access, the value must be retrieved through the <see cref="EventValue"/> property.</remarks>
    public class EventGuard<T> : Guard<T>
    {
        /// <summary>
        /// Arguments for when the value is set.
        /// </summary>
        public class EventGuardSetArgs : EventArgs
        {
            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="oldVal">The old value of the <see cref="IGuard{T}"/></param>
            /// <param name="newVal">The new value of the <see cref="IGuard{T}"/></param>
            public EventGuardSetArgs(T oldVal, T newVal)
            {
                this.oldVal = oldVal;
                this.newVal = newVal;
            }
            /// <summary>
            /// the old value of the <see cref="IGuard{T}"/>.
            /// </summary>
            public T oldVal { get; }
            /// <summary>
            /// the new value of the <see cref="IGuard{T}"/>.
            /// </summary>
            public T newVal { get; }
        }
        /// <summary>
        /// Arguments for when the value is accessed (either set ot get).
        /// </summary>
        public class EventGuardAccessArgs : EventArgs
        {
            /// <summary>
            /// The types of access to access an <see cref="EventGuard{T}"/>'s value.
            /// </summary>
            public enum AccessType
            {
                /// <summary>
                /// When the value was accessed as a <see langword="get"/>.
                /// </summary>
                Get,
                /// <summary>
                /// When the value was accessed as a <see langword="set"/>.
                /// </summary>
                Set
            }
            /// <summary>
            /// constructor
            /// </summary>
            /// <param name="accesType">The access type of the Access.</param>
            public EventGuardAccessArgs(AccessType accesType)
            {
                AccesType = accesType;
            }
            /// <summary>
            /// The access type of the Access.
            /// </summary>
            public AccessType AccesType { get; }
        }
        /// <summary>
        /// Handler for when the <see cref="IGuard{T}"/>'s value is set.
        /// </summary>
        /// <param name="sender">The <see cref="IGuard{T}"/> sender.</param>
        /// <param name="e">The event arguments.</param>
        public delegate void GuardSetHandler(object sender, EventGuardSetArgs e);
        /// <summary>
        /// Handler for when the <see cref="IGuard{T}"/>'s value is set or gotten.
        /// </summary>
        /// <param name="sender">The <see cref="IGuard{T}"/> sender.</param>
        /// <param name="e">The event arguments.</param>
        public delegate void GuardAccessedHandler(object sender, EventGuardAccessArgs e);
        /// <summary>
        /// Handler for when the <see cref="IGuard{T}"/>'s value is gotten.
        /// </summary>
        /// <param name="sender">The <see cref="IGuard{T}"/> sender.</param>
        /// <param name="e">The event arguments.</param>
        public delegate void GuardGetHandler(object sender, EventArgs e);
        /// <summary>
        /// Accesses the value, and triggers the relevant events
        /// </summary>
	    public T EventValue
        {
            get
            {
	            this.accessed?.Invoke(this, new EventGuardAccessArgs(EventGuardAccessArgs.AccessType.Get));
	            this.drawn?.Invoke(this, EventArgs.Empty);
	            return this.value;
            }
            set
            {
                T temp = this.value;
                this.value = value;
	            this.accessed?.Invoke(this, new EventGuardAccessArgs(EventGuardAccessArgs.AccessType.Set));
	            this.changed?.Invoke(this,new EventGuardSetArgs(temp, value));
            }
        }
	    /// <summary>
        /// is called when the direct value is changed, first parameter([0]) is the new value, second parameter ([1]) is the old value, third is whether the value is equal to the old value
        /// </summary>
        public event GuardSetHandler changed;
        /// <summary>
        /// is called whenever the value is accessed, first parameter dictates whether the value was accessed from get or set ("get","set")
        /// </summary>
        public event GuardAccessedHandler accessed;
        /// <summary>
        /// is called whenever the value is looked at, has no parameters
        /// </summary>
        public event GuardGetHandler drawn;
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="load">The initial value of the <see cref="EventGuard{T}"/>.</param>
        public EventGuard(T load = default(T))
        {
            value = load;
        }
        /// <inheritdoc />
        public override object Clone()
        {
            var ret = new EventGuard<T>(value)
            {
                accessed = this.accessed.Copy(),
                changed = this.changed.Copy(),
                drawn = this.drawn.Copy()
            };
            return ret;
        }
        /// <inheritdoc />
        public override int GetHashCode()
        {
            return this.value.GetHashCode() ^ this.changed.GetInvocationList().GetHashCode() ^
                this.accessed.GetInvocationList().GetHashCode() ^ this.drawn.GetInvocationList().GetHashCode();
        }
        /// <inheritdoc />
        public override string ToString()
        {
            return value.ToString();
        }
    }
}
