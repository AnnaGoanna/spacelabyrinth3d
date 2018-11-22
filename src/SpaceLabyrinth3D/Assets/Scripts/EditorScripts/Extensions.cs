using System;
using System.ComponentModel;
using System.Reflection;
using UnityEngine;

public static class Extensions
{
    public static GameObject FindObject(this GameObject parent, string name)
    {
        Transform[] trs = parent.GetComponentsInChildren<Transform>(true);
        foreach (Transform t in trs)
        {
            if (t.name == name)
            {
                return t.gameObject;
            }
        }
        return null;
    }

    public static int IndexOf<T>(this T[] array, T value) where T : struct
    {
        return Array.IndexOf<T>(array, value);
    }

    public static T ElementAfter<T>(this T[] array, int index) where T : struct
    {
        return array[(index + 1) % array.Length];
    }

    public static T ElementAfter<T>(this T[] array, T value) where T : struct
    {
        int index = array.IndexOf(value);
        return array.ElementAfter(index);
    }

    public static T ElementBefore<T>(this T[] array, int index) where T : struct
    {
        return (index == 0) ? array[array.Length - 1] : array[index - 1];
    }

    public static T ElementBefore<T>(this T[] array, T value) where T : struct
    {
        int index = array.IndexOf(value);
        return array.ElementBefore(index);
    }


    public static T Next<T>(this T src) where T : struct
    {
        if (!typeof(T).IsEnum) throw new ArgumentException(String.Format("Argument {0} is not an Enum", typeof(T).FullName));

        T[] Arr = (T[])Enum.GetValues(src.GetType());
        int j = Array.IndexOf<T>(Arr, src) + 1;
        return (Arr.Length == j) ? Arr[0] : Arr[j];
    }

    public static T Prev<T>(this T src) where T : struct
    {
        if (!typeof(T).IsEnum) throw new ArgumentException(String.Format("Argument {0} is not an Enum", typeof(T).FullName));

        T[] Arr = (T[])Enum.GetValues(src.GetType());
        int j = Array.IndexOf<T>(Arr, src) - 1;
        return (0 > j) ? Arr[Arr.Length - 1] : Arr[j];
    }

    public static string GetDescription<T>(this T src) where T : struct
    {
        MemberInfo[] memInfo = typeof(T).GetMember(src.ToString());
        object[] attributes = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
        string description = ((DescriptionAttribute)attributes[0]).Description;
        return description;
    }
}