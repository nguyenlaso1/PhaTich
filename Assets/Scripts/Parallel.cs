// @sonhg: class: Parallel
using System;
using System.Collections.Generic;

public class Parallel : Routine
{
	public Parallel(int successTimes, int failTimes, params Routine[] routines)
	{
		this.routines = new List<Routine>(routines);
		this.successTimes = successTimes;
		this.failTimes = failTimes;
	}

	public override void Start()
	{
		base.Start();
		this.routines.ForEach(delegate(Routine routine)
		{
			routine.Start();
		});
	}

	public override void Reset()
	{
		this.routines.ForEach(delegate(Routine routine)
		{
			routine.Reset();
		});
	}

	public override void Act()
	{
		if (this.AmountOfSucceed >= this.successTimes)
		{
			base.Succeed();
		}
		else if (this.AmountOfFail >= this.failTimes)
		{
			base.Fail();
		}
		else
		{
			this.routines.ForEach(delegate(Routine routine)
			{
				routine.Act();
			});
		}
	}

	private int AmountOfSucceed
	{
		get
		{
			int num = 0;
			foreach (Routine routine in this.routines)
			{
				if (routine.IsSuccess)
				{
					num++;
				}
			}
			return num;
		}
	}

	private int AmountOfFail
	{
		get
		{
			int num = 0;
			foreach (Routine routine in this.routines)
			{
				if (routine.IsFailure)
				{
					num++;
				}
			}
			return num;
		}
	}

	private List<Routine> routines;

	private int successTimes;

	private int failTimes;
}
