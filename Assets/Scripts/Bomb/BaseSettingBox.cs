// @sonhg: class: Bomb.BaseSettingBox
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Bomb
{
	public class BaseSettingBox : BaseBox
	{
		protected override void OnStart()
		{
			if (MusicManager.instance.MusicVolume == 0f)
			{
				this.musicToggle.isOn = true;
				this.musicAnimator.SetBool("isOn", true);
			}
			else
			{
				MusicManager.instance.MusicVolume = -60f;
				this.musicToggle.isOn = false;
				this.musicAnimator.SetBool("isOn", false);
			}
			if (MusicManager.instance.SoundVolume == 0f)
			{
				this.soundToggle.isOn = true;
				this.soundAnimator.SetBool("isOn", true);
			}
			else
			{
				MusicManager.instance.SoundVolume = -60f;
				this.soundToggle.isOn = false;
				this.soundAnimator.SetBool("isOn", false);
			}
			if (PlayerPrefs.GetInt("Joystick", 0) == 1)
			{
				this.controlToggle.isOn = false;
				this.controlAnimator.SetBool("isOn", false);
			}
			else
			{
				this.controlToggle.isOn = true;
				this.controlAnimator.SetBool("isOn", true);
			}
		}

		public void OnMusicToggleValueChange(bool isOn)
		{
			if (isOn)
			{
				MusicManager.instance.MusicVolume = 0f;
			}
			else
			{
				MusicManager.instance.MusicVolume = -60f;
			}
			this.musicAnimator.SetBool("isOn", isOn);
		}

		public void OnSoundToggleValueChange(bool isOn)
		{
			if (isOn)
			{
				MusicManager.instance.SoundVolume = 0f;
			}
			else
			{
				MusicManager.instance.SoundVolume = -60f;
			}
			this.soundAnimator.SetBool("isOn", isOn);
		}

		public void OnControlToggleValueChange(bool isOn)
		{
			if (isOn)
			{
				PlayerPrefs.SetInt("Joystick", 0);
			}
			else
			{
				PlayerPrefs.SetInt("Joystick", 1);
			}
			this.controlAnimator.SetBool("isOn", isOn);
		}

		public override void CloseBox()
		{
			this.OnDestroyBox();
		}

		[SerializeField]
		private Animator musicAnimator;

		[SerializeField]
		private Animator soundAnimator;

		[SerializeField]
		private Animator controlAnimator;

		[SerializeField]
		private Toggle musicToggle;

		[SerializeField]
		private Toggle soundToggle;

		[SerializeField]
		private Toggle controlToggle;
	}
}
