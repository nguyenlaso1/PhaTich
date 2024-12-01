// @sonhg: class: EndGamePanel
using System;
using System.Collections.Generic;
using Bomb;
using Sfs2X.Entities;
using UnityEngine;
using UnityEngine.UI;

public class EndGamePanel : MonoBehaviour
{
	public void ShowWinLose(bool _isWIn)
	{
		if (_isWIn)
		{
			this.titleWinLoseImg.sprite = this.winLoseSprites[0];
		}
	}

	public void ShowPanel()
	{
		base.gameObject.SetActive(true);
	}

	public void ClearAll()
	{
		foreach (EndGameSlot endGameSlot in this.list)
		{
			endGameSlot.GetComponent<Image>().enabled = false;
			endGameSlot.gameObject.SetActive(false);
		}
		this.titleWinLoseImg.sprite = this.winLoseSprites[1];
	}

	public void FillSlot(int index, string name, int old_expNextLevel, int expPlus, int isWin, int kill, int gold, int id, bool _isMe = false, bool _isTeamFight = false)
	{
		string formatDisplayName = this.GetFormatDisplayName(name, 0);
		string win = string.Empty;
		if (isWin == 1)
		{
			win = "Win";
		}
		else if (isWin == 0)
		{
			win = "Lose";
		}
		else
		{
			win = "Tie";
		}
		User userByID = MMOUserUtils.GetUserByID(id);
		if (_isTeamFight)
		{
			int userPosition = MMOUserUtils.GetUserPosition(userByID);
			if (userPosition > 1)
			{
				this.playerColor[index].sprite = this.teamColors[1];
			}
			else
			{
				this.playerColor[index].sprite = this.teamColors[0];
			}
		}
		int currentExp_old = JokerUserUtils.GetExp(userByID) - expPlus;
		int expNextLevel = JokerUserUtils.GetExpNextLevel(userByID);
		this.list[index].gameObject.SetActive(true);
		this.list[index].SetInformation(this.numberSprites[index], name, currentExp_old, expPlus, old_expNextLevel, expNextLevel, JokerUserUtils.GetExp(userByID) > old_expNextLevel, win, kill, gold, _isMe);
	}

	private string GetFormatDisplayName(string currentName, int maxLengh = 0)
	{
		if (maxLengh < 1)
		{
			maxLengh = 7;
		}
		string result = string.Empty;
		if (currentName.Length < maxLengh)
		{
			result = currentName;
		}
		else
		{
			result = currentName.Substring(0, maxLengh);
		}
		return result;
	}

	[SerializeField]
	private List<EndGameSlot> list;

	[SerializeField]
	private List<Sprite> numberSprites;

	[SerializeField]
	private Sprite[] teamColors;

	[SerializeField]
	private Image[] playerColor;

	[SerializeField]
	private Sprite[] winLoseSprites;

	[SerializeField]
	private Image titleWinLoseImg;
}
