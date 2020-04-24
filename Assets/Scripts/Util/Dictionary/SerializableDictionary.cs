using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

[Serializable]
public abstract class SerializableDictionary<K, V, E> : ISerializationCallbackReceiver
    where K : class where V : class where E : DictionaryElement<K, V>
{
    [SerializeField] private E[] arrayDictionaryElement = new E[0];

    public Dictionary<K, V> dictionary;

    public static T New<T>() where T : SerializableDictionary<K, V, E>, new()
    {
        T result = new T();
        result.dictionary = new Dictionary<K, V>();
        return result;
    }

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