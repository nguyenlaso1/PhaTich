// @sonhg: class: BombOffline.LivingFlame
using System;
using UnityEngine;

namespace BombOffline
{
	public class LivingFlame : SwampJelly, ISpecialAction
	{
		protected override void CreateBrain()
		{
			Repeat repeat = new Repeat(new TimeLimit(new Wander(this), 5f), -1);
			Repeat repeat2 = new Repeat(new SpecialAction(this), -1);
			this.brain = new Parallel(3, 3, new Routine[]
			{
				repeat,
				repeat2
			});
		}

		protected override void InitMonsterProperty()
		{
			base.InitMonsterProperty();
			this.flameCooldown = this.flameTime;
		}

		protected override void FixedUpdate()
		{
			base.FixedUpdate();
			if (!this.hasShield)
			{
				this.flameCooldown -= Time.deltaTime;
			}
		}

		public bool DoSpecialAction()
		{
			this.ShieldUp();
			return true;
		}

		public override bool GetHit(int x, int y)
		{
			if (this.hasShield)
			{
				this.hasShield = false;
				this.flame.Stop();
				this.flame.Clear();
				return false;
			}
			return base.GetHit(x, y);
		}

		private bool CanShieldUp()
		{
			return this.flameCooldown < 0f && !this.hasShield;
		}

		protected void ShieldUp()
		{
			if (this.CanShieldUp())
			{
				this.flame.Play();
				this.hasShield = true;
				this.flameCooldown = this.flameTime;
			}
		}

		[SerializeField]
		private ParticleSystem flame;

		[SerializeField]
		private float flameTime = 6f;

		private float flameCooldown;

		private bool hasShield = true;
	}
}
