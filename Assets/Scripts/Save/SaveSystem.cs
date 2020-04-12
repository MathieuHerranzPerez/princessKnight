﻿using System.IO;
using UnityEngine;

public static class SaveSystem
{
    public static string path = Application.persistentDataPath + "/Saves/Deck";

    public static void SaveDeck(Deck deck, MasterDeck masterDeck)
    {
        SaveDeckData saveDeckData = new SaveDeckData(deck, masterDeck);

        string json = JsonUtility.ToJson(saveDeckData);

        Debug.Log(json); // affD
        Debug.Log("Save at : " + path); // affD

        if (!Directory.Exists(Path.GetDirectoryName(path)))
        {
            // create it
            Directory.CreateDirectory(Path.GetDirectoryName(path));
        }

        // open or create
        using (StreamWriter streamWriter = File.CreateText(path))
        {
            streamWriter.Write(json);
        } // close
    }

    public static SaveDeckData LoadDeck()
    {
        if (Directory.Exists(Path.GetDirectoryName(path)) && File.Exists(path))
        {
            // open
            using (StreamReader streamReader = File.OpenText(path))
            {
                string json = streamReader.ReadToEnd();
                Debug.Log(json); // affD

                return JsonUtility.FromJson<SaveDeckData>(json);
            }// close
        }
        else
        {
            Debug.LogWarning(path + "Does not exists (or we having a problem reading at it)");
            return null;
        }
    }

    public static void DeleteSave()
    {
        if (Directory.Exists(Path.GetDirectoryName(path)) && File.Exists(path))
        {
            File.Delete(path);
        }
        else
        {
            Debug.LogWarning(path + "Does not exists");
        }
    }
}
