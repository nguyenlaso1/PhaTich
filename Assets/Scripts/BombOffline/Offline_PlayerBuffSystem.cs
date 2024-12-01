// @sonhg: class: BombOffline.Offline_PlayerBuffSystem
using System;
using UnityEngine;

namespace BombOffline
{
	public class Offline_PlayerBuffSystem : Offline_BaseBuffSystem
	{
		public override bool EatItem(ItemType type, int value, int x, int y)
		{
			return this.ApplyItem(type, value);
		}

		public override bool ApplyItem(ItemType type, int value = 1)
		{
			bool result = true;
			switch (type)
			{
			case ItemType.SLOW:
				this.slowTimer = 6f;
				this.player.bombScene.ParticlesController.PlaySlowParticle(base.transform, new float?(this.slowTimer));
				break;
			case ItemType.SLOW2:
				this.slowTimer = 1f;
				this.player.bombScene.ParticlesController.PlayDebuffParticle(base.transform);
				break;
			case ItemType.SNARE:
				this.snareTimer = 1.5f;
				this.player.Move(MoveDirection.STAND, false);
				this.player.bombScene.ParticlesController.PlayRootParticle(base.transform, new float?(this.snareTimer));
				break;
			case ItemType.REVERSE:
				this.reverseTimer = 3f;
				this.player.bombScene.ParticlesController.PlayDebuffParticle(base.transform);
				this.player.bombScene.ParticlesController.PlayReverseParticle(base.transform, new float?(this.reverseTimer));
				break;
			case ItemType.FIREPOWER:
				this.player.IncreaseBombLegth();
				this.player.bombScene.ParticlesController.PlayPickupParticle(base.transform);
				break;
			case ItemType.BOMB:
				this.player.IncreaseTotalBomb();
				this.player.bombScene.ParticlesController.PlayPickupParticle(base.transform);
				break;
			case ItemType.SHOES:
				this.player.IncreaseBaseMoveSpeed();
				this.player.bombScene.ParticlesController.PlayPickupParticle(base.transform);
				break;
			case ItemType.SHIELD:
				this.shieldTimer = 10f;
				this.player.hasShield = true;
				if (this.shieldParticle != null)
				{
					UnityEngine.Object.Destroy(this.shieldParticle);
				}
				this.shieldParticle = this.player.bombScene.ParticlesController.PlayShiledParticle(base.transform);
				break;
			case ItemType.GOLD:
				this.player.bombScene.ParticlesController.PlayPickupParticle(base.transform);
				this.player.UpdateGold(value);
				break;
			case ItemType.STOP_TIME:
				this.player.bombScene.GameController.StopAllMonster();
				break;
			case ItemType.ADD_TIME:
				this.player.bombScene.ParticlesController.PlayPickupParticle(base.transform);
				this.player.bombScene.GameController.AddClockTime(10f);
				break;
			case ItemType.FIRE_MAXIMUM:
				this.player.CurrentBombLength = 100;
				break;
			case ItemType.CHEST:
				this.ApplyItem(this.player.GetRandomItem(), 1);
				break;
			case ItemType.HEALTH:
				this.player.bombScene.ParticlesController.PlayPickupParticle(base.transform);
				this.player.TotalHeart++;
				break;
			case ItemType.HASTE:
				this.hasteTimer = 4f;
				this.player.bombScene.ParticlesController.PlayPickupParticle(base.transform);
				this.player.bombScene.ParticlesController.PlayBoostSpeedParticle(base.transform);
				break;
			case ItemType.AUTO_FIRE:
				this.autoFireTimer = 5f;
				this.player.bombScene.ParticlesController.PlayAutoFireParticle(base.transform, new float?(this.autoFireTimer));
				this.player.bombScene.ParticlesController.PlayPickupParticle(base.transform);
				break;
			case ItemType.DEATH:
			{
				BombModel bombModel = new BombModel();
				bombModel.isMine = true;
				this.player.GetHit(bombModel);
				result = false;
				break;
			}
			case ItemType.RADAR:
				this.player.bombScene.ParticlesController.PlayPickupParticle(base.transform);
				this.player.mapController.ItemVisible();
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
			if (this.player.hasShield)
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

		protected override void ApplyAutoFire()
		{
			this.isAutoFire = true;
			this.autoFireTimer -= Time.deltaTime;
		}

		protected override void ApplyHaste()
		{
			this.isHaste = true;
			this.hasteTimer -= Time.deltaTime;
		}

		protected override void ApplyBuff()
		{
			if (this.isSnare)
			{
				this.player.multiplyMoveSpeed = 0f;
			}
			else if (this.isSlow)
			{
				this.player.multiplyMoveSpeed = 0.5f;
			}
			else if (this.isHaste)
			{
				this.player.multiplyMoveSpeed = 3f / this.player.baseMoveSpeed;
			}
			else
			{
				this.player.multiplyMoveSpeed = 1f;
			}
			if (this.isAutoFire)
			{
				this.player.PlaceBomb();
			}
			if (this.isReverse)
			{
				this.player.reverseDirection = true;
			}
			else
			{
				this.player.reverseDirection = false;
			}
			if (this.hasShield)
			{
				this.player.hasShield = true;
			}
			else
			{
				this.player.hasShield = false;
				UnityEngine.Object.Destroy(this.shieldParticle);
			}
		}

		[Header("Player")]
		[HideInInspector]
		public Offline_PlayerController player;
	}
}
