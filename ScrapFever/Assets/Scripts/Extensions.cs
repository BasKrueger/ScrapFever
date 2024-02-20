using SurvivorDTO;
using System.Collections.Generic;
using UnityEngine;

public static class Extensions
{
    public static T GetRandom<T>(this List<T> list)
    {
        if(list.Count == 0) return default(T);
        return list[UnityEngine.Random.Range(0, list.Count)];
    }

    public static T GetRandomExcept<T>(this List<T> list, List<T> except)
    {
        if (list.Count == 0) return default(T);
        if (list.Count == 1) return list[0];

        var random = list[UnityEngine.Random.Range(0, list.Count)];
        while (except.Contains(random))
        {
            random = list[UnityEngine.Random.Range(0, list.Count)];
        }
        return random;
    }

    public static Vector2 ToVector2(this Vector3 vec) => new Vector2(vec.x, vec.z);
    public static Vector3 ToVector3(this Vector2 vec) => new Vector3(vec.x, 0, vec.y);
    public static bool TryReplaceFirst<T>(this object[] array, object with)
    {
        for (int i = 0; i < array.Length; i++)
        {
            if (array[i] is T)
            {
                array[i] = with;
                return true;
            }
        }

        return false;
    }
    public static bool TryReplaceFirst(this object[] array, object target, object with)
    {
        for (int i = 0; i < array.Length; i++)
        {
            if (array[i] == target)
            {
                array[i] = with;
                return true;
            }
        }

        return false;
    }
    public static bool ContainsAny<T>(this object[] array)
    {
        for (int i = 0; i < array.Length; i++)
        {
            if (array[i] is T)
            {
                return true;
            }
        }

        return false;
    }

    public static int IndexOf(this object[] array, object target)
    {
        for (int i = 0; i < array.Length; i++)
        {
            if (array[i] == target)
            {
                return i;
            }
        }

        throw new KeyNotFoundException();
    }

    public static int Count<T>(this object[] array)
    {
        var counter = 0;

        for (int i = 0; i < array.Length; i++)
        {
            if (array[i] is T)
            {
                counter++;
            }
        }

        return counter;
    }

    public static T GetFirst<T>(this object[] array)
    {
        for (int i = 0; i < array.Length; i++)
        {
            if (array[i] is T)
            {
                return (T)array[i];
            }
        }

        return default(T);
    }

    public static bool IsOnScreen(this GameObject go) => IsOnScreen(go, 0);

    public static bool IsOnScreen(this GameObject go, float tolerance)
    {
        var screenPos = Camera.main.WorldToScreenPoint(go.transform.position);
        var onScreen = screenPos.x > (tolerance * -1) && screenPos.x < (Screen.width + tolerance) && screenPos.y > (tolerance * -1) && screenPos.y < (Screen.height + tolerance);

        return onScreen;
    }

    public static Dictionary<TValue, TKey> Reverse<TKey, TValue>(this IDictionary<TKey, TValue> source)
    {
        var dictionary = new Dictionary<TValue, TKey>();
        foreach (var entry in source)
        {
            if (!dictionary.ContainsKey(entry.Value))
                dictionary.Add(entry.Value, entry.Key);
        }
        return dictionary;
    }

    public static bool SaveObjectBinary(this object obj, string name, string fileName)
    {
        return SaveFileUtils.SaveObjectBinary(obj, name, fileName);
    }

    public static bool SaveListBinary<T>(this List<T> list, string name, string fileName)
    {
        return SaveFileUtils.SaveObjectListBinary(list, name, fileName);
    }

    public static bool LoadObjectBinary<T>(this T data, string fileName)
    {
        return SaveFileUtils.LoadObjectBinary<T>(ref data, fileName);
    }

    public static bool LoadListBinary<T>(this List<T> list, T sample, string fileName) where T : new()
    {
        return SaveFileUtils.LoadObjectListBinary<T>(sample, ref list, fileName);
    }

    public static bool LoadGameFileBinary(this GameSaveFile gsf)
    {
        return SaveFileUtils.LoadGameFileBinary(ref gsf);
    }

    public static string ToBasicString(this Resolution res)
    {
        return Mathf.RoundToInt(res.width) + "X" + Mathf.RoundToInt(res.height);
    }

    public static Resolution TryParseToResolution(this string stri)
    {
        foreach (Resolution res in Screen.resolutions)
        {
            var str = Mathf.RoundToInt(res.width) + "X" + Mathf.RoundToInt(res.height);
            if (str == stri)
            {
                return res;
            }
        }

        return default(Resolution);
    }
}