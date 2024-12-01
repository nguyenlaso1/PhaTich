// @sonhg: class: RankingBox
using System;
using System.Collections;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Sfs2X.Entities.Data;
using UnityEngine;
using UnityEngine.UI;

public class RankingBox : BaseBox
{
	private new void Start()
	{
		if (this.defaultTab != null)
		{
			this.OnTabChange(this.defaultTab);
		}
	}

	private IEnumerator ResetTabPosition()
	{
		yield return new WaitForSeconds(0.1f);
		this.scrollRect.verticalNormalizedPosition = 1f;
		yield break;
	}

	public void OnTabChange(Toggle tabToggle)
	{
		if (tabToggle.isOn)
		{
			string name = tabToggle.name;
			switch (name)
			{
			case "MostCoinToggle":
				this.topText.text = "Top Coin";
				RankingRequest.SendMessage(this.GenerateParameter(1));
				break;
			case "MostLevelToggle":
				this.topText.text = "Top Level";
				RankingRequest.SendMessage(this.GenerateParameter(0));
				break;
			case "MostDiamondToggle":
				this.topText.text = "Top Diamond";
				RankingRequest.SendMessage(this.GenerateParameter(2));
				break;
			}
		}
	}

	public void ShowRanking(int task, SFSArray array)
	{
		this.contentHolder.DestroyChildren();
		this.FillContentHolder(task, array);
		base.StartCoroutine(this.ResetTabPosition());
	}

	private void FillContentHolder(int task, SFSArray array)
	{
		for (int i = 0; i < array.Size(); i++)
		{
			ISFSObject sfsobject = array.GetSFSObject(i);
			string utfString = sfsobject.GetUtfString("displayname");
			string score = string.Empty;
			switch (task)
			{
			case 0:
				score = sfsobject.GetInt("level") + string.Empty;
				break;
			case 1:
				score = sfsobject.GetInt("chip") + string.Empty;
				break;
			case 2:
				score = sfsobject.GetInt("gold") + string.Empty;
				break;
			}
			this.AddItem(i + 1, utfString, score);
		}
	}

	private void AddItem(int rank, string name, string score)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.rankingItemPrefab);
		gameObject.transform.SetParent(this.contentHolder, false);
		if (rank < 7)
		{
			CanvasGroup group = gameObject.transform.GetComponent<CanvasGroup>();
			group.alpha = 0f;
			DOTween.To(() => group.alpha, delegate(float alpha)
			{
				group.alpha = alpha;
			}, 1f, 3f).SetDelay((float)rank * 0.1f);
		}
		RankingItem component = gameObject.GetComponent<RankingItem>();
		component.AddInfo(rank, name, score);
	}

	private SFSObject GenerateParameter(int param)
	{
		SFSObject sfsobject = new SFSObject();
		sfsobject.PutInt("type-id", param);
		sfsobject.PutInt("p-page", 0);
		sfsobject.PutInt("p-length", 30);
		return sfsobject;
	}

	[SerializeField]
	private Toggle defaultTab;

	[SerializeField]
	private Text topText;

	[SerializeField]
	private Transform contentHolder;

	[SerializeField]
	private GameObject rankingItemPrefab;

	[SerializeField]
	private ScrollRect scrollRect;
}
