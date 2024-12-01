// @sonhg: class: TextureCutter
using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class TextureCutter : MonoBehaviour
{
	public static Sprite TextureToSprite(Texture2D _texture)
	{
		Rect rect = new Rect(0f, 0f, (float)_texture.width, (float)_texture.height);
		return Sprite.Create(_texture, rect, new Vector2(0.5f, 0.5f), 62f);
	}

	public static List<Sprite> TextureToMultipleSprite(Texture2D _texture, int _col, int _row, int _max, Vector2 _pivot)
	{
		float num = (float)(_texture.width / _col);
		float num2 = (float)(_texture.height / _row);
		List<Sprite> list = new List<Sprite>();
		int num3 = 0;
		for (int i = 0; i < _row; i++)
		{
			if (num3 >= _max)
			{
				break;
			}
			for (int j = 0; j < _col; j++)
			{
				if (num3 >= _max)
				{
					break;
				}
				Rect rect = new Rect((float)j * num, (float)i * num2, num, num2);
				list.Add(Sprite.Create(_texture, rect, _pivot));
				num3++;
			}
		}
		return list;
	}

	public static Sprite TextureToSpecialSprite(Texture2D _texture, int _col, int _number, Vector2 _pivot)
	{
		float num = (float)(_texture.width / _col);
		float height = (float)_texture.height;
		Rect rect = new Rect((float)_number * num, 0f, num, height);
		return Sprite.Create(_texture, rect, _pivot);
	}

	public static Sprite CutTextureByPixel(Texture2D _texture, float _width, float _height, Vector2 _bottomLeft, Vector2 _pivot)
	{
		Rect rect = new Rect(_bottomLeft.x, _bottomLeft.y, _width, _height);
		return Sprite.Create(_texture, rect, _pivot);
	}

	public static void CutAll(Texture2D[] _textureArr, Transform characterObj, Color _hairColor)
	{
		TextureCutter.CutHead(_textureArr[0], characterObj);
		TextureCutter.CutBody(_textureArr[1], characterObj);
		TextureCutter.CutHair(_textureArr[2], characterObj, _hairColor);
	}

	public static void CutHead(Texture2D _texture, Transform characterObj)
	{
		if (_texture != null)
		{
			List<Sprite> list = TextureCutter.TextureToMultipleSprite(_texture, 3, 1, 3, new Vector2(0.5f, 0.5f));
			Sprite sprite = list[0];
			if (characterObj.Find("Head").GetComponent<SpriteRenderer>() != null)
			{
				characterObj.Find("Head").Find("Face").GetComponent<SpriteRenderer>().sprite = sprite;
			}
			else
			{
				characterObj.Find("Head").Find("Face").GetComponent<Image>().sprite = sprite;
			}
			Sprite sprite2 = list[2];
			if (characterObj.Find("Head_Up") != null)
			{
				characterObj.Find("Head_Up").Find("Face").GetComponent<SpriteRenderer>().sprite = sprite2;
			}
			Sprite sprite3 = list[1];
			if (characterObj.Find("Head_Left") != null)
			{
				characterObj.Find("Head_Left").Find("Face").GetComponent<SpriteRenderer>().sprite = sprite3;
			}
			Sprite sprite4 = list[1];
			if (characterObj.Find("Head_Right") != null)
			{
				characterObj.Find("Head_Right").Find("Face").GetComponent<SpriteRenderer>().sprite = sprite4;
			}
		}
		else
		{
			UnityEngine.Debug.LogError("Null Head");
		}
	}

	public static Color Parse(string hex)
	{
		if (hex.Length > 4)
		{
			byte r = byte.Parse(hex.Substring(0, 2), NumberStyles.HexNumber);
			byte g = byte.Parse(hex.Substring(2, 2), NumberStyles.HexNumber);
			byte b = byte.Parse(hex.Substring(4, 2), NumberStyles.HexNumber);
			return new Color32(r, g, b, byte.MaxValue);
		}
		return Color.white;
	}

	public static void CutHair(Texture2D _texture, Transform characterObj, Color _color)
	{
		if (_texture != null)
		{
			List<Sprite> list = TextureCutter.TextureToMultipleSprite(_texture, 3, 1, 3, new Vector2(0.5f, 0.5f));
			Sprite sprite = list[0];
			if (characterObj.Find("Head").GetComponent<SpriteRenderer>() != null)
			{
				characterObj.Find("Head").Find("Hair").GetComponent<SpriteRenderer>().sprite = sprite;
				characterObj.Find("Head").Find("Hair").GetComponent<SpriteRenderer>().material.SetColor("_Color", _color);
			}
			else
			{
				characterObj.Find("Head").Find("Hair").GetComponent<Image>().sprite = sprite;
				characterObj.Find("Head").Find("Hair").GetComponent<Image>().color = _color;
			}
			Sprite sprite2 = list[2];
			if (characterObj.Find("Head_Up") != null)
			{
				characterObj.Find("Head_Up").Find("Hair").GetComponent<SpriteRenderer>().sprite = sprite2;
				characterObj.Find("Head_Up").Find("Hair").GetComponent<SpriteRenderer>().material.SetColor("_Color", _color);
			}
			Sprite sprite3 = list[1];
			if (characterObj.Find("Head_Left") != null)
			{
				characterObj.Find("Head_Left").Find("Hair").GetComponent<SpriteRenderer>().sprite = sprite3;
				characterObj.Find("Head_Left").Find("Hair").GetComponent<SpriteRenderer>().material.SetColor("_Color", _color);
			}
			Sprite sprite4 = list[1];
			if (characterObj.Find("Head_Right") != null)
			{
				characterObj.Find("Head_Right").Find("Hair").GetComponent<SpriteRenderer>().sprite = sprite4;
				characterObj.Find("Head_Right").Find("Hair").GetComponent<SpriteRenderer>().material.SetColor("_Color", _color);
			}
		}
		else
		{
			UnityEngine.Debug.LogError("Null Hair");
		}
	}

	public static void CutBody(Texture2D _texture, Transform characterObj)
	{
		if (_texture != null)
		{
			Sprite sprite = TextureCutter.CutTextureByPixel(_texture, 124f, 121f, new Vector2(0f, 0f), new Vector2(0.5f, 0.86f));
			if (characterObj.Find("Body").GetComponent<SpriteRenderer>() != null)
			{
				characterObj.Find("Body").GetComponent<SpriteRenderer>().sprite = sprite;
			}
			else
			{
				characterObj.Find("Body").GetComponent<Image>().sprite = sprite;
			}
			Sprite sprite2 = TextureCutter.CutTextureByPixel(_texture, 99f, 121f, new Vector2(248f, 0f), new Vector2(0.21f, 0.726f));
			if (characterObj.Find("LeftBody") != null)
			{
				characterObj.Find("LeftBody").GetComponent<SpriteRenderer>().sprite = sprite2;
			}
			Sprite sprite3 = TextureCutter.CutTextureByPixel(_texture, 124f, 121f, new Vector2(124f, 0f), new Vector2(0.45f, 0.7f));
			if (characterObj.Find("UpBody") != null)
			{
				characterObj.Find("UpBody").GetComponent<SpriteRenderer>().sprite = sprite3;
			}
			Sprite sprite4 = TextureCutter.CutTextureByPixel(_texture, 66f, 74f, new Vector2(348f, 10f), new Vector2(0.243f, 0.826f));
			if (characterObj.Find("RightHand").GetComponent<SpriteRenderer>() != null)
			{
				characterObj.Find("RightHand").GetComponent<SpriteRenderer>().sprite = sprite4;
			}
			else
			{
				characterObj.Find("RightHand").GetComponent<Image>().sprite = sprite4;
			}
			if (characterObj.Find("LeftHand").GetComponent<SpriteRenderer>() != null)
			{
				characterObj.Find("LeftHand").GetComponent<SpriteRenderer>().sprite = sprite4;
			}
			else
			{
				characterObj.Find("LeftHand").GetComponent<Image>().sprite = sprite4;
			}
			Sprite sprite5 = TextureCutter.CutTextureByPixel(_texture, 51f, 71f, new Vector2(420f, 11f), new Vector2(0.6f, 0.68f));
			if (characterObj.Find("LeftLeg").GetComponent<SpriteRenderer>() != null)
			{
				characterObj.Find("LeftLeg").GetComponent<SpriteRenderer>().sprite = sprite5;
			}
			else
			{
				characterObj.Find("RightLeg").GetComponent<Image>().sprite = sprite5;
			}
			if (characterObj.Find("RightLeg").GetComponent<SpriteRenderer>() != null)
			{
				characterObj.Find("RightLeg").GetComponent<SpriteRenderer>().sprite = sprite5;
			}
			else
			{
				characterObj.Find("LeftLeg").GetComponent<Image>().sprite = sprite5;
			}
		}
		else
		{
			UnityEngine.Debug.LogError("Null Body");
		}
	}

	public static Texture2D TextureFromSprite(Sprite sprite)
	{
		if (sprite.rect.width != (float)sprite.texture.width)
		{
			Texture2D texture2D = new Texture2D((int)sprite.rect.width, (int)sprite.rect.height);
			Color[] pixels = sprite.texture.GetPixels((int)sprite.textureRect.x, (int)sprite.textureRect.y, (int)sprite.textureRect.width, (int)sprite.textureRect.height);
			texture2D.SetPixels(pixels);
			texture2D.Apply();
			return texture2D;
		}
		return sprite.texture;
	}

	public static Texture2D GetSkinTexture(int _id)
	{
		Texture2D texture2D = null;
		string text = ResourcesUltis.ItemIdToLink(_id.ToString()).Replace(ResourceChecking.BaseIp(), string.Empty);
		if (string.IsNullOrEmpty(text))
		{
			UnityEngine.Debug.LogError(string.Concat(new object[]
			{
				"Id: ",
				_id,
				"File name: ",
				text
			}));
		}
		else
		{
			text = text.Substring(0, text.Length - 4);
			texture2D = Resources.Load<Texture2D>("Textures/" + text);
			if (texture2D == null && !string.IsNullOrEmpty(ResourcesUltis.ItemIdToLink(_id.ToString())))
			{
				text = ResourcesUltis.ItemIdToLink(_id.ToString()).Replace(ResourceChecking.BaseIp(), string.Empty);
				if (ResourcesManager.SpriteList.ContainsKey(text))
				{
					texture2D = TextureCutter.TextureFromSprite(ResourcesManager.SpriteList[text]);
				}
			}
		}
		return texture2D;
	}
}
