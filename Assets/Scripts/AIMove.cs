// @sonhg: class: AIMove
using System;
using BombOffline;

public class AIMove : Routine
{
	public AIMove(Offline_BaseMonster actor)
	{
		this.actor = actor;
	}

	public override void Start()
	{
		base.Start();
	}

	public override void Reset()
	{
		this.Start();
		this.actor.ChangeDirection();
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
		if (!this.actor.Move(this.actor.CurrentDirection, false))
		{
			base.Succeed();
		}
	}

	protected Offline_BaseMonster actor;
}
