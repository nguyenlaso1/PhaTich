// @sonhg: class: BombOffline.GlassPenguin
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BombOffline
{
	public class GlassPenguin : Offline_BaseMonster, ISpecialAction
	{
		protected override void InitMonsterProperty()
		{
			this.inviCooldown = this.inviTime;
		}

		protected override void CreateBrain()
		{
			Repeat repeat = new Repeat(new Wander(this), -1);
			Repeat repeat2 = new Repeat(new SpecialAction(this), -1);
			this.brain = new Parallel(3, 3, new Routine[]
			{
				repeat,
				repeat2
			});
		}

		protected virtual void Invi()
		{
			if (this.CanInvi())
			{
				this.inviCooldown = this.inviTime;
				foreach (SpriteRenderer render in this.listSprite)
				{
					render.DoAlpha(0f, 0.5f);
				}
				base.StartCoroutine(this.UnInvi());
			}
		}

		private IEnumerator UnInvi()
		{
			yield return new WaitForSeconds(4.5f);
			foreach (SpriteRenderer render in this.listSprite)
			{
				render.DoAlpha(1f, 0.5f);
			}
			yield break;
		}

		public bool CanInvi()
		{
			return this.inviCooldown <= 0f;
		}

		public virtual bool DoSpecialAction()
		{
			this.Invi();
			return true;
		}

		protected override void FixedUpdate()
		{
			base.FixedUpdate();
			if (this.inviCooldown > 0f)
			{
				this.inviCooldown -= Time.deltaTime;
			}
			if (!this.isUpdateCycle)
			{
				return;
			}
			this.board.PlaceObstacle(this.footPrefab, new Vector2((float)base.currentX, (float)base.currentY), new Vector3?(base.CurrentDirection.GetDircetionVector()));
		}

		[Header("GlassPenguin")]
		[SerializeField]
		protected List<SpriteRenderer> listSprite;

		[SerializeField]
		protected FootTrace footPrefab;

		[SerializeField]
		protected float inviTime = 9f;

		protected float inviCooldown;
	}
}
