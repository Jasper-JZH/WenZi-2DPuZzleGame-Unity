using UnityEngine;

public class plank_code : MonoBehaviour
{
	private Vector4 temp;

	private bool ran_flag;

	private void Start()
	{
		temp = base.gameObject.GetComponent<SpriteRenderer>().color;
	}

	private void FixedUpdate()
	{
		temp[3] -= 0.0015f;
		if (ran_flag)
		{
			temp[1] -= 0.015f;
			if (temp[1] <= 0f)
			{
				temp[0] -= 0.015f;
				temp[3] -= 0.015f;
			}
		}
		base.gameObject.GetComponent<SpriteRenderer>().color = temp;
		if (temp[3] < 0.02f)
		{
			Object.Destroy(base.gameObject);
		}
	}

	public void Ran()
	{
		ran_flag = true;
	}
}
