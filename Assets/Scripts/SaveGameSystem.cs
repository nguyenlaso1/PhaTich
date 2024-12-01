// @sonhg: class: SaveGameSystem
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveGameSystem
{
	public static bool SaveGame(SaveGame saveGame, string name)
	{
		BinaryFormatter binaryFormatter = new BinaryFormatter();
		using (FileStream fileStream = new FileStream(SaveGameSystem.GetSavePath(name), FileMode.Create))
		{
			try
			{
				binaryFormatter.Serialize(fileStream, saveGame);
			}
			catch (Exception)
			{
				return false;
			}
		}
		return true;
	}

	public static SaveGame LoadGame(string name)
	{
		if (!SaveGameSystem.DoesSaveGameExist(name))
		{
			return null;
		}
		BinaryFormatter binaryFormatter = new BinaryFormatter();
		SaveGame result;
		using (FileStream fileStream = new FileStream(SaveGameSystem.GetSavePath(name), FileMode.Open))
		{
			try
			{
				result = (binaryFormatter.Deserialize(fileStream) as SaveGame);
			}
			catch (Exception)
			{
				result = null;
			}
		}
		return result;
	}

	public static bool DeleteSaveGame(string name)
	{
		try
		{
			File.Delete(SaveGameSystem.GetSavePath(name));
		}
		catch (Exception)
		{
			return false;
		}
		return true;
	}

	public static bool DoesSaveGameExist(string name)
	{
		return File.Exists(SaveGameSystem.GetSavePath(name));
	}

	private static string GetSavePath(string name)
	{
		return Path.Combine(Application.persistentDataPath, name + ".sav");
	}
}
