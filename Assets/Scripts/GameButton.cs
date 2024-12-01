// @sonhg: class: GameButton
using System;
using UnityEngine;

public class GameButton : MonoBehaviour
{
	public void SetGame(string name, ScrollViewController scroller)
	{
		this.gameBackground.spriteName = name;
		this._gameName = name;
		this.scrollViewController = scroller;
		if (!Config.LIST_READY_GAMES.Contains(name))
		{
			this.Disable();
		}
	}

	public void Disable()
	{
		Color gray = Color.gray;
		base.GetComponent<UIButton>().defaultColor = gray;
		base.GetComponent<UIButton>().hover = gray;
		base.GetComponent<UIButton>().pressed = gray;
	}

	public void OnClickGameButton()
	{
		if (this.scrollViewController != null)
		{
			this.scrollViewController.mainMenuScene.ChooseGame(this._gameName, false);
		}
	}

	public UISprite gameBackground;

	private string _gameName;

	public ScrollViewController scrollViewController;
}
