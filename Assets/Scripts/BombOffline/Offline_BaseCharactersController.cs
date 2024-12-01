// @sonhg: class: BombOffline.Offline_BaseCharactersController
using System;
using System.Collections.Generic;
using UnityEngine;

namespace BombOffline
{
	public class Offline_BaseCharactersController : MonoBehaviour
	{
		public MoveDirection CurrentDirection
		{
			get
			{
				return this._currentDirection;
			}
			set
			{
				this._currentDirection = value;
			}
		}

		public virtual float CurrentMoveSpeed
		{
			get
			{
				return this.baseMoveSpeed * this.multiplyMoveSpeed;
			}
		}

		public bool isMine
		{
			get
			{
				return this._isMine;
			}
			set
			{
				this._isMine = value;
			}
		}

		public Vector2 position
		{
			get
			{
				return new Vector2(base.transform.position.x, -base.transform.position.y);
			}
			set
			{
				base.transform.position = new Vector3(value.x, value.y, base.transform.position.z);
			}
		}

		protected Animator animator
		{
			get
			{
				if (this._anim == null)
				{
					this._anim = base.GetComponent<Animator>();
				}
				return this._anim;
			}
		}

		public int currentX
		{
			get
			{
				float num = base.transform.position.x;
				int num2 = Mathf.FloorToInt(num);
				num -= (float)num2;
				if (num > 0.5f)
				{
					return num2 + 1;
				}
				if (num < 0f)
				{
					return num2 - 1;
				}
				return num2;
			}
		}

		public int currentY
		{
			get
			{
				float num = base.transform.position.y;
				int num2 = Mathf.FloorToInt(num);
				num -= (float)num2;
				if (num > 0.5f)
				{
					return num2 + 1;
				}
				if (num < 0f)
				{
					return num2 - 1;
				}
				return num2;
			}
		}

		protected virtual void Awake()
		{
			this.GenRayCast();
		}

		public virtual bool Move(MoveDirection direction, bool isCheck = false)
		{
			MoveDirection currentDirection = this._currentDirection;
			if (this._currentDirection != MoveDirection.STAND)
			{
				this.faceDirection = this._currentDirection;
			}
			if (!this.canAct)
			{
				return false;
			}
			this.isMoving = true;
			this._currentDirection = direction;
			MoveDirection moveDirection = direction;
			if (moveDirection != MoveDirection.STAND)
			{
				this.oldX = base.transform.localPosition.x;
				this.oldY = base.transform.localPosition.y;
			}
			else
			{
				this.isMoving = false;
			}
			if (this.rayCasts != null)
			{
				Offline_DrawRayCast[] array = new Offline_DrawRayCast[2];
				int num = 0;
				bool flag = false;
				foreach (Offline_DrawRayCast offline_DrawRayCast in this.rayCasts)
				{
					if (offline_DrawRayCast.direction == moveDirection)
					{
						array[num] = offline_DrawRayCast;
						num++;
						flag = true;
					}
				}
				if (flag)
				{
					if (!array[0].hit && array[1].hit)
					{
						moveDirection = array[0].directionPredict;
					}
					if (array[0].hit && !array[1].hit)
					{
						moveDirection = array[1].directionPredict;
					}
					if (array[0].hit && array[1].hit)
					{
						this.isMoving = false;
						if (array[0].TileHit.collider != null)
						{
							this.Notify(array[0].TileHit.collider.gameObject);
						}
					}
				}
			}
			if (this.isMoving && !isCheck)
			{
				Vector3 dircetionVector = this.GetDircetionVector(moveDirection);
				base.transform.Translate(dircetionVector * this.CurrentMoveSpeed * Time.smoothDeltaTime);
			}
			this.animator.SetInteger("State", (int)moveDirection);
			return this.isMoving;
		}

		public virtual void GetHit(BombModel bombModel)
		{
			UnityEngine.Debug.Log("Base Get Hit");
		}

		public virtual void PickUpItem(ItemType type)
		{
			UnityEngine.Debug.Log("Base PickUpItem");
		}

		protected virtual void Notify(GameObject collision)
		{
		}

		protected Vector3 GetDircetionVector(MoveDirection direction)
		{
			Vector3 result;
			switch (direction)
			{
			case MoveDirection.RIGHT:
				result = Vector3.right;
				break;
			case MoveDirection.LEFT:
				result = Vector3.left;
				break;
			case MoveDirection.DOWN:
				result = Vector3.down;
				break;
			case MoveDirection.UP:
				result = Vector3.up;
				break;
			default:
				result = Vector3.zero;
				break;
			}
			return result;
		}

		public virtual void RenderCharacter()
		{
		}

		public virtual void ResetCharacter()
		{
			this.animator.SetInteger("State", 1);
			this.animator.Play("Idle_Down");
		}

		public virtual void OnSpecial1AnimationExit()
		{
		}

		private void GenRayCast()
		{
			BoxCollider2D component = base.GetComponent<BoxCollider2D>();
			this.rayCasts = new List<Offline_DrawRayCast>();
			Vector3 position = new Vector3(component.bounds.extents.x, component.bounds.extents.y, 0f) + base.transform.position;
			position = base.transform.InverseTransformPoint(position);
			this.rayCasts.Add(this.GenRayCast(position, MoveDirection.UP, MoveDirection.RIGHT));
			this.rayCasts.Add(this.GenRayCast(position, MoveDirection.RIGHT, MoveDirection.UP));
			Vector3 position2 = new Vector3(-component.bounds.extents.x, component.bounds.extents.y, 0f) + base.transform.position;
			position2 = base.transform.InverseTransformPoint(position2);
			this.rayCasts.Add(this.GenRayCast(position2, MoveDirection.UP, MoveDirection.LEFT));
			this.rayCasts.Add(this.GenRayCast(position2, MoveDirection.LEFT, MoveDirection.UP));
			Vector3 position3 = new Vector3(component.bounds.extents.x, -component.bounds.extents.y, 0f) + base.transform.position;
			position3 = base.transform.InverseTransformPoint(position3);
			this.rayCasts.Add(this.GenRayCast(position3, MoveDirection.RIGHT, MoveDirection.DOWN));
			this.rayCasts.Add(this.GenRayCast(position3, MoveDirection.DOWN, MoveDirection.RIGHT));
			Vector3 position4 = new Vector3(-component.bounds.extents.x, -component.bounds.extents.y, 0f) + base.transform.position;
			position4 = base.transform.InverseTransformPoint(position4);
			this.rayCasts.Add(this.GenRayCast(position4, MoveDirection.LEFT, MoveDirection.DOWN));
			this.rayCasts.Add(this.GenRayCast(position4, MoveDirection.DOWN, MoveDirection.LEFT));
		}

		private Offline_DrawRayCast GenRayCast(Vector3 position, MoveDirection direction, MoveDirection directionPredict)
		{
			Offline_DrawRayCast offline_DrawRayCast = new GameObject(string.Concat(new object[]
			{
				"Raycast",
				(int)direction,
				string.Empty,
				(int)directionPredict
			}))
			{
				transform = 
				{
					parent = base.transform
				}
			}.AddComponent<Offline_DrawRayCast>();
			offline_DrawRayCast.actor = this;
			offline_DrawRayCast.transform.localPosition = position;
			offline_DrawRayCast.direction = direction;
			offline_DrawRayCast.directionPredict = directionPredict;
			offline_DrawRayCast.draw = true;
			return offline_DrawRayCast;
		}

		[Header("Base")]
		public float baseMoveSpeed = 3f;

		[Range(0f, 1f)]
		public float multiplyMoveSpeed = 1f;

		[HideInInspector]
		public bool isMoving;

		[HideInInspector]
		public bool updateNow;

		[HideInInspector]
		public bool canAct = true;

		[HideInInspector]
		public Offline_MapController mapController;

		[HideInInspector]
		public float px;

		[HideInInspector]
		public float py;

		[HideInInspector]
		public float lastRenderTime;

		[HideInInspector]
		public MoveDirection faceDirection = MoveDirection.DOWN;

		public bool enableAllRayCast;

		public GameObject arrowObject;

		public bool CanMoveThrough;

		protected MoveDirection _currentDirection;

		private float oldX;

		private float oldY;

		protected List<Offline_DrawRayCast> rayCasts;

		private bool _isMine;

		private Animator _anim;
	}
}
