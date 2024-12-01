// @sonhg: class: QuickBuyButton
using System;
using Bomb;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class QuickBuyButton : MonoBehaviour
{
	public int Id
	{
		get
		{
			return this._id;
		}
		set
		{
			this._id = value;
		}
	}

	public void AddOnClickEvent(UnityAction action)
	{
		this.confirmButton.onClick.RemoveAllListeners();
		this.confirmButton.onClick.AddListener(action);
		this.confirmButton.onClick.AddListener(delegate()
		{
			this.confirmButton.gameObject.SetActive(false);
		});
	}

	public void SetInformationById(int id)
	{
		this._id = id;
		this.confirmButton.onClick.RemoveAllListeners();
		this.confirmButton.onClick.AddListener(delegate()
		{
			BuyItemInGameRequest.SendMessage(this._id);
			this.confirmButton.gameObject.SetActive(false);
		});
		Item item = ResourcesManager.ItemsDict[id.ToString()];
		this.text.text = item.Price + string.Empty;
		this.image.sprite = ResourcesManager.GetSprite(item.Path);
	}

	public void ShowConfirmButton()
	{
		this.confirmButton.gameObject.SetActive(true);
		this.confirmButton.transform.DOKill(false);
		this.confirmButton.transform.rotation = Quaternion.Euler(new Vector3(0f, 90f, 0f));
		this.confirmButton.transform.DOLocalRotate(new Vector3(0f, 0f, 0f), 0.5f, RotateMode.Fast).OnComplete(delegate
		{
			this.confirmButton.transform.DOLocalRotate(new Vector3(0f, 90f, 0f), 0.5f, RotateMode.Fast).SetDelay(4f).OnComplete(delegate
			{
				this.confirmButton.gameObject.SetActive(false);
			});
		});
	}

	[SerializeField]
	private Image image;

	[SerializeField]
	private Text text;

	[SerializeField]
	private Button confirmButton;

	private int _id;
}
