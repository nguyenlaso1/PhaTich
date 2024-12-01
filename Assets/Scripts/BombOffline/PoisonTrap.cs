// @sonhg: class: BombOffline.PoisonTrap
using System;
using System.Collections;
using UnityEngine;

namespace BombOffline
{
	public class PoisonTrap : Offline_Obstacle
	{
		private void Start()
		{
			this.StartCount();
		}

		public void StartCount()
		{
			base.StartCoroutine(this.RemoveObstacle());
		}

		public override void Reset()
		{
			base.StopAllCoroutines();
			this.StartCount();
		}

		private IEnumerator RemoveObstacle()
		{
			if (this.time < 0f)
			{
				yield break;
			}
			yield return new WaitForSeconds(this.time);
			this.scene.GameController.RemoveObstacle(this);
			yield break;
		}

		public float time = 3f;
	}
}
