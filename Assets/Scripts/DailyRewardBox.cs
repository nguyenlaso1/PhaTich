// @sonhg: class: DailyRewardBox
using System;
using System.Collections;
using UnityEngine;

public class DailyRewardBox : MonoBehaviour
{
	public void SetInfo(int day, string textDay, string chip, string dailyReward)
	{
		this._burstAmount = Mathf.Max(this._burstAmount, this._burstAmount * day);
		this._dayLabel.text = textDay;
		this._chipLabel.text = chip;
		this._dailyRewardLabel.text = Language.DAILY_REWARD + dailyReward;
	}

	public virtual void OnClickCloseButton()
	{
		this._bubblesParticle.Play();
		this._bubblesParticle.Emit(this._burstAmount);
		this._bubblesButton.SetActive(false);
		base.StartCoroutine(this.waitThenCallback(2f, delegate
		{
			NGUITools.Destroy(base.gameObject);
		}));
	}

	private IEnumerator waitThenCallback(float time, Action callback)
	{
		yield return new WaitForSeconds(time);
		callback();
		yield break;
	}

	[SerializeField]
	private UILabel _dayLabel;

	[SerializeField]
	private UILabel _chipLabel;

	[SerializeField]
	private UILabel _dailyRewardLabel;

	[SerializeField]
	private GameObject _bubblesButton;

	[SerializeField]
	private ParticleSystem _bubblesParticle;

	[SerializeField]
	private int _burstAmount = 8;
}
