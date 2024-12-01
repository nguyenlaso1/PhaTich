// @sonhg: class: TopupCardInfo
using System;
using UnityEngine;
using UnityEngine.UI;

public class TopupCardInfo : MonoBehaviour
{
	public void AddInfo(string info, string coin, string diamond)
	{
		this._InfoLabel.text = info;
		this._coinLabel.text = coin;
		this._diamondLabel.text = diamond;
	}

	[SerializeField]
	private Text _InfoLabel;

	[SerializeField]
	private Text _coinLabel;

	[SerializeField]
	private Text _diamondLabel;
}
