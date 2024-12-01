// @sonhg: class: BombOffline.Offline_Bomb
using System;
using System.Collections;
using UnityEngine;

namespace BombOffline
{
	public class Offline_Bomb : MonoBehaviour
	{
		private void Start()
		{
			if (this.autoDestroy)
			{
				this.StartCount();
			}
		}

		public void StartCount()
		{
			base.StartCoroutine(this.ExplodeBomb());
		}

		public void ResetCount()
		{
			base.StopAllCoroutines();
			this.StartCount();
		}

		private IEnumerator ExplodeBomb()
		{
			yield return new WaitForSeconds(3f);
			this.scene.GameController.TriggerBomb(this.bomb.position);
			yield break;
		}

		public Offline_BombScene scene;

		public BombModel bomb;

		public bool isKicked;

		public bool isThrown;

		public bool autoDestroy = true;
	}
}
