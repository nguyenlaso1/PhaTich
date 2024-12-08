// @sonhg: class: Offline_TopupBox
using System;
using UnityEngine;

public class Offline_TopupBox : BaseBox
{
	public event Offline_TopupBox.PurchaseIab OnPurchaseIab;

	protected override void Start()
	{
		base.Start();
	}

	public void AddInapp(string chip, string gold, string price, string sku)
	{
		if (this._countInapp < this._inappButtons.Length)
		{
			this._inappButtons[this._countInapp].gold = gold;
			this._inappButtons[this._countInapp].chip = chip;
			//this._inappButtons[this._countInapp].price = "$" + price;
			this._inappButtons[this._countInapp].price = price + " VNĐ";
			this._inappButtons[this._countInapp].sku = sku;
			this._inappButtons[this._countInapp].ActiveButton(Joker2XConfigUtils.TURN_ON_INAPP);
			this._countInapp++;
		}
	}

	public void OnPurchaseIabClick(string sku, Context.OnDeletegateNone OnSuccess = null)
	{
		UnityEngine.Debug.Log("Purchase sku:" + sku);
		if (this.OnPurchaseIab != null)
		{
			this.OnPurchaseIab(sku, OnSuccess);
		}
	}

	public void OnSendSmsClick(string number, string text)
	{
		Context.GameInfo.SendSMS(number, text);
	}

	public override void CloseBox()
	{
		base.gameObject.SetActive(false);
	}

	public void Show()
	{
		base.gameObject.SetActive(true);
	}

	public GameObject inAppGameObject;

	public GameObject inappButton;

	private int _countInapp;

	[SerializeField]
	private Offline_TopupInappItem[] _inappButtons;

	public delegate void PurchaseIab(string sku, Context.OnDeletegateNone OnSuccess);
}
