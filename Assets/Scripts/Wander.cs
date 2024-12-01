// @sonhg: class: Wander
using System;
using System.Collections.Generic;
using BombOffline;

public class Wander : Routine
{
	public Wander(Offline_BaseMonster actor)
	{
		this.actor = actor;
	}

	public override void Start()
	{
		base.Start();
		this.CreateNewDestination();
		this.moveSequence.Start();
	}

	public override void Reset()
	{
		this.CreateNewDestination();
		this.actor.SnapGrid();
	}

	public override void Act()
	{
		if (!this.moveSequence.IsRunning)
		{
			return;
		}
		this.moveSequence.Act();
		if (this.moveSequence.IsSuccess)
		{
			base.Succeed();
		}
		else if (this.moveSequence.IsFailure)
		{
			base.Fail();
		}
	}

	protected virtual void CreateNewDestination()
	{
		this.moveSequence = new SequenceNode();
		IEnumerable<MapNode> mapRoute = this.actor.GetMapRoute();
		if (mapRoute == null)
		{
			base.Fail();
			return;
		}
		foreach (MapNode mapNode in mapRoute)
		{
			this.moveSequence.AddRoutine(new Routine[]
			{
				new AIMoveTo(this.actor, mapNode.X, mapNode.Y)
			});
		}
	}

	protected SequenceNode moveSequence;

	protected Offline_BaseMonster actor;
}
