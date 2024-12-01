// @sonhg: class: MathUtils
using System;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public class MathUtils
{
	public static string PasswordGenerator(int passwordLength, bool strongPassword)
	{
		int seed = UnityEngine.Random.Range(1, int.MaxValue);
		char[] array = new char[passwordLength];
		System.Random random = new System.Random(seed);
		for (int i = 0; i < passwordLength; i++)
		{
			if (strongPassword && i % UnityEngine.Random.Range(3, passwordLength) == 0)
			{
				array[i] = "!#$%&'()*+,-./:;<=>?@[\\]_"[random.Next(0, "!#$%&'()*+,-./:;<=>?@[\\]_".Length)];
			}
			else
			{
				array[i] = "abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ0123456789"[random.Next(0, "abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ0123456789".Length)];
			}
		}
		return new string(array);
	}

	public static string getHashSha256(string text)
	{
		byte[] bytes = Encoding.Unicode.GetBytes(text);
		SHA256Managed sha256Managed = new SHA256Managed();
		byte[] array = sha256Managed.ComputeHash(bytes);
		string text2 = string.Empty;
		foreach (byte b in array)
		{
			text2 += string.Format("{0:x2}", b);
		}
		return text2;
	}

	public static void ShiftElement<T>(T[] array, int oldIndex, int newIndex)
	{
		if (oldIndex == newIndex)
		{
			return;
		}
		T t = array[oldIndex];
		if (newIndex < oldIndex)
		{
			Array.Copy(array, newIndex, array, newIndex + 1, oldIndex - newIndex);
		}
		else
		{
			Array.Copy(array, oldIndex + 1, array, oldIndex, newIndex - oldIndex);
		}
		array[newIndex] = t;
	}

	public static float Max(params float[] values)
	{
		float num = 0f;
		for (int i = 0; i < values.Length; i++)
		{
			if (values[i] > num)
			{
				num = values[i];
			}
		}
		return num;
	}

	public static Vector3 Angle(Vector3 from, Vector3 to, float offset)
	{
		Vector3 vector = to - from;
		float z = Mathf.Atan2(vector.y, vector.x) * 57.29578f + offset;
		return new Vector3(0f, 0f, z);
	}
}
