// @sonhg: class: SequenceNode
using System;
using System.Collections.Generic;

public class SequenceNode : Routine
{
	public SequenceNode()
	{
		this.currentRoutine = null;
	}

	public void AddRoutine(params Routine[] routines)
	{
		for (int i = 0; i < routines.Length; i++)
		{
			this.routines.Add(routines[i]);
		}
	}

	public override void Reset()
	{
		foreach (Routine routine in this.routines)
		{
			routine.Reset();
		}
	}

	public override void Start()
	{
		base.Start();
		this.routineQueue.Clear();
		if (this.routines.Count > 0)
		{
			this.routineQueue = new Queue<Routine>(this.routines);
			this.currentRoutine = this.routineQueue.Dequeue();
			this.currentRoutine.Start();
		}
		else
		{
			base.Fail();
		}
	}

	public override void Act()
	{
		this.currentRoutine.Act();
		if (this.currentRoutine.IsRunning)
		{
			return;
		}
		if (this.currentRoutine.IsFailure)
		{
			base.Fail();
			return;
		}
		if (this.routineQueue.Count == 0)
		{
			this.state = this.currentRoutine.State;
		}
		else
		{
			this.currentRoutine = this.routineQueue.Dequeue();
			this.currentRoutine.Start();
		}
	}

	private Routine currentRoutine;

	private List<Routine> routines = new List<Routine>();

	private Queue<Routine> routineQueue = new Queue<Routine>();
}
