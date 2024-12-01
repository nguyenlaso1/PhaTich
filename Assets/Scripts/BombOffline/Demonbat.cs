// @sonhg: class: BombOffline.Demonbat
using System;
using UnityEngine;

namespace BombOffline
{
	public class Demonbat : Offline_BaseMonster, ISpecialAction
	{
		protected override void CreateBrain()
		{
			Repeat repeat = new Repeat(new Wander(this), -1);
			Repeat repeat2 = new Repeat(new SpecialAction(this), -1);
			this.brain = new Parallel(3, 3, new Routine[]
			{
				repeat2,
				repeat
			});
		}

		protected override void InitMonsterProperty()
		{
			this.shootCooldown = this.shootTime;
		}

		protected virtual void Shoot()
		{
			if (this.CanShoot())
			{
				this.shootCooldown = this.shootTime;
				Vector3 point = Vector3.zero;
				switch (base.CurrentDirection)
				{
				case MoveDirection.RIGHT:
					point = Vector3.down;
					break;
				case MoveDirection.LEFT:
					point = Vector3.up;
					break;
				case MoveDirection.DOWN:
					point = Vector3.left;
					break;
				case MoveDirection.UP:
					point = Vector3.right;
					break;
				}
				for (int i = 1; i <= 5; i++)
				{
					BaseBullet baseBullet = UnityEngine.Object.Instantiate<BaseBullet>(this.bulletPrefab);
					baseBullet.transform.position = new Vector3((float)base.currentX, (float)base.currentY, 0f);
					baseBullet.shootDirection = Quaternion.Euler(0f, 0f, (float)(i * 30)) * point;
				}
			}
		}

		public bool CanShoot()
		{
			return this.canAct && this.shootCooldown <= 0f;
		}

		public bool DoSpecialAction()
		{
			this.Shoot();
			return true;
		}

		protected override void FixedUpdate()
		{
			base.FixedUpdate();
			if (!this.canAct)
			{
				return;
			}
			if (this.shootCooldown > 0f)
			{
				this.shootCooldown -= Time.deltaTime;
			}
		}

		protected float shootCooldown;

		[SerializeField]
		protected float shootTime = 7f;

		[SerializeField]
		protected BaseBullet bulletPrefab;
	}
}
