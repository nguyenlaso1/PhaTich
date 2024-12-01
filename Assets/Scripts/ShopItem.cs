// @sonhg: class: ShopItem
using System;
using UnityEngine;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour
{
	public int ItemId
	{
		get
		{
			return this.itemId;
		}
		set
		{
			this.itemId = value;
		}
	}

	public void SetItem(Sprite _itemSprite, int _cost)
	{
		this.itemSprite.enabled = true;
		this.itemSprite.sprite = _itemSprite;
		this.cost = _cost;
		if (_cost > 0)
		{
			this.costLabel.text = _cost.ToString();
			if (!this.canByGold)
			{
				this.costLabel.transform.parent.gameObject.SetActive(true);
			}
		}
	}

	private void Update()
	{
	}

	public Image itemSprite;

	public Text costLabel;

	[HideInInspector]
	public int cost;

	[HideInInspector]
	public bool canByGold;

	private int itemId = -1;
}
