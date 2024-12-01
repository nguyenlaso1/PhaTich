// @sonhg: class: SettlersEngine.SpatialAStar<TPathNode, TUserContext> where TPathNode
using System;
using System.Collections.Generic;
using UnityEngine;

namespace SettlersEngine
{
	public class SpatialAStar<TPathNode, TUserContext> where TPathNode : IPathNode<TUserContext>
	{
		public SpatialAStar(TPathNode[,] inGrid)
		{
			this.SearchSpace = inGrid;
			this.Width = inGrid.GetLength(0);
			this.Height = inGrid.GetLength(1);
			this.m_SearchSpace = new SpatialAStar<TPathNode, TUserContext>.PathNode[this.Width, this.Height];
			this.m_ClosedSet = new SpatialAStar<TPathNode, TUserContext>.OpenCloseMap(this.Width, this.Height);
			this.m_OpenSet = new SpatialAStar<TPathNode, TUserContext>.OpenCloseMap(this.Width, this.Height);
			this.m_CameFrom = new SpatialAStar<TPathNode, TUserContext>.PathNode[this.Width, this.Height];
			this.m_RuntimeGrid = new SpatialAStar<TPathNode, TUserContext>.OpenCloseMap(this.Width, this.Height);
			this.m_OrderedOpenSet = new PriorityQueue<SpatialAStar<TPathNode, TUserContext>.PathNode>(SpatialAStar<TPathNode, TUserContext>.PathNode.Comparer);
			for (int i = 0; i < this.Width; i++)
			{
				for (int j = 0; j < this.Height; j++)
				{
					if (inGrid[i, j] == null)
					{
						throw new ArgumentNullException();
					}
					this.m_SearchSpace[i, j] = new SpatialAStar<TPathNode, TUserContext>.PathNode(i, j, inGrid[i, j]);
				}
			}
		}

		public TPathNode[,] SearchSpace { get; private set; }

		public int Width { get; private set; }

		public int Height { get; private set; }

		protected virtual double Heuristic(SpatialAStar<TPathNode, TUserContext>.PathNode inStart, SpatialAStar<TPathNode, TUserContext>.PathNode inEnd)
		{
			return Math.Sqrt((double)((inStart.X - inEnd.X) * (inStart.X - inEnd.X) + (inStart.Y - inEnd.Y) * (inStart.Y - inEnd.Y)));
		}

		protected virtual double NeighborDistance(SpatialAStar<TPathNode, TUserContext>.PathNode inStart, SpatialAStar<TPathNode, TUserContext>.PathNode inEnd)
		{
			int num = Math.Abs(inStart.X - inEnd.X);
			int num2 = Math.Abs(inStart.Y - inEnd.Y);
			switch (num + num2)
			{
			case 0:
				return 0.0;
			case 1:
				return 1.0;
			case 2:
				return SpatialAStar<TPathNode, TUserContext>.SQRT_2;
			default:
				throw new ApplicationException();
			}
		}

		public LinkedList<TPathNode> Search(Vector2 inStartNode, Vector2 inEndNode, TUserContext inUserContext)
		{
			SpatialAStar<TPathNode, TUserContext>.PathNode pathNode = this.m_SearchSpace[(int)inStartNode.x, (int)inStartNode.y];
			SpatialAStar<TPathNode, TUserContext>.PathNode pathNode2 = this.m_SearchSpace[(int)inEndNode.x, (int)inEndNode.y];
			if (pathNode == pathNode2)
			{
				return new LinkedList<TPathNode>(new TPathNode[]
				{
					pathNode.UserContext
				});
			}
			SpatialAStar<TPathNode, TUserContext>.PathNode[] array = new SpatialAStar<TPathNode, TUserContext>.PathNode[4];
			this.m_ClosedSet.Clear();
			this.m_OpenSet.Clear();
			this.m_RuntimeGrid.Clear();
			this.m_OrderedOpenSet.Clear();
			for (int i = 0; i < this.Width; i++)
			{
				for (int j = 0; j < this.Height; j++)
				{
					this.m_CameFrom[i, j] = null;
				}
			}
			pathNode.G = 0.0;
			pathNode.H = this.Heuristic(pathNode, pathNode2);
			pathNode.F = pathNode.H;
			this.m_OpenSet.Add(pathNode);
			this.m_OrderedOpenSet.Push(pathNode);
			this.m_RuntimeGrid.Add(pathNode);
			int num = 0;
			while (!this.m_OpenSet.IsEmpty)
			{
				SpatialAStar<TPathNode, TUserContext>.PathNode pathNode3 = this.m_OrderedOpenSet.Pop();
				if (pathNode3 == pathNode2)
				{
					LinkedList<TPathNode> linkedList = this.ReconstructPath(this.m_CameFrom, this.m_CameFrom[pathNode2.X, pathNode2.Y]);
					linkedList.AddLast(pathNode2.UserContext);
					return linkedList;
				}
				this.m_OpenSet.Remove(pathNode3);
				this.m_ClosedSet.Add(pathNode3);
				this.StoreNeighborNodes(pathNode3, array);
				foreach (SpatialAStar<TPathNode, TUserContext>.PathNode pathNode4 in array)
				{
					if (pathNode4 != null)
					{
						TPathNode userContext = pathNode4.UserContext;
						if (userContext.IsWalkable(inUserContext))
						{
							if (!this.m_ClosedSet.Contains(pathNode4))
							{
								num++;
								double num2 = this.m_RuntimeGrid[pathNode3].G + this.NeighborDistance(pathNode3, pathNode4);
								bool flag = false;
								bool flag2;
								if (!this.m_OpenSet.Contains(pathNode4))
								{
									this.m_OpenSet.Add(pathNode4);
									flag2 = true;
									flag = true;
								}
								else
								{
									flag2 = (num2 < this.m_RuntimeGrid[pathNode4].G);
								}
								if (flag2)
								{
									this.m_CameFrom[pathNode4.X, pathNode4.Y] = pathNode3;
									if (!this.m_RuntimeGrid.Contains(pathNode4))
									{
										this.m_RuntimeGrid.Add(pathNode4);
									}
									this.m_RuntimeGrid[pathNode4].G = num2;
									this.m_RuntimeGrid[pathNode4].H = this.Heuristic(pathNode4, pathNode2);
									this.m_RuntimeGrid[pathNode4].F = this.m_RuntimeGrid[pathNode4].G + this.m_RuntimeGrid[pathNode4].H;
									if (flag)
									{
										this.m_OrderedOpenSet.Push(pathNode4);
									}
									else
									{
										this.m_OrderedOpenSet.Update(pathNode4);
									}
								}
							}
						}
					}
				}
			}
			return null;
		}

		private LinkedList<TPathNode> ReconstructPath(SpatialAStar<TPathNode, TUserContext>.PathNode[,] came_from, SpatialAStar<TPathNode, TUserContext>.PathNode current_node)
		{
			LinkedList<TPathNode> result = new LinkedList<TPathNode>();
			this.ReconstructPathRecursive(came_from, current_node, result);
			return result;
		}

		private void ReconstructPathRecursive(SpatialAStar<TPathNode, TUserContext>.PathNode[,] came_from, SpatialAStar<TPathNode, TUserContext>.PathNode current_node, LinkedList<TPathNode> result)
		{
			SpatialAStar<TPathNode, TUserContext>.PathNode pathNode = came_from[current_node.X, current_node.Y];
			if (pathNode != null)
			{
				this.ReconstructPathRecursive(came_from, pathNode, result);
				result.AddLast(current_node.UserContext);
			}
			else
			{
				result.AddLast(current_node.UserContext);
			}
		}

		private void StoreNeighborNodes(SpatialAStar<TPathNode, TUserContext>.PathNode inAround, SpatialAStar<TPathNode, TUserContext>.PathNode[] inNeighbors)
		{
			int x = inAround.X;
			int y = inAround.Y;
			if (x > 0)
			{
				inNeighbors[0] = this.m_SearchSpace[x - 1, y];
			}
			else
			{
				inNeighbors[0] = null;
			}
			if (x < this.Width - 1)
			{
				inNeighbors[1] = this.m_SearchSpace[x + 1, y];
			}
			else
			{
				inNeighbors[1] = null;
			}
			if (y > 0)
			{
				inNeighbors[2] = this.m_SearchSpace[x, y - 1];
			}
			else
			{
				inNeighbors[2] = null;
			}
			if (y < this.Height - 1)
			{
				inNeighbors[3] = this.m_SearchSpace[x, y + 1];
			}
			else
			{
				inNeighbors[3] = null;
			}
		}

		private SpatialAStar<TPathNode, TUserContext>.OpenCloseMap m_ClosedSet;

		private SpatialAStar<TPathNode, TUserContext>.OpenCloseMap m_OpenSet;

		private PriorityQueue<SpatialAStar<TPathNode, TUserContext>.PathNode> m_OrderedOpenSet;

		private SpatialAStar<TPathNode, TUserContext>.PathNode[,] m_CameFrom;

		private SpatialAStar<TPathNode, TUserContext>.OpenCloseMap m_RuntimeGrid;

		private SpatialAStar<TPathNode, TUserContext>.PathNode[,] m_SearchSpace;

		private static readonly double SQRT_2 = Math.Sqrt(2.0);

		protected class PathNode : IIndexedObject, IPathNode<TUserContext>, IComparer<SpatialAStar<TPathNode, TUserContext>.PathNode>
		{
			public PathNode(int inX, int inY, TPathNode inUserContext)
			{
				this.X = inX;
				this.Y = inY;
				this.UserContext = inUserContext;
			}

			public TPathNode UserContext { get; internal set; }

			public double G { get; internal set; }

			public double H { get; internal set; }

			public double F { get; internal set; }

			public int Index { get; set; }

			public bool IsWalkable(TUserContext inContext)
			{
				TPathNode userContext = this.UserContext;
				return userContext.IsWalkable(inContext);
			}

			public int X { get; internal set; }

			public int Y { get; internal set; }

			public int Compare(SpatialAStar<TPathNode, TUserContext>.PathNode x, SpatialAStar<TPathNode, TUserContext>.PathNode y)
			{
				if (x.F < y.F)
				{
					return -1;
				}
				if (x.F > y.F)
				{
					return 1;
				}
				return 0;
			}

			public static readonly SpatialAStar<TPathNode, TUserContext>.PathNode Comparer = new SpatialAStar<TPathNode, TUserContext>.PathNode(0, 0, default(TPathNode));
		}

		private class OpenCloseMap
		{
			public OpenCloseMap(int inWidth, int inHeight)
			{
				this.m_Map = new SpatialAStar<TPathNode, TUserContext>.PathNode[inWidth, inHeight];
				this.Width = inWidth;
				this.Height = inHeight;
			}

			public int Width { get; private set; }

			public int Height { get; private set; }

			public int Count { get; private set; }

			public SpatialAStar<TPathNode, TUserContext>.PathNode this[int x, int y]
			{
				get
				{
					return this.m_Map[x, y];
				}
			}

			public SpatialAStar<TPathNode, TUserContext>.PathNode this[SpatialAStar<TPathNode, TUserContext>.PathNode Node]
			{
				get
				{
					return this.m_Map[Node.X, Node.Y];
				}
			}

			public bool IsEmpty
			{
				get
				{
					return this.Count == 0;
				}
			}

			public void Add(SpatialAStar<TPathNode, TUserContext>.PathNode inValue)
			{
				SpatialAStar<TPathNode, TUserContext>.PathNode pathNode = this.m_Map[inValue.X, inValue.Y];
				this.Count++;
				this.m_Map[inValue.X, inValue.Y] = inValue;
			}

			public bool Contains(SpatialAStar<TPathNode, TUserContext>.PathNode inValue)
			{
				return this.m_Map[inValue.X, inValue.Y] != null;
			}

			public void Remove(SpatialAStar<TPathNode, TUserContext>.PathNode inValue)
			{
				SpatialAStar<TPathNode, TUserContext>.PathNode pathNode = this.m_Map[inValue.X, inValue.Y];
				this.Count--;
				this.m_Map[inValue.X, inValue.Y] = null;
			}

			public void Clear()
			{
				this.Count = 0;
				for (int i = 0; i < this.Width; i++)
				{
					for (int j = 0; j < this.Height; j++)
					{
						this.m_Map[i, j] = null;
					}
				}
			}

			private SpatialAStar<TPathNode, TUserContext>.PathNode[,] m_Map;
		}
	}
}
