// @sonhg: class: InventoryController
using System;
using System.Collections.Generic;
using Bomb;
using Sfs2X.Entities.Data;
using UnityEngine;
using UnityEngine.UI;

public class InventoryController : MonoBehaviour
{
	private void Start()
	{
		this.Init();
	}

	public void Init()
	{
		this.avatarController.SetUser(SmartFoxConnection.Connection.MySelf);
	}

	private void DetectEquipedItem()
	{
		this.equipedItem.Clear();
		this.equipedItem.Add(MMOUserUtils.GetHead(SmartFoxConnection.Connection.MySelf));
		this.equipedItem.Add(MMOUserUtils.GetHair(SmartFoxConnection.Connection.MySelf));
		this.equipedItem.Add(MMOUserUtils.GetBody(SmartFoxConnection.Connection.MySelf));
		this.equipedItem.Add(MMOUserUtils.GetBomb(SmartFoxConnection.Connection.MySelf));
	}

	private void LoadItemByCategory(string _category)
	{
		switch (_category)
		{
		case "Hat":
			this.currentCategory = InventoryController.Category.Hat;
			this.LoadScrollViewItem(4);
			break;
		case "Hair":
			this.currentCategory = InventoryController.Category.Hair;
			this.LoadScrollViewItem(5);
			break;
		case "Bomb":
			this.currentCategory = InventoryController.Category.Bom;
			this.LoadScrollViewItem(3);
			break;
		case "Face":
			this.currentCategory = InventoryController.Category.Face;
			this.LoadScrollViewItem(6);
			break;
		case "Ava":
			this.currentCategory = InventoryController.Category.Avarta;
			this.LoadScrollViewItem(8);
			break;
		case "Body":
			this.currentCategory = InventoryController.Category.Body;
			this.LoadScrollViewItem(7);
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

	private void LoadScrollViewItem(int _category)
	{
		this.DetectEquipedItem();
		int childCount = this.gridTrans.childCount;
		for (int i = childCount - 1; i >= 0; i--)
		{
			if (i != 0)
			{
				UnityEngine.Object.Destroy(this.gridTrans.GetChild(i).gameObject);
			}
		}
		List<Item> list = new List<Item>();
		foreach (int num in InventoryController.myItemList)
		{
			if (ResourcesManager.ItemsDict.ContainsKey(num.ToString()))
			{
				list.Add(ResourcesManager.ItemsDict[num.ToString()]);
			}
		}
		foreach (Item item in list)
		{
			if (item.Category == _category && !string.IsNullOrEmpty(ResourcesUltis.ItemIdToLink(item.Id.ToString()).Replace(ResourceChecking.BaseIp(), string.Empty)))
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
				if (this.currentCategory != InventoryController.Category.Bom)
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
				if (this.equipedItem.IndexOf(_newShopItem.ItemId) >= 0)
				{
					_newShopItem.GetComponent<Outline>().enabled = true;
				}
			}
		}
		while (this.gridTrans.childCount < 9)
		{
			ShopItem shopItem = UnityEngine.Object.Instantiate<ShopItem>(this.shopItemPrefabs);
			shopItem.transform.SetParent(this.gridTrans);
			shopItem.transform.localPosition = Vector3.zero;
			shopItem.transform.localScale = Vector3.one;
		}
		this.shopPanel.GetComponent<ScrollRect>().verticalScrollbar.value = 1f;
	}

	public void ReloadItem()
	{
	}

	public void OnClickItem(GameObject _item)
	{
		ShopItem component = _item.GetComponent<ShopItem>();
		if (component.ItemId >= 0)
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
			this._currentItemId = component.ItemId;
			this.BuysItem();
			switch (this.currentCategory)
			{
			case InventoryController.Category.Hair:
				this.avatarController.SetHairById(int.Parse(_item.name));
				break;
			case InventoryController.Category.Face:
				this.avatarController.SetHeadById(int.Parse(_item.name));
				break;
			case InventoryController.Category.Body:
				this.avatarController.SetBody(int.Parse(_item.name));
				break;
			}
		}
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
	}

	public static string colorIndex = "FFFFFF";

	public static string currenColor = "FFFFF";

	public static List<int> myItemList = new List<int>();

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

	private InventoryController.Category currentCategory = InventoryController.Category.Body;

	private List<int> equipedItem = new List<int>();

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
