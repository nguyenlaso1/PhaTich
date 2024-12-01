// @sonhg: class: Tile
using System;

public class Tile
{
	public int Id { get; set; }

	public string Name { get; set; }

	public string Path { get; set; }

	public int IRow { get; set; }

	public int IColumn { get; set; }

	public int TiledCount { get; set; }

	public int Category { get; set; }

	public override string ToString()
	{
		return string.Format("[Tile: Id={0}, Name={1},  Path={2}, IRow={3}, IColumn={4}, TiledCount={5}, Category={6}]", new object[]
		{
			this.Id,
			this.Name,
			this.Path,
			this.IRow,
			this.IColumn,
			this.TiledCount,
			this.Category
		});
	}
}
