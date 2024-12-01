// @sonhg: class: BombOffline.MapFinder<TPathNode, TUserContext>
using System;
using SettlersEngine;

namespace BombOffline
{
	public class MapFinder<TPathNode, TUserContext> : SpatialAStar<TPathNode, TUserContext> where TPathNode : IPathNode<TUserContext>
	{
		public MapFinder(TPathNode[,] inGrid) : base(inGrid)
		{
		}

		protected override double Heuristic(SpatialAStar<TPathNode, TUserContext>.PathNode inStart, SpatialAStar<TPathNode, TUserContext>.PathNode inEnd)
		{
			int val = Math.Abs(inStart.X - inEnd.X);
			int val2 = Math.Abs(inStart.Y - inEnd.Y);
			return (double)Math.Min(val, val2);
		}

		protected override double NeighborDistance(SpatialAStar<TPathNode, TUserContext>.PathNode inStart, SpatialAStar<TPathNode, TUserContext>.PathNode inEnd)
		{
			return this.Heuristic(inStart, inEnd);
		}
	}
}
