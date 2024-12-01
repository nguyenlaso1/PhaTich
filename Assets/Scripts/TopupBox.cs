// @sonhg: class: TopupBox
using System;
using OnePF;
using UnityEngine;
using UnityEngine.UI;

public class TopupBox : BaseBox
{
	public event TopupBox.PurchaseIab OnPurchaseIab;

	public event TopupBox.SendSMS OnSendSMS;

	public event TopupBox.SubmitForm OnSubmitForm;

	protected override void Start()
	{
		base.Start();
		this.updateTab();
	}

	private void updateTab()
	{
		if (!Joker2XConfigUtils.TURN_ON_INAPP)
		{
		}
		if (Joker2XConfigUtils.TURN_ON_CARD || !Joker2XConfigUtils.TURN_ON_SMS)
		{
		}
		if (Joker2XConfigUtils.TURN_ON_INAPP || Joker2XConfigUtils.TURN_ON_CARD || !Joker2XConfigUtils.TURN_ON_SMS)
		{
		}
	}

	public void AddInapp(string chip, string gold, string price, string sku)
	{
		if (this._countInapp < this._inappButtons.Length)
		{
			this._inappButtons[this._countInapp].gold = gold;
			this._inappButtons[this._countInapp].chip = chip;
			this._inappButtons[this._countInapp].price = "$" + price;
			this._inappButtons[this._countInapp].sku = sku;
			this._inappButtons[this._countInapp].ActiveButton(Joker2XConfigUtils.TURN_ON_INAPP);
			this._countInapp++;
		}
	}

	public void AddSMS(SMSItem sms)
	{
		if (this._countSms < this._smsButtons.Length)
		{
			this._smsButtons[this._countSms].sms = sms;
			this._smsButtons[this._countSms].ActiveButton(Joker2XConfigUtils.TURN_ON_SMS);
			this._countSms++;
		}
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(StaticPrefab.GetPrefab("Prefabs/Joker2x/GameItem/TopupCardInfo2"));
		gameObject.GetComponent<TopupCardInfo>().AddInfo("SMS " + sms.Pay + "K: ", Joker2XUtils.FormatChip(sms.Chip), sms.Gold + string.Empty);
		gameObject.transform.SetParent(this.scrollRect.transform);
		gameObject.transform.localScale = Vector3.one;
		gameObject.transform.localPosition = Vector3.zero;
	}

	public void AddCard(CardItem card)
	{
		if (this._countCard < this._cardButtons.Length)
		{
			this._cardButtons[this._countCard].validationRequire = card.ValidationRequire;
			this._cardButtons[this._countCard].type = card.Type;
			this._cardButtons[this._countCard].distributor = card.Header;
			this._cardButtons[this._countCard].ActiveButton(Joker2XConfigUtils.TURN_ON_CARD);
			this._countCard++;
		}
		for (int i = 0; i < card.ItemList.Count; i++)
		{
			if (card.ItemList[i].Pay == 10000 || card.ItemList[i].Pay == 20000 || card.ItemList[i].Pay == 50000 || card.ItemList[i].Pay == 100000 || card.ItemList[i].Pay == 200000 || card.ItemList[i].Pay == 500000)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(StaticPrefab.GetPrefab("Prefabs/Joker2x/GameItem/TopupCardInfo2"));
				gameObject.GetComponent<TopupCardInfo>().AddInfo(card.Header + " " + Joker2XUtils.FormatChip(card.ItemList[i].Pay) + " : ", Joker2XUtils.FormatChip(card.ItemList[i].Chip), card.ItemList[i].Gold + string.Empty);
				gameObject.transform.SetParent(this.scrollRect.transform);
				gameObject.transform.localScale = Vector3.one;
				gameObject.transform.localPosition = Vector3.zero;
			}
		}
	}

	public void OpenSubmitForm(string distributor, string validationRequire, int type)
	{
		this.cardDisbleButton.SetActive(false);
		this.inAppDisbleButton.SetActive(false);
		this.cardGameObject.SetActive(false);
		this.inAppGameObject.SetActive(false);
		this.cardButton.SetActive(false);
		this.inappButton.SetActive(false);
		this._submitForm.OpenForm(distributor, validationRequire, type);
	}

	public void OnPurchaseIabClick(string sku)
	{
		UnityEngine.Debug.Log("Purchase sku:" + sku);
		if (this.OnPurchaseIab != null)
		{
			this.OnPurchaseIab(sku);
		}
	}

	public void OnInappTabClick()
	{
		this.inAppDisbleButton.SetActive(true);
		this.cardDisbleButton.SetActive(false);
		this.cardGameObject.SetActive(false);
		this.inAppGameObject.SetActive(true);
		this.cardButton.SetActive(true);
		this.inappButton.SetActive(false);
		string text = string.Empty;
		foreach (InappItem inappItem in ResourcesManager.inappList)
		{
			text = inappItem.Id.ToString();
			this.AddInapp(inappItem.Chip.ToString(), inappItem.Gold.ToString(), inappItem.Pay, inappItem.Id);
			if (!TopupResponse.isRegistered)
			{
				OpenIAB.mapSku(text, Context.GameInfo.StoreName, text);
			}
		}
		TopupResponse.isRegistered = true;
	}

	public void OnCardTabClick()
	{
		this._submitForm.gameObject.SetActive(false);
		this.cardDisbleButton.SetActive(true);
		this.inAppDisbleButton.SetActive(false);
		this.cardGameObject.SetActive(true);
		this.inAppGameObject.SetActive(false);
		this.cardButton.SetActive(false);
		this.inappButton.SetActive(true);
		this.scrollRect.transform.DestroyChildren();
		foreach (SMSItem sms in ResourcesManager.smsList)
		{
			this.AddSMS(sms);
		}
		foreach (CardItem card in ResourcesManager.cardList)
		{
			this.AddCard(card);
		}
	}

	public void OnSendSmsClick(string number, string text)
	{
		Context.GameInfo.SendSMS(number, text);
	}

	public VerticalLayoutGroup scrollRect;

	public GameObject cardGameObject;

	public GameObject inAppGameObject;

	public GameObject cardDisbleButton;

	public GameObject inAppDisbleButton;

	public GameObject cardButton;

	public GameObject inappButton;

	private int _countInapp;

	private int _countSms;

	private int _countCard;

	[SerializeField]
	private TopupSubmitForm _submitForm;

	[SerializeField]
	private TopupInappItem[] _inappButtons;

	[SerializeField]
	private TopupSMSItem[] _smsButtons;

	[SerializeField]
	private TopupCardItem[] _cardButtons;

	[SerializeField]
	private Toggle _tabInapp;

	[SerializeField]
	private Toggle _tabCar;

	[SerializeField]
	private GameObject _tabAtm;

	public delegate void PurchaseIab(string sku);

	public delegate void SendSMS(string price);

	public delegate void SubmitForm(JokerEnum.CardDistributor distributor, string serial, string pin);
}
