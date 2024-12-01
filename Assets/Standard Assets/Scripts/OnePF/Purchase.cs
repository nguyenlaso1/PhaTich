// @plugin: class: OnePF.Purchase
using System;

namespace OnePF
{
	public class Purchase
	{
		private Purchase()
		{
		}

		public Purchase(string jsonString)
		{
			JSON json = new JSON(jsonString);
			this.ItemType = json.ToString("itemType");
			this.OrderId = json.ToString("orderId");
			this.PackageName = json.ToString("packageName");
			this.Sku = json.ToString("sku");
			this.PurchaseTime = json.ToLong("purchaseTime");
			this.PurchaseState = json.ToInt("purchaseState");
			this.DeveloperPayload = json.ToString("developerPayload");
			this.Token = json.ToString("token");
			this.OriginalJson = json.ToString("originalJson");
			this.Signature = json.ToString("signature");
			this.AppstoreName = json.ToString("appstoreName");
		}

		public string ItemType { get; private set; }

		public string OrderId { get; private set; }

		public string PackageName { get; private set; }

		public string Sku { get; private set; }

		public long PurchaseTime { get; private set; }

		public int PurchaseState { get; private set; }

		public string DeveloperPayload { get; private set; }

		public string Token { get; private set; }

		public string OriginalJson { get; private set; }

		public string Signature { get; private set; }

		public string AppstoreName { get; private set; }

		public static Purchase CreateFromSku(string sku)
		{
			return Purchase.CreateFromSku(sku, string.Empty);
		}

		public static Purchase CreateFromSku(string sku, string developerPayload)
		{
			return new Purchase
			{
				Sku = sku,
				DeveloperPayload = developerPayload
			};
		}

		public static Purchase CreateFromSku(string sku, string developerPayload, string receipt)
		{
			return new Purchase
			{
				Sku = sku,
				DeveloperPayload = developerPayload,
				Signature = receipt
			};
		}

		public override string ToString()
		{
			return "SKU:" + this.Sku + ";" + this.OriginalJson;
		}

		public string Serialize()
		{
			JSON json = new JSON();
			json["itemType"] = this.ItemType;
			json["orderId"] = this.OrderId;
			json["packageName"] = this.PackageName;
			json["sku"] = this.Sku;
			json["purchaseTime"] = this.PurchaseTime;
			json["purchaseState"] = this.PurchaseState;
			json["developerPayload"] = this.DeveloperPayload;
			json["token"] = this.Token;
			json["originalJson"] = this.OriginalJson;
			json["signature"] = this.Signature;
			json["appstoreName"] = this.AppstoreName;
			return json.serialized;
		}
	}
}
