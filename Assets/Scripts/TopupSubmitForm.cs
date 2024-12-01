// @sonhg: class: TopupSubmitForm
using System;
using Sfs2X.Entities.Data;
using UnityEngine;
using UnityEngine.UI;

public class TopupSubmitForm : MonoBehaviour
{
	public void OpenForm(string distributor, string validationRequire, int type)
	{
		UnityEngine.Debug.LogError("Ope Form");
		if (type != this._type)
		{
			this._pinInput.text = string.Empty;
			this._serialInput.text = string.Empty;
		}
		this._type = type;
		this._validationRequire = validationRequire;
		this._distributor = distributor;
		this._cardNameLabel.text = this._distributor;
		base.gameObject.SetActive(true);
	}

	public void CloseForm()
	{
		this._serialInput.text = string.Empty;
		this._pinInput.text = string.Empty;
		UnityEngine.Debug.LogError("NAME: " + base.gameObject.name);
		this.closeButton.SetActive(false);
		this.topup.OnCardTabClick();
	}

	private void Close()
	{
		base.gameObject.SetActive(false);
	}

	public void OnSubmitFormClick()
	{
		if (string.IsNullOrEmpty(this._serialInput.text) && string.IsNullOrEmpty(this._pinInput.text))
		{
			this._serialInput.GetComponent<Animator>().SetTrigger("Notice");
			this._pinInput.GetComponent<Animator>().SetTrigger("Notice");
		}
		if (string.IsNullOrEmpty(this._serialInput.text))
		{
			this._serialInput.GetComponent<Animator>().SetTrigger("Notice");
			return;
		}
		if (string.IsNullOrEmpty(this._pinInput.text))
		{
			this._pinInput.GetComponent<Animator>().SetTrigger("Notice");
			return;
		}
		string validationRequire = this._validationRequire;
		if (!this.isValidCardTextInput(validationRequire.Split(new char[]
		{
			"|"[0]
		})[0], this._serialInput.text) || !this.isValidCardTextInput(validationRequire.Split(new char[]
		{
			"|"[0]
		})[1], this._pinInput.text))
		{
			Context.Messenger.AddOkMessage(Language.DATA_MISTYPED, null, null, string.Empty, string.Empty);
			return;
		}
		Context.Messenger.AddOkMessage(Language.TOPUP_IN_PROGRESS, null, null, string.Empty, string.Empty);
		SFSObject sfsobject = new SFSObject();
		sfsobject.PutInt("task", 3);
		sfsobject.PutUtfString("tu-serial", this._serialInput.text);
		sfsobject.PutUtfString("tu-pin", this._pinInput.text);
		sfsobject.PutInt("tu-type", this._type);
		CardSMSRequest.SendMessage(sfsobject);
		this.CloseForm();
	}

	public bool isValidCardTextInput(string validationString, string inputText)
	{
		string[] array = validationString.Split(new char[]
		{
			"-"[0]
		});
		if (array.Length < 4)
		{
			return true;
		}
		int num = int.Parse(array[1]);
		int num2 = int.Parse(array[2]);
		int num3 = int.Parse(array[3]);
		if (num2 > inputText.Length || num3 < inputText.Length)
		{
			return false;
		}
		switch (num)
		{
		case 0:
			foreach (char c in inputText)
			{
				if ((c < '0' || c > '9') && (c < 'a' || c > 'z') && (c < 'A' || c > 'Z'))
				{
					return false;
				}
			}
			break;
		case 1:
			foreach (char c in inputText)
			{
				if ((c < 'a' || c > 'z') && (c < 'A' || c > 'Z'))
				{
					return false;
				}
			}
			break;
		case 2:
			foreach (char c in inputText)
			{
				if (c < '0' || c > '9')
				{
					return false;
				}
			}
			break;
		}
		return true;
	}

	[SerializeField]
	private TopupBox topup;

	[SerializeField]
	private InputField _serialInput;

	[SerializeField]
	private InputField _pinInput;

	[SerializeField]
	private Text _cardNameLabel;

	private string _distributor;

	private string _validationRequire;

	private int _type;

	public GameObject closeButton;
}
