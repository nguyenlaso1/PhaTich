// @sonhg: class: StringUtils
using System;
using System.Collections.Generic;

public class StringUtils
{
	public static bool CheckNullOrEmpty(string str)
	{
		return str == null || str == string.Empty;
	}

	public static List<string> SplitString(string text)
	{
		List<string> list = new List<string>();
		List<char> list2 = new List<char>
		{
			',',
			'.',
			':',
			'!'
		};
		foreach (char item in text)
		{
			if (list2.Contains(item))
			{
				if (list.Count > 0)
				{
					list[list.Count - 1] = list[list.Count - 1] + item.ToString();
				}
			}
			else
			{
				list.Add(item.ToString());
			}
		}
		return list;
	}

	public static string RemoveEnter(string text)
	{
		string text2 = string.Empty;
		char[] array = new char[0];
		foreach (char c in text)
		{
			if (c != '\n')
			{
				text2 += c;
			}
		}
		return text2;
	}

	public static string Reverse(string text)
	{
		if (text == null)
		{
			return null;
		}
		char[] array = text.ToCharArray();
		Array.Reverse(array);
		return new string(array);
	}
}
