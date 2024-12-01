// @sonhg: class: BaseGameInfo
using System;
using UnityEngine;

public abstract class BaseGameInfo
{
	public abstract string Platform { get; }

	public void DeleteKey(string key)
	{
		PlayerPrefs.DeleteKey(key);
	}

	public void DeleteAll(string key)
	{
		PlayerPrefs.DeleteAll();
	}

	public string VersionName
	{
		get
		{
			return Config.versionName;
		}
	}

	public int VersionCode
	{
		get
		{
			return Config.versionCode;
		}
	}

	public virtual string CP
	{
		get
		{
			return Config.default_cp;
		}
	}

	public virtual string PackageName
	{
		get
		{
			return Config.package_name;
		}
	}

	public virtual string DeviceName
	{
		get
		{
			return SystemInfo.deviceName;
		}
	}

	public abstract int DeviceType { get; }

	public virtual int LanguageId
	{
		get
		{
			return 0;
		}
	}

	public int DeviceLanguageId
	{
		get
		{
			if (!PlayerPrefs.HasKey(StoreKey.KEY_LANGUAGE_ID))
			{
				PlayerPrefs.SetInt(StoreKey.KEY_LANGUAGE_ID, this.LanguageId);
			}
			return PlayerPrefs.GetInt(StoreKey.KEY_LANGUAGE_ID);
		}
		set
		{
			PlayerPrefs.SetInt(StoreKey.KEY_LANGUAGE_ID, value);
		}
	}

	public string DeviceLanguageName
	{
		get
		{
			JokerEnum.LanguageId deviceLanguageId = (JokerEnum.LanguageId)this.DeviceLanguageId;
			return deviceLanguageId.ToString();
		}
	}

	public virtual string DeviceId
	{
		get
		{
			return SystemInfo.deviceUniqueIdentifier;
		}
	}

	public string DeviceToken
	{
		get
		{
			return PlayerPrefs.GetString(StoreKey.KEY_DEVICE_TOKEN, string.Empty);
		}
		set
		{
			PlayerPrefs.SetString(StoreKey.KEY_DEVICE_TOKEN, value);
		}
	}

	public virtual string DisplayName
	{
		get
		{
			return this.DeviceName;
		}
	}

	public virtual string UserName
	{
		get
		{
			return PlayerPrefs.GetString(StoreKey.KEY_USERNAME, DateTime.UtcNow.Ticks + string.Empty);
		}
		set
		{
			PlayerPrefs.SetString(StoreKey.KEY_USERNAME, value);
		}
	}

	public string Password
	{
		get
		{
			if (!PlayerPrefs.HasKey(StoreKey.KEY_PASSWORD))
			{
				PlayerPrefs.SetString(StoreKey.KEY_PASSWORD, MathUtils.PasswordGenerator(15, true));
			}
			return PlayerPrefs.GetString(StoreKey.KEY_PASSWORD);
		}
		set
		{
			PlayerPrefs.SetString(StoreKey.KEY_PASSWORD, value);
		}
	}

	public int LastAccounType
	{
		get
		{
			if (!PlayerPrefs.HasKey(StoreKey.KEY_LAST_ACCOUNT_TYPE))
			{
				PlayerPrefs.SetInt(StoreKey.KEY_LAST_ACCOUNT_TYPE, 0);
			}
			return PlayerPrefs.GetInt(StoreKey.KEY_LAST_ACCOUNT_TYPE);
		}
		set
		{
			PlayerPrefs.SetInt(StoreKey.KEY_LAST_ACCOUNT_TYPE, value);
		}
	}

	public float Volume
	{
		get
		{
			if (!PlayerPrefs.HasKey(StoreKey.KEY_VOLUME))
			{
				PlayerPrefs.SetFloat(StoreKey.KEY_VOLUME, 1f);
			}
			return PlayerPrefs.GetFloat(StoreKey.KEY_VOLUME);
		}
		set
		{
			PlayerPrefs.SetFloat(StoreKey.KEY_VOLUME, value);
		}
	}

	public virtual void Vibrate()
	{
		UnityEngine.Debug.Log("Rung");
	}

	public bool IsVibrate
	{
		get
		{
			if (!PlayerPrefs.HasKey(StoreKey.KEY_VIBRATE))
			{
				PlayerPrefs.SetInt(StoreKey.KEY_VIBRATE, 1);
			}
			return PlayerPrefs.GetInt(StoreKey.KEY_VIBRATE) > 0;
		}
		set
		{
			PlayerPrefs.SetInt(StoreKey.KEY_VIBRATE, (!value) ? 0 : 1);
		}
	}

	public virtual void SendSMS(string phoneNo, string text)
	{
		UnityEngine.Debug.Log("SendSMS " + phoneNo + " " + text);
	}

	public virtual void Toast(string text)
	{
		UnityEngine.Debug.Log("Toast" + text);
	}

	public string Dump()
	{
		return string.Format("Language {0} Platform {1} UserName {2}", this.DeviceLanguageName, this.Platform, this.UserName);
	}

	public virtual string GetSKU(string sku)
	{
		return sku;
	}

	public virtual string StoreName
	{
		get
		{
			return string.Empty;
		}
	}

	public virtual void InitAppsFlyer()
	{
		UnityEngine.Debug.Log("InitAppsFlyer");
	}
}
