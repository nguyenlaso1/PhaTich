// @sonhg: class: BombOffline.Skeleton
using System;
using System.Collections.Generic;

namespace BombOffline
{
	public class Skeleton : Dengurin
	{
		public override IEnumerable<MapNode> GetMapRoute()
		{
			int toX = 0;
			int toY = 0;
			this.board.GetRandomPositionAt(ref toX, ref toY);
			return this.board.SearchFromTo(base.currentX, base.currentY, toX, toY, this);
		}
	}
}
