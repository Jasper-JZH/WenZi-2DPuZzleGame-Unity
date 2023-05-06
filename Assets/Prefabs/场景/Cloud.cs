using DG.Tweening;
using UnityEngine;

public class Cloud : MonoBehaviour
{
	[SerializeField]
	private SpriteRenderer sprite1;

	[SerializeField]
	private SpriteRenderer sprite2;

	[SerializeField]
	private float speed;

	[SerializeField]
	private bool isFadeIn;

	[SerializeField]
	private float fadeTime = 3f;

	private SpriteRenderer temSprite;

	private void Start()
	{
		if (sprite1.transform.localPosition.x < sprite1.sprite.bounds.size.x / 2f)
		{
			temSprite = sprite1;
		}
		else
		{
			temSprite = sprite2;
		}
		speed = Mathf.Abs(speed);
	}


	private void LateUpdate()
	{
		Vector3 localPosition = sprite1.transform.localPosition;
		localPosition.x += speed * Time.deltaTime; //* LogicManager.PhysTimeRate;
		sprite1.transform.localPosition = localPosition;
		Vector3 localPosition2 = sprite2.transform.localPosition;
		localPosition2.x += speed * Time.deltaTime; //* LogicManager.PhysTimeRate;
		sprite2.transform.localPosition = localPosition2;
		if (temSprite.transform.localPosition.x > 0f)
		{
			SpriteRenderer spriteRenderer = ((!(temSprite == sprite1)) ? sprite1 : sprite2);
			Vector3 localPosition3 = spriteRenderer.transform.localPosition;
			localPosition3.x -= temSprite.sprite.bounds.size.x * 2f;
			spriteRenderer.transform.localPosition = localPosition3;
			if (isFadeIn)
			{
				Debug.Log("Do Fade" + spriteRenderer.name);
				spriteRenderer.DOFade(0f, fadeTime).From();
			}
			temSprite = spriteRenderer;
		}
	}
}


