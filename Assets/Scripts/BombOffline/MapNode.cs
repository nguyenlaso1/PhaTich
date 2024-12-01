// @sonhg: class: BombOffline.MapNode
using System;
using SettlersEngine;

namespace BombOffline
{
	[Serializable]
	public class MapNode : IPathNode<Offline_BaseCharactersController>
	{
		public MapNode()
		{
		}

		public MapNode(int x, int y)
		{
			this.X = x;
			this.Y = y;
		}

		public bool IsWalkable(Offline_BaseCharactersController go)
		{
			bool result;
			if (go != null)
			{
				if (go.CanMoveThrough)
				{
					result = (true && !this.IsBorderWall);
				}
				else
				{
					result = !this.IsWall;
				}
			}
			else
			{
				result = !this.IsWall;
			}
			return result;
		}

		public int X;

		public int Y;

		public bool IsWall;

		public bool IsBorderWall;
	}
}
