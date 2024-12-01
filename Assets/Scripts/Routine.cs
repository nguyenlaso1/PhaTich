// @sonhg: class: Routine
using System;

public abstract class Routine
{
	public virtual void Start()
	{
		this.state = Routine.RoutineState.Running;
	}

	public abstract void Reset();

	public abstract void Act();

	protected void Succeed()
	{
		this.state = Routine.RoutineState.Success;
	}

	protected void Fail()
	{
		this.state = Routine.RoutineState.Failure;
	}

	public bool IsSuccess
	{
		get
		{
			return this.state.Equals(Routine.RoutineState.Success);
		}
	}

	public bool IsFailure
	{
		get
		{
			return this.state.Equals(Routine.RoutineState.Failure);
		}
	}

	public bool IsRunning
	{
		get
		{
			return this.state.Equals(Routine.RoutineState.Running);
		}
	}

	public Routine.RoutineState State
	{
		get
		{
			return this.state;
		}
	}

	protected Routine.RoutineState state;

	public enum RoutineState
	{
		NotActive,
		Success,
		Failure,
		Running
	}
}
