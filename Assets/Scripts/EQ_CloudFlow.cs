// @sonhg: class: EQ_CloudFlow
using System;
using UnityEngine;

public class EQ_CloudFlow : MonoBehaviour
{
	private void Start()
	{
		this.m_CloudList = new Cloud[base.transform.childCount];
		int num = UnityEngine.Random.Range(0, 2);
		int num2 = 0;
		foreach (object obj in base.transform)
		{
			Transform transform = (Transform)obj;
			this.m_CloudList[num2] = new Cloud();
			this.m_CloudList[num2].m_MoveSpeed = UnityEngine.Random.Range(this.m_MinSpeed, this.m_MaxSpeed);
			if (num == 0)
			{
				this.m_CloudList[num2].m_MoveSpeed *= -1f;
				if (this.m_Behavior == eCloudFlowBehavior.SwitchLeftRight)
				{
					num = 1;
				}
			}
			else if (this.m_Behavior == eCloudFlowBehavior.SwitchLeftRight)
			{
				num = 0;
			}
			this.m_CloudList[num2].m_Cloud = transform.gameObject;
			if (this.m_EnableLargeCloudLoop)
			{
				this.m_CloudList[num2].m_CloudFollower = UnityEngine.Object.Instantiate<GameObject>(transform.gameObject);
			}
			this.m_CloudList[num2].m_OriginalLocalPos = this.m_CloudList[num2].m_Cloud.transform.localPosition;
			num2++;
		}
		if (this.m_EnableLargeCloudLoop)
		{
			foreach (Cloud cloud in this.m_CloudList)
			{
				cloud.m_CloudFollower.transform.parent = base.transform;
			}
		}
		this.FindTheOrthographicCamera();
	}

	private void Update()
	{
		if (this.m_Camera == null)
		{
			this.FindTheOrthographicCamera();
		}
		if (this.m_Camera == null)
		{
			UnityEngine.Debug.LogWarning("There is no Orthographic camera in the scene.");
			return;
		}
		int num = 0;
		foreach (Cloud cloud in this.m_CloudList)
		{
			if (cloud.m_Cloud.activeSelf)
			{
				this.m_CloudList[num].m_Cloud.transform.localPosition = new Vector3(this.m_CloudList[num].m_Cloud.transform.localPosition.x + this.m_CloudList[num].m_MoveSpeed * Time.deltaTime, this.m_CloudList[num].m_Cloud.transform.localPosition.y, this.m_CloudList[num].m_Cloud.transform.localPosition.z);
				if (this.m_CloudList[num].m_MoveSpeed > 0f)
				{
					if (this.m_CloudList[num].m_CloudFollower != null)
					{
						this.m_CloudList[num].m_CloudFollower.transform.localPosition = new Vector3(this.m_CloudList[num].m_Cloud.transform.localPosition.x - this.m_CloudList[num].m_Cloud.GetComponent<Renderer>().bounds.size.x, this.m_CloudList[num].m_Cloud.transform.localPosition.y, this.m_CloudList[num].m_Cloud.transform.localPosition.z);
					}
					if (this.m_CloudList[num].m_Cloud.transform.localPosition.x > this.RightMostOfScreen.x + this.m_CloudList[num].m_Cloud.GetComponent<Renderer>().bounds.size.x / 2f)
					{
						if (this.m_EnableLargeCloudLoop)
						{
							GameObject cloud2 = this.m_CloudList[num].m_Cloud;
							this.m_CloudList[num].m_Cloud = this.m_CloudList[num].m_CloudFollower;
							this.m_CloudList[num].m_CloudFollower = cloud2;
						}
						else
						{
							this.m_CloudList[num].m_MoveSpeed = UnityEngine.Random.Range(this.m_MinSpeed, this.m_MaxSpeed);
							this.m_CloudList[num].m_Cloud.transform.localPosition = new Vector3(this.LeftMostOfScreen.x - this.m_CloudList[num].m_Cloud.GetComponent<Renderer>().bounds.size.x, UnityEngine.Random.Range(-this.m_Camera.orthographicSize / 2f, this.m_Camera.orthographicSize / 2f), this.m_CloudList[num].m_Cloud.GetComponent<Renderer>().bounds.size.z);
						}
					}
				}
				else
				{
					if (this.m_CloudList[num].m_CloudFollower != null)
					{
						this.m_CloudList[num].m_CloudFollower.transform.localPosition = new Vector3(this.m_CloudList[num].m_Cloud.transform.localPosition.x + this.m_CloudList[num].m_Cloud.GetComponent<Renderer>().bounds.size.x, this.m_CloudList[num].m_Cloud.transform.localPosition.y, this.m_CloudList[num].m_Cloud.transform.localPosition.z);
					}
					if (this.m_CloudList[num].m_Cloud.transform.localPosition.x < this.LeftMostOfScreen.x - this.m_CloudList[num].m_Cloud.GetComponent<Renderer>().bounds.size.x / 2f)
					{
						if (this.m_EnableLargeCloudLoop)
						{
							GameObject cloud3 = this.m_CloudList[num].m_Cloud;
							this.m_CloudList[num].m_Cloud = this.m_CloudList[num].m_CloudFollower;
							this.m_CloudList[num].m_CloudFollower = cloud3;
						}
						else
						{
							this.m_CloudList[num].m_MoveSpeed = -UnityEngine.Random.Range(this.m_MinSpeed, this.m_MaxSpeed);
							this.m_CloudList[num].m_Cloud.transform.localPosition = new Vector3(this.RightMostOfScreen.x + this.m_CloudList[num].m_Cloud.GetComponent<Renderer>().bounds.size.x, UnityEngine.Random.Range(this.m_CloudList[num].m_OriginalLocalPos.y - this.m_CloudList[num].m_Cloud.GetComponent<Renderer>().bounds.size.y, this.m_CloudList[num].m_OriginalLocalPos.y + this.m_CloudList[num].m_Cloud.GetComponent<Renderer>().bounds.size.y), this.m_CloudList[num].m_Cloud.GetComponent<Renderer>().bounds.size.z);
						}
					}
				}
			}
			num++;
		}
	}

	private void FindTheOrthographicCamera()
	{
		if (this.m_Camera == null)
		{
			Camera[] array = UnityEngine.Object.FindObjectsOfType<Camera>();
			foreach (Camera camera in array)
			{
				if (camera.orthographic)
				{
					this.m_Camera = camera;
					break;
				}
			}
		}
		if (this.m_Camera != null)
		{
			this.LeftMostOfScreen = this.m_Camera.ScreenToWorldPoint(new Vector3(0f, 0f, 0f));
			this.RightMostOfScreen = this.m_Camera.ScreenToWorldPoint(new Vector3((float)Screen.width, 0f, 0f));
		}
	}

	[HideInInspector]
	public Cloud[] m_CloudList;

	public bool m_EnableLargeCloudLoop;

	public eCloudFlowBehavior m_Behavior = eCloudFlowBehavior.FlowTheSameWay;

	public float m_MinSpeed = 0.05f;

	public float m_MaxSpeed = 0.3f;

	public Camera m_Camera;

	private Vector3 LeftMostOfScreen;

	private Vector3 RightMostOfScreen;
}
