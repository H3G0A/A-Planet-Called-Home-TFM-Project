using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class FileDataHandler
{
    private string dataDirPath;
    private string dataFileName;
    private string fullPath;

    public FileDataHandler(string path, string fileName)
    {
        this.dataDirPath = path;
        this.dataFileName = fileName;

        this.fullPath = Path.Combine(path, fileName);
    }

    public GameData Load()
    {
        string errorMessage = "Error while trying to load data from path: " + fullPath;
        GameData data = null;

        if (File.Exists(fullPath))
        {
            try
            {
                BinaryFormatter formatter = new BinaryFormatter();
                
                using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    data = formatter.Deserialize(stream) as GameData;
                }
            }
            catch (Exception e)
            {
                Debug.LogError(errorMessage + "\n" + e);
            }
        }
        else
        {
            Debug.LogWarning(errorMessage + "\n" + "File not found.");
        }

        return data;
    }

    public void Save(GameData data)
    {
        try
        {
            BinaryFormatter formatter = new BinaryFormatter();

            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                formatter.Serialize(stream, data);
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Error while trying to save data to file: " + fullPath + "\n" + e);
        }
    }
}
