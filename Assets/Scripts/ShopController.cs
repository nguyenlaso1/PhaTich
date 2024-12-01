// @sonhg: class: ShopController
using System;
using System.Collections.Generic;
using System.Linq;
using Bomb;
using Sfs2X.Entities.Data;
using UnityEngine;
using UnityEngine.UI;

public class ShopController : MonoBehaviour
{
	private void Start()
	{
		this.avatarController.SetUser(SmartFoxConnection.Connection.MySelf);
	}

	private void LoadItemByCategory(string _category)
	{
		switch (_category)
		{
		case "Hat":
			this.currentCategory = ShopController.Category.Hat;
			this.LoadScrollViewItem(4);
			break;
		case "Hair":
			this.currentCategory = ShopController.Category.Hair;
			this.LoadScrollViewItem(5);
			break;
		case "Bomb":
			this.currentCategory = ShopController.Category.Bom;
			this.LoadScrollViewItem(3);
			break;
		case "Face":
			this.currentCategory = ShopController.Category.Face;
			this.LoadScrollViewItem(6);
			break;
		case "Ava":
			this.currentCategory = ShopController.Category.Avarta;
			this.LoadScrollViewItem(8);
			break;
		case "Body":
			this.currentCategory = ShopController.Category.Body;
			this.LoadScrollViewItem(7);
			break;
		}
	}

	public void LoadScrollViewItemByCategory()
	{
		switch (this.currentCategory)
		{
		case ShopController.Category.Hair:
			this.LoadScrollViewItem(5);
			break;
		case ShopController.Category.Face:
			this.LoadScrollViewItem(6);
			break;
		case ShopController.Category.Body:
			this.LoadScrollViewItem(7);
			break;
		case ShopController.Category.Bom:
			this.LoadScrollViewItem(3);
			break;
		}
	}

	private void LoadScrollViewItem(int _category)
	{
		int i = this.gridTrans.childCount;
		for (int j = i - 1; j >= 0; j--)
		{
			if (j != 0)
			{
				UnityEngine.Object.Destroy(this.gridTrans.GetChild(j).gameObject);
			}
		}
		i = 1;
		List<Item> list = ResourcesManager.ItemsDict.Values.ToList<Item>();
		foreach (Item item in list)
		{
			if (item.Category == _category && InventoryController.myItemList.IndexOf(item.Id) < 0 && !string.IsNullOrEmpty(ResourcesUltis.ItemIdToLink(item.Id.ToString()).Replace(ResourceChecking.BaseIp(), string.Empty)))
			{
				ShopItem _newShopItem = UnityEngine.Object.Instantiate<ShopItem>(this.shopItemPrefabs);
				_newShopItem.ItemId = item.Id;
				_newShopItem.transform.GetComponent<Button>().onClick.AddListener(delegate()
				{
					this.OnClickItem(_newShopItem.gameObject);
				});
				_newShopItem.transform.SetParent(this.gridTrans);
				_newShopItem.transform.localScale = Vector3.one;
				_newShopItem.transform.localPosition = Vector3.zero;
				_newShopItem.name = item.Id.ToString();
				if (item.PType == 0)
				{
					_newShopItem.canByGold = true;
				}
				else
				{
					_newShopItem.canByGold = false;
				}
				Item item2 = ResourcesManager.ItemsDict[item.Id.ToString()];
				string text = string.Empty;
				if (this.currentCategory != ShopController.Category.Bom)
				{
					text = ResourcesUltis.ItemIdToIconLink(item.Id.ToString()).Replace(ResourceChecking.BaseIp(), string.Empty);
				}
				else
				{
					text = ResourcesUltis.ItemIdToLink(item.Id.ToString()).Replace(ResourceChecking.BaseIp(), string.Empty);
				}
				Sprite sprite = Resources.Load<Sprite>("Textures/" + text.Substring(0, text.Length - 4));
				if (sprite == null && ResourcesManager.SpriteList.ContainsKey(text))
				{
					sprite = ResourcesManager.SpriteList[text];
				}
				_newShopItem.SetItem(sprite, item2.Price);
				i++;
			}
		}
		while (i < 9)
		{
			ShopItem shopItem = UnityEngine.Object.Instantiate<ShopItem>(this.shopItemPrefabs);
			shopItem.transform.SetParent(this.gridTrans);
			shopItem.transform.localPosition = Vector3.zero;
			shopItem.transform.localScale = Vector3.one;
			i++;
		}
		this.shopPanel.GetComponent<ScrollRect>().verticalScrollbar.value = 1f;
	}

	public void ReloadItem()
	{
	}

	public void OnClickItem(GameObject _item)
	{
		foreach (object obj in this.gridTrans)
		{
			Transform transform = (Transform)obj;
			if (transform.GetComponent<Outline>() != null)
			{
				transform.GetComponent<Outline>().enabled = false;
			}
		}
		_item.GetComponent<Outline>().enabled = true;
		ShopItem component = _item.GetComponent<ShopItem>();
		this.detailPanel.SetActive(true);
		this.buyButton.GetComponent<Button>().onClick.RemoveAllListeners();
		this.buyButton.GetComponent<Button>().onClick.AddListener(delegate()
		{
			this.BuysItem();
		});
		this.bomberMainMenuScript.selectModePanel.transform.localScale = Vector3.zero;
		if (component.cost > 0)
		{
			this.detailCost.text = component.cost.ToString();
		}
		else
		{
			this.detailCost.text = "Free";
		}
		this.detailSprite.sprite = component.itemSprite.sprite;
		this._currentItemId = component.ItemId;
		if (component.canByGold)
		{
			this.buyButton.sprite = this.buyButtonSprites[1];
		}
		else
		{
			this.buyButton.sprite = this.buyButtonSprites[0];
		}
		switch (this.currentCategory)
		{
		case ShopController.Category.Hair:
			this.avatarController.SetHairById(int.Parse(_item.name));
			break;
		case ShopController.Category.Face:
			this.avatarController.SetHeadById(int.Parse(_item.name));
			break;
		case ShopController.Category.Body:
			this.avatarController.SetBody(int.Parse(_item.name));
			break;
		}
	}

	public void OnClickReturnCategory()
	{
		int childCount = this.gridTrans.childCount;
		for (int i = childCount - 1; i >= 0; i--)
		{
			if (i != 0)
			{
				UnityEngine.Object.Destroy(this.gridTrans.GetChild(i).gameObject);
			}
		}
	}

	public void OnClickClose(Animator _animControll)
	{
		_animControll.SetTrigger("End");
	}

	public void OnClickSlot(string _category)
	{
		this.categoryPanel.SetActive(false);
		this.shopPanel.SetActive(true);
		this.LoadItemByCategory(_category);
	}

	public void BuysItem()
	{
		if (this._currentItemId >= 0)
		{
			SFSObject sfsobject = new SFSObject();
			sfsobject.PutInt("task", 1);
			SFSArray sfsarray = SFSArray.NewInstance();
			sfsarray.AddInt(this._currentItemId);
			sfsobject.PutSFSArray("data", sfsarray);
			ItemRequest.SendMessage(sfsobject);
		}
		this.detailPanel.SetActive(false);
	}

	public Transform gridTrans;

	public GameObject categoryPanel;

	public GameObject shopPanel;

	public ShopItem shopItemPrefabs;

	public BomberMainMenu bomberMainMenuScript;

	public GameObject detailPanel;

	public Image detailSprite;

	public Text detailCost;

	public Sprite[] buyButtonSprites;

	public Image buyButton;

	private int _currentItemId = -1;

	public AvatarController avatarController;

	private ShopController.Category currentCategory = ShopController.Category.Body;

	private enum Category
	{
		HairColor,
		Hat,
		Hair,
		Face,
		Body,
		Avarta,
		Bom
	}
}
