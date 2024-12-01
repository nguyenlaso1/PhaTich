// @sonhg: class: ChatBox
using System;
using UnityEngine;

public class ChatBox : MonoBehaviour
{
	private void Start()
	{
	}

	private void Update()
	{
	}

	public void CloseBox()
	{
		NGUITools.Destroy(base.gameObject);
	}

	public void OnSubmit()
	{
		string text = this._chatbox.value.Trim();
		if (!string.IsNullOrEmpty(text))
		{
			PublicMessageRequest.SendMessage(null, text);
			this._chatbox.value = string.Empty;
		}
	}

	[SerializeField]
	private UIInput _chatbox;
}
