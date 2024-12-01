// @sonhg: class: BaseCharactersController
using System;
using System.Collections.Generic;
using Bomb;
using UnityEngine;

public class BaseCharactersController : MonoBehaviour
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

	public float CurrentMoveSpeed
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

	protected virtual void Awake()
	{
		this.GenRayCast();
	}

	public virtual void Move(MoveDirection direction)
	{
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
			DrawRayCast[] array = new DrawRayCast[2];
			int num = 0;
			bool flag = false;
			foreach (DrawRayCast drawRayCast in this.rayCasts)
			{
				if (drawRayCast.direction == moveDirection)
				{
					array[num] = drawRayCast;
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
					this.Notify(array[0].TileHit.collider.gameObject);
				}
			}
		}
		if (this.isMoving)
		{
			Vector3 dircetionVector = this.GetDircetionVector(moveDirection);
			base.transform.Translate(dircetionVector * this.CurrentMoveSpeed * Time.smoothDeltaTime);
		}
		this.animator.SetInteger("State", (int)moveDirection);
	}

	public virtual void GetHit(BombModel bombModel)
	{
		UnityEngine.Debug.Log("Base Get Hit");
	}

	public virtual void PickUpItem(ItemType type, int amount)
	{
		UnityEngine.Debug.Log("Base PickUpItem");
	}

	public virtual void Notify(GameObject collision)
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

	private void GenRayCast()
	{
		BoxCollider2D component = base.GetComponent<BoxCollider2D>();
		this.rayCasts = new List<DrawRayCast>();
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

	private DrawRayCast GenRayCast(Vector3 position, MoveDirection direction, MoveDirection directionPredict)
	{
		DrawRayCast drawRayCast = new GameObject(string.Concat(new object[]
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
		}.AddComponent<DrawRayCast>();
		drawRayCast.actor = this;
		drawRayCast.transform.localPosition = position;
		drawRayCast.direction = direction;
		drawRayCast.directionPredict = directionPredict;
		drawRayCast.draw = true;
		return drawRayCast;
	}

	[Header("Base")]
	public float baseMoveSpeed = 3f;

	[Range(0f, 1f)]
	public float multiplyMoveSpeed = 1f;

	[HideInInspector]
	public BombGameScene bombScene;

	[HideInInspector]
	public bool isMoving;

	[HideInInspector]
	public bool updateNow;

	[HideInInspector]
	public MapController mapController;

	[HideInInspector]
	public float px;

	[HideInInspector]
	public float py;

	[HideInInspector]
	public float lastRenderTime;

	[HideInInspector]
	public bool CanMoveThrough;

	protected MoveDirection _currentDirection;

	private float oldX;

	private float oldY;

	protected List<DrawRayCast> rayCasts;

	private bool _isMine;

	private Animator _anim;
}
