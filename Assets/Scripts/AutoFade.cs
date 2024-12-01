// @sonhg: class: AutoFade
using System;
using System.Collections;
using UnityEngine;

public class AutoFade : MonoBehaviour
{
	private static AutoFade Instance
	{
		get
		{
			if (AutoFade.m_Instance == null)
			{
				AutoFade.m_Instance = new GameObject("AutoFade").AddComponent<AutoFade>();
			}
			return AutoFade.m_Instance;
		}
	}

	public static bool Fading
	{
		get
		{
			return AutoFade.Instance.m_Fading;
		}
	}

	private void Awake()
	{
		UnityEngine.Object.DontDestroyOnLoad(this);
		AutoFade.m_Instance = this;
		this.m_Material = new Material("Shader \"Plane/No zTest\" { SubShader { Pass { Blend SrcAlpha OneMinusSrcAlpha ZWrite Off Cull Off Fog { Mode Off } BindChannels { Bind \"Color\",color } } } }");
	}

	private void DrawQuad(Color aColor, float aAlpha)
	{
		aColor.a = aAlpha;
		this.m_Material.SetPass(0);
		GL.PushMatrix();
		GL.LoadOrtho();
		GL.Begin(7);
		GL.Color(aColor);
		GL.Vertex3(0f, 0f, -1f);
		GL.Vertex3(0f, 1f, -1f);
		GL.Vertex3(1f, 1f, -1f);
		GL.Vertex3(1f, 0f, -1f);
		GL.End();
		GL.PopMatrix();
	}

	private IEnumerator Fade(float aFadeOutTime, float aFadeInTime, Color aColor)
	{
		float t = 0f;
		while (t < 1f)
		{
			yield return new WaitForEndOfFrame();
			t = Mathf.Clamp01(t + Time.deltaTime / aFadeOutTime);
			this.DrawQuad(aColor, t);
		}
		if (this.m_LevelName != string.Empty)
		{
			Application.LoadLevelAsync(this.m_LevelName);
		}
		else
		{
			Application.LoadLevelAsync(this.m_LevelIndex);
		}
		while (t > 0f)
		{
			yield return new WaitForEndOfFrame();
			t = Mathf.Clamp01(t - Time.deltaTime / aFadeInTime);
			this.DrawQuad(aColor, t);
		}
		this.m_Fading = false;
		yield break;
	}

	private void StartFade(float aFadeOutTime, float aFadeInTime, Color aColor)
	{
		this.m_Fading = true;
		base.StartCoroutine(this.Fade(aFadeOutTime, aFadeInTime, aColor));
	}

	public static void LoadLevel(string aLevelName, float aFadeOutTime, float aFadeInTime, Color aColor)
	{
		if (AutoFade.Fading)
		{
			return;
		}
		AutoFade.Instance.m_LevelName = aLevelName;
		AutoFade.Instance.StartFade(aFadeOutTime, aFadeInTime, aColor);
	}

	public static void LoadLevel(int aLevelIndex, float aFadeOutTime, float aFadeInTime, Color aColor)
	{
		if (AutoFade.Fading)
		{
			return;
		}
		AutoFade.Instance.m_LevelName = string.Empty;
		AutoFade.Instance.m_LevelIndex = aLevelIndex;
		AutoFade.Instance.StartFade(aFadeOutTime, aFadeInTime, aColor);
	}

	private static AutoFade m_Instance;

	private Material m_Material;

	private string m_LevelName = string.Empty;

	private int m_LevelIndex;

	private bool m_Fading;
}
