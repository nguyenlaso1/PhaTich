// @sonhg: class: ScrollViewController
using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ScrollViewController : MonoBehaviour
{
	public void InitScrollView()
	{
		bool flag = Config.defaultGame.Equals(string.Empty) && Joker2XConfigUtils.LIST_GAMES_ID.Count >= 2;
		bool flag2 = Context.clientGameState == JokerEnum.ClientGameState.GS_INIT_GAME;
		this.moreGameButton.SetActive(flag);
		if (flag && flag2)
		{
			this.LoadListGameButton(flag2);
		}
		else
		{
			this.mainMenuScene.ChooseGame(Context.currentGameId, true);
		}
	}

	public void SetButtonActive(int scrollType)
	{
		bool flag = scrollType == ScrollViewController.SCROLL_TYPE_GROUP;
		this.mainMenuScene.logoSprite.SetActive(!flag);
		this.mainMenuScene.gameLabel.SetActive(flag);
		if (flag)
		{
			this.mainMenuScene.playNowButton.transform.DOScale(Vector3.one, 0.3f).SetDelay(1f);
		}
		else
		{
			this.mainMenuScene.playNowButton.transform.DOScale(Vector3.zero, 0.3f);
		}
	}

	public void LoadListGroupButton(bool autoChoose)
	{
		this.SetButtonActive(ScrollViewController.SCROLL_TYPE_GROUP);
		base.StartCoroutine(this.RunAnimationLoadGroupButton(autoChoose));
	}

	private IEnumerator RunAnimationLoadGroupButton(bool autoChoose)
	{
		foreach (Transform t in this.uiGrid.GetChildList())
		{
			DOTween.Kill(t, false);
			t.DOLocalMoveX(0f, 1f, false);
			t.GetComponent<BoxCollider>().enabled = false;
		}
		if (!autoChoose)
		{
			yield return new WaitForSeconds(1f);
		}
		this.RemoveAllChild();
		this.scrollType = ScrollViewController.SCROLL_TYPE_GROUP;
		this.uiScrollView.ResetPosition();
		this.uiGrid.Reposition();
		List<Transform> listTransform = this.uiGrid.GetChildList();
		for (int i = 0; i < listTransform.Count; i++)
		{
			Transform t2 = listTransform[i];
			Vector3 currentPosition = t2.localPosition;
			t2.localPosition = Vector3.zero;
			t2.DOLocalMove(currentPosition, 0.5f, false);
		}
		this.moreGameButton.GetComponent<UIButton>().isEnabled = true;
		if (!autoChoose)
		{
			this.moreGameButton.transform.DOLocalMoveX(100f, 0.5f, false).SetRelative<Tweener>();
		}
		yield break;
	}

	public void LoadListGameButton(bool isInitGame = false)
	{
		this.SetButtonActive(ScrollViewController.SCROLL_TYPE_GAME);
		base.StartCoroutine(this.RunAnimationLoadGameButton(isInitGame));
	}

	private IEnumerator RunAnimationLoadGameButton(bool isInitGame)
	{
		this.moreGameButton.GetComponent<UIButton>().isEnabled = false;
		this.moreGameButton.transform.DOLocalMoveX(-100f, 0.5f, false).SetRelative<Tweener>();
		foreach (Transform t in this.uiGrid.GetChildList())
		{
			DOTween.Kill(t, false);
			t.DOLocalMoveX(0f, 1f, false);
			t.GetComponent<BoxCollider>().enabled = false;
		}
		if (!isInitGame)
		{
			yield return new WaitForSeconds(1f);
		}
		this.RemoveAllChild();
		this.scrollType = ScrollViewController.SCROLL_TYPE_GAME;
		int _gameCount = Joker2XConfigUtils.LIST_GAMES_ID.Count;
		for (int i = 0; i < _gameCount; i++)
		{
			GameObject gameButton = NGUITools.AddChild(this.uiGrid.gameObject, this.gameButtonPrefab);
			gameButton.name = "GameButton" + i;
			GameButton script = gameButton.GetComponent<GameButton>();
			script.SetGame(Joker2XConfigUtils.LIST_GAMES_ID[i], this);
		}
		this.scrollType = ScrollViewController.SCROLL_TYPE_GROUP;
		this.uiScrollView.ResetPosition();
		this.uiGrid.Reposition();
		List<Transform> listTransform = this.uiGrid.GetChildList();
		for (int j = 0; j < listTransform.Count; j++)
		{
			Transform t2 = listTransform[j];
			Vector3 currentPosition = t2.localPosition;
			t2.localPosition = Vector3.zero;
			t2.DOLocalMove(currentPosition, 0.5f, false);
		}
		yield break;
	}

	public void RemoveAllChild()
	{
		foreach (Transform obj in this.uiGrid.GetChildList())
		{
			NGUITools.Destroy(obj);
		}
		this.scrollType = ScrollViewController.SCROLL_TYPE_EMPTY;
	}

	public void OnClickMoreGameButton()
	{
		if (this.scrollType == ScrollViewController.SCROLL_TYPE_GROUP)
		{
			this.LoadListGameButton(false);
		}
		else if (this.scrollType == ScrollViewController.SCROLL_TYPE_GAME)
		{
		}
	}

	private const float TIME_COLLAPSE = 1f;

	private const float MOVE_TIME = 0.5f;

	private static int SCROLL_TYPE_EMPTY = -1;

	private static int SCROLL_TYPE_GAME = 1;

	private static int SCROLL_TYPE_GROUP = 2;

	private int scrollType = ScrollViewController.SCROLL_TYPE_EMPTY;

	public GameObject groupButtonPrefab;

	public GameObject gameButtonPrefab;

	public MainMenuScene mainMenuScene;

	public UIGrid uiGrid;

	public GameObject moreGameButton;

	public UIScrollView uiScrollView;
}
