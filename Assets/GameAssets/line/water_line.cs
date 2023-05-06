using System;
using UnityEngine;

public class water_line : MonoBehaviour
{
	private LineRenderer Body;

	public float Width;

	public float Top;

	public float Bottom;

	public float Left;

	private int edgecount;

	private int nodecount;

	private const int z = 20;

	private float[] xpositions;

	private float[] ypositions;

	private float[] velocities;

	private float[] accelerations;

	private GameObject[] meshobjects;

	private Mesh[] meshes;

	private GameObject[] colliders;

	private int counter;

	private System.Random rd = new System.Random();

	private const float springconstant = 0.02f;

	private const float damping = 0.04f;

	private const float spread = 0.05f;

	private float baseheight;

	private float left;

	private float bottom;

	public GameObject splash;

	public GameObject watermesh;

	private bool auto_Splash_flag = true;

	private void Start()
	{
		edgecount = Mathf.RoundToInt(Width) * 4;
		nodecount = edgecount + 1;
		Body = base.gameObject.GetComponent<LineRenderer>();
		Body.SetVertexCount(nodecount);
		xpositions = new float[nodecount];
		ypositions = new float[nodecount];
		velocities = new float[nodecount];
		accelerations = new float[nodecount];
		meshobjects = new GameObject[edgecount];
		meshes = new Mesh[edgecount];
		colliders = new GameObject[edgecount];
		baseheight = Top;
		bottom = Bottom;
		left = Left;
		watermesh.GetComponent<Renderer>().material.color = new Color(0.22745098f, 0.14901961f, 0.20784314f, 37f / 85f);
		watermesh.GetComponent<Renderer>().sortingLayerName = "up_player";
		new System.Random();

		for (int i = 0; i < nodecount; i++)
		{
			ypositions[i] = Top;
			xpositions[i] = Left + Width * (float)i / (float)edgecount;
			accelerations[i] = 0f;
			velocities[i] = 0f;
			Body.SetPosition(i, new Vector3(xpositions[i], ypositions[i], 20f));
		}
		for (int j = 0; j < edgecount; j++)
		{
			meshes[j] = new Mesh();
			Vector3[] vertices = new Vector3[4]
			{
				new Vector3(xpositions[j], ypositions[j], 20f),
				new Vector3(xpositions[j + 1], ypositions[j + 1], 20f),
				new Vector3(xpositions[j], bottom, 20f),
				new Vector3(xpositions[j + 1], bottom, 20f)
			};
			Vector2[] uv = new Vector2[4]
			{
				new Vector2(0f, 1f),
				new Vector2(1f, 1f),
				new Vector2(0f, 0f),
				new Vector2(1f, 0f)
			};

			int[] triangles = new int[6] { 0, 1, 3, 3, 2, 0 };
			meshes[j].vertices = vertices;
			meshes[j].uv = uv;
			meshes[j].triangles = triangles;
			meshobjects[j] = UnityEngine.Object.Instantiate(watermesh, Vector3.zero, Quaternion.identity);
			meshobjects[j].GetComponent<MeshFilter>().mesh = meshes[j];
			meshobjects[j].transform.parent = base.transform;
			colliders[j] = new GameObject();
			colliders[j].name = "Trigger";
			colliders[j].AddComponent<BoxCollider2D>();
			colliders[j].transform.parent = base.transform;
			colliders[j].transform.position = new Vector3(Left + Width * ((float)j + 0.5f) / (float)edgecount, Top - 0.5f, 0f);
			colliders[j].transform.localScale = new Vector3(Width / (float)edgecount, 2f, 1f);
			((Collider2D)colliders[j].GetComponent<BoxCollider2D>()).isTrigger = true;
			colliders[j].AddComponent<water_splash_trigger>();
		}
	}

	public void Splash(float xpos, float velocity)
	{
		if (xpos >= xpositions[0] && xpos <= xpositions[xpositions.Length - 1])
		{
			xpos -= xpositions[0];
			int num = Mathf.RoundToInt((float)(xpositions.Length - 1) * (xpos / (xpositions[xpositions.Length - 1] - xpositions[0])));
			velocities[num] = velocity;
		}
	}

	private void UpdateMeshes()
	{
		for (int i = 0; i < meshes.Length; i++)
		{
			Vector3[] vertices = new Vector3[4]
			{
				new Vector3(xpositions[i], ypositions[i], 20f),
				new Vector3(xpositions[i + 1], ypositions[i + 1], 20f),
				new Vector3(xpositions[i], bottom, 20f),
				new Vector3(xpositions[i + 1], bottom, 20f)
			};
			meshes[i].vertices = vertices;
		}
	}

	public void Disable_Auto_Splash()
	{
		auto_Splash_flag = false;
	}

	public void Enable_Auto_Splash()
	{
		auto_Splash_flag = true;
	}

	private void FixedUpdate()
	{
		counter++;
		if (counter > 100)
		{
			counter = 0;
			if (auto_Splash_flag)
			{
				Splash(rd.Next(Mathf.RoundToInt(xpositions[0]) + 1, Mathf.RoundToInt(xpositions[xpositions.Length - 1])), -0.6f);
			}
		}
		for (int i = 0; i < xpositions.Length; i++)
		{
			float num = 0.02f * (ypositions[i] - baseheight) + velocities[i] * 0.04f;
			accelerations[i] = 0f - num;
			ypositions[i] += velocities[i];
			velocities[i] += accelerations[i];
			Body.SetPosition(i, new Vector3(xpositions[i], ypositions[i], 20f));
		}
		float[] array = new float[xpositions.Length];
		float[] array2 = new float[xpositions.Length];
		for (int j = 0; j < 8; j++)
		{
			for (int k = 0; k < xpositions.Length; k++)
			{
				if (k > 0)
				{
					array[k] = 0.05f * (ypositions[k] - ypositions[k - 1]);
					velocities[k - 1] += array[k];
				}
				if (k < xpositions.Length - 1)
				{
					array2[k] = 0.05f * (ypositions[k] - ypositions[k + 1]);
					velocities[k + 1] += array2[k];
				}
			}
		}
		UpdateMeshes();
	}

	private void Update()
	{
	}
}
