// @sonhg: class: InviteItem
using System;
using System.Collections;
using System.IO;
using Sfs2X.Entities;
using Sfs2X.Entities.Data;
using UnityEngine;
using UnityEngine.UI;

public class InviteItem : MonoBehaviour
{
	private void Start()
	{
	}

	public int GetUserId()
	{
		return this._userId;
	}

	public void AddInfo(User user, int position)
	{
		this.currentUser = user;
		this.inviteButton.onClick.AddListener(delegate()
		{
			this.OnClickInvite();
		});
		this.nameLabel.text = JokerUserUtils.GetFormatDisplayName(user, 0);
		base.StartCoroutine(this.LoadAva(JokerUserUtils.GetAvatar(user)));
		this._userId = user.Id;
		this.position = position;
	}

	private IEnumerator LoadAva(string _url)
	{
		string fileName = Path.GetFileName(_url);
		string localName = Application.persistentDataPath + "/" + fileName;
		UnityEngine.Debug.Log("Local name: " + localName);
		if (File.Exists(localName) && !_url.Contains("http"))
		{
			UnityEngine.Debug.Log("Exists " + localName);
			WWW ccc = new WWW("file:///" + localName);
			yield return ccc;
			if (ccc.error == null)
			{
				this.avatarTexture.texture = ccc.texture;
			}
		}
		else
		{
			UnityEngine.Debug.Log("++++++++++++Exists " + localName);
			string urlPhoto = (!_url.Contains("http")) ? (Joker2XConfigUtils.URL_PHOTO + "/" + _url) : _url;
			WWW www = new WWW(urlPhoto);
			yield return www;
			if (www.error == null)
			{
				this.avatarTexture.texture = www.texture;
				File.WriteAllBytes(localName, www.bytes);
			}
		}
		yield break;
	}

	public void OnClickInvite()
	{
		SFSObject sfsobject = new SFSObject();
		sfsobject.PutInt("userId", this._userId);
		sfsobject.PutInt("r-id", SmartFoxConnection.Connection.LastJoinedRoom.Id);
		sfsobject.PutUtfString("r-gname", SmartFoxConnection.Connection.LastJoinedRoom.GroupId);
		sfsobject.PutInt("position", this.position);
		InviteRequest.SendMessage(sfsobject);
		this.invitePanel.SetActive(false);
	}

	public Text nameLabel;

	public RawImage avatarTexture;

	public Button inviteButton;

	[HideInInspector]
	public GameObject invitePanel;

	private int _userId;

	private User currentUser;

	private int position;
}
