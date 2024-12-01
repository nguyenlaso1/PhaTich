// @sonhg: class: FeedBackBox
using System;
using UnityEngine;

public class FeedBackBox : MonoBehaviour
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
		string text = this._feedbackBox.value.Trim();
		if (!string.IsNullOrEmpty(text))
		{
			FeedBackRequest.SendMessage(text);
			this._feedbackBox.value = string.Empty;
			Context.Tooltip.AddMessage(Language.FEEDBACK_THANKS, 5, string.Empty, string.Empty);
		}
	}

	[SerializeField]
	private UIInput _feedbackBox;
}
