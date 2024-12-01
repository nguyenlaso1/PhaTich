// @sonhg: class: Lazer
using System;
using UnityEngine;

public class Lazer : MonoBehaviour
{
	private void Update()
	{
		float num = 20f;
		float num2 = num;
		Vector2 direction = base.transform.right;
		RaycastHit2D raycastHit2D = Physics2D.Raycast(base.transform.position, direction, num);
		if (raycastHit2D.collider != null)
		{
			num2 = Vector2.Distance(raycastHit2D.point, base.transform.position);
			this.end.SetActive(true);
		}
		else if (this.end != null)
		{
			this.end.SetActive(false);
		}
		float x = this.start.GetComponent<Renderer>().bounds.size.x;
		this.middle.transform.localScale = new Vector3(num2 - x, this.middle.transform.localScale.y, this.middle.transform.localScale.z);
		this.middle.transform.localPosition = new Vector2(num2 / 2f, 0f);
		if (this.end != null)
		{
			this.end.transform.localPosition = new Vector2(num2, 0f);
		}
	}

	[Header("Laser pieces")]
	[SerializeField]
	private GameObject start;

	[SerializeField]
	private GameObject middle;

	[SerializeField]
	private GameObject end;
}
