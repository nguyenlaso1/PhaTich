// @sonhg: class: Bomb.BotBuffSystem
using System;
using UnityEngine;

namespace Bomb
{
	public class BotBuffSystem : BaseBuffSystem
	{
		public override bool EatItem(ItemType type, int x, int y)
		{
			PickItemRequest.SendMessage(x, y, this.botController.userID);
			return this.ApplyItem(type);
		}

		public override bool ApplyItem(ItemType type)
		{
			bool result = true;
			switch (type)
			{
			case ItemType.SLOW:
				this.slowTimer = 5f;
				this.botController.bombScene.ParticlesController.PlayDebuffParticle(base.transform);
				break;
			default:
				if (type == ItemType.DEATH)
				{
					BombModel bombModel = new BombModel();
					bombModel.isMine = true;
					bombModel.userId = SmartFoxConnection.Connection.MySelf.Id;
					this.botController.GetHit(bombModel);
					result = false;
				}
				break;
			case ItemType.SNARE:
				this.snareTimer = 2f;
				this.botController.Move(MoveDirection.STAND);
				this.botController.bombScene.ParticlesController.PlayDebuffParticle(base.transform);
				break;
			case ItemType.REVERSE:
				this.reverseTimer = 5f;
				this.botController.bombScene.ParticlesController.PlayDebuffParticle(base.transform);
				break;
			case ItemType.FIREPOWER:
			case ItemType.BOMB:
			case ItemType.SHOES:
				this.botController.bombScene.ParticlesController.PlayPickupParticle(base.transform);
				break;
			case ItemType.SHIELD:
				this.shieldTimer = 5f;
				this.botController.hasShield = true;
				if (this.shieldParticle != null)
				{
					UnityEngine.Object.Destroy(this.shieldParticle);
				}
				this.shieldParticle = this.botController.bombScene.ParticlesController.PlayShiledParticle(base.transform);
				break;
			case ItemType.GOLD:
				this.botController.bombScene.ParticlesController.PlayGoldDropParticle(base.transform, this.amount);
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
			if (this.botController.hasShield)
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
				this.botController.multiplyMoveSpeed = 0f;
			}
			else if (this.isSlow)
			{
				this.botController.multiplyMoveSpeed = 0.5f;
			}
			else
			{
				this.botController.multiplyMoveSpeed = 1f;
			}
			if (this.isReverse)
			{
				this.botController.reverseDirection = true;
			}
			else
			{
				this.botController.reverseDirection = false;
			}
			if (this.hasShield)
			{
				this.botController.hasShield = true;
			}
			else
			{
				this.botController.hasShield = false;
				UnityEngine.Object.Destroy(this.shieldParticle);
			}
		}

		[Header("Player")]
		[HideInInspector]
		public BotStupidController botController;
	}
}
