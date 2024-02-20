using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public interface IPoolable
{
    void OnReturnedToPool();
    void OnTakenFromPool();
}

public class Pool : MonoBehaviour
{
    private static Pool instance;

    public static event Action<GameObject> returned;

    [SerializeField, TabGroup("Pool tabs","Enemies", SdfIconType.EmojiAngry, TextColor = "red")]
    private List<PoolContent> enemies;
    [SerializeField, TabGroup("Pool tabs","Projectiles", SdfIconType.Bullseye, TextColor = "orange")]
    private List<PoolContent> projectiles;
    [SerializeField, TabGroup("Pool tabs", "Sounds", SdfIconType.MusicNote, TextColor = "blue")]
    private new List<PoolContent> audio;
    [SerializeField, TabGroup("Pool tabs", "Other", SdfIconType.Question)]
    private List<PoolContent> other;

    private List<PoolContent> contents;

    Dictionary<string, List<PoolContent>> knownPools = new Dictionary<string, List<PoolContent>>();
    Dictionary<GameObject, List<PoolContent>> knownPoolTemplate = new Dictionary<GameObject, List<PoolContent>>();

    private void Awake()
    {
        if(instance != null)
        {
            Destroy(this.gameObject);
            return;
        }

        instance = this;

        instance.contents = new List<PoolContent>();
        instance.contents.AddRange(enemies);
        instance.contents.AddRange(projectiles);
        instance.contents.AddRange(audio);
        instance.contents.AddRange(other);

        foreach (var content in contents)
        {
            content.SetUp(transform);
        }
    }

    public static T Get<T>(string name = "")
    {
        foreach(var content in GetPoolsOfType<T>(name))
        {
            return content.Get<T>();
        }

        return default(T);
    }

    public static GameObject Get(string name)
    {
        foreach (var content in GetPools(name))
        {
            return content.Get();
        }

        return null;
    }

    public static GameObject Get(GameObject template)
    {
        foreach (var content in GetPools(template))
        {
            return content.Get();
        }

        return null;
    }

    public static void Return<T>(GameObject toReturn)
    {
        foreach(var content in instance.contents)
        {
            if (content.outOfPool.Contains(toReturn))
            {
                content.Return<T>(toReturn);
                returned?.Invoke(toReturn);
                return;
            }
        }
    }

    public static List<GameObject> GetOutOfPools<T>()
    {
        var result = new List<GameObject>();
        foreach(var ob in GetPoolsOfType<T>())
        {
            result.AddRange(ob.outOfPool);
        }

        return result;
    }

    public static int GetOutOfPoolCount<T>()
    {
        var result = 0;

        foreach (var content in GetPoolsOfType<T>())
        {
            result += content.outOfPool.Count;
        }

        return result;
    }

    public static int GetReturnedToPoolCount<T>()
    {
        var result = 0;
        foreach (var content in GetPoolsOfType<T>())
        {
            result += content.returnedCount;
        }
        return result;
    }

    public static void ReturnAll<T>()
    {
        foreach(var content in GetPoolsOfType<T>())
        {
            content.ReturnAll();
        }
    }

    public static void Lock<T>(bool state)
    {
        foreach (var content in GetPoolsOfType<T>())
        {
            content.Lock(state);
        }
    }

    private static List<PoolContent> GetPoolsOfType<T>(string Name = "")
    {
        if (instance == null || instance.contents == null) return new List<PoolContent>();

        var key = $"{typeof(T)}_{Name}";
        if (instance.knownPools.Keys.Contains(key) && false)
        {
            return instance.knownPools[key];
        }

        List<PoolContent> result = new List<PoolContent>();

        foreach (var content in instance.contents)
        {
            if (content.IsPooling<T>(Name))
            {
                result.Add(content);
            }
        }

        if (!instance.knownPools.Keys.Contains(key))
        {
            instance.knownPools.Add(key, result);
        }

        return result;
    }

    private static List<PoolContent> GetPools(string Name)
    {
        if (instance == null || instance.contents == null) return new List<PoolContent>();

        if (instance.knownPools.Keys.Contains(Name))
        {
            return instance.knownPools[Name];
        }

        List<PoolContent> result = new List<PoolContent>();

        foreach (var content in instance.contents)
        {
            if (content.IsPooling(Name))
            {
                result.Add(content);
            }
        }

        instance.knownPools.Add(Name, result);

        return result;
    }

    private static List<PoolContent> GetPools(GameObject template)
    {
        if (instance == null || instance.contents == null) return new List<PoolContent>();

        if (instance.knownPoolTemplate.Keys.Contains(template))
        {
            return instance.knownPoolTemplate[template];
        }

        List<PoolContent> result = new List<PoolContent>();

        foreach (var content in instance.contents)
        {
            if (content.IsPooling(template))
            {
                result.Add(content);
            }
        }

        instance.knownPoolTemplate.Add(template, result);

        return result;
    }
}
