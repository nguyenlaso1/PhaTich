// @sonhg: class: RankingItem
using System;
using UnityEngine;
using UnityEngine.UI;

public class RankingItem : MonoBehaviour
{
	public void AddInfo(int orderNumber, string name, string score)
	{
		this.rankText.text = orderNumber + string.Empty;
		this.nameText.text = name;
		this.scoreText.text = score;
	}

	[SerializeField]
	private Text rankText;

	[SerializeField]
	private Text nameText;

	[SerializeField]
	private Text scoreText;
}
