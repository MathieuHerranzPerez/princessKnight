using System;
using System.Reflection;
using UnityEditor;

[CustomEditor(typeof(StatsAchievement))]
public class AchievementGameStatsEditor : Editor
{
    public string GameStatsAttribute;

    string[] choices = new[] { "" };
    int choiceIndex = 0;

    public override void OnInspectorGUI()
    {
        // Draw the default inspector
        DrawDefaultInspector();

        Type gameStatsType = typeof(GameStats);
        FieldInfo[] arrayGameStatsProps = gameStatsType.GetFields();

        if (arrayGameStatsProps.Length < 1)
            return;

        choices = new string[arrayGameStatsProps.Length];
        int i = 0;
        foreach (FieldInfo prop in arrayGameStatsProps)
        {
            choices[i++] = prop.Name;
        }

        choiceIndex = EditorGUILayout.Popup(choiceIndex, choices);
        StatsAchievement achievement = target as StatsAchievement;
        // Update the selected choice in the underlying object
        achievement.GameStatsAttributeCheckEarn = choices[choiceIndex];
        // Save the changes back to the object
        EditorUtility.SetDirty(target);
    }
}
