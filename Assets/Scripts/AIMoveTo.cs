// @sonhg: class: AIMoveTo
using System;
using BombOffline;

public class AIMoveTo : Routine
{
	public AIMoveTo(Offline_BaseMonster actor, int x, int y)
	{
		this.actor = actor;
		this.destX = x;
		this.destY = y;
	}

	public override void Start()
	{
		base.Start();
		if (!this.isAtDestination())
		{
			this.actor.CalculateDirection(this.destX, this.destY);
		}
	}

	public override void Reset()
	{
		this.Start();
	}

	public override void Act()
	{
		if (base.IsRunning)
		{
			if (this.actor.isDead)
			{
				base.Fail();
				return;
			}
			this.Move();
		}
	}

	protected void Move()
	{
		if (!this.isAtDestination() && !this.actor.Move(this.actor.CurrentDirection, false))
		{
			base.Fail();
		}
		if (this.isAtDestination())
		{
			base.Succeed();
		}
	}

	protected bool isAtDestination()
	{
		return this.actor.IsAtPosition(this.destX, this.destY, new float?(0.1f), new float?(0.1f));
	}

	protected int destX;

	protected int destY;

	protected Offline_BaseMonster actor;
}
