// @sonhg: class: FXQ_OrbitCamera
using System;
using UnityEngine;

public class FXQ_OrbitCamera : MonoBehaviour
{
	private void Start()
	{
		this.Distance = Vector3.Distance(this.TargetLookAt.transform.position, base.gameObject.transform.position);
		if (this.Distance > this.DistanceMax)
		{
			this.DistanceMax = this.Distance;
		}
		this.startingDistance = this.Distance;
		this.Reset();
	}

	private void Update()
	{
	}

	private void LateUpdate()
	{
		if (this.TargetLookAt == null)
		{
			return;
		}
		this.HandlePlayerInput();
		this.CalculateDesiredPosition();
		this.UpdatePosition();
	}

	private void HandlePlayerInput()
	{
		float num = 0.01f;
		if (Input.GetMouseButton(0))
		{
			this.mouseX += UnityEngine.Input.GetAxis("Mouse X") * this.X_MouseSensitivity;
			this.mouseY -= UnityEngine.Input.GetAxis("Mouse Y") * this.Y_MouseSensitivity;
		}
		this.mouseY = this.ClampAngle(this.mouseY, this.Y_MinLimit, this.Y_MaxLimit);
		if (UnityEngine.Input.GetAxis("Mouse ScrollWheel") < -num || UnityEngine.Input.GetAxis("Mouse ScrollWheel") > num)
		{
			this.desiredDistance = Mathf.Clamp(this.Distance - UnityEngine.Input.GetAxis("Mouse ScrollWheel") * this.MouseWheelSensitivity, this.DistanceMin, this.DistanceMax);
		}
	}

	private void CalculateDesiredPosition()
	{
		this.Distance = Mathf.SmoothDamp(this.Distance, this.desiredDistance, ref this.velocityDistance, this.DistanceSmooth);
		this.desiredPosition = this.CalculatePosition(this.mouseY, this.mouseX, this.Distance);
	}

	private Vector3 CalculatePosition(float rotationX, float rotationY, float distance)
	{
		Vector3 point = new Vector3(0f, 0f, -distance);
		Quaternion rotation = Quaternion.Euler(rotationX, rotationY, 0f);
		return this.TargetLookAt.position + rotation * point;
	}

	private void UpdatePosition()
	{
		float x = Mathf.SmoothDamp(this.position.x, this.desiredPosition.x, ref this.velX, this.X_Smooth);
		float y = Mathf.SmoothDamp(this.position.y, this.desiredPosition.y, ref this.velY, this.Y_Smooth);
		float z = Mathf.SmoothDamp(this.position.z, this.desiredPosition.z, ref this.velZ, this.X_Smooth);
		this.position = new Vector3(x, y, z);
		base.transform.position = this.position;
		base.transform.LookAt(this.TargetLookAt);
	}

	private void Reset()
	{
		this.mouseX = 0f;
		this.mouseY = 0f;
		this.Distance = this.startingDistance;
		this.desiredDistance = this.Distance;
	}

	private float ClampAngle(float angle, float min, float max)
	{
		while (angle < -360f || angle > 360f)
		{
			if (angle < -360f)
			{
				angle += 360f;
			}
			if (angle > 360f)
			{
				angle -= 360f;
			}
		}
		return Mathf.Clamp(angle, min, max);
	}

	public Transform TargetLookAt;

	public float Distance = 10f;

	public float DistanceMin = 5f;

	public float DistanceMax = 15f;

	private float startingDistance;

	private float desiredDistance;

	private float mouseX;

	private float mouseY;

	public float X_MouseSensitivity = 5f;

	public float Y_MouseSensitivity = 5f;

	public float MouseWheelSensitivity = 5f;

	public float Y_MinLimit = 15f;

	public float Y_MaxLimit = 70f;

	public float DistanceSmooth = 0.025f;

	private float velocityDistance;

	private Vector3 desiredPosition = Vector3.zero;

	public float X_Smooth = 0.05f;

	public float Y_Smooth = 0.1f;

	private float velX;

	private float velY;

	private float velZ;

	private Vector3 position = Vector3.zero;
}
