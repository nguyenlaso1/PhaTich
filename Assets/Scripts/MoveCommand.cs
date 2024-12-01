// @sonhg: class: MoveCommand
using System;
using UnityEngine;

public class MoveCommand : BaseInputCommand
{
	public MoveCommand(float xAxis, float yAxis)
	{
		if (xAxis == 0f && yAxis == 0f)
		{
			this._currentDirection = MoveDirection.STAND;
		}
		else
		{
			float value = Mathf.Atan2(xAxis, yAxis) * 57.29578f;
			if (this.BetweenValue(value, -45f, 45f))
			{
				this._currentDirection = MoveDirection.RIGHT;
			}
			else if (this.BetweenValue(value, 45f, 135f))
			{
				this._currentDirection = MoveDirection.UP;
			}
			else if (this.BetweenValue(value, -135f, -45f))
			{
				this._currentDirection = MoveDirection.DOWN;
			}
			else
			{
				this._currentDirection = MoveDirection.LEFT;
			}
		}
	}

	public MoveCommand(MoveDirection direction)
	{
		this._currentDirection = direction;
	}

	public override void Execute(BaseCharactersController actor)
	{
		actor.Move(this._currentDirection);
	}

	private bool BetweenValue(float value, float a, float b)
	{
		return a < value && value < b;
	}

	private MoveDirection _currentDirection;
}
