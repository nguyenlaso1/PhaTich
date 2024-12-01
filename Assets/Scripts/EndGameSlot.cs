// @sonhg: class: EndGameSlot
using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class EndGameSlot : MonoBehaviour
{
	public void SetInformation(Sprite _numberSprts, string name, int currentExp_old, int expPlus, int old_expNextLevel, int expNextLevel, bool levelUp, string win = null, int kill = 0, int gold = 0, bool _isMe = false)
	{
		this._number.GetComponent<Image>().sprite = _numberSprts;
		base.GetComponent<Image>().enabled = _isMe;
		this.nameText.text = name;
		if (this.killText != null)
		{
			this.killText.text = kill + string.Empty;
		}
		if (this.goldText != null)
		{
			this.goldText.gameObject.SetActive(true);
			this.goldText.text = gold + string.Empty;
		}
		float nextFillAmount = 1f;
		if (old_expNextLevel == expNextLevel)
		{
			this.ExpBar.fillAmount = (float)currentExp_old / (float)old_expNextLevel;
			nextFillAmount = (float)(currentExp_old + expPlus) / (float)old_expNextLevel;
			this.ExpBar.DOFillAmount(nextFillAmount, 2f).SetDelay(2f);
		}
		else
		{
			this.ExpBar.fillAmount = (float)currentExp_old / (float)old_expNextLevel;
			this.ExpBar.DOFillAmount(1f, 1f).OnComplete(delegate
			{
				nextFillAmount = (float)(currentExp_old + expPlus) / (float)expNextLevel;
				UnityEngine.Debug.LogError(string.Concat(new object[]
				{
					nextFillAmount,
					"old_expNextLevel",
					old_expNextLevel,
					"currentExp_old ",
					currentExp_old,
					"  expPlus ",
					expPlus,
					"Current",
					currentExp_old + expPlus,
					" expNextLevel ",
					expNextLevel
				}));
				this.ExpBar.fillAmount = 0f;
				this.ExpBar.DOFillAmount(nextFillAmount, 1f);
			}).SetDelay(2f);
		}
		if (levelUp)
		{
			this.expText.DOText("+" + expPlus, 1.5f, true, ScrambleMode.None, null).OnComplete(delegate
			{
				this.expText.text = "Level up";
			});
		}
		else
		{
			this.expText.text = "+" + expPlus;
		}
	}

	[SerializeField]
	private Transform _number;

	[SerializeField]
	private Text nameText;

	[SerializeField]
	private Text killText;

	[SerializeField]
	private Text goldText;

	[SerializeField]
	private Text expText;

	[SerializeField]
	private Image ExpBar;
}
