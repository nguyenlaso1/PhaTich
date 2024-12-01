// @sonhg: class: BombOffline.BaseEggMonster
using System;
using UnityEngine;

namespace BombOffline
{
	public class BaseEggMonster : Offline_BaseMonster
	{
		protected override void CreateBrain()
		{
			AIMove routine = new AIMove(this);
			this.brain = new Repeat(routine, -1);
		}

		protected override void InitMonsterProperty()
		{
			if (this.initalDirection == MoveDirection.STAND)
			{
				base.ChangeDirection();
			}
			else
			{
				base.CurrentDirection = this.initalDirection;
			}
		}

		[Header("FreakyEgg")]
		public MoveDirection initalDirection = MoveDirection.STAND;
	}
}
