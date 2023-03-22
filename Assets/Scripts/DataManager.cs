using System.IO;
using UnityEngine;

public class DataManager
{
    private string _saveFile = Application.persistentDataPath + "/savedata.json";

    public SaveData ReadData()
    {
        SaveData data = new SaveData();
        if (File.Exists(_saveFile))
        {
            string content = File.ReadAllText(_saveFile);
            data = JsonUtility.FromJson<SaveData>(content);
        }
        else
        {
            File.Create(_saveFile);
        }
        return data;
    }

    public void SaveData(SaveData data)
    {
        string content = JsonUtility.ToJson(data, true);
        File.WriteAllText(_saveFile, content);
    }
}