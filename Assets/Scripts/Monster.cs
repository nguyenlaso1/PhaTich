// @sonhg: class: Monster
using System;
using Sfs2X.Entities.Data;
using UnityEngine;

public class Monster
{
	public Monster Copy()
	{
		return new Monster
		{
			Id = this.Id,
			Name = this.Name,
			Path = this.Path,
			Position = this.Position,
			Type = this.Type,
			Point = this.Point,
			Drop = this.Drop
		};
	}

	public int Id { get; set; }

	public string Name { get; set; }

	public string Icon { get; set; }

	public string Path { get; set; }

	public Vector3 Position { get; set; }

	public float AppearTime { get; set; }

	public int Type { get; set; }

	public bool IsBoss
	{
		get
		{
			return this.Type == 1;
		}
	}

	public bool IsSpecial
	{
		get
		{
			return this.Type == 2;
		}
	}

	public int Point { get; set; }

	public ISFSObject Drop { get; set; }
}
