// @sonhg: class: SendChatMsg
using System;
using UnityEngine;
using UnityEngine.UI;

public class SendChatMsg : MonoBehaviour
{
	public void SendMsg()
	{
		if (this.CheckEmptyString(this.inputField.text))
		{
			LobbyMessageRequest.SendMessage(this.inputField.text);
		}
		this.inputField.text = string.Empty;
	}

	public void SendRoomMsg()
	{
		if (this.CheckEmptyString(this.inputField.text))
		{
			PublicMessageRequest.SendMessage(this.inputField.text);
		}
		this.inputField.text = string.Empty;
	}

	private bool CheckEmptyString(string text)
	{
		if (!string.IsNullOrEmpty(text))
		{
			string text2 = text.Trim();
			return text2.Length > 0;
		}
		return false;
	}

	public Text contentLabel;

	[SerializeField]
	private InputField inputField;
}
