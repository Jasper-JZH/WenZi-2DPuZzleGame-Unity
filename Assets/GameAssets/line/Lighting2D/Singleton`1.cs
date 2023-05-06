using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Lighting2D
{
	public class Singleton<T> : MonoBehaviour where T : Singleton<T>
	{
		private static List<T> instances = new List<T>();

		public static T Instance
		{
			get
			{
				for (int num = instances.Count - 1; num >= 0; num--)
				{
					if ((bool)(Object)instances[num])
					{
						Scene scene = instances[num].gameObject.scene;
						return instances[num];
					}
				}
				return null;
			}
		}

		public Singleton()
		{
			instances.Add(this as T);
		}
	}
}
