// @sonhg: class: SpritePool
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SpritePool : ScriptableObject
{
	public static SpritePool GetAtlasByName(string atlasName)
	{
		return Resources.Load<SpritePool>("Atlas/TiledMap/" + atlasName);
	}

	public Sprite FindByName(string name)
	{
		foreach (Sprite sprite in this.spriteList)
		{
			if (sprite.name.CompareTo(name) == 0)
			{
				return sprite;
			}
		}
		return null;
	}

	public void ClearAllItem()
	{
		this.spriteList.Clear();
	}

	public void AddItem(Sprite item)
	{
		this.spriteList.Add(item);
	}

	[SerializeField]
	protected List<Sprite> spriteList = new List<Sprite>();
}
