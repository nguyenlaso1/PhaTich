// @sonhg: class: EclipseBullet
using System;
using BombOffline;
using UnityEngine;

public class EclipseBullet : BaseBullet
{
	protected override void InitBullet()
	{
		this.h = base.transform.position.x;
		this.k = base.transform.position.y;
		this.shootDirection += new Vector3(0f, 0.01f, 0f);
		this.positions = DrawCurveUtil.CreateEllipse(this.a, this.b, this.h, this.k, this.resolution, this.shootDirection, 2f);
		this.index = 0;
	}

	protected override void MoveBullet()
	{
		int num = this.positions.Length / 4 * 3;
		if (this.index < num)
		{
			if (this.MoveTo(this.positions[this.index]))
			{
				this.index++;
			}
		}
		else if (this.MoveTo(this.shooter.position))
		{
			this.DestroyBullet();
		}
	}

	protected new bool MoveTo(Vector3 destination)
	{
		Vector3 normalized = (destination - base.transform.position).normalized;
		float magnitude = (destination - base.transform.position).magnitude;
		float num = Mathf.Min(magnitude, this.bulletSpeed * Time.deltaTime);
		base.transform.Translate(num * normalized);
		return num == magnitude;
	}

	protected override void DrawDebugLine()
	{
		for (int i = 0; i < this.positions.Length - 1; i++)
		{
			UnityEngine.Debug.DrawLine(this.positions[i], this.positions[i + 1], Color.red);
		}
	}

	protected override void OnTriggerEnter2D(Collider2D collider)
	{
		if (collider.gameObject.CompareTag("Player"))
		{
			Offline_BaseCharactersController component = collider.gameObject.GetComponent<Offline_BaseCharactersController>();
			component.GetHit(null);
			this.DestroyBullet();
		}
	}

	[Header("EclipseBullet")]
	public float a = 5f;

	public float b = 3f;

	public float h = 1f;

	public float k = 1f;

	public int resolution = 16;

	private Vector3[] positions;

	private int index;
}
