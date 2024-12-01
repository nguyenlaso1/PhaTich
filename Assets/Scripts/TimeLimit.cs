// @sonhg: class: TimeLimit
using System;
using UnityEngine;

public class TimeLimit : Routine
{
	public TimeLimit(Routine routine, float time)
	{
		this.time = time;
		this.routine = routine;
	}

	public override void Start()
	{
		base.Start();
		this.currentTime = 0f;
		this.routine.Start();
	}

	public override void Reset()
	{
		this.routine.Reset();
		this.Start();
	}

	public override void Act()
	{
		if (this.currentTime > this.time)
		{
			base.Succeed();
		}
		else
		{
			this.currentTime += Time.deltaTime;
			if (this.routine.IsFailure)
			{
				base.Fail();
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
	}

	private Routine routine;

	protected float time;

	protected float currentTime;
}
