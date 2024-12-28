// @sonhg: class: Offline_ShopController
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BombOffline;
using DG.Tweening;
using OnePF;
using UnityEngine;
using UnityEngine.UI;
using BestHTTP;
using UnityEditor;
using System.Threading;

public class Offline_ShopController : MonoBehaviour
{
	protected void Start()
	{
        string[] array = DataManager.GetMyItem().Split(new char[]
		{
			'-'
		});
		foreach (string s in array)
		{
			int num;
			if (int.TryParse(s, out num))
			{
				this.myItemList.Add(int.Parse(s));
			}
		}
		this.UpdateCoin();
		this.UpdateAchievementStatus();
	}

	private void Update()
	{
		if (UnityEngine.Input.GetKeyDown(KeyCode.Escape))
		{
			BaseBox[] array = UnityEngine.Object.FindObjectsOfType<BaseBox>();
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i].gameObject.activeInHierarchy)
				{
					return;
				}
			}
			if (this.shopPanel.transform.parent.gameObject.activeInHierarchy)
			{
				return;
			}
			this.ConfirmExit();
		}
	}

	public void UpdateCoin()
	{
		this.playerCoinLabel[0].text = Joker2XUtils.FormatChip(Offline_ShopController.GetCharacterCoin());
		this.playerCoinLabel[1].text = Joker2XUtils.FormatChip(Offline_ShopController.GetCharacterCoin());
	}

	public void Init()
	{
		int[] array = new int[]
		{
			PlayerPrefs.GetInt("PlayerHead", 53),
			PlayerPrefs.GetInt("PlayerBody", 57),
			PlayerPrefs.GetInt("PlayerHair", 49)
		};
		this.SetSkinForPlayer(array, this.player.transform, TextureCutter.Parse("FFFFFF"));
		this.myUsedItemList.Clear();
		foreach (int item in array)
		{
			this.myUsedItemList.Add(item);
		}
		this.myUsedItemList.Add(PlayerPrefs.GetInt("PlayerBomb", 30));
		string text = string.Empty;
		for (int j = 0; j < 4; j++)
		{
			this._myCart[j] = this.myUsedItemList[j];
			text = ResourcesUltis.ItemIdToIconLink(this.myUsedItemList[j].ToString()).Replace(ResourceChecking.BaseIp(), string.Empty);
			Sprite sprite = Resources.Load<Sprite>("Textures/" + text.Substring(0, text.Length - 4));
			if (sprite == null && ResourcesManager.SpriteList.ContainsKey(text))
			{
				sprite = ResourcesManager.SpriteList[text];
			}
			this.shopSlotImage[j].GetComponent<Image>().sprite = sprite;
			if (j == 3)
			{
				this.bombImage.sprite = sprite;
			}
		}
	}

	public Sprite GetSpriteFromId(int _id)
	{
		string text = string.Empty;
		Sprite sprite = null;
		for (int i = 0; i < 4; i++)
		{
			text = ResourcesUltis.ItemIdToIconLink(_id.ToString()).Replace(ResourceChecking.BaseIp(), string.Empty);
			sprite = Resources.Load<Sprite>("Textures/" + text.Substring(0, text.Length - 4));
			if (sprite == null && ResourcesManager.SpriteList.ContainsKey(text))
			{
				sprite = ResourcesManager.SpriteList[text];
			}
		}
		return sprite;
	}

	public void SetSkinForPlayer(int[] _id, Transform _char, Color _hairColor)
	{
		Texture2D[] textureArr = new Texture2D[]
		{
			TextureCutter.GetSkinTexture(_id[0]),
			TextureCutter.GetSkinTexture(_id[1]),
			TextureCutter.GetSkinTexture(_id[2])
		};
		TextureCutter.CutAll(textureArr, _char, _hairColor);
	}

	public void LoadItemByCategory(string _category)
	{
		switch (_category)
		{
			case "Hair":
				this.currentCategory = Offline_ShopController.Category.Hair;
				this.LoadScrollViewItem(5);
				break;
			case "Bomb":
				this.currentCategory = Offline_ShopController.Category.Bom;
				this.LoadScrollViewItem(3);
				break;
			case "Face":
				this.currentCategory = Offline_ShopController.Category.Face;
				this.LoadScrollViewItem(6);
				break;
			case "Body":
				this.currentCategory = Offline_ShopController.Category.Body;
				this.LoadScrollViewItem(7);
				break;
		}
	}

	private void LoadScrollViewItem(int _category)
	{
		int num = 0;
		int num2 = 0;
		this.ResetShopItem();
		List<Item> list = ResourcesManager.ItemsDict.Values.ToList<Item>();
		this.shopPanel.gameObject.SetActive(true);
		foreach (Item item in list)
		{
			if (item.Category == _category && !string.IsNullOrEmpty(ResourcesUltis.ItemIdToLink(item.Id.ToString()).Replace(ResourceChecking.BaseIp(), string.Empty)) && !string.IsNullOrEmpty(ResourcesUltis.ItemIdToIconLink(item.Id.ToString())))
			{
				int num3 = Mathf.FloorToInt((float)(num / 18) + 0.5f);
				num++;
				this.shopPageArr[num3].SetActive(true);
				if (num2 > 17)
				{
					num2 = 0;
				}
				ShopItem _newShopItem = this.shopPageArr[num3].transform.GetChild(num2).GetComponent<ShopItem>();
				_newShopItem.ItemId = item.Id;
				_newShopItem.transform.GetComponent<Button>().onClick.RemoveAllListeners();
				_newShopItem.transform.GetComponent<Button>().onClick.AddListener(delegate ()
				{
					this.OnClickItem(_newShopItem.gameObject);
				});
				num2++;
				_newShopItem.name = item.Id.ToString();
				Item item2 = ResourcesManager.ItemsDict[item.Id.ToString()];
				string text = string.Empty;
				text = ResourcesUltis.ItemIdToIconLink(item.Id.ToString()).Replace(ResourceChecking.BaseIp(), string.Empty);
				Sprite sprite = Resources.Load<Sprite>("Textures/" + text.Substring(0, text.Length - 4));
				if (sprite == null && ResourcesManager.SpriteList.ContainsKey(text))
				{
					sprite = ResourcesManager.SpriteList[text];
				}
				if (this._myCart.IndexOf(item.Id) >= 0)
				{
					_newShopItem.GetComponent<Outline>().enabled = true;
				}
				else
				{
					_newShopItem.GetComponent<Outline>().enabled = false;
				}
				if (this.myItemList.IndexOf(item.Id) >= 0)
				{
					_newShopItem.SetItem(sprite, 0);
					_newShopItem.canByGold = true;
				}
				else
				{
					_newShopItem.canByGold = false;
					_newShopItem.SetItem(sprite, item2.Price);
				}
			}
		}
		base.StartCoroutine(this.SetShopPanelToLeft());
	}

	private IEnumerator SetShopPanelToLeft()
	{
		yield return new WaitForSeconds(0.5f);
		DOTween.To(() => this.shopPanel.horizontalNormalizedPosition, delegate (float x)
		{
			this.shopPanel.horizontalNormalizedPosition = x;
		}, 0f, 0.5f);
		yield break;
	}

	private void ResetShopItem()
	{
		for (int i = 0; i < 3; i++)
		{
			if (this.shopPageArr[i].activeSelf)
			{
				for (int j = 0; j < 18; j++)
				{
					ShopItem component = this.shopPageArr[i].transform.GetChild(j).GetComponent<ShopItem>();
					component.itemSprite.enabled = false;
					component.costLabel.transform.parent.gameObject.SetActive(false);
					component.GetComponent<Outline>().enabled = false;
					component.cost = 0;
					this.shopPageArr[i].transform.GetChild(j).GetComponent<Button>().onClick.RemoveAllListeners();
				}
				this.shopPageArr[i].SetActive(false);
			}
		}
	}

	public static int GetCharacterCoin()
	{
		return DataManager.GetGold();
	}

	public static void SetCharacterCoint(int gold)
	{
		DataManager.SetGold(gold);
	}

	public void CancelSpecialItem(int _index, Transform _moneyButton, int _cost)
	{
		switch (_index)
		{
			case 0:
				{
					int @int = PlayerPrefs.GetInt("PlayerHead", 53);
					TextureCutter.CutHead(TextureCutter.GetSkinTexture(@int), this.player.transform);
					this._myCart[0] = @int;
					if (this.GetSpriteFromId(@int) != null)
					{
						this.shopSlotImage[0].GetComponent<Image>().sprite = this.GetSpriteFromId(@int);
					}
					break;
				}
			case 1:
				{
					int @int = PlayerPrefs.GetInt("PlayerBody", 57);
					TextureCutter.CutBody(TextureCutter.GetSkinTexture(@int), this.player.transform);
					this._myCart[1] = @int;
					if (this.GetSpriteFromId(@int) != null)
					{
						this.shopSlotImage[1].GetComponent<Image>().sprite = this.GetSpriteFromId(@int);
					}
					break;
				}
			case 2:
				{
					int @int = PlayerPrefs.GetInt("PlayerHair", 49);
					TextureCutter.CutHair(TextureCutter.GetSkinTexture(@int), this.player.transform, Color.white);
					this._myCart[2] = @int;
					if (this.GetSpriteFromId(@int) != null)
					{
						this.shopSlotImage[2].GetComponent<Image>().sprite = this.GetSpriteFromId(@int);
					}
					break;
				}
			case 3:
				{
					int @int = PlayerPrefs.GetInt("PlayerBomb", 30);
					this._myCart[3] = @int;
					if (this.GetSpriteFromId(@int) != null)
					{
						this.shopSlotImage[3].GetComponent<Image>().sprite = this.GetSpriteFromId(@int);
						this.bombImage.sprite = this.shopSlotImage[3].GetComponent<Image>().sprite;
					}
					break;
				}
		}
		this.CaculateTotalCost();
		_moneyButton.gameObject.SetActive(false);
	}

	public void OnClickItem(GameObject _item)
	{
		ShopItem _shopItemScript = _item.GetComponent<ShopItem>();
		Sprite sprite = _item.GetComponent<ShopItem>().itemSprite.sprite;
		switch (this.currentCategory)
		{
			case Offline_ShopController.Category.Hair:
				TextureCutter.CutHair(TextureCutter.GetSkinTexture(_shopItemScript.ItemId), this.player.transform, Color.white);
				this._myCart[2] = _shopItemScript.ItemId;
				this.shopSlotImage[2].GetComponent<Image>().sprite = sprite;
				if (!_shopItemScript.canByGold)
				{
					Transform _moneyButton = this.shopSlotImage[2].parent.Find("Money");
					if (!_shopItemScript.canByGold)
					{
						_moneyButton.gameObject.SetActive(true);
					}
					else
					{
						_moneyButton.gameObject.SetActive(false);
					}
					_moneyButton.GetChild(0).GetComponent<Text>().text = _shopItemScript.cost.ToString();
					_moneyButton.GetComponent<Button>().onClick.RemoveAllListeners();
					_moneyButton.GetComponent<Button>().onClick.AddListener(delegate ()
					{
						this.CancelSpecialItem(2, _moneyButton, _shopItemScript.cost);
					});
				}
				else
				{
					this.myUsedItemList[2] = _shopItemScript.ItemId;
					PlayerPrefs.SetInt("PlayerHair", _shopItemScript.ItemId);
					this.shopSlotImage[2].parent.Find("Money").gameObject.SetActive(false);
				}
				break;
			case Offline_ShopController.Category.Face:
				TextureCutter.CutHead(TextureCutter.GetSkinTexture(_shopItemScript.ItemId), this.player.transform);
				this._myCart[0] = _shopItemScript.ItemId;
				this.shopSlotImage[0].GetComponent<Image>().sprite = sprite;
				if (!_shopItemScript.canByGold)
				{
					Transform _moneyButton = this.shopSlotImage[0].parent.Find("Money");
					if (!_shopItemScript.canByGold)
					{
						_moneyButton.gameObject.SetActive(true);
					}
					else
					{
						_moneyButton.gameObject.SetActive(false);
					}
					_moneyButton.GetChild(0).GetComponent<Text>().text = _shopItemScript.cost.ToString();
					_moneyButton.GetComponent<Button>().onClick.RemoveAllListeners();
					_moneyButton.GetComponent<Button>().onClick.AddListener(delegate ()
					{
						this.CancelSpecialItem(0, _moneyButton, _shopItemScript.cost);
					});
				}
				else
				{
					this.myUsedItemList[0] = _shopItemScript.ItemId;
					PlayerPrefs.SetInt("PlayerHead", _shopItemScript.ItemId);
					this.shopSlotImage[0].parent.Find("Money").gameObject.SetActive(false);
				}
				break;
			case Offline_ShopController.Category.Body:
				TextureCutter.CutBody(TextureCutter.GetSkinTexture(_shopItemScript.ItemId), this.player.transform);
				this._myCart[1] = _shopItemScript.ItemId;
				this.shopSlotImage[1].GetComponent<Image>().sprite = sprite;
				if (!_shopItemScript.canByGold)
				{
					Transform _moneyButton = this.shopSlotImage[1].parent.Find("Money");
					if (!_shopItemScript.canByGold)
					{
						_moneyButton.gameObject.SetActive(true);
					}
					else
					{
						_moneyButton.gameObject.SetActive(false);
					}
					_moneyButton.GetChild(0).GetComponent<Text>().text = _shopItemScript.cost.ToString();
					_moneyButton.GetComponent<Button>().onClick.RemoveAllListeners();
					_moneyButton.GetComponent<Button>().onClick.AddListener(delegate ()
					{
						this.CancelSpecialItem(1, _moneyButton, _shopItemScript.cost);
					});
				}
				else
				{
					this.myUsedItemList[1] = _shopItemScript.ItemId;
					PlayerPrefs.SetInt("PlayerBody", _shopItemScript.ItemId);
					this.shopSlotImage[1].parent.Find("Money").gameObject.SetActive(false);
				}
				break;
			case Offline_ShopController.Category.Bom:
				this.bombImage.sprite = sprite;
				this._myCart[3] = _shopItemScript.ItemId;
				this.shopSlotImage[3].GetComponent<Image>().sprite = sprite;
				if (!_shopItemScript.canByGold)
				{
					Transform _moneyButton = this.shopSlotImage[3].parent.Find("Money");
					if (!_shopItemScript.canByGold)
					{
						_moneyButton.gameObject.SetActive(true);
					}
					else
					{
						_moneyButton.gameObject.SetActive(false);
					}
					_moneyButton.GetChild(0).GetComponent<Text>().text = _shopItemScript.cost.ToString();
					_moneyButton.GetComponent<Button>().onClick.RemoveAllListeners();
					_moneyButton.GetComponent<Button>().onClick.AddListener(delegate ()
					{
						this.CancelSpecialItem(3, _moneyButton, _shopItemScript.cost);
					});
				}
				else
				{
					this.myUsedItemList[3] = _shopItemScript.ItemId;
					PlayerPrefs.SetInt("PlayerBomb", _shopItemScript.ItemId);
					this.shopSlotImage[3].parent.Find("Money").gameObject.SetActive(false);
				}
				break;
		}
		this.CaculateTotalCost();
		this.shopPanel.gameObject.SetActive(false);
	}

	private void CaculateTotalCost()
	{
		this.totalCost = 0;
		foreach (int num in this._myCart)
		{
			if (num > 0 && this.myItemList.IndexOf(num) < 0)
			{
				this.totalCost += ResourcesManager.ItemsDict[num.ToString()].Price;
			}
		}
		this.coinLabel.text = this.totalCost.ToString();
		if (this.totalCost > 0)
		{
			this.buyButton.SetActive(true);
		}
		else
		{
			this.buyButton.SetActive(false);
		}
	}

	public void OnClickInappButton()
	{
		this.ShowTopUpInApp();
		this.topupBox.Show();
	}

	public void ShowTopUpInApp()
	{
		foreach (InappItem inappItem in ResourcesManager.inappList)
		{
			this.topupBox.AddInapp(inappItem.Chip + string.Empty, Joker2XUtils.FormatChip(inappItem.Gold), inappItem.Pay, inappItem.Id);
			if (!Offline_ShopController.isRegistered)
			{
				OpenIAB.mapSku(inappItem.Id, Context.GameInfo.StoreName, inappItem.Id);
				OpenIAB.mapSku(inappItem.Id, Context.GameInfo.StoreName, inappItem.Id);
			}
		}
		Offline_ShopController.isRegistered = true;
	}

	public void OnClickBuyItem()
	{
		if (Offline_ShopController.GetCharacterCoin() >= this.totalCost)
		{
			Offline_ShopController.SetCharacterCoint(Offline_ShopController.GetCharacterCoin() - this.totalCost);
			for (int i = 0; i < this._myCart.Count; i++)
			{
				if (this._myCart[i] > 0 && this.myItemList.IndexOf(this._myCart[i]) < 0)
				{
					this.myItemList.Add(this._myCart[i]);
				}
				if (this._myCart[i] > 0)
				{
					this.myUsedItemList[i] = this._myCart[i];
					switch (i)
					{
						case 0:
							PlayerPrefs.SetInt("PlayerHead", this._myCart[i]);
							break;
						case 1:
							PlayerPrefs.SetInt("PlayerBody", this._myCart[i]);
							break;
						case 2:
							PlayerPrefs.SetInt("PlayerHair", this._myCart[i]);
							break;
						case 3:
							PlayerPrefs.SetInt("PlayerBomb", this._myCart[i]);
							break;
					}
				}
			}
			this.buyButton.SetActive(false);
			this.UpdateCoin();
			List<string> list = DataManager.GetMyItem().Split(new char[]
			{
				'-'
			}).ToList<string>();
			string text = string.Empty;
			foreach (int num in this.myItemList)
			{
				if (!text.Contains(num.ToString()))
				{
					text = text + "-" + num.ToString();
				}
			}
			DataManager.SetMyItem(text);
			foreach (Transform transform in this.shopSlotImage)
			{
				transform.parent.Find("Money").gameObject.SetActive(false);
			}
			//this.confirmPopUp.AddMessageYes("Buy Success", null, null, string.Empty);
			this.confirmPopUp.AddMessageYes("Mua thành công", null, null, string.Empty);
		}
		else
		{
			this.OnClickInappButton();
		}
	}

	public void CancelChangingItem()
	{
		foreach (Transform transform in this.shopSlotImage)
		{
			transform.parent.Find("Money").gameObject.SetActive(false);
		}
		for (int j = 0; j < this._myCart.Count; j++)
		{
			this._myCart[j] = 0;
		}
		int[] array2 = new int[]
		{
			PlayerPrefs.GetInt("PlayerHead", 53),
			PlayerPrefs.GetInt("PlayerBody", 57),
			PlayerPrefs.GetInt("PlayerHair", 49)
		};
		this.SetSkinForPlayer(array2, this.player.transform, TextureCutter.Parse("FFFFFF"));
		this.myUsedItemList.Clear();
		foreach (int item in array2)
		{
			this.myUsedItemList.Add(item);
		}
		this.myUsedItemList.Add(PlayerPrefs.GetInt("PlayerBomb", 30));
		this.buyButton.SetActive(false);
		this.Init();
		PlayerPrefs.Save();
	}

	public void ConfirmExit()
	{
		this.mapChooser.gameObject.SetActive(false);
		this.confirmPopUp.AddMessageYesNo("Do you want exit ?", delegate (object o)
		{
			Application.Quit();
		}, null, delegate (object o)
		{
			this.mapChooser.gameObject.SetActive(true);
		}, null, string.Empty, string.Empty, false);
	}

	public void Achievement()
	{
		this.achievementBox.SetActive(true);
	}

	// private void ResponseRequest(HTTPRequest request, HTTPResponse response)
	//   {
	// 	Debug.Log("State: " + request.State);
	// 	switch (request.State)
	// 	{

	// 		// The request finished without any problem.
	// 		case HTTPRequestStates.Finished:
	// 			Debug.Log("Finish: " + response.Message + "--" + response.DataAsText);
	// 			break;

	// 		// The request finished with an unexpected error. The request's Exception property may contain more info about the error.
	// 		case HTTPRequestStates.Error:
	// 			Debug.Log("Error: " + response.Data);
	// 			break;

	// 		// The request aborted, initiated by the user.
	// 		case HTTPRequestStates.Aborted:
	// 			Debug.Log("Aborted: " + response.Data);
	// 			break;

	// 		// Connecting to the server is timed out.
	// 		case HTTPRequestStates.ConnectionTimedOut:
	// 			Debug.Log("ConnectionTimedOut: " + response.Data);
	// 			break;

	// 		// The request didn't finished in the given time.
	// 		case HTTPRequestStates.TimedOut:
	// 			Debug.Log("TimedOut: " + response.Data);
	// 			break;
	// 	}
	// }

	public void LeaderBoard()
	{
		this.leaderboardBox.SetActive(true);
		// HTTPRequest request = new HTTPRequest(new Uri("https://google.com"), HTTPMethods.Get, new OnRequestFinishedDelegate(ResponseRequest));
		// request.Send();
		StartCoroutine(API.Instance.GetTopScore(FillLeaderBoard));
	}

	public void FillLeaderBoard(string res)
	{
		JSONObject json = new JSONObject(res);
		if (json["error"].b == false)
		{
			var data = json["data"];
			Debug.Log("Json: " + json["data"]);
			int playerCount = data.Count;
			List<Player> playerList = new List<Player>();
			for (int i = 0; i < playerCount; i++)
			{
				try
                {
                    Player player = new Player();
                    player.Name = data[i]["name"].str;
                    int map = Convert.ToInt16(data[i]["map"].i);
                    Debug.Log("Map:" + map);
                    player.Map = map;
                    player.Level = data[i]["score"].i;
                    playerList.Add(player);
                }
				catch(Exception) { }    
			}

			playerList.Sort((a, b) =>
			{
				if (a.Map < b.Map) return 1;
				else if (a.Map > b.Map) return -1;
				else
				{
					if(a.Level< b.Level) return 1;

					else if(a.Level> b.Level) return -1;
					return 0;
				};
			});

			int rank = 1;
			foreach (Player p in playerList)
			{
				GameObject leaderBoardItem = Instantiate(Resources.Load<GameObject>("prefabs/LeaderBoardItem")) as GameObject;
				leaderBoardItem.transform.parent = this.leaderboardBox.transform.GetChild(1).GetChild(0).GetChild(2).GetChild(0).GetChild(0);
				leaderBoardItem.transform.localScale = new Vector3(1, 1, 1);
				leaderBoardItem.transform.localPosition = new Vector3(leaderBoardItem.transform.localPosition.x, leaderBoardItem.transform.localPosition.y, 0);
				leaderBoardItem.transform.Find("Rank").GetComponent<Text>().text = Convert.ToString(rank);
				leaderBoardItem.transform.Find("Name").GetComponent<Text>().text = p.Name;
				string strLevel = Convert.ToString(p.Map) + "-" + Convert.ToString(p.Level);
				Debug.Log(strLevel);
				leaderBoardItem.transform.Find("Level").GetComponent<Text>().text = strLevel;
				rank++;
			}

		}
	}
	public void UpdateAchievementStatus()
	{
		if (this.checkAchievement())
		{
			this.notice.SetActive(true);
			this.notice.transform.DOScale(1.2f, 0.7f).SetLoops(-1);
		}
		else
		{
			this.notice.SetActive(false);
		}
	}

	private bool checkAchievement()
	{
		//List<string> list = Achievements.listAchievement.Keys.ToList<string>();
		//foreach (string text in list)
		//{
		//	int num = DataManager.AchievementCount(text);
		//	int num2 = DataManager.AchievementProgess(text);
		//	List<Achievement> list2 = Achievements.listAchievement[text];
		//	if (num2 < list2.Count)
		//	{
		//		if (num >= list2[num2].total)
		//		{
		//			return true;
		//		}
		//	}
		//}
		return false;
	}

	public ScrollRect shopPanel;

	public ConfirmBox confirmPopUp;

	public Text[] playerCoinLabel;

	public Image bombImage;

	public Text coinLabel;

	public GameObject buyButton;

	public GameObject player;

	public GameObject[] shopPageArr;

	public Transform[] shopSlotImage;

	private Offline_ShopController.Category currentCategory = Offline_ShopController.Category.Body;

	public List<int> myUsedItemList = new List<int>();

	public List<int> myItemList = new List<int>();

	private List<int> _myCart = new List<int>
	{
		0,
		0,
		0,
		0
	};

	private int totalCost;

	public Offline_TopupBox topupBox;

	public GameObject achievementBox;

	public GameObject leaderboardBox;

	public GameObject notice;

	public GameObject mapChooser;

	public static bool isRegistered;

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
