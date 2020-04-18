using System;

[Serializable]
public class RewardDictionary : SerializableDictionary<string, Reward, RewardDicElem>
{
}

[Serializable]
public class RewardDicElem : DictionaryElement<string, Reward>
{
}