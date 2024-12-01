// @sonhg: class: BombOffline.MasterZombie0
using System;
using UnityEngine;

namespace BombOffline
{
	public class MasterZombie0 : IceTower
	{
		public override void DestroyMonster()
		{
			base.DestroyMonster();
			Offline_BaseMonster offline_BaseMonster = this.board.PutMonsterAt(base.currentX, base.currentY, this.spawnMonsterOnDeath);
		}

		[SerializeField]
		protected Offline_BaseMonster spawnMonsterOnDeath;
	}
}
