// @sonhg: class: Offline_Item
using System;
using UnityEngine;
using UnityEngine.UI;

public class Offline_Item : MonoBehaviour
{
	private void Start()
	{
		this.number = DataManager.GetMyItemHelper(this.id);
		this.numberLabel.text = this.number.ToString();
		base.transform.Find("Cost").GetComponent<Text>().text = this.cost.ToString();
		if (this.number >= this.maxNumber)
		{
			base.transform.Find("Cost").GetComponent<Text>().text = "Max";
		}
	}

	public void AddItem()
	{
		if (this.number < this.maxNumber)
		{
			this.number++;
			DataManager.AddMyItemHelper(this.id, 1);
			if (this.number <= this.maxNumber)
			{
				this.numberLabel.text = this.number.ToString();
				if (this.number == this.maxNumber)
				{
					base.transform.Find("Cost").GetComponent<Text>().text = "Max";
				}
			}
		}
		else
		{
			base.transform.Find("Cost").GetComponent<Text>().text = "Max";
		}
		this.numberLabel.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
		iTween.ScaleTo(this.numberLabel.gameObject, Vector3.one, 0.2f);
	}

	public void RemoveItem()
	{
		if (this.number > 0)
		{
			this.number--;
			this.numberLabel.text = this.number.ToString();
		}
		this.numberLabel.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
		iTween.ScaleTo(this.numberLabel.gameObject, Vector3.one, 0.5f);
	}

	public int GetItemNumber()
	{
		return this.number;
	}

	[SerializeField]
	private Text numberLabel;

	[HideInInspector]
	public bool _isSelected;

	public int cost;

	public string id = string.Empty;

	private int number;

	public int maxNumber;
}
