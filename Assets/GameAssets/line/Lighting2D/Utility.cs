using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Lighting2D
{
	public static class Utility
	{
		public class CallbackYieldInstruction : CustomYieldInstruction
		{
			private Func<bool> callback;

			public override bool keepWaiting
			{
				get
				{
					Func<bool> func = callback;
					if (func == null)
					{
						return true;
					}
					return func();
				}
			}

			public CallbackYieldInstruction(Func<bool> callback)
			{
				this.callback = callback;
			}
		}

		public static void ForEach<T>(this IEnumerable<T> ts, Action<T> callback)
		{
			foreach (T t in ts)
			{
				callback(t);
			}
		}

		public static IEnumerable<T> RandomTake<T>(this IEnumerable<T> list, int count)
		{
			T[] source = Enumerable.ToArray<T>(list);
			for (int i = 0; i < count; i++)
			{
				int idx = UnityEngine.Random.Range(0, source.Length - i);
				yield return source[idx];
				source[idx] = source[count - i - 1];
			}
		}

		public static IEnumerable<GameObject> GetChildren(this GameObject gameObject)
		{
			for (int i = 0; i < gameObject.transform.childCount; i++)
			{
				yield return gameObject.transform.GetChild(i).gameObject;
			}
		}

		public static IEnumerable<U> Map<T, U>(this IEnumerable<T> collection, Func<T, U> callback)
		{
			return Enumerable.Select<T, U>(collection, callback);
		}

		public static IEnumerable<T> NotNull<T>(this IEnumerable<T> source)
		{
			foreach (T item in source)
			{
				if (item != null)
				{
					yield return item;
				}
			}
		}

		public static TResult Merge<TResult, Tkey, TElement>(this IGrouping<Tkey, TElement> group, TResult mergeTarget, Func<TElement, TResult, TResult> mergeFunc)
		{
			foreach (TElement item in (IEnumerable<TElement>)group)
			{
				mergeTarget = mergeFunc(item, mergeTarget);
			}
			return mergeTarget;
		}

		public static GameObject Instantiate(this UnityEngine.Object self, GameObject original, Scene scene)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate(original);
			SceneManager.MoveGameObjectToScene(gameObject, scene);
			return gameObject;
		}

		public static GameObject Instantiate(GameObject original, Scene scene)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate(original);
			SceneManager.MoveGameObjectToScene(gameObject, scene);
			return gameObject;
		}

		public static GameObject Instantiate(GameObject original, GameObject parent)
		{
			GameObject gameObject = Instantiate(original, parent.scene);
			gameObject.transform.SetParent(parent.transform);
			return gameObject;
		}

		public static GameObject Instantiate(GameObject original, GameObject parent, Vector3 relativePosition, Quaternion relativeRotation)
		{
			GameObject gameObject = Instantiate(original, parent);
			gameObject.transform.localPosition = relativePosition;
			gameObject.transform.localRotation = relativeRotation;
			return gameObject;
		}

		public static void ClearChildren(this GameObject self)
		{
			self.GetChildren().ForEach(delegate(GameObject child)
			{
				child.ClearChildren();
				UnityEngine.Object.Destroy(child);
			});
		}

		public static void ClearChildImmediate(this GameObject self)
		{
			while (self.transform.childCount > 0)
			{
				GameObject gameObject = self.transform.GetChild(0).gameObject;
				gameObject.ClearChildImmediate();
				UnityEngine.Object.DestroyImmediate(gameObject);
			}
		}

		public static void SetLayerRecursive(this GameObject gameObject, int layer)
		{
			gameObject.layer = layer;
			gameObject.GetChildren().ForEach(delegate(GameObject child)
			{
				child.SetLayerRecursive(layer);
			});
		}

		public static void NextFrame(this MonoBehaviour context, Action callback)
		{
			context.StartCoroutine(NextFrameCoroutine(callback));
		}

		public static IEnumerator NextFrameCoroutine(Action callback)
		{
			yield return new WaitForEndOfFrame();
			yield return new WaitForEndOfFrame();
			if (callback != null)
			{
				callback();
			}
		}

		public static Coroutine NumericAnimate(this MonoBehaviour context, float time, Action<float> tick, Action complete = null)
		{
			return context.StartCoroutine(NumericAnimateEnumerator(time, tick, complete));
		}

		public static IEnumerator NumericAnimateEnumerator(float time, Action<float> callback, Action complete)
		{
			float startTime = Time.time;
			for (float num = 0f; num < time; num = Time.time - startTime)
			{
				if (callback != null)
				{
					callback(num / time);
				}
				yield return new WaitForEndOfFrame();
			}
			if (callback != null)
			{
				callback(1f);
			}
			if (complete != null)
			{
				complete();
			}
		}

		public static Coroutine WaitForSecond(this MonoBehaviour context, Action callback, float seconds = 0f)
		{
			return context.StartCoroutine(WaitForSecondEnumerator(callback, seconds));
		}

		public static Coroutine SetInterval(this MonoBehaviour context, Action callback, float seconds = 0f)
		{
			return context.StartCoroutine(IntervalCoroutine(callback, seconds));
		}

		public static IEnumerator IntervalCoroutine(Action callback, float seconds)
		{
			while (true)
			{
				yield return new WaitForSeconds(seconds);
				if (callback != null)
				{
					callback();
				}
			}
		}

		public static IEnumerator WaitForSecondEnumerator(Action callback, float seconds = 0f)
		{
			yield return new WaitForSeconds(seconds);
			if (callback != null)
			{
				callback();
			}
		}

		public static IEnumerable<float> Timer(float time)
		{
			float startTime = Time.time;
			while (Time.time < startTime + time)
			{
				yield return Time.time - startTime;
			}
			yield return time;
		}

		public static IEnumerable<float> TimerNormalized(float time)
		{
			foreach (float item in Timer(time))
			{
				yield return item / time;
			}
		}

		public static IEnumerator ShowUI(Graphic ui, float time, float targetAlpha = 1f)
		{
			Color color = ui.color;
			color.a = 0f;
			ui.gameObject.SetActive(true);
			foreach (float item in TimerNormalized(time))
			{
				color.a = item * targetAlpha;
				ui.color = color;
				yield return null;
			}
		}

		public static IEnumerator ShowUI(CanvasGroup canvasGroup, float time)
		{
			time = (1f - canvasGroup.alpha) * time;
			canvasGroup.alpha = 0f;
			((Component)(object)canvasGroup).gameObject.SetActive(true);
			float alpha = canvasGroup.alpha;
			foreach (float item in TimerNormalized(time))
			{
				canvasGroup.alpha = alpha + item * (1f - alpha);
				yield return null;
			}
		}

		public static IEnumerator HideUI(CanvasGroup canvasGroup, float time)
		{
			foreach (float item in TimerNormalized(time))
			{
				canvasGroup.alpha = 1f - item;
				yield return null;
			}
			((Component)(object)canvasGroup).gameObject.SetActive(false);
		}

		public static IEnumerator HideUI(Graphic ui, float time, bool deactive = false)
		{
			Color color = ui.color;
			color.a = 1f;
			foreach (float item in TimerNormalized(time))
			{
				color.a = 1f - item;
				ui.color = color;
				yield return null;
			}
			if (deactive)
			{
				ui.gameObject.SetActive(false);
			}
		}

		public static T GetInterface<T>(this Component component)
		{
			return (T)(object)Enumerable.FirstOrDefault<Component>(Enumerable.Where<Component>((IEnumerable<Component>)component.GetComponents<Component>(), (Func<Component, bool>)((Component c) => c is T)));
		}

		public static T GetInterface<T>(this GameObject obj)
		{
			return (T)(object)Enumerable.FirstOrDefault<Component>(Enumerable.Where<Component>((IEnumerable<Component>)obj.GetComponents<Component>(), (Func<Component, bool>)((Component c) => c is T)));
		}

		public static bool All<T>(this IEnumerable<T> ts, Func<T, int, bool> predicate)
		{
			int num = 0;
			foreach (T t in ts)
			{
				if (!predicate(t, num++))
				{
					return false;
				}
			}
			return true;
		}

		public static bool Any<T>(this IEnumerable<T> ts, Func<T, int, bool> predicate)
		{
			int num = 0;
			foreach (T t in ts)
			{
				if (predicate(t, num++))
				{
					return true;
				}
			}
			return false;
		}

		public static void ForceDestroy(GameObject gameObject)
		{
			if (Application.isPlaying)
			{
				UnityEngine.Object.Destroy(gameObject);
			}
			else
			{
				UnityEngine.Object.DestroyImmediate(gameObject);
			}
		}

		public static void DestroyChildren(this GameObject gameObject)
		{
			if (Application.isPlaying)
			{
				foreach (GameObject child in gameObject.GetChildren())
				{
					child.SetActive(false);
					UnityEngine.Object.Destroy(child);
				}
				return;
			}
			int childCount = gameObject.transform.childCount;
			for (int i = 0; i < childCount; i++)
			{
				UnityEngine.Object.DestroyImmediate(gameObject.transform.GetChild(0).gameObject);
			}
		}

		public static bool Diff<T>(this IEnumerable<T> ts, IEnumerable<T> target) where T : class
		{
			return ts.Diff(target, (T s, T t) => s == t);
		}

		public static bool Diff<T, U>(this IEnumerable<T> ts, IEnumerable<U> target, Func<T, U, bool> comparerer)
		{
			IEnumerator<U> enumerator = target.GetEnumerator();
			foreach (T t in ts)
			{
				if (!enumerator.MoveNext())
				{
					return false;
				}
				U current2 = enumerator.Current;
				if (!comparerer(t, current2))
				{
					return false;
				}
			}
			if (enumerator.MoveNext())
			{
				return false;
			}
			return true;
		}

		public static Color RandomColor()
		{
			return new Color(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value);
		}

		public static Vector2 Sum<T>(this IEnumerable<T> ts, Func<T, Vector2> selector)
		{
			Vector2 zero = Vector2.zero;
			foreach (T t in ts)
			{
				zero += selector(t);
			}
			return zero;
		}

		public static Vector3 Sum<T>(this IEnumerable<T> ts, Func<T, Vector3> selector)
		{
			Vector3 zero = Vector3.zero;
			foreach (T t in ts)
			{
				zero += selector(t);
			}
			return zero;
		}

		public static GenericPlatform GetGenericPlatform(RuntimePlatform platform)
		{
			if (platform == RuntimePlatform.WindowsEditor || platform == RuntimePlatform.WindowsPlayer || platform == RuntimePlatform.LinuxEditor || platform == RuntimePlatform.LinuxPlayer || platform == RuntimePlatform.OSXEditor || platform == RuntimePlatform.OSXPlayer)
			{
				return GenericPlatform.Desktop;
			}
			return GenericPlatform.Mobile;
		}

		public static IEnumerable<T> FindObjectOfAll<T>() where T : Component
		{
			return Enumerable.Where<T>((IEnumerable<T>)Resources.FindObjectsOfTypeAll<T>(), (Func<T, bool>)delegate(T obj)
			{
				Scene scene = obj.gameObject.scene;
				return obj.gameObject.scene.isLoaded;
			});
		}

		public static IEnumerable<int> Times(int times)
		{
			for (int i = 0; i < times; i++)
			{
				yield return i;
			}
		}

		public static bool IsInHierarchy(this GameObject gameObject)
		{
			if ((bool)gameObject)
			{
				Scene scene = gameObject.scene;
				return gameObject.scene.name != null;
			}
			return false;
		}
	}
}
