// @sonhg: class: BombOffline.Offline_Tiled
using System;
using UnityEngine;

namespace BombOffline
{
	public class Offline_Tiled
	{
		public Offline_Tiled(string tiled, int isBorder)
		{
			string[] array = tiled.Split(new char[]
			{
				'-'
			});
			this.brickID = array[0];
			if (array.Length > 1)
			{
				this.itemId = array[1];
			}
			this.isBorder = isBorder;
		}

		public int status
		{
			get
			{
				if (this.isBorder == 5)
				{
					return this.isBorder;
				}
				if (this.IsEmptyTiled())
				{
					return 0;
				}
				if (this.bomb != null)
				{
					return 3;
				}
				if (this.IsItem())
				{
					return 4;
				}
				if (this.IsBrick())
				{
					return 2;
				}
				if (this.IsPit())
				{
					return 6;
				}
				return 1;
			}
		}

		public bool IsBrick()
		{
			return ResourcesManager.TilesDict.ContainsKey(this.brickID) && ResourcesManager.TilesDict[this.brickID].Category == 11;
		}

		public bool IsPit()
		{
			return ResourcesManager.TilesDict.ContainsKey(this.brickID) && ResourcesManager.TilesDict[this.brickID].Category == 23;
		}

		public bool IsItem()
		{
			return this.itemId != null && this.brickID.Equals("1") && !this.itemId.Equals("1");
		}

		public bool CanPlaceBomb()
		{
			return this.brickID.Equals("1") && this.bomb == null;
		}

		public void Explode()
		{
			if (!this.brickID.Equals("1"))
			{
				this.brickID = "1";
			}
			else
			{
				this.itemId = "1";
			}
			this.bomb = null;
		}

		public void DestroyBrick()
		{
			this.brickID = "1";
		}

		public void DestroyItem()
		{
			this.itemId = "1";
		}

		public void SetEmptyTiled()
		{
			this.brickID = "1";
			this.itemId = "1";
			this.bomb = null;
		}

		public bool IsEmptyTiled()
		{
			return this.brickID == "1" && (this.itemId == null || this.itemId == "1") && this.bomb == null;
		}

		public void PlaceBomb(BombModel bomb)
		{
			if (!this.brickID.Equals("1"))
			{
				UnityEngine.Debug.LogError("Can place Bomb on tiled");
			}
			else
			{
				this.bomb = bomb;
			}
		}

		public void RemoveBomb()
		{
			this.bomb = null;
		}

		public void PlaceObstacle(Offline_Obstacle obstacle)
		{
			if (!this.brickID.Equals("1"))
			{
				UnityEngine.Debug.LogError("Can't place obstacle on tiled");
			}
			else
			{
				this.obstacle = obstacle;
			}
		}

		public void SetItem(string item)
		{
			this.itemId = item;
		}

		public string brickID;

		private string itemId;

		public BombModel bomb;

		public Offline_Obstacle obstacle;

		public int isBorder;
	}
}
