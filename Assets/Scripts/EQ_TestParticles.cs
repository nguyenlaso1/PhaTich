// @sonhg: class: EQ_TestParticles
using System;
using UnityEngine;

public class EQ_TestParticles : MonoBehaviour
{
	private void Start()
	{
		if (this.m_CategoryList.Length > 0)
		{
			this.m_CurrentCategoryIndex = 0;
			this.m_CurrentCategoryIndexOld = -1;
			this.m_CurrentParticleIndex = 0;
			this.m_CurrentParticleIndexOld = -1;
			this.ShowParticle();
		}
	}

	private void Update()
	{
		if (UnityEngine.Input.GetKeyUp(KeyCode.UpArrow))
		{
			this.m_CurrentCategoryIndexOld = this.m_CurrentCategoryIndex;
			this.m_CurrentCategoryIndex++;
			this.m_CurrentParticleIndex = 0;
			this.ShowParticle();
		}
		else if (UnityEngine.Input.GetKeyUp(KeyCode.DownArrow))
		{
			this.m_CurrentCategoryIndexOld = this.m_CurrentCategoryIndex;
			this.m_CurrentCategoryIndex--;
			this.m_CurrentParticleIndex = 0;
			this.ShowParticle();
		}
		else if (UnityEngine.Input.GetKeyUp(KeyCode.LeftArrow))
		{
			this.m_CurrentParticleIndexOld = this.m_CurrentParticleIndex;
			this.m_CurrentParticleIndex--;
			this.ShowParticle();
		}
		else if (UnityEngine.Input.GetKeyUp(KeyCode.RightArrow))
		{
			this.m_CurrentParticleIndexOld = this.m_CurrentParticleIndex;
			this.m_CurrentParticleIndex++;
			this.ShowParticle();
		}
	}

	private void OnGUI()
	{
		GUI.Window(1, new Rect((float)(Screen.width - 260), 5f, 250f, 105f), new GUI.WindowFunction(this.AppNameWindow), "FX Quest 0.3.0");
		GUI.Window(2, new Rect(10f, (float)(Screen.height - 65), 290f, 60f), new GUI.WindowFunction(this.DemoSceneWindow), "Demo Scenes");
		GUI.Window(3, new Rect((float)(Screen.width - 360), (float)(Screen.height - 85), 350f, 80f), new GUI.WindowFunction(this.ParticleInformationWindow), "Information");
	}

	private void ShowParticle()
	{
		if (this.m_CurrentCategoryIndex >= this.m_CategoryList.Length)
		{
			this.m_CurrentCategoryIndex = 0;
		}
		else if (this.m_CurrentCategoryIndex < 0)
		{
			this.m_CurrentCategoryIndex = this.m_CategoryList.Length - 1;
		}
		if (this.m_CurrentCategoryIndex != this.m_CurrentCategoryIndexOld)
		{
			if (this.m_CurrentCategoryIndexOld >= 0)
			{
				int num = 0;
				foreach (object obj in this.m_CategoryList[this.m_CurrentCategoryIndexOld])
				{
					Transform transform = (Transform)obj;
					this.m_CurrentParticle = transform.gameObject.GetComponent<ParticleSystem>();
					if (this.m_CurrentParticle != null)
					{
						this.m_CurrentParticle.Stop();
						this.m_CurrentParticle.gameObject.SetActive(false);
					}
					num++;
				}
			}
			if (this.m_CurrentCategoryIndex >= 0)
			{
				int num = 0;
				foreach (object obj2 in this.m_CategoryList[this.m_CurrentCategoryIndex])
				{
					Transform transform2 = (Transform)obj2;
					this.m_CurrentParticle = transform2.gameObject.GetComponent<ParticleSystem>();
					if (this.m_CurrentParticle != null)
					{
						this.m_CurrentParticle.Stop();
						this.m_CurrentParticle.gameObject.SetActive(false);
					}
					num++;
				}
			}
			if (this.m_CurrentCategoryIndexOld >= 0)
			{
				this.m_CategoryList[this.m_CurrentCategoryIndexOld].gameObject.SetActive(false);
			}
			if (this.m_CurrentCategoryIndex >= 0)
			{
				this.m_CategoryList[this.m_CurrentCategoryIndex].gameObject.SetActive(true);
			}
			this.m_CurrentCategoryName = this.m_CategoryList[this.m_CurrentCategoryIndex].name;
			this.m_CurrentCategoryChildCount = this.m_CategoryList[this.m_CurrentCategoryIndex].childCount;
		}
		if (this.m_CurrentParticleIndex >= this.m_CurrentCategoryChildCount)
		{
			this.m_CurrentParticleIndex = 0;
		}
		else if (this.m_CurrentParticleIndex < 0)
		{
			this.m_CurrentParticleIndex = this.m_CurrentCategoryChildCount - 1;
		}
		if (this.m_CurrentParticleIndex != this.m_CurrentParticleIndexOld || this.m_CurrentCategoryIndex != this.m_CurrentCategoryIndexOld)
		{
			if (this.m_CurrentParticle != null)
			{
				this.m_CurrentParticle.Stop();
				this.m_CurrentParticle.gameObject.SetActive(false);
			}
			int num = 0;
			foreach (object obj3 in this.m_CategoryList[this.m_CurrentCategoryIndex])
			{
				Transform transform3 = (Transform)obj3;
				if (num == this.m_CurrentParticleIndex)
				{
					this.m_CurrentParticle = transform3.gameObject.GetComponent<ParticleSystem>();
					if (this.m_CurrentParticle != null)
					{
						this.m_CurrentParticle.gameObject.SetActive(true);
						this.m_CurrentParticle.Play();
						this.m_CurrentParticleName = this.m_CurrentParticle.name;
					}
					break;
				}
				num++;
			}
		}
	}

	private void AppNameWindow(int id)
	{
		if (GUI.Button(new Rect(15f, 25f, 220f, 20f), "www.ge-team.com"))
		{
			Application.OpenURL("http://ge-team.com/pages/unity-3d/");
		}
		if (GUI.Button(new Rect(15f, 50f, 220f, 20f), "geteamdev@gmail.com"))
		{
			Application.OpenURL("mailto:geteamdev@gmail.com");
		}
		if (GUI.Button(new Rect(15f, 75f, 220f, 20f), "Tutorial"))
		{
			Application.OpenURL("http://youtu.be/TWpKPCGYEyI");
		}
	}

	private void DemoSceneWindow(int id)
	{
		if (this.m_CurrentParticleIndex >= 0)
		{
			GUILayout.BeginHorizontal(new GUILayoutOption[0]);
			if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "2D_Demo")
			{
				GUI.enabled = false;
			}
			else
			{
				GUI.enabled = true;
			}
			if (GUI.Button(new Rect(12f, 25f, 125f, 25f), "2D Demo Scene"))
			{
				UnityEngine.SceneManagement.SceneManager.LoadScene("2D_Demo");
			}
			if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "3D_Demo")
			{
				GUI.enabled = false;
			}
			else
			{
				GUI.enabled = true;
			}
			if (GUI.Button(new Rect(155f, 25f, 125f, 25f), "3D Demo Scene"))
			{
				UnityEngine.SceneManagement.SceneManager.LoadScene("3D_Demo");
			}
			GUILayout.EndHorizontal();
		}
	}

	private void ParticleInformationWindow(int id)
	{
		if (this.m_CurrentParticleIndex >= 0)
		{
			GUI.Label(new Rect(12f, 25f, 350f, 20f), string.Concat(new object[]
			{
				"Up / Down: Type (",
				this.m_CurrentCategoryIndex + 1,
				" of ",
				this.m_CategoryList.Length,
				" ",
				this.m_CurrentCategoryName,
				")"
			}));
			GUI.Label(new Rect(12f, 50f, 350f, 20f), string.Concat(new object[]
			{
				"Left / Right: Particle (",
				this.m_CurrentParticleIndex + 1,
				" of ",
				this.m_CurrentCategoryChildCount,
				" ",
				this.m_CurrentParticleName,
				")"
			}));
		}
	}

	public Transform[] m_CategoryList;

	private int m_CurrentCategoryIndex;

	private int m_CurrentCategoryIndexOld = -1;

	private int m_CurrentCategoryChildCount;

	private int m_CurrentParticleIndex;

	private int m_CurrentParticleIndexOld = -1;

	private ParticleSystem m_CurrentParticle;

	private string m_CurrentCategoryName = string.Empty;

	private string m_CurrentParticleName = string.Empty;
}
