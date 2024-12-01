// @sonhg: class: BombOffline.Offline_BaseMonster
using System;
using System.Collections;
using System.Collections.Generic;
using Sfs2X.Entities.Data;
using UnityEngine;
using UnityEngine.UI;

namespace BombOffline
{
	[RequireComponent(typeof(BoxCollider2D))]
	[RequireComponent(typeof(Rigidbody2D))]
	public abstract class Offline_BaseMonster : Offline_BaseCharactersController
	{
		public override float CurrentMoveSpeed
		{
			get
			{
				return base.CurrentMoveSpeed * 2f;
			}
		}

		public virtual bool IsAtPosition(int x, int y, float? offsetX = null, float? offsetY = null)
		{
			float num = (offsetX == null) ? 0.5f : offsetX.Value;
			float num2 = (offsetY == null) ? 0.5f : offsetY.Value;
			float num3 = base.transform.position.x - num;
			float num4 = base.transform.position.x + num;
			float num5 = base.transform.position.y - num2;
			float num6 = base.transform.position.y + num2;
			return num3 <= (float)x && (float)x <= num4 && num5 <= (float)y && (float)y <= num6;
		}

		public override bool Move(MoveDirection direction, bool isCheck = false)
		{
			return base.Move(direction, isCheck) && this.canMove;
		}

		public bool CheckMove(MoveDirection direction)
		{
			return this.Move(direction, true);
		}

		protected override void Notify(GameObject collision)
		{
			this.collisionObject = collision;
		}

		public void CalculateDirection(int destX, int destY)
		{
			if (base.currentX == destX)
			{
				if (destY > base.currentY)
				{
					base.CurrentDirection = MoveDirection.UP;
				}
				else
				{
					base.CurrentDirection = MoveDirection.DOWN;
				}
			}
			else if (destX > base.currentX)
			{
				base.CurrentDirection = MoveDirection.RIGHT;
			}
			else
			{
				base.CurrentDirection = MoveDirection.LEFT;
			}
		}

		public void ChangeDirection()
		{
			if (this.canMove)
			{
				List<MoveDirection> list = new List<MoveDirection>();
				List<MoveDirection> list2 = new List<MoveDirection>();
				foreach (Offline_DrawRayCast offline_DrawRayCast in this.rayCasts)
				{
					offline_DrawRayCast.CheckCollision();
					if (!offline_DrawRayCast.hit && !list.Contains(offline_DrawRayCast.direction))
					{
						list.Add(offline_DrawRayCast.direction);
						if (offline_DrawRayCast.direction != base.CurrentDirection && offline_DrawRayCast.direction != base.CurrentDirection.Reverse())
						{
							list2.Add(offline_DrawRayCast.direction);
						}
					}
				}
				if (list.Count == 0)
				{
					base.CurrentDirection = MoveDirection.STAND;
					return;
				}
				int index = UnityEngine.Random.Range(0, list.Count);
				int num = UnityEngine.Random.Range(0, list2.Count);
				base.CurrentDirection = list[index];
			}
		}

		public virtual bool GetHit(int x, int y)
		{
			if (!base.gameObject.activeInHierarchy)
			{
				return false;
			}
			this.health--;
			this.RenderHealthBar();
			return this.health == 0;
		}

		public virtual bool CheckBombCollision()
		{
			return this.collisionObject != null && this.collisionObject.CompareTag("Bomb");
		}

		public virtual IEnumerable<MapNode> GetMapRoute()
		{
			if (this.isChasePlayer)
			{
				return this.ReturnChasePlayerRoute();
			}
			return this.ReturnRandomRoute();
		}

		private void InitMonsterHealthBar()
		{
			if (this.slider != null)
			{
				if (this.health > 1)
				{
					this.slider.maxValue = (float)this.health;
					this.slider.minValue = 0f;
					this.slider.value = (float)this.health;
				}
				else
				{
					this.slider.transform.parent.gameObject.SetActive(false);
				}
			}
		}

		protected virtual void InitMonsterProperty()
		{
		}

		public virtual void ShowMonster()
		{
			base.gameObject.SetActive(true);
		}

		public virtual void DestroyMonster()
		{
			this.board.Scene.ParticlesController.PlayDeathParticle(base.transform, 0.5f);
			if (this.deathSound != null)
			{
				MusicManager.instance.PlaySingle(this.deathSound, 1f);
			}
			UnityEngine.Object.Destroy(base.gameObject);
		}

		protected abstract void CreateBrain();

		private void Start()
		{
			this.CreateBrain();
			this.InitMonsterHealthBar();
			this.InitMonsterProperty();
		}

		protected virtual void FixedUpdate()
		{
			this.isUpdateCycle = !this.isUpdateCycle;
			if (!this.isUpdateCycle)
			{
				return;
			}
			if (this.canAct)
			{
				if (this.brain.State == Routine.RoutineState.NotActive)
				{
					this.brain.Start();
				}
				else
				{
					this.brain.Act();
				}
			}
		}

		public virtual void SnapGrid()
		{
			base.transform.position = new Vector3((float)base.currentX, (float)base.currentY, 0f);
		}

		public void RandomCycle()
		{
			this.isUpdateCycle = (UnityEngine.Random.Range(0, 2) > 0);
		}

		protected void RenderHealthBar()
		{
			if (this.slider != null)
			{
				this.slider.value = (float)this.health;
			}
		}

		protected void FreezeAction(float time)
		{
			base.StartCoroutine(this.FreezeActionCoroutine(time));
		}

		private IEnumerator FreezeActionCoroutine(float time)
		{
			this.canAct = false;
			yield return new WaitForSeconds(time);
			this.canAct = true;
			yield break;
		}

		private IEnumerable<MapNode> ReturnRandomRoute()
		{
			int toX = 0;
			int toY = 0;
			this.board.GetRandomPositionAt(base.currentX, base.currentY, ref toX, ref toY);
			return this.board.SearchFromTo(base.currentX, base.currentY, toX, toY, null);
		}

		private IEnumerable<MapNode> ReturnChasePlayerRoute()
		{
			LinkedList<MapNode> linkedList = this.board.SearchFromToPlayer(base.currentX, base.currentY, this);
			if (linkedList == null)
			{
				return this.ReturnRandomRoute();
			}
			return linkedList;
		}

		[Header("Base Monster")]
		public string monsterName = "New Monster";

		public bool isDead;

		public int health = 1;

		[HideInInspector]
		public Offline_GameController board;

		[HideInInspector]
		public bool isUpdateCycle = true;

		[HideInInspector]
		public Offline_BaseCharactersController target;

		protected GameObject collisionObject;

		[SerializeField]
		private Slider slider;

		[SerializeField]
		protected bool canMove = true;

		[SerializeField]
		protected bool canEatItem = true;

		[SerializeField]
		protected bool isChasePlayer;

		[SerializeField]
		protected AudioClip deathSound;

		protected Routine brain;

		public int Point;

		public bool IsBoss;

		public ISFSObject Drop;
	}
}
