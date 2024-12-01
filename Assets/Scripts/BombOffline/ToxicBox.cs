// @sonhg: class: BombOffline.ToxicBox
using System;
using UnityEngine;

namespace BombOffline
{
	public class ToxicBox : Offline_BaseMonster
	{
		protected override void CreateBrain()
		{
		}

		protected override void InitMonsterProperty()
		{
			this.canAct = false;
			this.board.Scene.MapController.PlaceBomb(this.bombPrefab, 1, base.transform.position, false, 0, null, false);
			this.DestroyMonster();
		}

		public override void DestroyMonster()
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}

		private Offline_Tiled[,] mapTiled;

		[SerializeField]
		protected GameObject bombPrefab;
	}
}
