// @sonhg: class: DataRow
using System;
using System.Collections.Generic;

public class DataRow : Dictionary<string, object>
{
	public new object this[string column]
	{
		get
		{
			if (this.ContainsKey(column))
			{
				return base[column];
			}
			return null;
		}
		set
		{
			if (this.ContainsKey(column))
			{
				base[column] = value;
			}
			else
			{
				this.Add(column, value);
			}
		}
	}
}
