// @sonhg: class: UserItemList
using System;
using System.Collections.Generic;
using Sfs2X.Entities.Data;

public class UserItemList
{
	public UserItemList(ISFSArray arr)
	{
		this._listGameItem.Clear();
		for (int i = 0; i < arr.Size(); i++)
		{
			ISFSObject item = (ISFSObject)arr.GetElementAt(i);
			GameItem item2 = new GameItem(item);
			this._listGameItem.Add(item2);
		}
	}

	public List<GameItem> GetAllItem()
	{
		return this._listGameItem;
	}

	public List<GameItem> GetListItem(int category)
	{
		List<GameItem> list = new List<GameItem>();
		for (int i = 0; i < this._listGameItem.Count; i++)
		{
			if (this._listGameItem[i].category == category)
			{
				list.Add(this._listGameItem[i]);
			}
		}
		return list;
	}

	public string Dump(int category)
	{
		string text = "Dump: ";
		for (int i = 0; i < this._listGameItem.Count; i++)
		{
			if (this._listGameItem[i].category == category)
			{
				text = text + "\n" + this._listGameItem[i].Dump();
			}
		}
		return text;
	}

	public string Dump()
	{
		string text = "Dump: ";
		for (int i = 0; i < this._listGameItem.Count; i++)
		{
			text = text + "\n" + this._listGameItem[i].Dump();
		}
		return text;
	}

	public GameItem GetItem(int itemId)
	{
		for (int i = 0; i < this._listGameItem.Count; i++)
		{
			if (this._listGameItem[i].itemId == itemId)
			{
				return this._listGameItem[i];
			}
		}
		return null;
	}

	protected List<GameItem> _listGameItem = new List<GameItem>();
}
