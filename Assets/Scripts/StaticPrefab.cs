// @sonhg: class: StaticPrefab
using System;
using System.Collections.Generic;
using UnityEngine;

public class StaticPrefab
{
	public static GameObject GetPrefab(string path)
	{
		if (StaticPrefab.staticPrefabs.ContainsKey(path))
		{
			if (StaticPrefab.staticPrefabs[path] != null)
			{
				return StaticPrefab.staticPrefabs[path];
			}
			StaticPrefab.staticPrefabs.Remove(path);
		}
		StaticPrefab.staticPrefabs.Add(path, Resources.Load(path) as GameObject);
		return StaticPrefab.staticPrefabs[path];
	}

	private static Dictionary<string, GameObject> staticPrefabs = new Dictionary<string, GameObject>();
}
