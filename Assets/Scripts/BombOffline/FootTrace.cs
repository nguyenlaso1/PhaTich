// @sonhg: class: BombOffline.FootTrace
using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace BombOffline
{
	public class FootTrace : Offline_Obstacle
	{
		private void Start()
		{
			this.StartCount();
		}

		public void StartCount()
		{
			this.foot1.DOKill(false);
			this.foot2.DOKill(false);
			this.foot1.SetAlpha(1f);
			this.foot2.SetAlpha(1f);
			this.foot1.DoAlpha(0f, this.time);
			this.foot2.DoAlpha(0f, this.time);
			base.StartCoroutine(this.RemoveObstacle());
		}

		public override void Reset()
		{
			base.StopAllCoroutines();
			this.StartCount();
		}

		private IEnumerator RemoveObstacle()
		{
			yield return new WaitForSeconds(this.time);
			this.scene.GameController.RemoveObstacle(this);
			yield break;
		}

		public float time = 3f;

		[SerializeField]
		private SpriteRenderer foot1;

		[SerializeField]
		private SpriteRenderer foot2;
	}
}
