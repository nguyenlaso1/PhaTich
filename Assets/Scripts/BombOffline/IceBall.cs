// @sonhg: class: BombOffline.IceBall
using System;
using System.Collections.Generic;
using UnityEngine;

namespace BombOffline
{
	public class IceBall : Offline_BaseMonster, ISpecialAction
	{
		protected override void CreateBrain()
		{
			SpecialAction routine = new SpecialAction(this);
			Wander routine2 = new Wander(this);
			Repeat repeat = new Repeat(routine2, -1);
			Repeat repeat2 = new Repeat(routine, -1);
			Parallel brain = new Parallel(3, 3, new Routine[]
			{
				repeat,
				repeat2
			});
			this.brain = brain;
		}

		protected override void InitMonsterProperty()
		{
			this.shootDirection = new List<Vector3>();
			this.shootDirection.Add(Vector3.up);
			this.shootDirection.Add(Vector3.down);
			this.shootDirection.Add(Vector3.left);
			this.shootDirection.Add(Vector3.right);
			this.shootCooldown = this.cooldown;
		}

		protected override void FixedUpdate()
		{
			base.FixedUpdate();
			if (this.shootCooldown > 0f)
			{
				this.shootCooldown -= Time.deltaTime;
			}
		}

		protected virtual void Shoot()
		{
			if (this.CanShoot())
			{
				this.shootCooldown = this.cooldown;
				MusicManager.instance.PlayOneShot(this.shootSound, 1f);
				foreach (Vector3 vector in this.shootDirection)
				{
					BaseBullet baseBullet = UnityEngine.Object.Instantiate<BaseBullet>(this.bulletPrefab);
					baseBullet.transform.position = new Vector3((float)base.currentX, (float)base.currentY, 0f);
					baseBullet.shootDirection = vector;
				}
			}
		}

		public virtual bool CanShoot()
		{
			return this.shootCooldown <= 0f;
		}

		public bool DoSpecialAction()
		{
			this.Shoot();
			return true;
		}

		[Header("Iceal")]
		[SerializeField]
		protected float cooldown = 3f;

		[SerializeField]
		protected BaseBullet bulletPrefab;

		[SerializeField]
		protected AudioClip shootSound;

		protected float shootCooldown;

		protected List<Vector3> shootDirection;
	}
}
