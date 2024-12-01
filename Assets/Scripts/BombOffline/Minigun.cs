// @sonhg: class: BombOffline.Minigun
using System;
using UnityEngine;

namespace BombOffline
{
	public class Minigun : IceBall
	{
		protected override void Shoot()
		{
			if (this.CanShoot())
			{
				this.shootCooldown = this.cooldown + (float)UnityEngine.Random.Range(0, 3);
				MusicManager.instance.PlayOneShot(this.shootSound, 1f);
				for (int i = 1; i <= 5; i++)
				{
					BaseBullet baseBullet = UnityEngine.Object.Instantiate<BaseBullet>(this.bulletPrefab);
					baseBullet.transform.position = new Vector3((float)base.currentX, (float)base.currentY, 0f);
					baseBullet.shootDirection = Quaternion.Euler(0f, 0f, (float)(i - 3) * this.angle) * base.CurrentDirection.GetDircetionVector();
				}
				base.FreezeAction(0.5f);
			}
		}

		[Header("Minigun")]
		[SerializeField]
		protected float angle = 15f;
	}
}
