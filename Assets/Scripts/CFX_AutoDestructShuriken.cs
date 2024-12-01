// @sonhg: class: CFX_AutoDestructShuriken
using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class CFX_AutoDestructShuriken : MonoBehaviour
{
	private void OnEnable()
	{
		base.StartCoroutine("CheckIfAlive");
	}

	private IEnumerator CheckIfAlive()
	{
		do
		{
			yield return new WaitForSeconds(0.5f);
		}
		while (base.GetComponent<ParticleSystem>().IsAlive(true));
		if (this.OnlyDeactivate)
		{
			base.gameObject.SetActive(false);
		}
		else
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
		yield break;
	}

	public bool OnlyDeactivate;
}
