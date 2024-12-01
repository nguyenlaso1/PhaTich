// @sonhg: class: DrawCurveUtil
using System;
using UnityEngine;

public class DrawCurveUtil
{
	public static Vector3[] CreateEllipse(float a, float b, float h, float k, int resolution, Vector3 direction, float curveRadian = 2f)
	{
		Vector3[] array = new Vector3[resolution + 1];
		Quaternion rotation = Quaternion.FromToRotation(Vector3.right, direction);
		Vector3 b2 = new Vector3(h, k, 0f);
		for (int i = 0; i <= resolution; i++)
		{
			float f = (float)i / (float)resolution * curveRadian * 3.14159274f;
			array[i] = new Vector3(a * Mathf.Cos(f), b * Mathf.Sin(f), 0f);
			array[i] = rotation * array[i] + b2;
		}
		Vector3 a2 = array[0];
		for (int j = 0; j <= resolution; j++)
		{
			array[j] = a2 - array[j] + b2;
		}
		return array;
	}
}
