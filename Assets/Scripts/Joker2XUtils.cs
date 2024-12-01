// @sonhg: class: Joker2XUtils
using System;
using System.Collections.Generic;
using UnityEngine;

public class Joker2XUtils
{
	public static void DrawCurvedText(string curvedText, GameObject[] curvedLabel)
	{
		UnityEngine.Debug.Log("curvedText " + curvedText);
		Joker2XUtils.ClearCurvedText(curvedLabel);
		List<string> list = StringUtils.SplitString(curvedText);
		if (list.Count > curvedLabel.Length)
		{
			list = list.GetRange(0, curvedLabel.Length);
		}
		int num = curvedLabel.Length / 2 - list.Count / 2;
		for (int i = num; i < num + list.Count; i++)
		{
			curvedLabel[i].GetComponent<UILabel>().text = list[i - num];
		}
	}

	public static void ClearCurvedText(GameObject[] curvedLabel)
	{
		for (int i = 0; i < curvedLabel.Length; i++)
		{
			curvedLabel[i].GetComponent<UILabel>().text = string.Empty;
		}
	}

	public static string FormatChip(int chip)
	{
		int num = 1000000;
		string text = string.Empty;
		if (chip >= num)
		{
			float num2 = (float)chip / (float)num;
			if (chip % num == 0)
			{
				text = string.Empty + num2.ToString("F0") + "M";
			}
			else
			{
				text = string.Empty + (float)((int)(num2 * 100f)) / 100f + "M";
			}
		}
		else
		{
			text = string.Empty + chip + string.Empty;
		}
		text.Replace(".0", string.Empty);
		return text;
	}

	public static void LoadImage(GameObject textureObject, string link, bool cache = false)
	{
		LoadAndCache loadAndCache = textureObject.GetComponent<LoadAndCache>();
		if (loadAndCache == null)
		{
			loadAndCache = textureObject.AddComponent<LoadAndCache>();
		}
		ImageManager.AddToMap(link, loadAndCache, cache);
	}
}
