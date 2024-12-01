// @sonhg: class: BombOffline.SwampTile
using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace BombOffline
{
	public class SwampTile : Offline_BaseMonster, ISpecialAction
	{
		protected override void InitMonsterProperty()
		{
			this.explodeCooldown = this.explodeTime;
		}

		protected override void CreateBrain()
		{
			Wander routine = new Wander(this);
			Repeat repeat = new Repeat(new TimeLimit(routine, 5f), -1);
			Repeat repeat2 = new Repeat(new SpecialAction(this), -1);
			this.brain = new Parallel(3, 3, new Routine[]
			{
				repeat,
				repeat2
			});
		}

		public bool CanExplode()
		{
			return this.explodeCooldown <= 0f;
		}

		protected virtual void Explode()
		{
			if (this.CanExplode())
			{
				this.explodeCooldown = this.explodeTime;
				base.StartCoroutine(this.ExplodeAnimate());
			}
		}

		public virtual bool DoSpecialAction()
		{
			this.Explode();
			return true;
		}

		protected override void FixedUpdate()
		{
			base.FixedUpdate();
			if (this.explodeCooldown > 0f)
			{
				this.explodeCooldown -= Time.deltaTime;
			}
		}

		private IEnumerator ExplodeAnimate()
		{
			Sequence sequence = DOTween.Sequence();
			base.transform.ChangeColorRecursive(Color.red);
			this.canAct = false;
			yield return base.transform.DOScale(1.3f, 1f).WaitForCompletion();
			Camera.main.transform.DOShakePosition(0.7f, new Vector3(0.6f, 0.1f, 0.5f), 30, 90f, false);
			this.board.TriggerSpecialBomb(new Vector2((float)base.currentX, (float)base.currentY), 3, null);
			Camera.main.transform.DOShakePosition(0.7f, new Vector3(0.6f, 0.1f, 0.5f), 30, 90f, false);
			yield break;
		}

		[Header("SwampTile")]
		[SerializeField]
		protected float explodeTime = 1f;

		protected float explodeCooldown;
	}
}
