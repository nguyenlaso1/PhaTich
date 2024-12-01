// @sonhg: class: SpecialAction
using System;
using BombOffline;

public class SpecialAction : Routine
{
	public SpecialAction(Offline_BaseMonster actor)
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
		if (base.IsRunning)
		{
			if (this.actor.isDead)
			{
				base.Fail();
				return;
			}
			this.DoSpecialAct();
		}
	}

	private void DoSpecialAct()
	{
		if (this.actor is ISpecialAction)
		{
			ISpecialAction specialAction = this.actor as ISpecialAction;
			if (specialAction.DoSpecialAction())
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
