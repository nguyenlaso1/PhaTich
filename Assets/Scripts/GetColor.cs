// @sonhg: class: GetColor
using System;
using UnityEngine;
using UnityEngine.UI;

public class GetColor : MonoBehaviour
{
	private void Start()
	{
		base.GetComponent<Button>().onClick.AddListener(delegate()
		{
			this.GetColorOfButton();
		});
	}

	private void GetColorOfButton()
	{
		foreach (Image image in this.focusObj)
		{
			image.color = base.gameObject.GetComponent<Image>().color;
		}
		Color color = base.gameObject.GetComponent<Image>().color;

        InventoryController.colorIndex = color.r.ToString("X2") + color.g.ToString("X2") + color.b.ToString("X2");//.ToHexStringRGB();
	}

	public Image[] focusObj;
}
