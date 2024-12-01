// @sonhg: class: BombOffline.IceTower
using System;
using System.Collections;
using UnityEngine;

namespace BombOffline
{
	public class IceTower : BigDespider
	{
		protected override void FixedUpdate()
		{
			base.FixedUpdate();
			if (this.shootCooldown > 0f && this.shootCooldown < 1f)
			{
				base.transform.ChangeColorRecursive(Color.yellow);
			}
		}

		protected override void ShootBullet()
		{
			if (base.CanShoot())
			{
				this.shootCooldown = this.cooldown + this.waitTime;
				foreach (Vector3 shootDirection in this.shootDirection)
				{
					GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.bulletPrefab);
					gameObject.transform.position = new Vector3((float)base.currentX, (float)base.currentY, 0f);
					BaseBullet component = gameObject.GetComponent<BaseBullet>();
					component.shootDirection = shootDirection;
					component.shooter = base.transform;
				}
				this.SetTargetableBody(true);
				base.StartCoroutine(this.WaitThenBecomeUntargetable(this.waitTime));
			}
		}

		private IEnumerator WaitThenBecomeUntargetable(float waitTime)
		{
			yield return new WaitForSeconds(waitTime);
			this.SetTargetableBody(false);
			yield break;
		}

		protected virtual void SetTargetableBody(bool isTargetable)
		{
			if (isTargetable)
			{
				base.transform.ChangeColorRecursive(Color.red);
				this.canAct = false;
			}
			else
			{
				base.transform.ChangeColorRecursive(Color.white);
				this.canAct = true;
			}
		}

		public override bool GetHit(int x, int y)
		{
			return !this.canAct && base.GetHit(x, y);
		}

		[SerializeField]
		protected float waitTime = 4f;
	}
}
