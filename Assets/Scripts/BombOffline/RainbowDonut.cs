// @sonhg: class: BombOffline.RainbowDonut
using System;
using System.Collections;
using UnityEngine;

namespace BombOffline
{
	public class RainbowDonut : Offline_BaseMonster, ISpecialAction
	{
		protected override void InitMonsterProperty()
		{
			this.diveCooldown = this.spawnTime;
		}

		public bool DoSpecialAction()
		{
			this.BubbleShield();
			return true;
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

		private void BubbleShield()
		{
			if (this.CanBubbleShield())
			{
				this.diveCooldown = this.spawnTime;
				this.hasShield = true;
				GameObject shield = this.board.Scene.ParticlesController.PlayBubbleShield1(base.transform, null);
				base.StartCoroutine(this.UnBubbleShield(shield));
			}
		}

		private IEnumerator UnBubbleShield(GameObject shield)
		{
			yield return new WaitForSeconds(4f);
			this.hasShield = false;
			UnityEngine.Object.Destroy(shield);
			yield break;
		}

		private bool CanBubbleShield()
		{
			return this.diveCooldown <= 0f;
		}

		public override bool GetHit(int x, int y)
		{
			if (this.hasShield)
			{
				this.hasShield = false;
				return false;
			}
			return base.GetHit(x, y);
		}

		protected override void FixedUpdate()
		{
			base.FixedUpdate();
			if (!this.canAct)
			{
				return;
			}
			if (this.diveCooldown > 0f)
			{
				this.diveCooldown -= Time.deltaTime;
			}
		}

		private bool hasShield;

		protected float spawnTime = 6f;

		private float diveCooldown;
	}
}
