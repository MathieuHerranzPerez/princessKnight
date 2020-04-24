using System;

[Serializable]
public class AchievementDictionary : SerializableDictionary<string, Achievement, AchievementDicElem>
{
}

[Serializable]
public class AchievementDicElem : DictionaryElement<string, Achievement>
{
}