// @sonhg: class: ShowTutorial
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ShowTutorial : MonoBehaviour
{
	private void Start()
	{
		this.firstRun = PlayerPrefs.GetInt(this.keyGuide, 0);
		if (this.firstRun == 0)
		{
			PlayerPrefs.SetInt(this.keyGuide, 1);
		}
		else
		{
			base.gameObject.SetActive(false);
		}
		if (this.readyButton != null)
		{
			if (this.readyButton.activeSelf)
			{
				this.handPointObjects[2].transform.Find("text").GetComponent<Text>().text = "Tap to ready";
			}
			else
			{
				this.handPointObjects[2].transform.Find("text").GetComponent<Text>().text = "Tap to start";
			}
		}
		if (this.keyGuide == "OfflineInGame")
		{
			UnityEngine.Object.Destroy(base.gameObject, 6f);
		}
	}

	public void OnClickArena()
	{
		this.handPointObjects[0].SetActive(false);
		this.handPointObjects[1].SetActive(false);
		if (this.firstRun == 0)
		{
			base.StartCoroutine(this.WaitAnimationSelectMode(0.5f, this.handPointObjects[2], true));
		}
	}

	private IEnumerator WaitAnimationSelectMode(float _time, GameObject _obj, bool _isShow)
	{
		yield return new WaitForSeconds(_time);
		_obj.SetActive(_isShow);
		yield break;
	}

	public void OnClickSelectMode()
	{
		this.handPointObjects[2].SetActive(false);
		if (this.firstRun == 0)
		{
			base.StartCoroutine(this.WaitAnimationSelectMode(0.2f, this.handPointObjects[3], true));
			base.StartCoroutine(this.WaitAnimationSelectMode(0.2f, this.handPointObjects[4], true));
		}
	}

	public void OnClickInvite()
	{
		this.handPointObjects[0].SetActive(false);
		this.handPointObjects[1].SetActive(true);
	}

	public void OnClickSelectMap()
	{
		this.handPointObjects[1].SetActive(false);
		this.handPointObjects[2].SetActive(true);
	}

	public void Skip()
	{
		base.gameObject.SetActive(false);
	}

	private int firstRun;

	public string keyGuide = string.Empty;

	[SerializeField]
	private GameObject[] handPointObjects;

	[SerializeField]
	private GameObject readyButton;
}
