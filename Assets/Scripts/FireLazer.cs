// @sonhg: class: FireLazer
using System;
using UnityEngine;

public class FireLazer : MonoBehaviour
{
	public void DrawFireDirection(int lengh)
	{
		int layerMask = 1 << LayerMask.NameToLayer("Wall") | 1 << LayerMask.NameToLayer("BorderWall");
		this.DrawFireDirection(lengh, base.transform.right, layerMask);
		this.DrawFireDirection(lengh, -base.transform.right, layerMask);
		this.DrawFireDirection(lengh, base.transform.up, layerMask);
		this.DrawFireDirection(lengh, -base.transform.up, layerMask);
	}

	private void DrawFireDirection(int lengh, Vector3 direction, int layerMask)
	{
		int num = lengh;
		RaycastHit2D raycastHit2D = Physics2D.Raycast(base.transform.position, direction, (float)lengh, layerMask);
		if (raycastHit2D.collider != null)
		{
			num = Mathf.FloorToInt(raycastHit2D.distance);
		}
		for (int i = 0; i < num; i++)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate(this.firePrefab, base.transform.position + (float)(i + 1) * direction, Quaternion.identity) as GameObject;
			gameObject.transform.SetParent(base.transform);
		}
	}

	public GameObject firePrefab;
}
