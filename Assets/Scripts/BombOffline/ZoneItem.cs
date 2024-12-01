// @sonhg: class: BombOffline.ZoneItem
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BombOffline
{
	public class ZoneItem : MonoBehaviour
	{
		public bool Interactable
		{
			get
			{
				return this._interactable;
			}
			set
			{
				this._interactable = value;
				this._group.interactable = this._interactable;
				if (!this._interactable)
				{
					this._zoneText.text = "?";
				}
			}
		}

		private void Start()
		{
			string path = "Levels/Bomber/" + this.zoneName;
			List<TextAsset> list = new List<TextAsset>(Resources.LoadAll<TextAsset>(path));
			BombSaveGame save = BombSaveGame.LoadZoneProgress(this.zoneName);
			this.bestScore = save.BestZonePoint;
			list.Sort(delegate(TextAsset x, TextAsset y)
			{
				int num2 = int.Parse(x.name.Split(new char[]
				{
					'-'
				})[1]);
				int num3 = int.Parse(y.name.Split(new char[]
				{
					'-'
				})[1]);
				return num2 - num3;
			});
			int num = list.FindIndex((TextAsset level) => level.name.CompareTo(save.CurrentLevel) == 0);
			if (num < 0)
			{
				num = 0;
			}
			if (save.IsPassed)
			{
				num++;
			}
			this._progressText.text = string.Format("{0}/{1}", num, list.Count);
			this.Interactable = this.Interactable;
			if (!this.Interactable)
			{
				base.transform.Find("DoorImage").gameObject.SetActive(false);
				foreach (object obj in this._image.transform)
				{
					Transform transform = (Transform)obj;
					if (transform.name.ToLower().Contains("monster"))
					{
						transform.gameObject.SetActive(false);
					}
				}
				this._image.color = Color.black;
			}
		}

		private void Update()
		{
			if (base.transform.localScale.x < 1.1f)
			{
				this._image.color = new Color(this._image.color.r, this._image.color.g, this._image.color.b, 0.7f);
				this._animator.enabled = false;
				if (this._effect != null)
				{
					this._effect.Pause();
					this._effect.Clear();
					if (!this.isReadyToPlaySound)
					{
						this.isReadyToPlaySound = true;
					}
				}
			}
			else
			{
				if (this.Interactable)
				{
					this._image.color = Color.white;
				}
				else
				{
					this._image.color = Color.black;
				}
				this._animator.enabled = true;
				if (this._effect != null)
				{
					this._effect.Play();
					if (this.isReadyToPlaySound)
					{
						this.isReadyToPlaySound = false;
						MusicManager.instance.PlaySingle(this._soundEffect, 1f);
					}
				}
				if (this.Interactable)
				{
					OfflineMapChooser.TempZone = this.zoneName;
					this.bestScoreText.text = string.Format("Best Score: {0}", this.bestScore);
				}
			}
		}

		public void OnZoneItemClick()
		{
			PlayerPrefs.SetInt("ScrollIndex", base.transform.GetSiblingIndex());
			this.offlineMapChooser.LoadZone(this.zoneName, this.zoneID);
		}

		public void ResetZone()
		{
			this.exitTooltip.transform.SetParent(base.transform.parent.parent.parent, false);
			this.exitTooltip.AddMessageYesNo("Do you want to play from beginning ?", delegate(object x)
			{
				OfflineMapChooser.CurrentZone = this.zoneName;
				BombSaveGame bombSaveGame = BombSaveGame.LoadZoneProgress(this.zoneName);
				BombSaveGame.CreateNewSaveGame(this.zoneName, bombSaveGame.BestZonePoint);
				this.offlineMapChooser.LoadZone(this.zoneName, this.zoneID);
			}, null, null, null, string.Empty, string.Empty, false);
		}

		public string zoneName;

		public string zoneID;

		private bool _interactable;

		[SerializeField]
		private ParticleSystem _effect;

		[SerializeField]
		private Image _image;

		[SerializeField]
		private Animator _animator;

		[SerializeField]
		private Text _progressText;

		[SerializeField]
		private Text _zoneText;

		[SerializeField]
		private CanvasGroup _group;

		[SerializeField]
		private AudioClip _soundEffect;

		[SerializeField]
		private ConfirmBox exitTooltip;

		[SerializeField]
		private Text bestScoreText;

		private bool isReadyToPlaySound = true;

		private int bestScore;

		public OfflineMapChooser offlineMapChooser;
	}
}
