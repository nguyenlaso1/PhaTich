// @sonhg: class: ObjectPool
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectPool
{
	public ObjectPool(GameObject gameObject, int initialCapacity, Action<GameObject> initAction = null, Action<GameObject> resetAction = null)
	{
		this.poolGameObject = gameObject;
		this.initAction = initAction;
		this.resetAction = resetAction;
		this.availableGameObjects = new Stack<GameObject>();
		this.allGameObjects = new List<GameObject>();
		for (int i = 0; i < initialCapacity; i++)
		{
			GameObject gameObject2 = this.GetGameObject();
			this.allGameObjects.Add(gameObject2);
			this.availableGameObjects.Push(gameObject2);
		}
	}

	private GameObject GetGameObject()
	{
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.poolGameObject);
		gameObject.name = this.poolGameObject.name + "_" + (this.allGameObjects.Count + 1).ToString();
		if (this.initAction != null)
		{
			this.initAction(gameObject);
		}
		gameObject.SetActive(false);
		return gameObject;
	}

	public GameObject Spawn(Vector3 position, Quaternion rotation)
	{
		GameObject gameObject;
		if (!this.availableGameObjects.Any<GameObject>())
		{
			gameObject = this.GetGameObject();
			this.allGameObjects.Add(gameObject);
			this.availableGameObjects.Push(gameObject);
		}
		gameObject = this.availableGameObjects.Pop();
		Transform transform = gameObject.transform;
		transform.position = position;
		transform.rotation = rotation;
		this.SetActive(gameObject, true);
		if (this.resetAction != null)
		{
			this.resetAction(gameObject);
		}
		return gameObject;
	}

	public bool Destroy(GameObject target)
	{
		this.availableGameObjects.Push(target);
		this.SetActive(target, false);
		return true;
	}

	public void Clear()
	{
		foreach (GameObject obj in this.availableGameObjects)
		{
			UnityEngine.Object.Destroy(obj);
		}
		foreach (GameObject obj2 in this.allGameObjects)
		{
			UnityEngine.Object.Destroy(obj2);
		}
		this.availableGameObjects.Clear();
		this.allGameObjects.Clear();
	}

	public void Reset()
	{
		this.availableGameObjects.Clear();
		foreach (GameObject gameObject in this.allGameObjects)
		{
			gameObject.SetActive(false);
			this.availableGameObjects.Push(gameObject);
		}
	}

	protected void SetActive(GameObject target, bool value)
	{
		target.SetActive(value);
	}

	private GameObject poolGameObject;

	private Stack<GameObject> availableGameObjects;

	private List<GameObject> allGameObjects;

	private Action<GameObject> initAction;

	private Action<GameObject> resetAction;
}
