// @sonhg: class: BombOffline.CoupyStone
using System;

namespace BombOffline
{
	public class CoupyStone : Kabuton
	{
		protected override void CreateBrain()
		{
			Wander routine = new Wander(this);
			SequenceNode sequenceNode = new SequenceNode();
			sequenceNode.AddRoutine(new Routine[]
			{
				new TimeLimit(routine, 5f)
			});
			SequenceNode sequenceNode2 = new SequenceNode();
			sequenceNode2.AddRoutine(new Routine[]
			{
				new CheckBombCollider(this),
				new SpecialAction(this)
			});
			Selector selector = new Selector();
			selector.AddRoutine(new Routine[]
			{
				sequenceNode2,
				sequenceNode
			});
			this.brain = new Repeat(selector, -1);
		}
	}
}
