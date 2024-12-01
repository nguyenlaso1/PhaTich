// @sonhg: class: BombOffline.Bee
using System;

namespace BombOffline
{
	public class Bee : Offline_BaseMonster
	{
		protected override void CreateBrain()
		{
			Wander routine = new Wander(this);
			this.brain = new Repeat(new TimeLimit(routine, 5f), -1);
		}
	}
}
