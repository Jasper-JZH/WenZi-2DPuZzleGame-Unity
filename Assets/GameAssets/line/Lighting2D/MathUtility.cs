using UnityEngine;

namespace Lighting2D
{
	public static class MathUtility
	{
		public static Vector3 ToVector3(this Vector4 v)
		{
			return new Vector3(v.x, v.y, v.z);
		}

		public static Vector3 ToVector3(this Vector2 v)
		{
			return new Vector3(v.x, v.y, 0f);
		}

		public static Vector3 ToVector3(this Vector2 v, float z)
		{
			return new Vector3(v.x, v.y, z);
		}

		public static Vector3 ToVector3XZ(this Vector2 v)
		{
			return new Vector3(v.x, 0f, v.y);
		}

		public static Vector3 ClipY(this Vector3 v)
		{
			return Vector3.Scale(v, new Vector3(1f, 0f, 1f));
		}

		public static Vector3 Set(this Vector3 v, float x = float.NaN, float y = float.NaN, float z = float.NaN)
		{
			x = (float.IsNaN(x) ? v.x : x);
			y = (float.IsNaN(y) ? v.y : y);
			z = (float.IsNaN(z) ? v.z : z);
			return new Vector3(x, y, z);
		}

		public static Vector2 ToVector2(this Vector3 v)
		{
			return new Vector2(v.x, v.y);
		}

		public static Vector2 ToVector2XZ(this Vector3 v)
		{
			return new Vector2(v.x, v.z);
		}

		public static Vector2 Abs(this Vector2 v)
		{
			return new Vector2(Mathf.Abs(v.x), Mathf.Abs(v.y));
		}

		public static Color Set(this Color color, float r = float.NaN, float g = float.NaN, float b = float.NaN, float a = float.NaN)
		{
			color.r = (float.IsNaN(r) ? color.r : r);
			color.g = (float.IsNaN(g) ? color.g : g);
			color.b = (float.IsNaN(b) ? color.b : b);
			color.a = (float.IsNaN(a) ? color.a : a);
			return color;
		}

		public static int SignInt(float x)
		{
			if (x > 0f)
			{
				return 1;
			}
			if (x < 0f)
			{
				return -1;
			}
			return 0;
		}

		public static float MapAngle(float ang)
		{
			if (ang > 180f)
			{
				ang -= 360f;
			}
			else if (ang < -180f)
			{
				ang += 360f;
			}
			return ang;
		}

		public static Quaternion QuaternionBetweenVector(Vector3 u, Vector3 v)
		{
			u = u.normalized;
			v = v.normalized;
			float num = Vector3.Dot(u, v);
			float w = Mathf.Sqrt(0.5f * (1f + num));
			float num2 = Mathf.Sqrt(0.5f * (1f - num));
			Vector3 vector = Vector3.Cross(u, v);
			return new Quaternion(num2 * vector.x, num2 * vector.y, num2 * vector.z, w);
		}

		public static float ToAng(float y, float x)
		{
			return Mathf.Atan2(y, x) * 57.29578f;
		}

		public static float ToAng(Vector2 v)
		{
			return Mathf.Atan2(v.y, v.x) * 57.29578f;
		}

		public static Vector2 Rotate(Vector2 v, float rad)
		{
			float num = Mathf.Cos(rad);
			float num2 = Mathf.Sin(rad);
			return new Vector2(num * v.x - num2 * v.y, num * v.y + num2 * v.x);
		}
	}
}
