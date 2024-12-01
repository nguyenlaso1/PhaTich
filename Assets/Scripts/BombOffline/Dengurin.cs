// @sonhg: class: BombOffline.Dengurin
using System;

namespace BombOffline
{
	public class Dengurin : Offline_BaseMonster
	{
		protected override void CreateBrain()
		{
			Wander routine = new Wander(this);
			this.brain = new Repeat(routine, -1);
		}
	}
}
