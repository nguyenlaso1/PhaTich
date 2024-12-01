// @sonhg: class: Item
using System;

public class Item
{
	public int Id { get; set; }

	public string Name { get; set; }

	public string Description { get; set; }

	public int IRow { get; set; }

	public int IColumn { get; set; }

	public int TiledCount { get; set; }

	public int Price { get; set; }

	public int PType { get; set; }

	public int Expire { get; set; }

	public int Experience { get; set; }

	public string Path { get; set; }

	public string Icon { get; set; }

	public string Data { get; set; }

	public int Category { get; set; }

	public override string ToString()
	{
		return string.Format("[Item: Id={0}, Name={1},  Description={2}, IRow={3}, IColumn={4}, TiledCount={5}, Price={6}, PType={7}, Expire={8}, Experience={9}, Path={10}, Data={11}, Category={12}]", new object[]
		{
			this.Id,
			this.Name,
			this.Description,
			this.IRow,
			this.IColumn,
			this.TiledCount,
			this.Price,
			this.PType,
			this.Expire,
			this.Experience,
			this.Path,
			this.Data,
			this.Category
		});
	}
}
