// @sonhg: class: BombOffline.Offline_BorderWall
using System;
using UnityEngine;

namespace BombOffline
{
	public class Offline_BorderWall : MonoBehaviour
	{
		private void OnTriggerEnter2D(Collider2D collider)
		{
			if (collider.gameObject.tag == "Item")
			{
				Offline_ItemController component = collider.gameObject.GetComponent<Offline_ItemController>();
				UnityEngine.Object.Destroy(collider.gameObject);
			}
		}
	}
}
