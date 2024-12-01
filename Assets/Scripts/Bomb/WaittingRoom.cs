// @sonhg: class: Bomb.WaittingRoom
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
//using Facebook.Unity;
using Sfs2X.Entities;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Bomb
{
	public class WaittingRoom : MonoBehaviour
	{
		private void OnEnable()
		{
			if (this.flagLevelUp)
			{
				MusicManager.instance.PlaySingle(this.levelUp, 1f);
				this.flagLevelUp = false;
			}
			OnlineAdmob.HideBannerAds();
		}

		private void ShareLinkFBonLevelUp(object param)
		{
			//if (!FB.IsInitialized)
			//{
			//	FacebookAPI.Instance.CallFBInit(delegate
			//	{
			//		this.ShareLink();
			//	});
			//}
			//else
			//{
			//	this.ShareLink();
			//}
		}

		private void ShareLink()
		{
			//FB.ShareLink(new Uri(Joker2XConfigUtils.FB_APPLINK_URI), "Bomb Title", "Bomb content", new Uri(Joker2XConfigUtils.FB_APPLINK_PICTURE_URI), delegate(IShareResult result)
			//{
			//	if (result.Cancelled || !string.IsNullOrEmpty(result.Error))
			//	{
			//		Context.Confirm.AddMessage("ShareLink Error: " + result.Error, string.Empty, string.Empty);
			//	}
			//	else if (!string.IsNullOrEmpty(result.PostId))
			//	{
			//		Context.Confirm.AddMessage(result.PostId, string.Empty, string.Empty);
			//	}
			//	else
			//	{
			//		Context.Confirm.AddMessage("ShareLink success!", string.Empty, string.Empty);
			//	}
			//});
		}

		private void Start()
		{
			List<Map> list = ResourcesManager.MapDict.Values.ToList<Map>();
			foreach (Map map in list)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.mapPrefabs);
				gameObject.name = map.Id.ToString();
				string text = ResourcesUltis.MapIdToLink(map.Id.ToString()).Replace(ResourceChecking.BaseIp(), string.Empty);
				gameObject.GetComponent<Button>().onClick.AddListener(delegate()
				{
					this.OnClickMapThumb();
				});
				gameObject.transform.Find("MapName").GetComponent<Text>().text = map.Name;
				gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("Textures/" + text.Substring(0, text.Length - 4));
				if (gameObject.GetComponent<Image>().sprite == null && !string.IsNullOrEmpty(text) && ResourcesManager.SpriteList.ContainsKey(text))
				{
					gameObject.GetComponent<Image>().sprite = ResourcesManager.SpriteList[text];
				}
				gameObject.transform.SetParent(this.gridMap);
				gameObject.transform.localScale = Vector3.one;
			}
		}

		public void MeChat(InputField _chatContent)
		{
			foreach (AvatarController avatarController in this.CharacterAvatar)
			{
				if (avatarController.GetUserId() == SmartFoxConnection.Connection.MySelf.Id)
				{
					avatarController.ShowChatMessage(_chatContent.text);
					_chatContent.text = string.Empty;
				}
			}
		}

		public void OthersChat(int _userId, string _Msg)
		{
			foreach (AvatarController avatarController in this.CharacterAvatar)
			{
				if (avatarController.GetUserId() == _userId)
				{
					avatarController.ShowChatMessage(_Msg);
				}
			}
		}

		public int GetPosFromId(User user)
		{
			int num = -1;
			foreach (AvatarController avatarController in this.CharacterAvatar)
			{
				if (avatarController.GetUser() != null && avatarController.GetUserId() == user.Id)
				{
					num = avatarController.position;
				}
			}
			UnityEngine.Debug.LogError(string.Concat(new object[]
			{
				"ID: ",
				JokerUserUtils.GetFormatDisplayName(user, 0),
				"----Position: ",
				num
			}));
			return num;
		}

		public void OnClickSelectMap()
		{
			if (RoomUtils.GetRoomOwnerId() == SmartFoxConnection.Connection.MySelf.Id)
			{
				this.HightLightMapSelect(MMORoomUtils.GetMapID().ToString());
				this.selectMapPanel.SetActive(true);
			}
		}

		public void OnClickCloseSelectMap()
		{
			this.selectMapPanel.SetActive(false);
		}

		public void OnClickMapThumb()
		{
			string name = EventSystem.current.currentSelectedGameObject.name;
			ChooseMapRequest.SendMessage(int.Parse(name));
			string text = ResourcesUltis.MapIdToLink(name.ToString()).Replace(ResourceChecking.BaseIp(), string.Empty);
			this.thumbImg.sprite = Resources.Load<Sprite>("Textures/" + text.Substring(0, text.Length - 4));
			if (this.thumbImg.sprite == null)
			{
				this.thumbImg.sprite = ResourcesManager.SpriteList[text];
			}
			this.OnClickCloseSelectMap();
		}

		private void HightLightMapSelect(string name)
		{
			int num = 0;
			foreach (object obj in this.gridMap)
			{
				Transform transform = (Transform)obj;
				if (name == transform.name)
				{
					transform.Find("hightLight").gameObject.SetActive(true);
					transform.localScale = Vector3.one * 1.1f;
					num = transform.GetSiblingIndex();
				}
				else
				{
					transform.Find("hightLight").gameObject.SetActive(false);
					transform.localScale = Vector3.one;
				}
			}
			float rate = (float)num / (float)this.gridMap.childCount;
			base.StartCoroutine(this.ChangeValueOfScrollBar(rate));
		}

		private IEnumerator ChangeValueOfScrollBar(float _rate)
		{
			yield return new WaitForSeconds(0.4f);
			this.scrollBarMap.value = _rate;
			yield break;
		}

		public void ResetReady()
		{
			foreach (AvatarController avatarController in this.CharacterAvatar)
			{
				if (avatarController.GetUser() != null)
				{
					avatarController.ResetReady();
				}
			}
		}

		public void HideAllLevelUp()
		{
			foreach (AvatarController avatarController in this.CharacterAvatar)
			{
				avatarController.HideLevelUp();
			}
		}

		public AvatarController[] CharacterAvatar;

		public GameObject selectMapPanel;

		public GameObject mapPrefabs;

		public Transform gridMap;

		public GameObject iconLoadingMapThumb;

		public Image thumbImg;

		public Scrollbar scrollBarMap;

		[HideInInspector]
		public bool flagLevelUp;

		[HideInInspector]
		public int levelUpPosition = -1;

		public AudioClip levelUp;
	}
}
