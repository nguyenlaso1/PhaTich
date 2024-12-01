// @sonhg: class: MyPathNode
using System;
using SettlersEngine;

public class MyPathNode : IPathNode<object>
{
	public int X { get; set; }

	public int Y { get; set; }

	public bool IsWall { get; set; }

	public bool IsWalkable(object unused)
	{
		return !this.IsWall;
	}
}
