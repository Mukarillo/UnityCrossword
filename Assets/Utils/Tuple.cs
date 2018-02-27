using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tuple<T1, T2>{
    public T1 value1;
    public T2 value2;

    public Tuple(T1 value1 = default(T1), T2 value2 = default(T2))
    {
        this.value1 = value1;
        this.value2 = value2;
    }

    public override string ToString()
    {
        return string.Format("[Tuple] value1: {0}, value2: {1}", value1, value2);
    }
}
