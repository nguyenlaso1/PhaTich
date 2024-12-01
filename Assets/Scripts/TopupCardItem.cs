// @sonhg: class: TopupCardItem
using System;
using UnityEngine;
using UnityEngine.UI;

public class TopupCardItem : MonoBehaviour
{
	private void Start()
	{
	}

	public void ActiveButton(bool acvite)
	{
		if (!acvite)
		{
			base.GetComponent<Button>().enabled = false;
		}
	}

	public void OnCardButtonClick()
	{
		this.topup.OpenSubmitForm(this.distributor, this.validationRequire, this.type);
	}

	[SerializeField]
	private TopupBox topup;

	public string distributor;

	public string validationRequire;

	public int type;
}
