// @sonhg: class: BombOffline.Offline_ItemController
using System;
using UnityEngine;

namespace BombOffline
{
	public class Offline_ItemController : MonoBehaviour
	{
		private void Awake()
		{
			base.gameObject.tag = "Item";
		}

		public ItemType type = ItemType.SNARE;

		public int value;
	}
}
