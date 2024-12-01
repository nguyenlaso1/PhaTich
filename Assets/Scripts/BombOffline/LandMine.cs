// @sonhg: class: BombOffline.LandMine
using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace BombOffline
{
	public class LandMine : BombScript
	{
		private void Start()
		{
			BoxCollider2D component = base.GetComponent<BoxCollider2D>();
			component.size = new Vector2(this.range, this.range);
		}

		protected virtual void OnTriggerEnter2D(Collider2D collider)
		{
			if (collider.gameObject.CompareTag("Player"))
			{
				this.DestroyBomb();
			}
		}

		private IEnumerator Explode()
		{
			Sequence sequence = DOTween.Sequence();
			sequence.Append(this.spriteRender.DOColor(Color.red, 0.2f));
			sequence.Append(this.spriteRender.DOColor(Color.yellow, 0.2f));
			sequence.Append(this.spriteRender.DOColor(Color.red, 0.2f));
			sequence.Append(this.spriteRender.DOColor(Color.yellow, 0.2f));
			sequence.Append(this.spriteRender.DOColor(Color.red, 0.2f));
			yield return new WaitForSeconds(this.lifeTime);
			this.board.TriggerSpecialBomb(base.transform.position, this.bombLength, null);
			UnityEngine.Object.Destroy(base.gameObject);
			yield break;
		}

		public override void DestroyBomb()
		{
			if (!this._isExplode)
			{
				this._isExplode = true;
				base.StartCoroutine(this.Explode());
			}
		}

		private bool _isExplode;

		[SerializeField]
		private float range = 3f;

		public float lifeTime = 1f;

		public int bombLength;
	}
}
