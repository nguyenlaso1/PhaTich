// @sonhg: class: TurnToWall
using System;
using UnityEngine;

public class TurnToWall : MonoBehaviour
{
	private void Start()
	{
	}

	private void OnMouseDown()
	{
		string[] array = base.gameObject.name.Split(new char[]
		{
			','
		});
		if (!this.isWall)
		{
			this.Game.addWall(int.Parse(array[0]), int.Parse(array[1]));
			this.isWall = true;
			base.GetComponent<Renderer>().material.color = Color.red;
		}
		else
		{
			this.Game.removeWall(int.Parse(array[0]), int.Parse(array[1]));
			this.isWall = false;
			base.GetComponent<Renderer>().material.color = Color.white;
		}
	}

	private void Update()
	{
	}

	public GameManager Game;

	private bool isWall;
}
