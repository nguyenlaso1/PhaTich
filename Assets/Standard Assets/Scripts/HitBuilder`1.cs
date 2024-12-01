// @plugin: class: HitBuilder`1
using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class HitBuilder<T>
{
	public abstract T GetThis();

	public abstract T Validate();

	public T SetCustomDimension(int dimensionNumber, string value)
	{
		this.customDimensions.Add(dimensionNumber, value);
		return this.GetThis();
	}

	public Dictionary<int, string> GetCustomDimensions()
	{
		return this.customDimensions;
	}

	public T SetCustomMetric(int metricNumber, string value)
	{
		this.customMetrics.Add(metricNumber, value);
		return this.GetThis();
	}

	public Dictionary<int, string> GetCustomMetrics()
	{
		return this.customMetrics;
	}

	public string GetCampaignName()
	{
		return this.campaignName;
	}

	public T SetCampaignName(string campaignName)
	{
		if (campaignName != null)
		{
			this.campaignName = campaignName;
		}
		return this.GetThis();
	}

	public string GetCampaignSource()
	{
		return this.campaignSource;
	}

	public T SetCampaignSource(string campaignSource)
	{
		if (campaignSource != null)
		{
			this.campaignSource = campaignSource;
		}
		else
		{
			UnityEngine.Debug.Log("Campaign source cannot be null or empty");
		}
		return this.GetThis();
	}

	public string GetCampaignMedium()
	{
		return this.campaignMedium;
	}

	public T SetCampaignMedium(string campaignMedium)
	{
		if (campaignMedium != null)
		{
			this.campaignMedium = campaignMedium;
		}
		return this.GetThis();
	}

	public string GetCampaignKeyword()
	{
		return this.campaignKeyword;
	}

	public T SetCampaignKeyword(string campaignKeyword)
	{
		if (campaignKeyword != null)
		{
			this.campaignKeyword = campaignKeyword;
		}
		return this.GetThis();
	}

	public string GetCampaignContent()
	{
		return this.campaignContent;
	}

	public T SetCampaignContent(string campaignContent)
	{
		if (campaignContent != null)
		{
			this.campaignContent = campaignContent;
		}
		return this.GetThis();
	}

	public string GetCampaignID()
	{
		return this.campaignID;
	}

	public T SetCampaignID(string campaignID)
	{
		if (campaignID != null)
		{
			this.campaignID = campaignID;
		}
		return this.GetThis();
	}

	public string GetGclid()
	{
		return this.gclid;
	}

	public T SetGclid(string gclid)
	{
		if (gclid != null)
		{
			this.gclid = gclid;
		}
		return this.GetThis();
	}

	public string GetDclid()
	{
		return this.dclid;
	}

	public T SetDclid(string dclid)
	{
		if (dclid != null)
		{
			this.dclid = dclid;
		}
		return this.GetThis();
	}

	private Dictionary<int, string> customDimensions = new Dictionary<int, string>();

	private Dictionary<int, string> customMetrics = new Dictionary<int, string>();

	private string campaignName = string.Empty;

	private string campaignSource = string.Empty;

	private string campaignMedium = string.Empty;

	private string campaignKeyword = string.Empty;

	private string campaignContent = string.Empty;

	private string campaignID = string.Empty;

	private string gclid = string.Empty;

	private string dclid = string.Empty;
}
