// @sonhg: class: BombOffline.LavaSurger
using System;
using System.Collections.Generic;
using UnityEngine;

namespace BombOffline
{
	public class LavaSurger : IceBall
	{
		protected override void InitMonsterProperty()
		{
			this.shootCooldown = this.cooldown;
			this.OnDestroy = delegate(BaseBullet bullet, Transform hitObject)
			{
				List<Vector3> list = new List<Vector3>();
				list.Add(bullet.transform.position);
				Dictionary<string, bool> dictionary = new Dictionary<string, bool>();
				dictionary.Add("isDestroyTile", false);
				dictionary.Add("isDestroyItem", false);
				dictionary.Add("isHitMonster", false);
				dictionary.Add("isHitPlayer", false);
				this.board.TriggerCustomBomb(list, dictionary, null);
			};
			this.OnHitBomb = delegate(BaseBullet bullet, Transform hitObject)
			{
				BombModel bombModelAt = this.board.GetBombModelAt(hitObject.position);
				this.board.RemoveBomb(bombModelAt.position);
				UnityEngine.Object.Destroy(bombModelAt.bomb);
				this.board.player.DegreeBomb();
				this.board.Scene.ParticlesController.PlayPickupParticle(bullet.transform);
			};
		}

		protected override void Shoot()
		{
			if (this.CanShoot())
			{
				this.shootCooldown = this.cooldown;
				MusicManager.instance.PlayOneShot(this.shootSound, 1f);
				Vector3 dircetionVector = base.CurrentDirection.GetDircetionVector();
				Vector3 position = new Vector3((float)base.currentX, (float)base.currentY, 0f);
				BaseBullet baseBullet = UnityEngine.Object.Instantiate<BaseBullet>(this.bulletPrefab);
				baseBullet.transform.position = position;
				baseBullet.shootDirection = dircetionVector;
				baseBullet.OnHitBomb = this.OnHitBomb;
				base.FreezeAction(0.5f);
			}
		}

		protected BaseBullet.BulletCallbackDelegate OnDestroy;

		protected BaseBullet.BulletCallbackDelegate OnHitBomb;
	}
}
