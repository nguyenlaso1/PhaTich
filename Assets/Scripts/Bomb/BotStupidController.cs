// @sonhg: class: Bomb.BotStupidController
using System;
using System.Collections;
using UnityEngine;

namespace Bomb
{
	public class BotStupidController : BaseCharactersController
	{
		protected override void Awake()
		{
			base.Awake();
			BotBuffSystem component = base.GetComponent<BotBuffSystem>();
			component.botController = this;
			this._buffSystem = component;
		}

		protected virtual void Start()
		{
			this.botAI = base.GetComponent<EnemyAStar>();
			base.transform.ChangeColorRecursive(this.monsterColor);
		}

		protected virtual void FixedUpdate()
		{
		}

		public override void Notify(GameObject collision)
		{
		}

		private IEnumerator WaitToChangeDirection()
		{
			this.canMove = true;
			yield return new WaitForSeconds(3f);
			this.canMove = false;
			this.botAI.initializePosition();
			yield break;
		}

		public bool IsAtPosition(int x, int y, float offset = 0.5f)
		{
			float num = (float)x - offset;
			float num2 = (float)x + offset;
			float num3 = (float)y - offset;
			float num4 = (float)y + offset;
			return num <= base.transform.position.x && base.transform.position.x <= num2 && num3 <= base.transform.position.y && base.transform.position.y <= num4;
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
					DieRequest.SendMessage(bombModel.userId, this.userID);
				}
			}
		}

		public void PlaceBomb()
		{
			if (this.bombPrefab != null && !this.mapController.gameObject.GetComponent<GameController>().endGamePanel.gameObject.activeSelf)
			{
				this.mapController.addedBomb = true;
				BombModel bombModel = this.mapController.PlaceBomb(this.bombPrefab, BombUserUtils.GetBombLength(base.GetComponent<BotStupidController>().userID), base.transform.position, false, this.userID, MMOUserUtils.GetBomb(MMOUserUtils.GetUserByID(this.userID)));
				if (bombModel != null)
				{
					BombRequest.SendMessage((int)bombModel.position.x, (int)bombModel.position.y, this.userID);
				}
			}
		}

		public override void RenderCharacter()
		{
			if (this.updateNow)
			{
				base.transform.position = new Vector2(this.px, this.py);
				this.updateNow = false;
			}
			if (!this.isDead)
			{
				if (this._buffSystem.IsPoisioned && !this.isDead)
				{
					base.animator.SetInteger("Status", 1);
				}
				else
				{
					base.animator.SetInteger("Status", 0);
				}
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

		[Header("StupidBot")]
		[SerializeField]
		public bool canMove;

		protected MoveDirection defaultDirect = MoveDirection.RIGHT;

		[SerializeField]
		public Color monsterColor;

		public int userID;

		public EnemyAStar botAI;

		public bool reverseDirection;

		public bool hasShield;

		private BotBuffSystem _buffSystem;

		public GameObject bombPrefab;

		public GameObject tombPrefab;

		public bool isDead;

		private GameObject tombObject;
	}
}
