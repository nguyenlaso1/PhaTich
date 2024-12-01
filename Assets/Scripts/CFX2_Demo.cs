// @sonhg: class: CFX2_Demo
using System;
using System.Collections;
using System.Text.RegularExpressions;
using UnityEngine;

public class CFX2_Demo : MonoBehaviour
{
	private void OnMouseDown()
	{
		RaycastHit raycastHit = default(RaycastHit);
		if (base.GetComponent<Collider>().Raycast(Camera.main.ScreenPointToRay(UnityEngine.Input.mousePosition), out raycastHit, 9999f))
		{
			GameObject gameObject = this.spawnParticle();
			gameObject.transform.position = raycastHit.point + gameObject.transform.position;
		}
	}

	private GameObject spawnParticle()
	{
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.ParticleExamples[this.exampleIndex]);
		gameObject.transform.position = new Vector3(0f, gameObject.transform.position.y, 0f);
		gameObject.SetActive(true);
		for (int i = 0; i < gameObject.transform.childCount; i++)
		{
			gameObject.transform.GetChild(i).gameObject.SetActive(true);
		}
		return gameObject;
	}

	private void OnGUI()
	{
		GUILayout.BeginArea(new Rect(5f, 20f, (float)(Screen.width - 10), 60f));
		GUILayout.BeginHorizontal(new GUILayoutOption[0]);
		GUILayout.Label("Effect", new GUILayoutOption[0]);
		if (GUILayout.Button("<", new GUILayoutOption[0]))
		{
			this.prevParticle();
		}
		GUILayout.Label(this.ParticleExamples[this.exampleIndex].name, new GUILayoutOption[]
		{
			GUILayout.Width(210f)
		});
		if (GUILayout.Button(">", new GUILayoutOption[0]))
		{
			this.nextParticle();
		}
		GUILayout.Label("Click on the ground to spawn selected particles", new GUILayoutOption[]
		{
			GUILayout.Width(150f)
		});
		if (GUILayout.Button((!CFX_Demo_RotateCamera.rotating) ? "Rotate Camera" : "Pause Camera", new GUILayoutOption[0]))
		{
			CFX_Demo_RotateCamera.rotating = !CFX_Demo_RotateCamera.rotating;
		}
		if (GUILayout.Button((!this.randomSpawns) ? "Start UnityEngine.Random Spawns" : "Stop UnityEngine.Random Spawns", new GUILayoutOption[]
		{
			GUILayout.Width(140f)
		}))
		{
			this.randomSpawns = !this.randomSpawns;
			if (this.randomSpawns)
			{
				base.StartCoroutine("RandomSpawnsCoroutine");
			}
			else
			{
				base.StopCoroutine("RandomSpawnsCoroutine");
			}
		}
		this.randomSpawnsDelay = GUILayout.TextField(this.randomSpawnsDelay, 10, new GUILayoutOption[]
		{
			GUILayout.Width(42f)
		});
		this.randomSpawnsDelay = Regex.Replace(this.randomSpawnsDelay, "[^0-9.]", string.Empty);
		if (GUILayout.Button((!base.GetComponent<Renderer>().enabled) ? "Show Ground" : "Hide Ground", new GUILayoutOption[]
		{
			GUILayout.Width(90f)
		}))
		{
			base.GetComponent<Renderer>().enabled = !base.GetComponent<Renderer>().enabled;
		}
		if (GUILayout.Button((!this.slowMo) ? "Slow Motion" : "Normal Speed", new GUILayoutOption[]
		{
			GUILayout.Width(100f)
		}))
		{
			this.slowMo = !this.slowMo;
			if (this.slowMo)
			{
				Time.timeScale = 0.33f;
			}
			else
			{
				Time.timeScale = 1f;
			}
		}
		GUILayout.EndHorizontal();
		GUILayout.EndArea();
	}

	private IEnumerator RandomSpawnsCoroutine()
	{
		for (;;)
		{
			GameObject particles = this.spawnParticle();
			if (this.orderedSpawns)
			{
				particles.transform.position = base.transform.position + new Vector3(this.order, particles.transform.position.y, 0f);
				this.order -= this.step;
				if (this.order < -this.range)
				{
					this.order = this.range;
				}
			}
			else
			{
				particles.transform.position = base.transform.position + new Vector3(UnityEngine.Random.Range(-this.range, this.range), 0f, UnityEngine.Random.Range(-this.range, this.range)) + new Vector3(0f, particles.transform.position.y, 0f);
			}
			yield return new WaitForSeconds(float.Parse(this.randomSpawnsDelay));
		}
		yield break;
	}

	private void Update()
	{
		if (UnityEngine.Input.GetKeyDown(KeyCode.LeftArrow))
		{
			this.prevParticle();
		}
		else if (UnityEngine.Input.GetKeyDown(KeyCode.RightArrow))
		{
			this.nextParticle();
		}
	}

	private void prevParticle()
	{
		this.exampleIndex--;
		if (this.exampleIndex < 0)
		{
			this.exampleIndex = this.ParticleExamples.Length - 1;
		}
		if (this.ParticleExamples[this.exampleIndex].name.Contains("Splash") || this.ParticleExamples[this.exampleIndex].name.Contains("Skim"))
		{
			base.GetComponent<Renderer>().material = this.waterMat;
		}
		else
		{
			base.GetComponent<Renderer>().material = this.groundMat;
		}
	}

	private void nextParticle()
	{
		this.exampleIndex++;
		if (this.exampleIndex >= this.ParticleExamples.Length)
		{
			this.exampleIndex = 0;
		}
		if (this.ParticleExamples[this.exampleIndex].name.Contains("Splash") || this.ParticleExamples[this.exampleIndex].name.Contains("Skim"))
		{
			base.GetComponent<Renderer>().material = this.waterMat;
		}
		else
		{
			base.GetComponent<Renderer>().material = this.groundMat;
		}
	}

	public bool orderedSpawns = true;

	public float step = 1f;

	public float range = 5f;

	private float order = -5f;

	public Material groundMat;

	public Material waterMat;

	public GameObject[] ParticleExamples;

	private int exampleIndex;

	private string randomSpawnsDelay = "0.5";

	private bool randomSpawns;

	private bool slowMo;
}
