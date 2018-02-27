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
}
