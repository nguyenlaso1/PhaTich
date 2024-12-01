// @sonhg: class: ResourcesManager
using System;
using System.Collections.Generic;
using UnityEngine;

public class ResourcesManager
{
	public static Sprite GetSprite(string key)
	{
		if (key[0] == '/')
		{
			key = key.Remove(0, 1);
		}
		if (key.Contains(".png") || key.Contains(".jpg"))
		{
			key = key.Replace(".png", string.Empty);
			key = key.Replace(".jpg", string.Empty);
		}
		string path = "Textures/" + key;
		Sprite sprite = null;
		try
		{
			sprite = Resources.Load<Sprite>(path);
		}
		catch (Exception ex)
		{
		}
		if (sprite == null)
		{
			sprite = ResourcesManager.SpriteList[key];
		}
		return sprite;
	}

	public static Dictionary<string, Tile> TilesDict = new Dictionary<string, Tile>();

	public static Dictionary<string, Item> ItemsDict = new Dictionary<string, Item>();

	public static Dictionary<string, Map> MapDict = new Dictionary<string, Map>();

	public static Dictionary<string, Sprite> SpriteList = new Dictionary<string, Sprite>();

	public static List<CardItem> cardList = new List<CardItem>();

	public static List<SMSItem> smsList = new List<SMSItem>();

	public static List<InappItem> inappList = new List<InappItem>();

	public static Dictionary<string, Monster> MonsterDict = new Dictionary<string, Monster>();
}
