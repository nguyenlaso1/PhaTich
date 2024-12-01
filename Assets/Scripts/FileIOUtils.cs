// @sonhg: class: FileIOUtils
using System;
using System.IO;
using UnityEngine;

public class FileIOUtils
{
	public static string ReadTextFile(string sFileName)
	{
		string path = string.Empty;
		if (File.Exists(sFileName))
		{
			path = sFileName;
		}
		else
		{
			if (!File.Exists(sFileName + ".txt"))
			{
				UnityEngine.Debug.Log("Could not find file '" + sFileName + "'.");
				return null;
			}
			path = sFileName + ".txt";
		}
		StreamReader streamReader;
		try
		{
			streamReader = new StreamReader(path);
		}
		catch (Exception ex)
		{
			UnityEngine.Debug.LogWarning("Something went wrong with read.  " + ex.Message);
			return null;
		}
		string result = streamReader.ReadToEnd();
		streamReader.Close();
		return result;
	}

	public static void WriteTextFile(string sFilePathAndName, string sTextContents)
	{
		StreamWriter streamWriter = new StreamWriter(sFilePathAndName);
		streamWriter.WriteLine(sTextContents);
		streamWriter.Flush();
		streamWriter.Close();
	}
}
