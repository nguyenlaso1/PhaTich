// @plugin: class: OnePF.SkuDetails
using System;

namespace OnePF
{
	public class SkuDetails
	{
		public SkuDetails(string jsonString)
		{
			JSON json = new JSON(jsonString);
			this.ItemType = json.ToString("itemType");
			this.Sku = json.ToString("sku");
			this.Type = json.ToString("type");
			this.Price = json.ToString("price");
			this.Title = json.ToString("title");
			this.Description = json.ToString("description");
			this.Json = json.ToString("json");
			this.CurrencyCode = json.ToString("currencyCode");
			this.PriceValue = json.ToString("priceValue");
			this.ParseFromJson();
		}

		public string ItemType { get; private set; }

		public string Sku { get; private set; }

		public string Type { get; private set; }

		public string Price { get; private set; }

		public string Title { get; private set; }

		public string Description { get; private set; }

		public string Json { get; private set; }

		public string CurrencyCode { get; private set; }

		public string PriceValue { get; private set; }

		private void ParseFromJson()
		{
			if (string.IsNullOrEmpty(this.Json))
			{
				return;
			}
			JSON json = new JSON(this.Json);
			if (string.IsNullOrEmpty(this.PriceValue))
			{
				float num = json.ToFloat("price_amount_micros");
				this.PriceValue = (num / 1000000f).ToString();
			}
			if (string.IsNullOrEmpty(this.CurrencyCode))
			{
				this.CurrencyCode = json.ToString("price_currency_code");
			}
		}

		public override string ToString()
		{
			return string.Format("[SkuDetails: type = {0}, SKU = {1}, title = {2}, price = {3}, description = {4}, priceValue={5}, currency={6}]", new object[]
			{
				this.ItemType,
				this.Sku,
				this.Title,
				this.Price,
				this.Description,
				this.PriceValue,
				this.CurrencyCode
			});
		}
	}
}
