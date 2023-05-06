using UnityEngine;

namespace Lighting2D.Demo
{
	public class Drag2Move : MonoBehaviour
	{
		public bool Drag;

		private void OnMouseDown()
		{
			Drag = true;
		}

		private void OnMouseUp()
		{
			Drag = false;
		}

		private void Update()
		{
			if (Drag)
			{
				Vector3 position = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>().ScreenToWorldPoint(Input.mousePosition);
				position.z = 0f;
				base.transform.position = position;
			}
		}
	}
}
