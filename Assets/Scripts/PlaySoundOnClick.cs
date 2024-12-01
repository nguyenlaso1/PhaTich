// @sonhg: class: PlaySoundOnClick
using System;
using UnityEngine;
using UnityEngine.UI;

public class PlaySoundOnClick : MonoBehaviour
{
	public Button button
	{
		get
		{
			if (this._button == null)
			{
				this._button = base.GetComponent<Button>();
			}
			return this._button;
		}
	}

	private void Awake()
	{
		if (this.button != null)
		{
			this.button.onClick.AddListener(delegate()
			{
				MusicManager.instance.PlaySingle(this.sound, MusicManager.instance.SoundVolume + 1);
			});
		}
	}

	[SerializeField]
	private AudioClip sound;

	private Button _button;
}
