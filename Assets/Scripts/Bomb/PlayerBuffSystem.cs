// @sonhg: class: Bomb.PlayerBuffSystem
using System;
using UnityEngine;

namespace Bomb
{
	public class PlayerBuffSystem : BaseBuffSystem
	{
		public override bool EatItem(ItemType type, int x, int y)
		{
			PickItemRequest.SendMessage(x, y, -1);
			return this.ApplyItem(type);
		}

		public override bool ApplyItem(ItemType type)
		{
			bool result = true;
			switch (type)
			{
			case ItemType.SLOW:
				this.slowTimer = 5f;
				this.player.bombScene.ParticlesController.PlayDebuffParticle(base.transform);
				break;
			default:
				if (type == ItemType.DEATH)
				{
					BombModel bombModel = new BombModel();
					bombModel.isMine = true;
					bombModel.userId = SmartFoxConnection.Connection.MySelf.Id;
					this.player.GetHit(bombModel);
					result = false;
				}
				break;
			case ItemType.SNARE:
				this.snareTimer = 2f;
				this.player.Move(MoveDirection.STAND);
				this.player.bombScene.ParticlesController.PlayDebuffParticle(base.transform);
				break;
			case ItemType.REVERSE:
				this.reverseTimer = 5f;
				this.player.bombScene.ParticlesController.PlayDebuffParticle(base.transform);
				break;
			case ItemType.FIREPOWER:
			case ItemType.BOMB:
			case ItemType.SHOES:
				this.player.bombScene.ParticlesController.PlayPickupParticle(base.transform);
				break;
			case ItemType.SHIELD:
				this.shieldTimer = 5f;
				this.player.hasShield = true;
				if (this.shieldParticle != null)
				{
					UnityEngine.Object.Destroy(this.shieldParticle);
				}
				this.shieldParticle = this.player.bombScene.ParticlesController.PlayShiledParticle(base.transform);
				break;
			case ItemType.GOLD:
				this.player.bombScene.ParticlesController.PlayGoldDropParticle(base.transform, this.amount);
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
			else
			{
				this.player.multiplyMoveSpeed = 1f;
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

		[HideInInspector]
		[Header("Player")]
		public PlayerController player;
	}
}
