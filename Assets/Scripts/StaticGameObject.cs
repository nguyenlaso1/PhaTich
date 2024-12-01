// @sonhg: class: StaticGameObject
using System;
using System.Collections.Generic;
using UnityEngine;

public class StaticGameObject
{
	public static GameObject GetGameObject(string path)
	{
		if (StaticGameObject.staticGameObjects.ContainsKey(path))
		{
			if (StaticGameObject.staticGameObjects[path] != null)
			{
				return StaticGameObject.staticGameObjects[path];
			}
			StaticGameObject.staticGameObjects.Remove(path);
		}
		StaticGameObject.staticGameObjects.Add(path, StaticGameObject.AddChild(Context.currentMono.gameObject, StaticPrefab.GetPrefab(path)));
		return StaticGameObject.staticGameObjects[path];
	}

	public static bool IsGameObject(string path)
	{
		return StaticGameObject.staticGameObjects.ContainsKey(path) && StaticGameObject.staticGameObjects[path] != null;
	}

	public static GameObject AddChild(GameObject parent, GameObject prefab)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(prefab);
		if (gameObject != null && parent != null)
		{
			Transform transform = gameObject.transform;
			transform.SetParent(parent.transform, false);
			gameObject.layer = parent.layer;
		}
		return gameObject;
	}

	private static Dictionary<string, GameObject> staticGameObjects = new Dictionary<string, GameObject>();
}
