// @sonhg: class: Bomb.EndGameResponse
using System;
using System.Collections;
using Sfs2X.Core;
using Sfs2X.Entities.Data;
using UnityEngine;

namespace Bomb
{
	public class EndGameResponse : BaseResponse
	{
		public override void UpdateBusiness()
		{
			SFSObject sfsobject = (SFSObject)this.evt.Params["params"];
			ISFSArray sfsarray = sfsobject.GetSFSArray("data");
			EndGamePanel endGamePanel = ((BombGameScene)this.baseScene).GameController.endGamePanel;
			int num = 0;
			((BombGameScene)this.baseScene).GameController.endGamePanel.ShowPanel();
			((BombGameScene)this.baseScene).GameController.clock.PauseRaising();
			((BombGameScene)this.baseScene).GameController.endGamePanel.ClearAll();
			((BombGameScene)this.baseScene).MapController.DeactiveDoomMode();
			MusicManager.instance.StopMusic();
			foreach (object obj in sfsarray)
			{
				SFSObject sfsobject2 = (SFSObject)obj;
				string utfString = sfsobject2.GetUtfString("displayname");
				int @int = sfsobject2.GetInt("is-win");
				int int2 = sfsobject2.GetInt("total-kill");
				int int3 = sfsobject2.GetInt("gold");
				int int4 = sfsobject2.GetInt("id");
				int int5 = sfsobject2.GetInt("exp");
				int int6 = sfsobject2.GetInt("expToNextLevel");
				AvatarController[] characterAvatar = ((BombGameScene)this.baseScene).GameController.waitingPanel.CharacterAvatar;
				if (int4 == SmartFoxConnection.Connection.MySelf.Id)
				{
					if (@int == 1)
					{
						if (Joker2XConfigUtils.ONLINE_PLAY_COUNT_TYPE == 1)
						{
							AdsController.play_count++;
						}
						endGamePanel.ShowWinLose(true);
						MusicManager.instance.PlaySingle(((BombGameScene)this.baseScene).winSound, 1f);
					}
					else
					{
						if (Joker2XConfigUtils.ONLINE_PLAY_COUNT_TYPE == 0)
						{
							AdsController.play_count++;
						}
						MusicManager.instance.PlaySingle(((BombGameScene)this.baseScene).loseSound, 1f);
					}
				}
				endGamePanel.FillSlot(num, utfString, int6, int5, @int, int2, int3, int4, int4 == SmartFoxConnection.Connection.MySelf.Id, false);
				num++;
			}
		}

		public override void UpdateGUI()
		{
			((BombGameScene)this.baseScene).StartCoroutine(this.AutoClose());
		}

		private IEnumerator AutoClose()
		{
			yield return new WaitForSeconds(8f);
			((BombGameScene)this.baseScene).OnCloseButtonClick();
			yield break;
		}

		public static void RunMessage(BaseEvent baseEvent, BaseScene gameScene)
		{
			BaseResponse baseResponse = new EndGameResponse();
			baseResponse.SetParams(baseEvent, gameScene);
			baseResponse.Run(true);
		}

		protected override string Dump
		{
			get
			{
				return "<color=green>" + base.GetType().Name + "</color>";
			}
		}
	}
}
