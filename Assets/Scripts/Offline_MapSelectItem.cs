// @sonhg: class: Offline_MapSelectItem
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Offline_MapSelectItem : BaseBox
{
	protected override void Start()
	{
		this.Init();
	}

	private void Init()
	{
		this._listSelectedItem.Clear();
		this._itemSelectedCount = 0;
		string @string = PlayerPrefs.GetString("ItemSelected", "none");
		List<string> list = new List<string>();
		if (@string != "none")
		{
			foreach (string item in @string.Split(new char[]
			{
				'-'
			}))
			{
				list.Add(item);
			}
		}
		foreach (Offline_Item offline_Item in this.itemObjectArr)
		{
			if (list.Count > 0 && list.IndexOf(offline_Item.id) >= 0)
			{
				this._itemSelectedCount++;
				this._listSelectedItem.Add(offline_Item);
				offline_Item._isSelected = true;
				offline_Item.GetComponent<Image>().sprite = this.itemSelectSlotSprite[2];
			}
		}
		foreach (Offline_Item offline_Item2 in this.itemObjectArr)
		{
			if (this._itemSelectedCount < 2)
			{
				if (!offline_Item2._isSelected)
				{
					offline_Item2.GetComponent<Image>().sprite = this.itemSelectSlotSprite[0];
				}
			}
			else if (!offline_Item2._isSelected)
			{
				offline_Item2.GetComponent<Image>().sprite = this.itemSelectSlotSprite[1];
			}
		}
	}

	public void OnClickItem(Offline_Item _obj)
	{
		if (DataManager.GetGold() >= _obj.cost)
		{
			DataManager.MinusGold(_obj.cost);
			this._shopController.UpdateCoin();
			_obj.AddItem();
		}
		else
		{
			this._shopController.OnClickInappButton();
		}
	}

	public void OnClickSelectItem(Offline_Item _obj)
	{
		if (this._itemSelectedCount < 2)
		{
			if (!_obj._isSelected)
			{
				this._itemSelectedCount++;
				_obj._isSelected = true;
				_obj.GetComponent<Image>().sprite = this.itemSelectSlotSprite[2];
				this._listSelectedItem.Add(_obj);
				string text = string.Empty;
				foreach (Offline_Item offline_Item in this._listSelectedItem)
				{
					text = text + offline_Item.id + "-";
				}
				if (string.IsNullOrEmpty(text))
				{
					text = "none";
				}
				PlayerPrefs.SetString("ItemSelected", text);
			}
			else
			{
				this._itemSelectedCount--;
				_obj._isSelected = false;
				_obj.GetComponent<Image>().sprite = this.itemSelectSlotSprite[0];
				this._listSelectedItem.Remove(_obj);
				string text2 = string.Empty;
				foreach (Offline_Item offline_Item2 in this._listSelectedItem)
				{
					text2 = text2 + offline_Item2.id + "-";
				}
				if (string.IsNullOrEmpty(text2))
				{
					text2 = "none";
				}
				PlayerPrefs.SetString("ItemSelected", text2);
			}
		}
		else if (_obj._isSelected)
		{
			this._itemSelectedCount--;
			_obj._isSelected = false;
			_obj.GetComponent<Image>().sprite = this.itemSelectSlotSprite[0];
			this._listSelectedItem.Remove(_obj);
			string text3 = string.Empty;
			foreach (Offline_Item offline_Item3 in this._listSelectedItem)
			{
				text3 = text3 + offline_Item3.id + "-";
			}
			if (string.IsNullOrEmpty(text3))
			{
				text3 = "none";
			}
			PlayerPrefs.SetString("ItemSelected", text3);
		}
		foreach (Offline_Item offline_Item4 in this.itemObjectArr)
		{
			if (this._itemSelectedCount < 2)
			{
				if (!offline_Item4._isSelected)
				{
					offline_Item4.GetComponent<Image>().sprite = this.itemSelectSlotSprite[0];
				}
			}
			else if (!offline_Item4._isSelected)
			{
				offline_Item4.GetComponent<Image>().sprite = this.itemSelectSlotSprite[1];
			}
		}
	}

	public void OnClickPlay()
	{
		if (this._listSelectedItem.Count > 0)
		{
			if (this._listSelectedItem.Count == 1)
			{
				DataManager.SetItemHelper2(this._listSelectedItem[0].id);
				DataManager.SetItemHelper1(null);
			}
			else if (this._listSelectedItem.Count == 2)
			{
				DataManager.SetItemHelper2(this._listSelectedItem[0].id);
				DataManager.SetItemHelper1(this._listSelectedItem[1].id);
			}
		}
		else
		{
			DataManager.SetItemHelper1(null);
			DataManager.SetItemHelper2(null);
		}
	}

	public void OnClickClosePanel()
	{
		base.gameObject.SetActive(false);
	}

	private int _itemSelectedCount;

	[SerializeField]
	private Offline_ShopController _shopController;

	public Sprite[] itemSelectSlotSprite;

	public Offline_Item[] itemObjectArr;

	private List<Offline_Item> _listSelectedItem = new List<Offline_Item>();
}
