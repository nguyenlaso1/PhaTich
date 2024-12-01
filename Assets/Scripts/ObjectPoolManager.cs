// @sonhg: class: ObjectPoolManager
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectPoolManager
{
	public static ObjectPoolManager Instance
	{
		get
		{
			if (ObjectPoolManager.instance == null)
			{
				ObjectPoolManager.instance = new ObjectPoolManager();
				ObjectPoolManager.instance.objectsPool = new Dictionary<string, ObjectPool>();
			}
			return ObjectPoolManager.instance;
		}
	}

	public static ObjectPoolManager NewInstance
	{
		get
		{
			if (ObjectPoolManager.instance == null)
			{
				ObjectPoolManager.instance = new ObjectPoolManager();
				ObjectPoolManager.instance.objectsPool = new Dictionary<string, ObjectPool>();
			}
			else
			{
				foreach (ObjectPool objectPool in ObjectPoolManager.instance.objectsPool.Values)
				{
					objectPool.Clear();
				}
				ObjectPoolManager.instance.objectsPool.Clear();
			}
			return ObjectPoolManager.instance;
		}
	}

	public void CreatePool(string type, ObjectPool pool)
	{
		if (!this.objectsPool.ContainsKey(type))
		{
			this.objectsPool.Add(type, pool);
		}
		else
		{
			this.objectsPool[type].Clear();
		}
	}

	public void Reset()
	{
		foreach (KeyValuePair<string, ObjectPool> keyValuePair in this.objectsPool)
		{
			keyValuePair.Value.Reset();
		}
	}

	public ObjectPool GetPool(string type)
	{
		if (this.objectsPool != null && this.objectsPool.ContainsKey(type))
		{
			return this.objectsPool[type];
		}
		return null;
	}

	public void Spawn(string type, Vector3 instantiateVector)
	{
		if (ObjectPoolManager.Instance.objectsPool.Any<KeyValuePair<string, ObjectPool>>())
		{
			ObjectPoolManager.Instance.GetPool(type).Spawn(instantiateVector, Quaternion.identity);
		}
	}

	public void Destroy(string type, GameObject target)
	{
		if (ObjectPoolManager.Instance.objectsPool.Any<KeyValuePair<string, ObjectPool>>())
		{
			ObjectPoolManager.Instance.GetPool(type).Destroy(target);
		}
	}

	public Dictionary<string, ObjectPool> objectsPool;

	private static ObjectPoolManager instance;
}
