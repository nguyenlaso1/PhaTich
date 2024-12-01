// @sonhg: class: DrawRayCast
using System;
using UnityEngine;

public class DrawRayCast : MonoBehaviour
{
	private void FixedUpdate()
	{
		if (this.actor.CurrentDirection == this.direction)
		{
			this.CheckCollision();
			if (this.draw)
			{
				UnityEngine.Debug.DrawLine(base.transform.position, base.transform.position + this.GetDircetionVector() * this.rayDistance, Color.red);
			}
		}
	}

	private Vector3 GetDircetionVector()
	{
		Vector3 result = Vector3.zero;
		switch (this.direction)
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
		}
		return result;
	}

	public bool CheckCollision()
	{
		int layerMask = 1 << LayerMask.NameToLayer("Bomb") | 1 << LayerMask.NameToLayer("Wall") | 1 << LayerMask.NameToLayer("BorderWall");
		RaycastHit2D[] array = Physics2D.RaycastAll(base.transform.position, this.GetDircetionVector(), this.rayDistance + 0.001f, layerMask);
		if (array != null && array.Length > 0)
		{
			this.TileHit = array[0];
			int num = 0;
			while (this.TileHit.collider.bounds.Contains(this.actor.transform.position))
			{
				num++;
				if (num >= array.Length)
				{
					this.hit = false;
					return this.hit;
				}
				this.TileHit = array[num];
			}
			if (this.TileHit.collider != null)
			{
				if (this.actor.CanMoveThrough && !this.TileHit.collider.CompareTag("BorderWall"))
				{
					this.hit = false;
					return this.hit;
				}
				this.hit = true;
				return this.hit;
			}
		}
		this.hit = false;
		return this.hit;
	}

	public MoveDirection direction = MoveDirection.RIGHT;

	public MoveDirection directionPredict = MoveDirection.UP;

	public RaycastHit2D TileHit;

	public bool hit;

	public bool draw;

	public BaseCharactersController actor;

	[SerializeField]
	private float rayDistance = 0.2f;
}
