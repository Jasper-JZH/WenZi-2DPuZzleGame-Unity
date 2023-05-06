using UnityEngine;
using UnityEngine.Rendering;

namespace Lighting2D
{
	public class Light2D : Light2DBase
	{
		public LightType LightType;

		[Range(-1f, 1f)]
		public float Attenuation;

		public Color LightColor = Color.white;

		public float Intensity = 1f;

		public Texture LightTexture;

		private Mesh Mesh;

		private Material LightMaterial;

		public Shader shader_2DLight;

		private Vector3 localScale111;

		private Quaternion Rotation000;

		private void Awake()
		{
			float num = LightDistance / 2f;
			Mesh = new Mesh();
			Mesh.vertices = new Vector3[4]
			{
				new Vector3(0f - num, 0f - num, 0f),
				new Vector3(num, 0f - num, 0f),
				new Vector3(0f - num, num, 0f),
				new Vector3(num, num, 0f)
			};
			Mesh.triangles = new int[6] { 0, 2, 1, 2, 3, 1 };
			Mesh.RecalculateNormals();
			Mesh.uv = new Vector2[4]
			{
				new Vector2(0f, 0f),
				new Vector2(1f, 0f),
				new Vector2(0f, 1f),
				new Vector2(1f, 1f)
			};
			Mesh.MarkDynamic();
			localScale111 = new Vector3(1f, 1f, 1f);
			Rotation000 = new Quaternion(0f, 0f, 0f, 1f);
		}

		private void Update()
		{
			UpdateMesh();
		}

		public void UpdateMesh()
		{
			Mesh.vertices = new Vector3[4]
			{
				new Vector3(0f - LightDistance, 0f - LightDistance, 0f),
				new Vector3(LightDistance, 0f - LightDistance, 0f),
				new Vector3(0f - LightDistance, LightDistance, 0f),
				new Vector3(LightDistance, LightDistance, 0f)
			};
		}

		public void RenderLightForXiaomi(CommandBuffer cmd, Shader LightShader)
		{
			if (!LightMaterial)
			{
				LightMaterial = new Material(LightShader);
			}
			LightMaterial.SetColor("_Color", LightColor);
			LightMaterial.SetFloat("_Intensity", Intensity);
			Matrix4x4 matrix = Matrix4x4.TRS(base.transform.position, base.transform.rotation, localScale111);
			cmd.DrawMesh(Mesh, matrix, LightMaterial);
		}

		public bool RenderLightForHuawei(RenderTexture lightmap_original, RenderTexture lightmap_temp_new, Camera cam, Shader LightShader)
		{
			if (!LightMaterial)
			{
				LightMaterial = new Material(LightShader);
			}
			Vector2 vector = cam.WorldToViewportPoint(base.transform.position + new Vector3((0f - LightDistance) / 2f, (0f - LightDistance) / 2f, 0f - base.transform.position.z));
			Vector2 vector2 = cam.WorldToViewportPoint(base.transform.position + new Vector3(LightDistance / 2f, LightDistance / 2f, 0f - base.transform.position.z));
			if (vector.x > 1f || vector.y > 1f || vector2.x < 0f || vector2.y < 0f)
			{
				return false;
			}
			LightMaterial.SetColor("_Color", LightColor);
			LightMaterial.SetFloat("_Intensity", Intensity);
			LightMaterial.SetFloat("_MaxDistancex", vector2.x - vector.x);
			LightMaterial.SetFloat("_MaxDistancey", vector2.y - vector.y);
			LightMaterial.SetFloat("_Positionx", (vector2.x + vector.x) / 2f);
			LightMaterial.SetFloat("_Positiony", (vector2.y + vector.y) / 2f);
			Graphics.Blit(lightmap_original, lightmap_temp_new, LightMaterial);
			return true;
		}
	}
}
