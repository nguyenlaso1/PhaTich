// @sonhg: class: CheckCanMove
using System;
using BombOffline;

public class CheckCanMove : Routine
{
	public CheckCanMove(Offline_BaseMonster actor)
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
	}

	public override void Act()
	{
		if (this.actor.CheckMove(this.actor.CurrentDirection))
		{
			base.Succeed();
		}
		else
		{
			base.Fail();
		}
	}

	protected Offline_BaseMonster actor;
}
