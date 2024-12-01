// @sonhg: class: Bomb.InitGameMapResponse
using System;
using Sfs2X.Core;
using Sfs2X.Entities.Data;
using UnityEngine;

namespace Bomb
{
	public class InitGameMapResponse : BaseResponse
	{
		public override void UpdateBusiness()
		{
			SFSObject sfsobject = (SFSObject)this.evt.Params["params"];
			if (sfsobject.ContainsKey("foreground"))
			{
				int @int = sfsobject.GetInt("foreground");
				((BombGameScene)this.baseScene).MapController.SetForeGround(@int);
			}
			if (sfsobject.ContainsKey("map"))
			{
				ISFSArray sfsarray = sfsobject.GetSFSArray("map");
				ISFSArray sfsarray2 = sfsobject.GetSFSArray("background");
				string[][] array = new string[sfsarray.Count][];
				string[][] array2 = new string[sfsarray.Count][];
				for (int i = 0; i < sfsarray.Count; i++)
				{
					string[] array3 = sfsarray.GetUtfString(i).Split(new char[]
					{
						','
					});
					array[i] = new string[array3.Length];
					string[] array4 = sfsarray2.GetUtfString(i).Split(new char[]
					{
						','
					});
					array2[i] = new string[array4.Length];
					for (int j = 0; j < array3.Length; j++)
					{
						array[i][j] = array3[j];
						array2[i][j] = array4[j];
					}
				}
				((BombGameScene)this.baseScene).MapController.ResetMap();
				((BombGameScene)this.baseScene).MapController.LoadMap(array, array2);
			}
			UnityEngine.Debug.Log(sfsobject.GetDump());
		}

		public override void UpdateGUI()
		{
		}

		public static void RunMessage(BaseEvent baseEvent, BaseScene gameScene)
		{
			BaseResponse baseResponse = new InitGameMapResponse();
			baseResponse.SetParams(baseEvent, gameScene);
			baseResponse.Run(true);
		}
	}
}
