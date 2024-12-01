// @sonhg: class: BombOffline.Offline_Bot
using System;
using System.Collections.Generic;
using UnityEngine;

namespace BombOffline
{
	public class Offline_Bot : Offline_BaseCharactersController
	{
		private void Start()
		{
			base.CurrentDirection = this.defaultDirect;
		}

		private void FixedUpdate()
		{
			this.Move(base.CurrentDirection, false);
			this.countTime++;
			if (this.countTime == 20)
			{
				this.RandomChangeDirection();
				this.countTime = 0;
			}
		}

		private void RandomChangeDirection()
		{
			int num = UnityEngine.Random.Range(0, 20);
			if (num < 5)
			{
				this.ChangeDirection();
			}
		}

		private void ChangeDirection()
		{
			List<MoveDirection> list = new List<MoveDirection>();
			foreach (Offline_DrawRayCast offline_DrawRayCast in this.rayCasts)
			{
				if (!offline_DrawRayCast.hit && !list.Contains(offline_DrawRayCast.direction))
				{
					list.Add(offline_DrawRayCast.direction);
				}
			}
			if (list.Count == 0)
			{
				return;
			}
			int index = UnityEngine.Random.Range(0, list.Count);
			base.CurrentDirection = list[index];
		}

		protected override void Notify(GameObject collision)
		{
			this.ChangeDirection();
		}

		private bool _changed;

		private MoveDirection defaultDirect = MoveDirection.RIGHT;

		private int countTime;
	}
}
