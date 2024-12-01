// @sonhg: class: Bomb.EnemyBuffSystem
using System;
using UnityEngine;

namespace Bomb
{
	public class EnemyBuffSystem : BaseBuffSystem
	{
		public override bool EatItem(ItemType type, int x, int y)
		{
			return this.ApplyItem(type);
		}

		public override bool ApplyItem(ItemType type)
		{
			bool result = true;
			switch (type)
			{
			case ItemType.SLOW:
				this.slowTimer = 5f;
				this.enemy.bombScene.ParticlesController.PlayDebuffParticle(base.transform);
				break;
			default:
				if (type == ItemType.DEATH)
				{
					result = false;
				}
				break;
			case ItemType.SNARE:
				this.snareTimer = 2f;
				this.enemy.Move(MoveDirection.STAND);
				this.enemy.bombScene.ParticlesController.PlayDebuffParticle(base.transform);
				break;
			case ItemType.REVERSE:
				this.reverseTimer = 5f;
				this.enemy.bombScene.ParticlesController.PlayDebuffParticle(base.transform);
				break;
			case ItemType.FIREPOWER:
			case ItemType.BOMB:
			case ItemType.SHOES:
				this.enemy.bombScene.ParticlesController.PlayPickupParticle(base.transform);
				break;
			case ItemType.SHIELD:
				this.shieldTimer = 5f;
				this.enemy.hasShield = true;
				if (this.shieldParticle != null)
				{
					UnityEngine.Object.Destroy(this.shieldParticle);
				}
				this.shieldParticle = this.enemy.bombScene.ParticlesController.PlayShiledParticle(base.transform);
				break;
			case ItemType.GOLD:
				this.enemy.bombScene.ParticlesController.PlayGoldDropParticle(base.transform, this.amount);
				break;
			}
			return result;
		}

		protected override void ApplySlow()
		{
			this.isSlow = true;
			this.slowTimer -= Time.deltaTime;
		}

		protected override void ApplySnare()
		{
			this.isSnare = true;
			this.snareTimer -= Time.deltaTime;
		}

		protected override void ApplyReverse()
		{
			this.isReverse = true;
			this.reverseTimer -= Time.deltaTime;
		}

		protected override void ApplyShield()
		{
			if (this.enemy.hasShield)
			{
				this.hasShield = true;
				this.shieldTimer -= Time.deltaTime;
			}
			else
			{
				this.shieldTimer = -1f;
				UnityEngine.Object.Destroy(this.shieldParticle);
			}
		}

		protected override void ApplyBuff()
		{
			if (this.isSnare)
			{
				this.enemy.multiplyMoveSpeed = 0f;
			}
			else if (this.isSlow)
			{
				this.enemy.multiplyMoveSpeed = 0.5f;
			}
			else
			{
				this.enemy.multiplyMoveSpeed = 1f;
			}
			if (this.hasShield)
			{
				this.enemy.hasShield = true;
			}
			else
			{
				this.enemy.hasShield = false;
				UnityEngine.Object.Destroy(this.shieldParticle);
			}
		}

		[Header("Enemy")]
		[HideInInspector]
		public EnemyController enemy;
	}
}
