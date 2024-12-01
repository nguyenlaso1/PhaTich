// @sonhg: class: BombOffline.MonsterBomb
using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace BombOffline
{
	public class MonsterBomb : MonoBehaviour
	{
		private void Start()
		{
			base.StartCoroutine(this.Explode());
		}

		private IEnumerator Explode()
		{
			this.shadow.DoAlpha(0.75f, this.lifeTime);
			Vector3 scale = this.shadow.transform.localScale;
			this.shadow.transform.DOScale(scale * 1.3f, this.lifeTime);
			this.iceTranform.DOLocalMove(new Vector2(0f, 0.3f), 0.5f, false).SetEase(Ease.Linear).SetDelay(1f);
			yield return new WaitForSeconds(this.lifeTime);
			this.board.TriggerSpecialBomb(base.transform.position, this.bombLength, this.iceExplosion.gameObject);
			UnityEngine.Object.Destroy(base.gameObject);
			yield break;
		}

		[SerializeField]
		private Transform iceTranform;

		[SerializeField]
		private SpriteRenderer shadow;

		[SerializeField]
		private ParticleSystem iceExplosion;

		public float lifeTime = 1.5f;

		public int bombLength;

		[HideInInspector]
		public Offline_GameController board;
	}
}
