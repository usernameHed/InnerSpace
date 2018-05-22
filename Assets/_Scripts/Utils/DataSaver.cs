﻿using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

/// <summary>
/// Save data to binary format
/// </summary>
public class DataSaver
{
    #region Core
    /// <summary>
    /// Save data from path
    /// </summary>
	public void Save(PersistantData data)
    {
		if (!data.GetType().IsSerializable)
		{
			return;
		}

        BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create(Application.persistentDataPath + data.GetFilePath());

		bf.Serialize(file, data);
        file.Close();
        Debug.Log(data.GetFilePath() + " saved!");
    }

    /// <summary>
    /// Load data from path
    /// </summary>
	public T Load<T>(string path)
    {
		if (typeof(T).IsSerializable && File.Exists (Application.persistentDataPath + path))
		{
			BinaryFormatter bf = new BinaryFormatter ();
			FileStream file = File.Open (Application.persistentDataPath + path, FileMode.Open);
			T currentData = (T)bf.Deserialize (file);
			file.Close ();

			Debug.Log (path + " loaded!");
			return currentData;
		}

		return default(T);
    }

    /// <summary>
    /// Delete save file
    /// </summary>
	public void DeleteSave(string name)
    {
		if (File.Exists(Application.persistentDataPath + name))
        {
			File.Delete(Application.persistentDataPath + name);
        }
    }
		
    #endregion
}

[Serializable]
public abstract class PersistantData
{
	public abstract string GetFilePath ();
}