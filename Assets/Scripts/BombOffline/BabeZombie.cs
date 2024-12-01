// @sonhg: class: BombOffline.BabeZombie
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BombOffline
{
	public class BabeZombie : BigDespider
	{
		protected List<Vector3> ShootDirection
		{
			get
			{
				return (UnityEngine.Random.Range(0, 2) != 0) ? this.shootDirectionAlternate : this.shootDirection;
			}
		}

		protected override void InitMonsterProperty()
		{
			this.shootDirection = new List<Vector3>();
			this.shootDirection.Add(Vector3.up);
			this.shootDirection.Add(Vector3.down);
			this.shootDirection.Add(Vector3.left);
			this.shootDirection.Add(Vector3.right);
			this.shootDirectionAlternate = new List<Vector3>();
			this.shootDirectionAlternate.Add(Vector3.up + Vector3.left);
			this.shootDirectionAlternate.Add(Vector3.up + Vector3.right);
			this.shootDirectionAlternate.Add(Vector3.down + Vector3.left);
			this.shootDirectionAlternate.Add(Vector3.down + Vector3.right);
			this.shootCooldown = this.cooldown;
		}

		protected override void ShootBullet()
		{
			if (base.CanShoot())
			{
				this.shootCooldown = this.cooldown;
				foreach (Vector3 shootDirection in this.ShootDirection)
				{
					GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.bulletPrefab);
					gameObject.transform.position = new Vector3((float)base.currentX, (float)base.currentY, 0f);
					BaseBullet component = gameObject.GetComponent<BaseBullet>();
					component.shootDirection = shootDirection;
					component.shooter = base.transform;
				}
				this.canAct = false;
				base.StartCoroutine(this.WaitThenRun(this.waitTime));
			}
		}

		private IEnumerator WaitThenRun(float waitTime)
		{
			this.SnapGrid();
			yield return new WaitForSeconds(waitTime);
			this.canAct = true;
			yield break;
		}

		[SerializeField]
		protected float waitTime = 1f;

		protected List<Vector3> shootDirectionAlternate;
	}
}
