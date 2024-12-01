// @sonhg: class: SlotCoin
using System;
using UnityEngine;
using UnityEngine.UI;

public class SlotCoin : MonoBehaviour
{
	public void SetCost(int _cost)
	{
		this.cost = _cost;
		base.transform.Find("Text").GetComponent<Text>().text = Joker2XUtils.FormatChip(_cost);
	}

	public int GetCost()
	{
		return this.cost;
	}

	private int cost;
}
