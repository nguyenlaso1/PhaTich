// @sonhg: class: DataTable
using System;
using System.Collections.Generic;

public class DataTable
{
	public DataTable()
	{
		this.Columns = new List<string>();
		this.Rows = new List<DataRow>();
	}

	public List<string> Columns { get; set; }

	public List<DataRow> Rows { get; set; }

	public DataRow this[int row]
	{
		get
		{
			return this.Rows[row];
		}
	}

	public void AddRow(object[] values)
	{
		if (values.Length != this.Columns.Count)
		{
			throw new IndexOutOfRangeException("The number of values in the row must match the number of column");
		}
		DataRow dataRow = new DataRow();
		for (int i = 0; i < values.Length; i++)
		{
			dataRow[this.Columns[i]] = values[i];
		}
		this.Rows.Add(dataRow);
	}
}
