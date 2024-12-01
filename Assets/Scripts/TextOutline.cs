// @sonhg: class: TextOutline
using System;
using UnityEngine;

public class TextOutline : MonoBehaviour
{
	private void Start()
	{
		this.textMesh = base.GetComponent<TextMesh>();
		this.meshRenderer = base.GetComponent<MeshRenderer>();
		for (int i = 0; i < 8; i++)
		{
			MeshRenderer component = new GameObject("outline", new Type[]
			{
				typeof(TextMesh)
			})
			{
				transform = 
				{
					parent = base.transform,
					localScale = new Vector3(1f, 1f, 1f)
				}
			}.GetComponent<MeshRenderer>();
			component.material = new Material(this.meshRenderer.material);
			component.castShadows = false;
			component.receiveShadows = false;
			component.sortingLayerID = this.meshRenderer.sortingLayerID;
			component.sortingLayerName = this.meshRenderer.sortingLayerName;
		}
	}

	private void LateUpdate()
	{
		Vector3 a = Camera.main.WorldToScreenPoint(base.transform.position);
		this.outlineColor.a = this.textMesh.color.a * this.textMesh.color.a;
		for (int i = 0; i < base.transform.childCount; i++)
		{
			TextMesh component = base.transform.GetChild(i).GetComponent<TextMesh>();
			component.color = this.outlineColor;
			component.text = this.textMesh.text;
			component.alignment = this.textMesh.alignment;
			component.anchor = this.textMesh.anchor;
			component.characterSize = this.textMesh.characterSize;
			component.font = this.textMesh.font;
			component.fontSize = this.textMesh.fontSize;
			component.fontStyle = this.textMesh.fontStyle;
			component.richText = this.textMesh.richText;
			component.tabSize = this.textMesh.tabSize;
			component.lineSpacing = this.textMesh.lineSpacing;
			component.offsetZ = this.textMesh.offsetZ;
			bool flag = this.resolutionDependant && (Screen.width > this.doubleResolution || Screen.height > this.doubleResolution);
			Vector3 b = this.GetOffset(i) * ((!flag) ? this.pixelSize : (2f * this.pixelSize));
			Vector3 position = Camera.main.ScreenToWorldPoint(a + b);
			component.transform.position = position;
			MeshRenderer component2 = base.transform.GetChild(i).GetComponent<MeshRenderer>();
			component2.sortingLayerID = this.meshRenderer.sortingLayerID;
			component2.sortingLayerName = this.meshRenderer.sortingLayerName;
		}
	}

	private Vector3 GetOffset(int i)
	{
		switch (i % 8)
		{
		case 0:
			return new Vector3(0f, 1f, 0f);
		case 1:
			return new Vector3(1f, 1f, 0f);
		case 2:
			return new Vector3(1f, 0f, 0f);
		case 3:
			return new Vector3(1f, -1f, 0f);
		case 4:
			return new Vector3(0f, -1f, 0f);
		case 5:
			return new Vector3(-1f, -1f, 0f);
		case 6:
			return new Vector3(-1f, 0f, 0f);
		case 7:
			return new Vector3(-1f, 1f, 0f);
		default:
			return Vector3.zero;
		}
	}

	public float pixelSize = 1f;

	public Color outlineColor = Color.black;

	public bool resolutionDependant;

	public int doubleResolution = 1024;

	private TextMesh textMesh;

	private MeshRenderer meshRenderer;
}
