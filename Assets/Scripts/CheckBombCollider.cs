// @sonhg: class: CheckBombCollider
using System;
using BombOffline;

public class CheckBombCollider : Routine
{
	public CheckBombCollider(Offline_BaseMonster actor)
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
		if (this.actor.CheckBombCollision())
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
