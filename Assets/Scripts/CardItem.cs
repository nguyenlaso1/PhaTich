// @sonhg: class: CardItem
using System;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;

public class CardItem
{
	public CardItem(int id, string header, string cp, string cardName, float coefficient, int zoder, int validationType, string description, string validationRequire, int type)
	{
		this._type = type;
		this._id = id;
		this._header = header;
		this._cardName = cardName;
		this._cp = cp;
		this._coefficient = coefficient;
		this._zoder = zoder;
		this._validationType = validationType;
		this._validationRequire = validationRequire;
		JSONNode jsonnode = JSONNode.Parse(description);
		this._itemList = new List<CardItemInfo>();
		for (int i = 0; i < jsonnode["infoList"].Count; i++)
		{
			this._itemList.Add(new CardItemInfo(int.Parse(jsonnode["infoList"][i]["pay"]), (int)Mathf.Round((float)int.Parse(jsonnode["infoList"][i]["chip"]) * coefficient), int.Parse(jsonnode["infoList"][i]["gold"])));
		}
		UnityEngine.Debug.Log(string.Concat(new object[]
		{
			"CardItem: ",
			id,
			" | _itemList: ",
			this._itemList.Count
		}));
	}

	public virtual int Type
	{
		get
		{
			return this._type;
		}
	}

	public virtual string ValidationRequire
	{
		get
		{
			return this._validationRequire;
		}
	}

	public virtual string Header
	{
		get
		{
			return this._header;
		}
	}

	public virtual string CardName
	{
		get
		{
			return this._cardName;
		}
	}

	public List<CardItemInfo> ItemList
	{
		get
		{
			return this._itemList;
		}
	}

	private int _id;

	private string _header;

	private string _cardName;

	private string _cp;

	private List<CardItemInfo> _itemList;

	private float _coefficient;

	private int _zoder;

	private int _validationType;

	private int _type;

	private string _validationRequire;
}
