using UnityEngine;
using UnityEngine.Rendering;

namespace Lighting2D
{
	public struct Light2DProfile
	{
		public Camera Camera;

		public CommandBuffer CommandBuffer;

		public RenderTexture LightMapOrig;
	}
}
