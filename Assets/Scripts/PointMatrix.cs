// @sonhg: class: PointMatrix
using System;

public class PointMatrix
{
	public PointMatrix(int x, int y, bool _isWall)
	{
		this.X = x;
		this.Y = y;
		this.isWall = _isWall;
	}

	public int X;

	public int Y;

	public bool isWall;
}
