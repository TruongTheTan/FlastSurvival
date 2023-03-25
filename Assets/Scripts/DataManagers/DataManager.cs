using System.IO;
using UnityEngine;
/// <summary>
/// This class use for "Read" and "Write" data to JSON file.
/// </summary>
public class DataManager
{
    public SaveData ReadDataFromFile()
    {
        string saveFilePath = DataPreserve.saveFilePath;

        if (File.Exists(saveFilePath))
            return JsonUtility.FromJson<SaveData>(File.ReadAllText(saveFilePath));

        else
            File.Create(saveFilePath);

        return null;
    }


    public void SaveDataToFile(SaveData data)
    {
        File.WriteAllText(DataPreserve.saveFilePath, JsonUtility.ToJson(data, true));
    }
}