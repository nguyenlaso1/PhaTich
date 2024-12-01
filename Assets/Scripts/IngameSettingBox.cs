// @sonhg: class: IngameSettingBox
using System;
using Bomb;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class IngameSettingBox : BaseSettingBox
{
	protected override void OnStart()
	{
		base.OnStart();
	}

	public override void CloseBox()
	{
		this.OnDestroyBox();
	}

	public void AddClickEvent(UnityAction onExitClick, UnityAction onResetClick)
	{
		this.exitButton.onClick.AddListener(onExitClick);
		this.resetButton.onClick.AddListener(onResetClick);
	}

	[SerializeField]
	private Button exitButton;

	[SerializeField]
	private Button resetButton;
}
