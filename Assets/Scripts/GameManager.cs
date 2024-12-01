// @sonhg: class: GameManager
using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	private void Start()
	{
		this.grid = new MyPathNode[this.gridWidth, this.gridHeight];
		for (int i = 0; i < this.gridWidth; i++)
		{
			for (int j = 0; j < this.gridHeight; j++)
			{
				bool isWall = false;
				this.grid[i, j] = new MyPathNode
				{
					IsWall = isWall,
					X = i,
					Y = j
				};
			}
		}
	}

	public void createEnemy()
	{
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.enemy);
		gameObject.SetActive(true);
	}

	private void Update()
	{
	}

	public void addWall(int x, int y)
	{
		this.grid[x, y].IsWall = true;
	}

	public void removeWall(int x, int y)
	{
		this.grid[x, y].IsWall = false;
	}

	public MyPathNode[,] grid;

	public GameObject enemy;

	public GameObject gridBox;

	public int gridWidth;

	public int gridHeight;

	public float gridSize;

	public GameObject mapHolder;

	public GameObject bgHolder;

	public GameObject mapController;

	public static string distanceType;

	public static int distance = 2;
}
