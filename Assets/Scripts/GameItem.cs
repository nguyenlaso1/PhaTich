// @sonhg: class: GameItem
using System;
using Sfs2X.Entities.Data;

public class GameItem
{
	public GameItem(ISFSObject item)
	{
		this.itemId = item.GetInt("i-id");
		this.category = item.GetInt("category");
		this.itemUrl = item.GetUtfString("i-path");
		this.itemName = item.GetUtfString("i-name");
		this.itemDes = item.GetUtfString("i-des");
		this.itemSubName = item.GetUtfString("subcatName");
		this.itemSubId = item.GetInt("subcatId");
		this.itemPrice = item.GetInt("i-price");
		this.itemExp = item.GetInt("i-experience");
		this.itemExpire = item.GetInt("i-exprice");
		this.itemPriceType = item.GetInt("i-priceType");
	}

	public string Dump()
	{
		return string.Concat(new object[]
		{
			"GameItem: ",
			this.itemId,
			";itemName: ",
			this.itemName,
			";itemSubName",
			this.itemSubName,
			";category: ",
			this.category
		});
	}

	public int itemId;

	public int category;

	public string itemUrl;

	public string itemName;

	public string itemDes;

	public string itemSubName;

	public int itemSubId;

	public int itemPrice;

	public int itemExp;

	public int itemExpire;

	public int itemPriceType;
}
