// @sonhg: class: CFX2_AutoRotate
using System;
using UnityEngine;

public class CFX2_AutoRotate : MonoBehaviour
{
	private void Update()
	{
		base.transform.Rotate(this.speed * Time.deltaTime);
	}

	public Vector3 speed = new Vector3(0f, 40f, 0f);
}
