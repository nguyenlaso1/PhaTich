// @sonhg: class: Bomb.ItemResponse
using System;
using Sfs2X.Core;
using Sfs2X.Entities.Data;
using UnityEngine;

namespace Bomb
{
	public class ItemResponse : BaseResponse
	{
		public override void UpdateBusiness()
		{
			SFSObject sfsobject = (SFSObject)this.evt.Params["params"];
			switch (sfsobject.GetInt("task"))
			{
			case 0:
			{
				UnityEngine.Debug.Log("------------topup: " + sfsobject.GetDump());
				InventoryController.myItemList.Clear();
				ISFSArray sfsarray = sfsobject.GetSFSArray("p-data");
				foreach (object obj in sfsarray)
				{
					SFSObject sfsobject2 = (SFSObject)obj;
					InventoryController.myItemList.Add(sfsobject2.GetInt("i-id"));
				}
				((BomberMainMenu)this.baseScene)._shopController.LoadScrollViewItemByCategory();
				BomberMainMenu.SaveDataToPlayerPref();
				break;
			}
			case 1:
			{
				InventoryController.currenColor = MMOUserUtils.GetHairColor(SmartFoxConnection.Connection.MySelf);
				SFSObject sfsobject3 = new SFSObject();
				sfsobject3.PutInt("task", 0);
				sfsobject3.PutInt("i-id", 1);
				ItemRequest.SendMessage(sfsobject3);
				string utfString = sfsobject.GetUtfString("sysm_content");
				Context.OnDeletegateObject onDeletegateObject = this.okBuySuccess;
				Context.Messenger.AddOkMessage(utfString, null, null, string.Empty, string.Empty);
				BomberMainMenu.SaveDataToPlayerPref();
				break;
			}
			case 2:
				BomberMainMenu.SaveDataToPlayerPref();
				break;
			case 3:
			{
				((BomberMainMenu)this.baseScene)._inventoryController.Init();
				string utfString2 = sfsobject.GetUtfString("sysm_content");
				Context.Messenger.AddOkMessage(utfString2, null, null, string.Empty, string.Empty);
				break;
			}
			}
		}

		public override void UpdateGUI()
		{
		}

		public static void RunMessage(BaseEvent evt, BaseScene gameScene)
		{
			BaseResponse baseResponse = new ItemResponse();
			baseResponse.SetParams(evt, gameScene);
			baseResponse.Run(true);
		}

		private Context.OnDeletegateObject okBuySuccess = delegate(object o)
		{
			SFSObject sfsobject = new SFSObject();
			sfsobject.PutInt("task", 0);
			ItemRequest.SendMessage(sfsobject);
		};

		private Context.OnDeletegateObject okBuyFailed = delegate(object o)
		{
			Context.Tooltip.onDestroy();
		};
	}
}
