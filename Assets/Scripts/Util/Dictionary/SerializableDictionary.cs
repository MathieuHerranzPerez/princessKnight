using System.Collections.Generic;
using UnityEngine;

public abstract class SerializableDictionary<K, V> : ISerializationCallbackReceiver
{
    [SerializeField] private K[] keys;
    [SerializeField] private V[] values;

    public Dictionary<K, V> dictionary;


    public static T New<T>() where T : SerializableDictionary<K, V>, new()
    {
        T result = new T();
        result.dictionary = new Dictionary<K, V>();
        return result;
    }

    public void OnAfterDeserialize()
    {
        dictionary = new Dictionary<K, V>(keys.Length);
        for (int i = 0; i < keys.Length; ++i)
        {
            dictionary[keys[i]] = values[i];
        }
        keys = null;
        values = null;
    }

    public void OnBeforeSerialize()
    {
        keys = new K[dictionary.Count];
        values = new V[dictionary.Count];
        int i = 0;
        while (dictionary.GetEnumerator().MoveNext())
        {
            var kvp = dictionary.GetEnumerator().Current;
            keys[i] = kvp.Key;
            values[i] = kvp.Value;
            ++i;
        }
    }
}