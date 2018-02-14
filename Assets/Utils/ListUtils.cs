using System;
using System.Collections;
using System.Collections.Generic;


public static class ListUtils {
    private static Random rng = new Random();

    public static void Shuffle<T>(this IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    public static T GetRandomElement<T>(this IList<T> list){
        if (list.Count == 0) return default(T);
        return list[rng.Next(0, list.Count)];
    }
}
