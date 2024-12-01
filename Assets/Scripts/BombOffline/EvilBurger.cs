// @sonhg: class: BombOffline.EvilBurger
using System;
using System.Collections;
using UnityEngine;

namespace BombOffline
{
	public class EvilBurger : Offline_BaseMonster, ISpecialAction
	{
		protected override void InitMonsterProperty()
		{
			this.spawnCooldown = this.spawnTime;
		}

		protected override void FixedUpdate()
		{
			base.FixedUpdate();
			if (!this.canAct)
			{
				return;
			}
			if (this.spawnCooldown > 0f)
			{
				this.spawnCooldown -= Time.deltaTime;
			}
		}

		public bool DoSpecialAction()
		{
			this.Rage();
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

		private void Rage()
		{
			if (this.IsRage())
			{
				this.board.Scene.ParticlesController.PlaySpecialSkillParticle(base.transform, new float?(3f));
				this.spawnCooldown = this.spawnTime;
				this.multiplyMoveSpeed = 2f;
				this.isChasePlayer = true;
				base.StartCoroutine(this.unRage());
			}
		}

		private bool IsRage()
		{
			return this.spawnCooldown <= 0f;
		}

		private IEnumerator unRage()
		{
			yield return new WaitForSeconds(3f);
			this.multiplyMoveSpeed = 1f;
			this.isChasePlayer = false;
			yield break;
		}

		protected float spawnTime = 10f;

		protected float spawnCooldown;
	}
}
