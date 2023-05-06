using System.Collections.Generic;
using UnityEngine;

namespace Lighting2D
{
	public class LightSystem : Singleton<LightSystem>
	{
		public bool PreviewInInspector = true;

		public float ExposureLimit = -1f;

		public Material LightingMaterial;

		public Material LightingMaterialMerge;

		private Dictionary<Camera, Light2DProfile> cameraProfiles = new Dictionary<Camera, Light2DProfile>();

		public int LightMapResolutionScale = 1;

		public int ShadowMapResolutionScale = 1;

		public FilterMode ShadowMapFilterMode = FilterMode.Bilinear;

		private void Start()
		{
			cameraProfiles = new Dictionary<Camera, Light2DProfile>();
			if (!LightingMaterial)
			{
				Debug.Log("ERROR");
			}
		}


		public void Reset()
		{
			cameraProfiles.Clear();
		}
	}
}
