using System;
using Lighting2D;
using UnityEngine;

public class zuanable_code : MonoBehaviour
{
	public bool free0_flag;

	public bool free1_flag;

	private float DisPoint2Line(Vector2 point, Vector2 linePoint, Vector2 lineVector)
	{
		Vector2 vector = point - linePoint;
		Vector2 vector2 = Vector3.Project(vector, lineVector);
		return Mathf.Sqrt(Mathf.Pow(vector.magnitude, 2f) - Mathf.Pow(vector2.magnitude, 2f));
	}

	public void free0()
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		free0_flag = true;
		if (!free1_flag)
		{
			return;
		}
		Rigidbody2D component = GetComponent<Rigidbody2D>();
		if ((UnityEngine.Object)(object)component != null)
		{
			if ((int)component.bodyType != 0)
			{
				component.bodyType = (RigidbodyType2D)0;
			}
		}
		else
		{
			base.gameObject.AddComponent<Rigidbody2D>().bodyType = (RigidbodyType2D)0;
		}
	}

	public void free1()
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		free1_flag = true;
		if (!free0_flag)
		{
			return;
		}
		Rigidbody2D component = GetComponent<Rigidbody2D>();
		if ((UnityEngine.Object)(object)component != null)
		{
			if ((int)component.bodyType != 0)
			{
				component.bodyType = (RigidbodyType2D)0;
			}
		}
		else
		{
			base.gameObject.AddComponent<Rigidbody2D>().bodyType = (RigidbodyType2D)0;
		}
	}

	public void succeedFreeFlag(bool b0, bool b1)
	{
		free0_flag = b0;
		free1_flag = b1;
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		//IL_0194: Unknown result type (might be due to invalid IL or missing references)
		//IL_0199: Unknown result type (might be due to invalid IL or missing references)
		if (!collision.gameObject.CompareTag("Player"))
		{
			return;
		}
		//player component = collision.gameObject.GetComponent<player>();
		//if (!component.getZuan() || !((UnityEngine.Object)(object)collision.gameObject.GetComponent<Rigidbody2D>() != null))
		//{
		//	return;
		//}
		Quaternion rotation = base.transform.rotation;
		rotation.x = 0f - rotation.x;
		rotation.y = 0f - rotation.y;
		rotation.z = 0f - rotation.z;
		Vector3 vector = rotation * (collision.contacts[0]).normal;
		bool flag = Math.Abs(vector.x) < Math.Abs(vector.y);
		Vector2 vector2 = (base.transform.rotation * new Vector2(1f, 0f)).normalized;
		Vector2 vector3 = (base.transform.rotation * new Vector2(0f, 1f)).normalized;
		if (flag)
		{
			float magnitude = (collision.relativeVelocity * vector3).magnitude;
		}
		else
		{
			float magnitude2 = (collision.relativeVelocity * vector2).magnitude;
		}
		int num = -1;
		if ((UnityEngine.Object)(object)GetComponent<BoxCollider2D>() != null)
		{
			num = 0;
		}
		else if ((UnityEngine.Object)(object)GetComponent<PolygonCollider2D>() != null)
		{
			num = 1;
		}
		else if ((UnityEngine.Object)(object)GetComponent<CircleCollider2D>() != null)
		{
			num = 2;
		}
		if (num < 0)
		{
			return;
		}
		//component.stopZuan();
		ContactPoint2D val = collision.contacts[0];
		Texture2D texture = GetComponent<SpriteRenderer>().sprite.texture;
		if (flag)
		{
			Vector2 vector4 = new Vector2(0f, 0f);
			Vector2 vector5 = new Vector2(0f, 0f);
			switch (num)
			{
			case 0:
			{
				BoxCollider2D component4 = GetComponent<BoxCollider2D>();
				vector4 = ((Component)(object)component4).transform.TransformPoint(((Collider2D)component4).offset - new Vector2(component4.size.x, 0f) * 0.5f).ToVector2();
				vector5 = ((Component)(object)component4).transform.TransformPoint(((Collider2D)component4).offset + new Vector2(component4.size.x, 0f) * 0.5f).ToVector2();
				break;
			}
			case 1:
			{
				PolygonCollider2D component3 = GetComponent<PolygonCollider2D>();
				float num2 = 0f;
				float num3 = 0f;
				Vector2[] points = component3.points;
				for (int i = 0; i < points.Length; i++)
				{
					Vector2 vector6 = points[i];
					if (0f - vector6.x > num2)
					{
						num2 = 0f - vector6.x;
					}
					if (vector6.x > num3)
					{
						num3 = vector6.x;
					}
				}
				vector4 = ((Component)(object)component3).transform.TransformPoint(((Collider2D)component3).offset - new Vector2(num2, 0f)).ToVector2();
				vector5 = ((Component)(object)component3).transform.TransformPoint(((Collider2D)component3).offset + new Vector2(num3, 0f)).ToVector2();
				break;
			}
			case 2:
			{
				CircleCollider2D component2 = GetComponent<CircleCollider2D>();
				vector4 = ((Component)(object)component2).transform.TransformPoint(((Collider2D)component2).offset - new Vector2(component2.radius, 0f)).ToVector2();
				vector5 = ((Component)(object)component2).transform.TransformPoint(((Collider2D)component2).offset + new Vector2(component2.radius, 0f)).ToVector2();
				break;
			}
			default:
				Debug.LogError(">3");
				break;
			}
			float num4 = DisPoint2Line(val.point, vector4, vector3);
			float num5 = DisPoint2Line(val.point, vector5, vector3);
			Vector2 vector7 = (vector5 - vector4) * (num4 / (num4 + num5)) + vector4;
			int num6 = (int)((float)texture.width * num4 / (num4 + num5));
			int num7 = (int)((float)texture.width * num5 / (num4 + num5));
			int height = texture.height;
			Texture2D texture2D = new Texture2D(num6, height);
			Texture2D texture2D2 = new Texture2D(num7, height);
			for (int j = 0; j < num6; j++)
			{
				for (int k = 0; k < height; k++)
				{
					texture2D.SetPixel(j, k, texture.GetPixel(j, k));
				}
			}
			texture2D.Apply();
			for (int l = 0; l < num7; l++)
			{
				for (int m = 0; m < height; m++)
				{
					texture2D2.SetPixel(l, m, texture.GetPixel(Math.Min(l + num6, texture.width - 1), m));
				}
			}
			texture2D2.Apply();
			GameObject gameObject = UnityEngine.Object.Instantiate(base.gameObject, (vector4 + vector7) / 2f, base.transform.rotation);
			Sprite sprite2 = (gameObject.GetComponent<SpriteRenderer>().sprite = Sprite.Create(texture2D, new Rect(0f, 0f, texture2D.width, texture2D.height), new Vector2(0.5f, 0.5f)));
			GameObject gameObject2 = UnityEngine.Object.Instantiate(base.gameObject, (vector5 + vector7) / 2f, base.transform.rotation);
			Sprite sprite4 = (gameObject2.GetComponent<SpriteRenderer>().sprite = Sprite.Create(texture2D2, new Rect(0f, 0f, texture2D2.width, texture2D2.height), new Vector2(0.5f, 0.5f)));
			switch (num)
			{
			case 0:
				UnityEngine.Object.Destroy((UnityEngine.Object)(object)gameObject.GetComponent<BoxCollider2D>());
				gameObject.AddComponent<BoxCollider2D>();
				UnityEngine.Object.Destroy((UnityEngine.Object)(object)gameObject2.GetComponent<BoxCollider2D>());
				gameObject2.AddComponent<BoxCollider2D>();
				break;
			case 1:
				UnityEngine.Object.Destroy((UnityEngine.Object)(object)gameObject.GetComponent<PolygonCollider2D>());
				gameObject.AddComponent<BoxCollider2D>();
				UnityEngine.Object.Destroy((UnityEngine.Object)(object)gameObject2.GetComponent<PolygonCollider2D>());
				gameObject2.AddComponent<BoxCollider2D>();
				break;
			case 2:
				UnityEngine.Object.Destroy((UnityEngine.Object)(object)gameObject.GetComponent<CircleCollider2D>());
				gameObject.AddComponent<PolygonCollider2D>();
				UnityEngine.Object.Destroy((UnityEngine.Object)(object)gameObject2.GetComponent<CircleCollider2D>());
				gameObject2.AddComponent<PolygonCollider2D>();
				break;
			}
			gameObject.GetComponent<zuanable_code>().succeedFreeFlag(free0_flag, free1_flag);
			gameObject.GetComponent<zuanable_code>().free1();
			gameObject2.GetComponent<zuanable_code>().succeedFreeFlag(free0_flag, free1_flag);
			gameObject2.GetComponent<zuanable_code>().free0();
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		Vector2 vector8 = new Vector2(0f, 0f);
		Vector2 vector9 = new Vector2(0f, 0f);
		switch (num)
		{
		case 0:
		{
			BoxCollider2D component7 = GetComponent<BoxCollider2D>();
			vector8 = ((Component)(object)component7).transform.TransformPoint(((Collider2D)component7).offset - new Vector2(0f, component7.size.y) * 0.5f).ToVector2();
			vector9 = ((Component)(object)component7).transform.TransformPoint(((Collider2D)component7).offset + new Vector2(0f, component7.size.y) * 0.5f).ToVector2();
			break;
		}
		case 1:
		{
			PolygonCollider2D component6 = GetComponent<PolygonCollider2D>();
			float num8 = 0f;
			float num9 = 0f;
			Vector2[] points = component6.points;
			for (int i = 0; i < points.Length; i++)
			{
				Vector2 vector10 = points[i];
				if (0f - vector10.y > num8)
				{
					num8 = 0f - vector10.y;
				}
				if (vector10.y > num9)
				{
					num9 = vector10.y;
				}
			}
			vector8 = ((Component)(object)component6).transform.TransformPoint(((Collider2D)component6).offset - new Vector2(0f, num8)).ToVector2();
			vector9 = ((Component)(object)component6).transform.TransformPoint(((Collider2D)component6).offset + new Vector2(0f, num9)).ToVector2();
			break;
		}
		case 2:
		{
			CircleCollider2D component5 = GetComponent<CircleCollider2D>();
			vector8 = ((Component)(object)component5).transform.TransformPoint(((Collider2D)component5).offset - new Vector2(0f, component5.radius)).ToVector2();
			vector9 = ((Component)(object)component5).transform.TransformPoint(((Collider2D)component5).offset + new Vector2(0f, component5.radius)).ToVector2();
			break;
		}
		default:
			Debug.LogError(">3");
			break;
		}
		float num10 = DisPoint2Line(val.point, vector8, vector2);
		float num11 = DisPoint2Line(val.point, vector9, vector2);
		Vector2 vector11 = (vector9 - vector8) * (num10 / (num10 + num11)) + vector8;
		int num12 = (int)((float)texture.height * num10 / (num10 + num11));
		int num13 = (int)((float)texture.height * num11 / (num10 + num11));
		int width = texture.width;
		Texture2D texture2D3 = new Texture2D(width, num12);
		Texture2D texture2D4 = new Texture2D(width, num13);
		for (int n = 0; n < width; n++)
		{
			for (int num14 = 0; num14 < num12; num14++)
			{
				texture2D3.SetPixel(n, num14, texture.GetPixel(n, num14));
			}
		}
		texture2D3.Apply();
		for (int num15 = 0; num15 < width; num15++)
		{
			for (int num16 = 0; num16 < num13; num16++)
			{
				texture2D4.SetPixel(num15, num16, texture.GetPixel(num15, Math.Min(num16 + num12, texture.height - 1)));
			}
		}
		texture2D4.Apply();
		GameObject gameObject3 = UnityEngine.Object.Instantiate(base.gameObject, (vector8 + vector11) / 2f, base.transform.rotation);
		Sprite sprite6 = (gameObject3.GetComponent<SpriteRenderer>().sprite = Sprite.Create(texture2D3, new Rect(0f, 0f, texture2D3.width, texture2D3.height), new Vector2(0.5f, 0.5f)));
		GameObject gameObject4 = UnityEngine.Object.Instantiate(base.gameObject, (vector9 + vector11) / 2f, base.transform.rotation);
		Sprite sprite8 = (gameObject4.GetComponent<SpriteRenderer>().sprite = Sprite.Create(texture2D4, new Rect(0f, 0f, texture2D4.width, texture2D4.height), new Vector2(0.5f, 0.5f)));
		switch (num)
		{
		case 0:
			UnityEngine.Object.Destroy((UnityEngine.Object)(object)gameObject3.GetComponent<BoxCollider2D>());
			gameObject3.AddComponent<BoxCollider2D>();
			UnityEngine.Object.Destroy((UnityEngine.Object)(object)gameObject4.GetComponent<BoxCollider2D>());
			gameObject4.AddComponent<BoxCollider2D>();
			break;
		case 1:
			UnityEngine.Object.Destroy((UnityEngine.Object)(object)gameObject3.GetComponent<PolygonCollider2D>());
			gameObject3.AddComponent<BoxCollider2D>();
			UnityEngine.Object.Destroy((UnityEngine.Object)(object)gameObject4.GetComponent<PolygonCollider2D>());
			gameObject4.AddComponent<BoxCollider2D>();
			break;
		case 2:
			UnityEngine.Object.Destroy((UnityEngine.Object)(object)gameObject3.GetComponent<CircleCollider2D>());
			gameObject3.AddComponent<PolygonCollider2D>();
			UnityEngine.Object.Destroy((UnityEngine.Object)(object)gameObject4.GetComponent<CircleCollider2D>());
			gameObject4.AddComponent<PolygonCollider2D>();
			break;
		}
		gameObject3.GetComponent<zuanable_code>().succeedFreeFlag(free0_flag, free1_flag);
		gameObject3.GetComponent<zuanable_code>().free1();
		gameObject4.GetComponent<zuanable_code>().succeedFreeFlag(free0_flag, free1_flag);
		gameObject4.GetComponent<zuanable_code>().free0();
		UnityEngine.Object.Destroy(base.gameObject);
	}
}
