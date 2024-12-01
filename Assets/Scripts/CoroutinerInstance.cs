// @sonhg: class: CoroutinerInstance
using System;
using System.Collections;
using UnityEngine;

public class CoroutinerInstance : MonoBehaviour
{
	private void Awake()
	{
		UnityEngine.Object.DontDestroyOnLoad(this);
	}

	public Coroutine ProcessWork(IEnumerator iterationResult)
	{
		return base.StartCoroutine(this.DestroyWhenComplete(iterationResult));
	}

	public IEnumerator DestroyWhenComplete(IEnumerator iterationResult)
	{
		yield return base.StartCoroutine(iterationResult);
		UnityEngine.Object.Destroy(base.gameObject);
		yield break;
	}
}
