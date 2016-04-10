using System;
using System.Collections.Generic;
using System.Linq;

namespace SharpDev
{
    public static class SharpExtensions
    {
        #region [ WithValue ]

        public static void WithValue<T>(this T value, 
            Action<T> onNotEmpty, 
            Action<T> onEmpty = null, 
            Func<T, bool> isEmpty = null, 
            Action<Exception> onException = null)
        {
            try
            {
                bool empty = isEmpty?.Invoke(value) ?? value.IsEmpty();
                if (!empty)
                    onNotEmpty?.Invoke(value);
                else
                    onEmpty?.Invoke(value);
            }
            catch (Exception ex)
            {
                if (onException != null)
                {
                    onException(ex);
                }
                else
                    throw;
            }
        }

        public static TResult WithValue<T, TResult>(this T value, 
            Func<T, TResult> onNotNull, 
            Func<T, TResult> onNull = null,
            Func<T, bool> isEmpty = null,
            Func<Exception, TResult> onException = null)
        {
            try
            {
                bool empty = isEmpty?.Invoke(value) ?? value.IsEmpty();
                if (!empty)
                    return onNotNull != null ? onNotNull(value) : default(TResult);
                return onNull != null ? onNull(value) : default(TResult);
            }
            catch (Exception ex)
            {
                if (onException != null)
                {
                    return onException(ex);
                }
                throw;
            }
        }
        #endregion [ WithValue ]

        #region [ IsEmpty/IsNotEmpty ]
        public static bool IsEmpty(this object value)
        {
            if (ReferenceEquals(value, null)) return true;
            var str = value as string;
            if (str != null)
                return string.IsNullOrEmpty(str);
            if (value is DBNull)
                return true;
            return false;
        }

        public static bool IsNotEmpty(this object value)
        {
            return !value.IsEmpty();
        }
        #endregion [ IsEmpty/IsNotEmpty ]

        #region [ IsOneOf/IsNotOneOf ]
        public static bool IsOneOf<T>(this T value, IEqualityComparer<T> comparer, params T[] values)
        {
            if (comparer == null) comparer = EqualityComparer<T>.Default;
            if (values == null) return false;
            return values.Any(v => comparer.Equals(v, value));
        }
        public static bool IsOneOf<T>(this T value, params T[] values)
        {
            return IsOneOf(value, EqualityComparer<T>.Default, values);
        }
        public static bool IsOneOf<T>(this T value, IEqualityComparer<T> comparer, T value1)
        {
            if (comparer == null) comparer = EqualityComparer<T>.Default;
            return comparer.Equals(value1, value);
        }
        public static bool IsOneOf<T>(this T value, T value1)
        {
            return IsOneOf(value, EqualityComparer<T>.Default, value1);
        }
        public static bool IsOneOf<T>(this T value, IEqualityComparer<T> comparer, T value1, T value2)
        {
            if (comparer == null) comparer = EqualityComparer<T>.Default;
            return comparer.Equals(value1, value) || comparer.Equals(value2, value);
        }
        public static bool IsOneOf<T>(this T value, T value1, T value2)
        {
            return IsOneOf(value, EqualityComparer<T>.Default, value1, value2);
        }
        public static bool IsOneOf<T>(this T value, IEqualityComparer<T> comparer, T value1, T value2, T value3)
        {
            if (comparer == null) comparer = EqualityComparer<T>.Default;
            return comparer.Equals(value1, value) || comparer.Equals(value2, value) || comparer.Equals(value3, value);
        }
        public static bool IsOneOf<T>(this T value, T value1, T value2, T value3)
        {
            return IsOneOf(value, EqualityComparer<T>.Default, value1, value2, value3);
        }

        public static bool IsNotOneOf<T>(this T value, IEqualityComparer<T> comparer, params T[] values)
        {
            return !IsOneOf(value, comparer, values);
        }
        public static bool IsNotOneOf<T>(this T value, params T[] values)
        {
            return !IsOneOf(value, values);
        }
        public static bool IsNotOneOf<T>(this T value, IEqualityComparer<T> comparer, T value1)
        {
            return !IsOneOf(value, comparer, value1);
        }
        public static bool IsNotOneOf<T>(this T value, T value1)
        {
            return !IsOneOf(value, value1);
        }
        public static bool IsNotOneOf<T>(this T value, IEqualityComparer<T> comparer, T value1, T value2)
        {
            return !IsOneOf(value, comparer, value1, value2);
        }
        public static bool IsNotOneOf<T>(this T value, T value1, T value2)
        {
            return !IsOneOf(value, value1, value2);
        }
        public static bool IsNotOneOf<T>(this T value, IEqualityComparer<T> comparer, T value1, T value2, T value3)
        {
            return !IsOneOf(value, comparer, value1, value2, value3);
        }
        public static bool IsNotOneOf<T>(this T value, T value1, T value2, T value3)
        {
            return !IsOneOf(value, value1, value2, value3);
        }
        #endregion [ IsOneOf ]
    }
}
