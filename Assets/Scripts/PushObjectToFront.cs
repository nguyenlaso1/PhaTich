// @sonhg: class: PushObjectToFront
using System;
using UnityEngine;

public class PushObjectToFront : MonoBehaviour
{
	private void Start()
	{
		base.GetComponent<Renderer>().sortingLayerName = this.layerToPushTo;
	}

	public string layerToPushTo;
}
