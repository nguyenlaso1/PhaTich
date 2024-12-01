// @sonhg: class: BombOffline.Offline_BombScene
using System;
using System.Collections;
using System.Collections.Generic;
//using Facebook.Unity;
using UnityEngine;

namespace BombOffline
{
	[RequireComponent(typeof(Offline_MapController))]
	[RequireComponent(typeof(Offline_MapController))]
	[RequireComponent(typeof(Offline_ParticlesController))]
	public class Offline_BombScene : MonoBehaviour
	{
		private void Start()
		{
			if (Context.googleAnalytics != null)
			{
				Context.googleAnalytics.LogScreen(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name + "-" + OfflineMapChooser.CurrentLevel);
			}
			if (Offline_Config.OFFLINE_PLAY_COUNT_TYPE == -1)
			{
				BomberAds.play_count++;
			}
		}

		private void OnApplicationPause(bool pauseStatus)
		{
			if (!pauseStatus)
			{
				//if (FB.IsInitialized)
				//{
				//	FB.ActivateApp();
				//}
				//else
				//{
				//	FB.Init(delegate()
				//	{
				//		FB.ActivateApp();
				//	}, null, null);
				//}
			}
		}

		public Offline_ParticlesController ParticlesController
		{
			get
			{
				if (this._particlesController == null)
				{
					this._particlesController = base.GetComponent<Offline_ParticlesController>();
				}
				return this._particlesController;
			}
		}

		public Offline_MapController MapController
		{
			get
			{
				if (this._mapController == null)
				{
					this._mapController = base.GetComponent<Offline_MapController>();
				}
				return this._mapController;
			}
		}

		public Offline_GameController GameController
		{
			get
			{
				if (this._gameController == null)
				{
					this._gameController = base.GetComponent<Offline_GameController>();
				}
				return this._gameController;
			}
		}

		private void Update()
		{
			if (UnityEngine.Input.GetKeyDown(KeyCode.Escape))
			{
				BaseBox[] array = UnityEngine.Object.FindObjectsOfType<BaseBox>();
				for (int i = 0; i < array.Length; i++)
				{
					if (array[i].gameObject.activeInHierarchy)
					{
						return;
					}
				}
				this.OnClickPauseButton();
			}
		}

		public void OnClickExit()
		{
			Offline_Context.Waitting.ShowWaiting();
			Time.timeScale = 1f;
			LoadingScreenManager.LoadScene("BomberMap", true);
		}

		public void OnClickPauseButton()
		{
			this.GameController.PauseGame(true);
		}

		public void OnClickResumeButton()
		{
			this.GameController.PauseGame(false);
		}

		public void OnClickResetButton()
		{
			Offline_Context.Waitting.ShowWaiting();
			Time.timeScale = 1f;
			LoadingScreenManager.LoadScene("OfflineMainScene", true);
		}

		public void OnClickReload()
		{
			Offline_Context.Waitting.ShowWaiting();
			LoadingScreenManager.LoadScene("OfflineMainScene", true);
		}

		public void OnClickFacebookCapture()
		{
			base.StartCoroutine(this.TakeScreenshot());
		}

		//private void AuthCallback(ILoginResult result)
		//{
		//	Offline_Context.Waitting.HideWaiting();
		//	if (result.Cancelled)
		//	{
		//		return;
		//	}
		//	if (result.Error == null && this.tex != null)
		//	{
		//		this.Share(this.tex);
		//	}
		//	else
		//	{
		//		Offline_Context.Confirm.AddMessageYes("You are not login !", null, null, string.Empty);
		//	}
		//}

		private IEnumerator TakeScreenshot()
		{
			yield return new WaitForEndOfFrame();
			int width = Screen.width;
			int height = Screen.height;
			this.tex = new Texture2D(width, height, TextureFormat.RGB24, false);
			this.tex.ReadPixels(new Rect(0f, 0f, (float)width, (float)height), 0, 0);
			this.tex.Apply();
			this.shareFacebookBox.image.sprite = Sprite.Create(this.tex, new Rect(0f, 0f, (float)width, (float)height), Vector2.zero);
			this.shareFacebookBox.AddMessageYesNo("Do you want share to your friend?", delegate(object o)
			{
				this.Login();
			}, null, null, null, string.Empty, string.Empty, false);
			yield break;
		}

		private void Login()
		{
			Offline_Context.Waitting.ShowWaiting();
			//FB.LogInWithPublishPermissions(new List<string>
			//{
			//	"publish_actions"
			//}, new FacebookDelegate<ILoginResult>(this.AuthCallback));
		}

		private void Share(Texture2D tex)
		{
			byte[] contents = tex.EncodeToPNG();
			WWWForm wwwform = new WWWForm();
			wwwform.AddBinaryData("image", contents, "Screenshot.png");
			Offline_Context.Waitting.ShowWaiting();
			//FB.API("me/photos", HttpMethod.POST, new FacebookDelegate<IGraphResult>(this.Callback), wwwform);
		}

		//private void Callback(IGraphResult result)
		//{
		//	Offline_Context.Waitting.HideWaiting();
		//	if (result.Error != null)
		//	{
		//		Offline_Context.Confirm.AddMessageYes("Share error or canceled !", null, null, string.Empty);
		//	}
		//	else
		//	{
		//		Offline_Context.Confirm.AddMessageYes("Share success !", null, null, string.Empty);
		//	}
		//}

		private Offline_ParticlesController _particlesController;

		public ShareFacebookBox shareFacebookBox;

		private Offline_MapController _mapController;

		private Offline_GameController _gameController;

		private Texture2D tex;
	}
}
