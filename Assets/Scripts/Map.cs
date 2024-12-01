// @sonhg: class: Map
using System;

public class Map
{
	public int Id
	{
		get
		{
			return this.id;
		}
		set
		{
			this.id = value;
		}
	}

	public string Path
	{
		get
		{
			return this.path;
		}
		set
		{
			this.path = value;
		}
	}

	public string Thumb
	{
		get
		{
			return this.thumb;
		}
		set
		{
			this.thumb = value;
		}
	}

	public string Description
	{
		get
		{
			return this.description;
		}
		set
		{
			this.description = value;
		}
	}

	public string Name
	{
		get
		{
			return this.name;
		}
		set
		{
			this.name = value;
		}
	}

	private int id = -1;

	private string path = string.Empty;

	private string thumb = string.Empty;

	private string description = string.Empty;

	private string name = string.Empty;
}
