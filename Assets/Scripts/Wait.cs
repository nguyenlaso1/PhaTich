// @sonhg: class: Wait
using System;
using UnityEngine;

public class Wait : Routine
{
	public Wait(float time)
	{
		this.time = time;
	}

	public override void Start()
	{
		base.Start();
		this.currentTime = 0f;
	}

	public override void Reset()
	{
		this.Start();
	}

	public override void Act()
	{
		this.currentTime += Time.deltaTime;
		if (this.currentTime > this.time)
		{
			base.Succeed();
		}
	}

	protected float time;

	protected float currentTime;
}
