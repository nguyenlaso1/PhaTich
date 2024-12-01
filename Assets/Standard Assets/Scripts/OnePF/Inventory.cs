// @plugin: class: OnePF.Inventory
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OnePF
{
	public class Inventory
	{
		public Inventory(string json)
		{
			JSON json2 = new JSON(json);
			foreach (object obj in ((List<object>)json2.fields["purchaseMap"]))
			{
				List<object> list = (List<object>)obj;
				string key = list[0].ToString();
				Purchase value = new Purchase(list[1].ToString());
				this._purchaseMap.Add(key, value);
			}
			foreach (object obj2 in ((List<object>)json2.fields["skuMap"]))
			{
				List<object> list2 = (List<object>)obj2;
				string key2 = list2[0].ToString();
				SkuDetails value2 = new SkuDetails(list2[1].ToString());
				this._skuMap.Add(key2, value2);
			}
		}

		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("{purchaseMap:{");
			foreach (KeyValuePair<string, Purchase> keyValuePair in this._purchaseMap)
			{
				stringBuilder.Append(string.Concat(new string[]
				{
					"\"",
					keyValuePair.Key,
					"\":{",
					keyValuePair.Value.ToString(),
					"},"
				}));
			}
			stringBuilder.Append("},");
			stringBuilder.Append("skuMap:{");
			foreach (KeyValuePair<string, SkuDetails> keyValuePair2 in this._skuMap)
			{
				stringBuilder.Append(string.Concat(new string[]
				{
					"\"",
					keyValuePair2.Key,
					"\":{",
					keyValuePair2.Value.ToString(),
					"},"
				}));
			}
			stringBuilder.Append("}}");
			return stringBuilder.ToString();
		}

		public SkuDetails GetSkuDetails(string sku)
		{
			if (!this._skuMap.ContainsKey(sku))
			{
				return null;
			}
			return this._skuMap[sku];
		}

		public Purchase GetPurchase(string sku)
		{
			if (!this._purchaseMap.ContainsKey(sku))
			{
				return null;
			}
			return this._purchaseMap[sku];
		}

		public bool HasPurchase(string sku)
		{
			return this._purchaseMap.ContainsKey(sku);
		}

		public bool HasDetails(string sku)
		{
			return this._skuMap.ContainsKey(sku);
		}

		public void ErasePurchase(string sku)
		{
			if (this._purchaseMap.ContainsKey(sku))
			{
				this._purchaseMap.Remove(sku);
			}
		}

		public List<string> GetAllOwnedSkus()
		{
			return this._purchaseMap.Keys.ToList<string>();
		}

		public List<string> GetAllOwnedSkus(string itemType)
		{
			List<string> list = new List<string>();
			foreach (Purchase purchase in this._purchaseMap.Values)
			{
				if (purchase.ItemType == itemType)
				{
					list.Add(purchase.Sku);
				}
			}
			return list;
		}

		public List<Purchase> GetAllPurchases()
		{
			return this._purchaseMap.Values.ToList<Purchase>();
		}

		public List<SkuDetails> GetAllAvailableSkus()
		{
			return this._skuMap.Values.ToList<SkuDetails>();
		}

		public void AddSkuDetails(SkuDetails d)
		{
			this._skuMap.Add(d.Sku, d);
		}

		public void AddPurchase(Purchase p)
		{
			this._purchaseMap.Add(p.Sku, p);
		}

		private Dictionary<string, SkuDetails> _skuMap = new Dictionary<string, SkuDetails>();

		private Dictionary<string, Purchase> _purchaseMap = new Dictionary<string, Purchase>();
	}
}
