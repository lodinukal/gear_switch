using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Util
{
    public static void RemoveBySwap<T>(this List<T> list, int index)
    {
        list[index] = list[list.Count - 1];
        list.RemoveAt(list.Count - 1);
    }
}