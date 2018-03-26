public static class MathUtils {
    public static T Clamp<T>(this T value, T min, T max) where T : System.IComparable<T>
    {
        T result = value;
        if (value.CompareTo(max) > 0)
            result = max;
        if (value.CompareTo(min) < 0)
            result = min;
        return result;
    }

    public static T Min<T>(this T value1, T value2) where T : System.IComparable<T>
    {
        if (value1.CompareTo(value2) < 0) return value1;
        return value2;
    }

    public static T Max<T>(this T value1, T value2) where T : System.IComparable<T>
    {
        if (value1.CompareTo(value2) >= 0) return value1;
        return value2;
    }
}
