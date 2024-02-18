using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class CustomExtensions
{
    private static System.Random rng = new System.Random();

    public static T GetOrAddComponent<T>(this GameObject g) where T : Component
    {
        var component = g.GetComponent<T>();
        if (component)
            return component;
        else
            return g.AddComponent<T>();
    }

    public static T GetRandomElement<T>(this T[] array)
    {
        var rdIndex = UnityEngine.Random.Range(0, array.Length);
        return array[rdIndex];
    }
    public static T GetRandomElement<T>(this List<T> lst)
    {
        var rdIndex = UnityEngine.Random.Range(0, lst.Count);
        return lst[rdIndex];
    }

    // See https://stackoverflow.com/a/48089
    public static List<T> GetRandomSample<T>(this List<T> list, int sampleSize)
    {
        var result = new List<T>();
        sampleSize = Math.Min(sampleSize, list.Count);

        int count = 0;
        for (int i = 0; i < list.Count; ++i)
        {
            var chance = (float)(sampleSize - count) / (list.Count - count);
            var decision = UnityEngine.Random.Range(0, 1f) < chance;
            if (decision == true)
            {
                result.Add(list[i]);
                count += 1;
            }

            if (count == sampleSize)
                break;
        }
        return result;
    }

    public static Transform DestroyAllChildren(this Transform t)
    {
        foreach (Transform child in t)
        {
            UnityEngine.Object.Destroy(child.gameObject);
        }
        return t;
    }

    public static Transform DestroyAllChildrenWithComponent<T>(this Transform t) where T : Component
    {
        var children = t.GetComponentsInChildren<T>();
        foreach (var child in children)
        {
            UnityEngine.Object.Destroy(child.gameObject);
        }
        return t;
    }
}
