// @sonhg: class: Repeat
using System;

public class Repeat : Routine
{
	public Repeat(Routine routine, int times = -1)
	{
		if (times < -1)
		{
			times = -1;
		}
		this.routine = routine;
		this.times = times;
		this.originalTimes = times;
	}

	public override void Start()
	{
		base.Start();
		this.routine.Start();
	}

	public override void Reset()
	{
		this.times = this.originalTimes;
		this.routine.Reset();
	}

	public override void Act()
	{
		if (this.routine.IsFailure)
		{
			this.routine.Reset();
			this.routine.Start();
		}
		else if (this.routine.IsSuccess)
		{
			if (this.times == 0)
			{
				base.Succeed();
				return;
			}
			if (this.times > 0 || this.times <= -1)
			{
				this.times--;
				this.routine.Reset();
				this.routine.Start();
			}
		}
		if (this.routine.IsRunning)
		{
			this.routine.Act();
		}
	}

	private Routine routine;

	private int times;

	private int originalTimes;
}
