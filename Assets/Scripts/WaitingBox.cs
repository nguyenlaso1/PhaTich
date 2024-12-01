// @sonhg: class: WaitingBox
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class WaitingBox : MonoBehaviour
{
	public static WaitingBox Setup()
	{
		if (WaitingBox.instance == null)
		{
			WaitingBox.instance = UnityEngine.Object.Instantiate<GameObject>(Resources.Load("Prefabs/Joker2x/Boxs/WaitingBox") as GameObject);
			GameObject gameObject = GameObject.Find("Canvas");
			WaitingBox.instance.transform.SetParent(gameObject.transform);
			WaitingBox.instance.transform.localScale = Vector3.one;
			WaitingBox.instance.transform.localPosition = Vector3.zero;
			WaitingBox.instance.GetComponent<RectTransform>().anchorMin = Vector2.zero;
			WaitingBox.instance.GetComponent<RectTransform>().anchorMax = Vector2.one;
			WaitingBox.instance.GetComponent<RectTransform>().sizeDelta = Vector2.zero;
		}
		return WaitingBox.instance.GetComponent<WaitingBox>();
	}

	public void ShowWaiting()
	{
		base.gameObject.SetActive(true);
		base.GetComponent<RectTransform>().rect.Set(0f, 0f, 0f, 0f);
		this.icon.SetAlpha(1f);
		this.background.SetAlpha(0.5f);
	}

	public void ShowWaitingWithoutIcon()
	{
		this.ShowWaiting();
		this.icon.SetAlpha(0f);
		this.background.SetAlpha(0f);
	}

	public void HideWaiting()
	{
		base.gameObject.SetActive(false);
		base.StopAllCoroutines();
	}

	public void ShowWaiting(float time)
	{
		this.action = null;
		this.ShowWaiting();
		base.StartCoroutine(this.TimeOut(time));
	}

	public void ShowWaiting(float time, Action<Hashtable> action)
	{
		this.ShowWaiting();
		this.action = action;
		base.StopAllCoroutines();
		base.StartCoroutine(this.TimeOut(time));
	}

	private IEnumerator TimeOut(float time)
	{
		yield return new WaitForSeconds(time);
		UnityEngine.Debug.Log("TimeOut");
		base.gameObject.SetActive(false);
		if (this.action != null)
		{
			this.action(new Hashtable());
		}
		yield break;
	}

	public Action<Hashtable> action;

	[SerializeField]
	private Image icon;

	[SerializeField]
	private Image background;

	private static GameObject instance;
}
