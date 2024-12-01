// @sonhg: class: SpriteCollection
using System;
using UnityEngine;

public class SpriteCollection
{
	public SpriteCollection(string spritesheet)
	{
		this.sprites = Resources.LoadAll<Sprite>(spritesheet);
		this.names = new string[this.sprites.Length];
		for (int i = 0; i < this.names.Length; i++)
		{
			this.names[i] = this.sprites[i].name;
		}
	}

	public Sprite GetSprite(string name)
	{
		return this.sprites[Array.IndexOf<string>(this.names, name)];
	}

	public bool CheckSpriteExist(string name)
	{
		bool result = false;
		for (int i = 0; i < this.names.Length; i++)
		{
			if (this.names[i] == name)
			{
				result = true;
			}
		}
		return result;
	}

	private Sprite[] sprites;

	private string[] names;
}
