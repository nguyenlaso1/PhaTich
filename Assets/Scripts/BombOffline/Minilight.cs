// @sonhg: class: BombOffline.Minilight
using System;
using UnityEngine;

namespace BombOffline
{
	public class Minilight : IceBall
	{
		protected override void InitMonsterProperty()
		{
			base.InitMonsterProperty();
			this.shootCooldown += (float)UnityEngine.Random.Range(0, 3);
			this.OnHitPlayer = delegate(BaseBullet bullet, Transform hitObject)
			{
				this.board.player.PickUpItem(ItemType.SLOW2);
			};
			this.OnDestroy = delegate(BaseBullet bullet, Transform hitObject)
			{
				Vector3 position = new Vector3((float)bullet.currentX, (float)bullet.currentY, 0f);
				this.board.Scene.MapController.DrawFireManual(position, null);
				foreach (Vector3 shootDirection in this.shootDirection)
				{
					BaseBullet baseBullet = UnityEngine.Object.Instantiate<BaseBullet>(this.bulletPrefab);
					baseBullet.transform.position = position;
					baseBullet.shootDirection = shootDirection;
					baseBullet.OnHitPlayer = this.OnHitPlayer;
				}
			};
		}

		protected override void Shoot()
		{
			if (this.CanShoot())
			{
				this.shootCooldown = this.cooldown;
				MusicManager.instance.PlayOneShot(this.shootSound, 1f);
				BaseBullet baseBullet = UnityEngine.Object.Instantiate<BaseBullet>(this.bulletPrefab);
				baseBullet.transform.position = new Vector3((float)base.currentX, (float)base.currentY, 0f);
				baseBullet.transform.localScale = Vector3.one * 1.5f;
				baseBullet.shootDirection = base.CurrentDirection.GetDircetionVector();
				baseBullet.OnDestroyBullet = this.OnDestroy;
				baseBullet.OnHitPlayer = this.OnHitPlayer;
				base.FreezeAction(0.5f);
			}
		}

		protected BaseBullet.BulletCallbackDelegate OnDestroy;

		protected BaseBullet.BulletCallbackDelegate OnHitPlayer;
	}
}
