// @sonhg: class: Bomb.PlayerController
using System;
using UnityEngine;

namespace Bomb
{
	[RequireComponent(typeof(PlayerBuffSystem))]
	public class PlayerController : BaseCharactersController, IPlaceBomb
	{
		protected override void Awake()
		{
			base.Awake();
			PlayerBuffSystem component = base.GetComponent<PlayerBuffSystem>();
			component.player = this;
			this._buffSystem = component;
		}

		public void PlaceBomb()
		{
			if (this.bombPrefab != null && !this.isDead && this.canPlaceBomb && this.mapController.MyBombCount() < this.TotalBomb())
			{
				BombModel bombModel = this.mapController.PlaceBomb(this.bombPrefab, BombUserUtils.GetCurrentBombLength(), base.transform.position, true, SmartFoxConnection.Connection.MySelf.Id, MMOUserUtils.GetBomb(SmartFoxConnection.Connection.MySelf));
				if (bombModel != null)
				{
					BombRequest.SendMessage((int)bombModel.position.x, (int)bombModel.position.y, -1);
				}
			}
		}

		public override void Move(MoveDirection direction)
		{
			MoveDirection currentDirection = this._currentDirection;
			base.Move((!this.reverseDirection) ? direction : this.GetReverseDirection(direction));
			if (currentDirection != this._currentDirection)
			{
				Vector3 position = this.ValidateEndPosition(base.transform.position);
				MoveRequest.SendMessage(position.x, position.y, this._currentDirection);
				base.transform.position = position;
			}
		}

		public override void GetHit(BombModel bombModel)
		{
			if (!this.isDead)
			{
				if (this.hasShield)
				{
					this.hasShield = false;
				}
				else
				{
					this.isDead = true;
					this._buffSystem.canEatItem = false;
					this.CanMoveThrough = true;
					base.animator.SetInteger("Status", 2);
					this.tombObject = (UnityEngine.Object.Instantiate(this.tombPrefab, base.transform.position, Quaternion.identity) as GameObject);
					DieRequest.SendMessage(bombModel.userId, -1);
				}
			}
		}

		public override void RenderCharacter()
		{
			if (!this.isDead)
			{
				if (this._buffSystem.IsPoisioned)
				{
					base.animator.SetInteger("Status", 1);
				}
				else
				{
					base.animator.SetInteger("Status", 0);
				}
			}
		}

		public override void ResetCharacter()
		{
			base.ResetCharacter();
			this.isDead = false;
			this._buffSystem.canEatItem = true;
			this.CanMoveThrough = false;
			base.animator.SetInteger("Status", 0);
			if (this.tombObject != null)
			{
				UnityEngine.Object.Destroy(this.tombObject);
				this.tombObject = null;
			}
		}

		public override void PickUpItem(ItemType type, int amount)
		{
			UnityEngine.Debug.Log(string.Concat(new object[]
			{
				"<color=red>Player PickUpItem:</color>",
				type.ToString(),
				" amount:",
				amount
			}));
			this._buffSystem.amount = amount;
			this._buffSystem.ApplyItem(type);
		}

		private Vector3 ValidateEndPosition(Vector3 position)
		{
			string[][] gameMap = this.bombScene.MapController.gameMap;
			int num = Mathf.RoundToInt(position.x);
			int num2 = Mathf.RoundToInt(position.y);
			bool flag = false;
			if (num < 0)
			{
				num = 0;
				flag = true;
			}
			if (num > gameMap[0].Length - 1)
			{
				num = gameMap[0].Length - 1;
				flag = true;
			}
			if (num2 < 0)
			{
				num2 = 0;
				flag = true;
			}
			if (num2 > gameMap.Length - 1)
			{
				num2 = gameMap.Length - 1;
				flag = true;
			}
			if (flag)
			{
				return new Vector2((float)num, (float)num2);
			}
			return position;
		}

		private int TotalBomb()
		{
			return BombUserUtils.GetTotalBomb();
		}

		private MoveDirection GetReverseDirection(MoveDirection direction)
		{
			switch (direction)
			{
			case MoveDirection.RIGHT:
				return MoveDirection.LEFT;
			case MoveDirection.LEFT:
				return MoveDirection.RIGHT;
			case MoveDirection.DOWN:
				return MoveDirection.UP;
			case MoveDirection.UP:
				return MoveDirection.DOWN;
			default:
				return direction;
			}
		}

		[Header("Player")]
		public GameObject bombPrefab;

		public GameObject tombPrefab;

		public bool reverseDirection;

		public bool hasShield;

		[SerializeField]
		private bool canPlaceBomb = true;

		[SerializeField]
		private bool isDead;

		private GameObject tombObject;

		private PlayerBuffSystem _buffSystem;
	}
}
