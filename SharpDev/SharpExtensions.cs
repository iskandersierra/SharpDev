using System;

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
    }
}
