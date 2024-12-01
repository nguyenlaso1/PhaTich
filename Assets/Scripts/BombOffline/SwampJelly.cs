// @sonhg: class: BombOffline.SwampJelly
using System;
using UnityEngine;

namespace BombOffline
{
	public class SwampJelly : BaseEggMonster
	{
		protected override void FixedUpdate()
		{
			base.FixedUpdate();
			if (!this.isUpdateCycle)
			{
				return;
			}
			this.board.PlaceObstacle(this.poisonTrap, new Vector2((float)base.currentX, (float)base.currentY), null);
		}

		[Header("SwampJelly")]
		[SerializeField]
		protected PoisonTrap poisonTrap;
	}
}
