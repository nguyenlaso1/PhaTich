// @sonhg: class: BombOffline.Offline_Obstacle
using System;
using UnityEngine;

namespace BombOffline
{
	public class Offline_Obstacle : MonoBehaviour
	{
		public virtual void Reset()
		{
		}

		[HideInInspector]
		public Offline_BombScene scene;

		public ObstacleType type;

		public string poolName;
	}
}
