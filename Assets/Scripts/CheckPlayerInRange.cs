// @sonhg: class: CheckPlayerInRange
using System;
using BombOffline;

public class CheckPlayerInRange : Routine
{
	public CheckPlayerInRange(Offline_BaseMonster actor)
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
		if (this.actor is IAlert)
		{
			IAlert alert = this.actor as IAlert;
			bool flag = alert.IsPlayerInRange();
			if (flag)
			{
				base.Succeed();
			}
			else
			{
				base.Fail();
			}
		}
		else
		{
			base.Fail();
		}
	}

	protected Offline_BaseMonster actor;
}
