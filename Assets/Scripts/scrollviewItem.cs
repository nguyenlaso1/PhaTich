// @sonhg: class: scrollviewItem
using System;
using UnityEngine;

public class scrollviewItem
{
	public scrollviewItem(GameObject _go, string _name, float _width = 0f, float _heigh = 0f)
	{
		this.go = _go;
		this.strName = _name;
		this.cellHeigh = _heigh;
		this.cellWidth = _width;
	}

	public string strName;

	public GameObject go;

	public float cellHeigh;

	public float cellWidth;
}
