// @sonhg: class: Succeeder
using System;

public class Succeeder : Routine
{
	public Succeeder(Routine routine)
	{
		this.routine = routine;
	}

	public override void Start()
	{
		base.Start();
		this.routine.Start();
	}

	public override void Reset()
	{
		this.Start();
	}

	public override void Act()
	{
		if (this.routine.IsFailure)
		{
			base.Succeed();
		}
		else if (this.routine.IsSuccess)
		{
			base.Succeed();
		}
		if (this.routine.IsRunning)
		{
			this.routine.Act();
		}
	}

	private Routine routine;
}
