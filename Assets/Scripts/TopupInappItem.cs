// @sonhg: class: TopupInappItem
using System;
using UnityEngine;
using UnityEngine.UI;

public class TopupInappItem : MonoBehaviour
{
	public Button Button
	{
		get
		{
			if (this._button == null)
			{
				this._button = base.GetComponent<Button>();
			}
			return this._button;
		}
	}

	public void ActiveButton(bool acvite)
	{
		this.goldMoneyText.text = this.gold;
		this.ingameMoneyText.text = this.chip;
		this.realMoneyText.text = this.price;
		this.Button.onClick.AddListener(delegate()
		{
			this.OnInappClick();
		});
		this.Button.enabled = true;
	}

	public void OnInappClick()
	{
		try
		{
			this.topup.OnPurchaseIabClick(this.sku);
		}
		catch (Exception ex)
		{
			UnityEngine.Debug.Log("OnInappClick. exception" + ex.Message);
			Context.Tooltip.AddMessage("OnInappClick. exception" + ex.Message, 5, string.Empty, string.Empty);
		}
	}

	[SerializeField]
	private Text ingameMoneyText;

	[SerializeField]
	private Text realMoneyText;

	[SerializeField]
	private Text goldMoneyText;

	[SerializeField]
	private TopupBox topup;

	private Button _button;

	public string chip;

	public string gold;

	public string price;

	public string sku;
}
