// @sonhg: class: Bomb.ProfileBox
using System;
//using Facebook.Unity;
using Sfs2X.Entities;
using UnityEngine;
using UnityEngine.UI;

namespace Bomb
{
	public class ProfileBox : BaseBox
	{
		protected override void Start()
		{
			base.Start();
			this.ShowInfo();
		}

		private void ShowInfo()
		{
			User mySelf = SmartFoxConnection.Connection.MySelf;
			int num = JokerUserUtils.GetWin(mySelf) + JokerUserUtils.GetLose(mySelf);
			if (num <= 0)
			{
				num = 1;
			}
			this._dbUserIdLabel.text = "UID: " + JokerUserUtils.GetUserId(mySelf).ToString();
			this._avatarController.SetUser(mySelf);
			this._lvLabel.text = JokerUserUtils.GetLevel(mySelf).ToString();
			this._chipLabel.text = JokerUserUtils.GetFormatGold(mySelf);
			this._nameInput.text = JokerUserUtils.GetFormatDisplayName(mySelf, 15);
			this._winRatioLabel.text = Language.PROFILE_WIN_RATING + JokerUserUtils.GetWin(mySelf) * 100 / num + "%";
			this._GoldLabel.text = JokerUserUtils.GetFormatChip(mySelf);
			if (Context.GameInfo.LastAccounType == 2)
			{
				this._faceBookLabel.text = Language.PROFILE_FB_LOGOUT;
			}
		}

		public void OnClickFacebook()
		{
			Debug.Log(":ProfileBox:OnClickFacebook:");
			//if (Context.GameInfo.LastAccounType == 0)
			//{
			//	Context.Waiting.ShowWaiting();
			//	FacebookAPI.Instance.LoginFB(new FacebookDelegate<ILoginResult>(Context.currentMono.MergeFBCallBack), Context.currentMono.onLoginFBSuccess);
			//}
			//else
			//{
			//	Context.currentMono.Logout(JokerEnum.ClientGameState.GS_LOGOUT_NORMAL);
			//}
		}

		[SerializeField]
		private Text _GoldLabel;

		[SerializeField]
		private Text _winRatioLabel;

		[SerializeField]
		private Text _dbUserIdLabel;

		[SerializeField]
		private Text _lvLabel;

		[SerializeField]
		private Text _faceBookLabel;

		[SerializeField]
		private Text _chipLabel;

		[SerializeField]
		private Text _nameInput;

		[SerializeField]
		private GameObject _avatarContainer;

		[SerializeField]
		private AvatarController _avatarController;
	}
}
