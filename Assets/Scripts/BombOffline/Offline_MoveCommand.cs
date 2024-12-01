// @sonhg: class: BombOffline.Offline_MoveCommand
using System;
using UnityEngine;

namespace BombOffline
{
	public class Offline_MoveCommand
	{
		public Offline_MoveCommand(float xAxis, float yAxis, Offline_BaseCharactersController actor, bool isDynamic)
		{
			if (xAxis == 0f && yAxis == 0f)
			{
				this._currentDirection = MoveDirection.STAND;
				actor.arrowObject.SetActive(false);
			}
			else
			{
				float num = Mathf.Atan2(xAxis, yAxis) * 57.29578f;
				actor.arrowObject.SetActive(true);
				actor.arrowObject.transform.localEulerAngles = new Vector3(actor.arrowObject.transform.localRotation.x, actor.arrowObject.transform.localRotation.y, num - 90f);
				if (isDynamic)
				{
					if (this.BetweenValue(num, -15f, 15f))
					{
						this._currentDirection = MoveDirection.RIGHT;
					}
					else if (this.BetweenValue(num, 75f, 105f))
					{
						this._currentDirection = MoveDirection.UP;
					}
					else if (this.BetweenValue(num, -105f, -75f))
					{
						this._currentDirection = MoveDirection.DOWN;
					}
					else if (this.BetweenValue(num, 165f, 180f))
					{
						this._currentDirection = MoveDirection.LEFT;
					}
					else if (this.BetweenValue(num, -180f, -165f))
					{
						this._currentDirection = MoveDirection.LEFT;
					}
					else if (this.BetweenValue(num, 15f, 45f))
					{
						if (actor.Move(MoveDirection.RIGHT, true))
						{
							this._currentDirection = MoveDirection.RIGHT;
						}
						else
						{
							this._currentDirection = MoveDirection.UP;
						}
					}
					else if (this.BetweenValue(num, 45f, 75f))
					{
						if (actor.Move(MoveDirection.UP, true))
						{
							this._currentDirection = MoveDirection.UP;
						}
						else
						{
							this._currentDirection = MoveDirection.RIGHT;
						}
					}
					else if (this.BetweenValue(num, 105f, 135f))
					{
						if (actor.Move(MoveDirection.UP, true))
						{
							this._currentDirection = MoveDirection.UP;
						}
						else
						{
							this._currentDirection = MoveDirection.LEFT;
						}
					}
					else if (this.BetweenValue(num, 135f, 165f))
					{
						if (actor.Move(MoveDirection.LEFT, true))
						{
							this._currentDirection = MoveDirection.LEFT;
						}
						else
						{
							this._currentDirection = MoveDirection.UP;
						}
					}
					else if (this.BetweenValue(num, -75f, -45f))
					{
						if (actor.Move(MoveDirection.DOWN, true))
						{
							this._currentDirection = MoveDirection.DOWN;
						}
						else
						{
							this._currentDirection = MoveDirection.RIGHT;
						}
					}
					else if (this.BetweenValue(num, -45f, -15f))
					{
						if (actor.Move(MoveDirection.RIGHT, true))
						{
							this._currentDirection = MoveDirection.RIGHT;
						}
						else
						{
							this._currentDirection = MoveDirection.DOWN;
						}
					}
					else if (this.BetweenValue(num, -135f, -105f))
					{
						if (actor.Move(MoveDirection.DOWN, true))
						{
							this._currentDirection = MoveDirection.DOWN;
						}
						else
						{
							this._currentDirection = MoveDirection.LEFT;
						}
					}
					else if (actor.Move(MoveDirection.LEFT, true))
					{
						this._currentDirection = MoveDirection.LEFT;
					}
					else
					{
						this._currentDirection = MoveDirection.DOWN;
					}
				}
				else if (this.BetweenValue(num, -45f, 45f))
				{
					this._currentDirection = MoveDirection.RIGHT;
				}
				else if (this.BetweenValue(num, 45f, 135f))
				{
					this._currentDirection = MoveDirection.UP;
				}
				else if (this.BetweenValue(num, -135f, -45f))
				{
					this._currentDirection = MoveDirection.DOWN;
				}
				else
				{
					this._currentDirection = MoveDirection.LEFT;
				}
			}
		}

		public Offline_MoveCommand(MoveDirection direction)
		{
			this._currentDirection = direction;
		}

		public void Execute(Offline_BaseCharactersController actor)
		{
			actor.Move(this._currentDirection, false);
		}

		private bool BetweenValue(float value, float a, float b)
		{
			return a < value && value <= b;
		}

		private MoveDirection _currentDirection;
	}
}
