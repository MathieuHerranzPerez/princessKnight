using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

[Serializable]
public abstract class SerializableDictionary<K, V, E> : Dictionary<K, V>, ISerializationCallbackReceiver
    where K : class where V : class where E : DictionaryElement<K, V>
{
    [SerializeField] private E[] arrayDictionaryElement = new E[0];

    private Dictionary<K, V> dictionary;

    public SerializableDictionary()
    {
        dictionary = new Dictionary<K, V>();
    }

    //public static T New<T>() where T : SerializableDictionary<K, V, E>, new()
    //{
    //    T result = new T();
    //    result.dictionary = new Dictionary<K, V>();
    //    return result;
    //}

    public void OnAfterDeserialize()
    {
        dictionary = new Dictionary<K, V>(arrayDictionaryElement.Length);
        for (int i = 0; i < arrayDictionaryElement.Length; ++i)
        {
            dictionary[arrayDictionaryElement[i].key] = arrayDictionaryElement[i].value;
        }
    }

    public void OnBeforeSerialize()
    {

    }

#region properties
    public new int Count { get { return dictionary.Count; } }
    public new V this[K key] {
        get {
            return dictionary[key];
        }
        set
        {
            dictionary[key] = value;
        }
    }
    public new Dictionary<K, V>.KeyCollection Keys { get { return dictionary.Keys; } }
    public new Dictionary<K, V>.ValueCollection Values { get { return dictionary.Values; } }
#endregion

#region methods

public new void Add(K key, V value)
    {
        dictionary.Add(key, value);
    }

    public new void Clear()
    {
        dictionary.Clear();
    }

    public new bool ContainsKey(K key)
    {
        return dictionary.ContainsKey(key);
    }

    public new bool ContainsValue(V value)
    {
        return dictionary.ContainsValue(value);
    }

    public new Dictionary<K, V>.Enumerator GetEnumerator()
    {
        return dictionary.GetEnumerator();
    }

    public new void GetObjectData(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
    {
        dictionary.GetObjectData(info, context);
    }

    public new void OnDeserialization(object sender)
    {
        dictionary.OnDeserialization(sender);
    }

    public new bool Remove(K key)
    {
        return dictionary.Remove(key);
    }

    public new bool TryGetValue(K key, out V value)
    {
        return dictionary.TryGetValue(key, out value);
    }
    #endregion

}

[Serializable]
public abstract class DictionaryElement<K, V> where K : class where V : class
{
    [SerializeField] public K key;
    [SerializeField] public V value;

    public DictionaryElement()
    {
    }

    public DictionaryElement(K key, V value)
    {
        this.key = key;
        this.value = value;
    }

    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("[");
        sb.Append(key.ToString());
        sb.Append(" : ");
        sb.Append(value.ToString());
        sb.Append("]");
        return sb.ToString();
    }
}