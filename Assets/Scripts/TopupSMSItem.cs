// @sonhg: class: TopupSMSItem
using System;
using UnityEngine;
using UnityEngine.UI;

public class TopupSMSItem : MonoBehaviour
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
		if (acvite)
		{
			this.Button.onClick.AddListener(delegate()
			{
				this.OnSMSClick();
			});
			this.Button.enabled = true;
		}
		else
		{
			this.Button.enabled = false;
		}
	}

	public void OnSMSClick()
	{
		UnityEngine.Debug.Log(string.Concat(new string[]
		{
			this.sms.number,
			" ",
			this.sms.text,
			" ",
			SmartFoxConnection.Connection.MySelf.Name
		}));
		this.topup.OnSendSmsClick(this.sms.number, this.sms.text + " " + SmartFoxConnection.Connection.MySelf.Name);
	}

	[SerializeField]
	private Text realMoneyText;

	[SerializeField]
	private TopupBox topup;

	private Button _button;

	public SMSItem sms;

	public string price;
}
