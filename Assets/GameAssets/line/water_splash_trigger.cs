using System;
using UnityEngine;

public class water_splash_trigger : MonoBehaviour
{
	private AudioClip[] sound = (AudioClip[])(object)new AudioClip[5];

	private AudioSource[] soundAudioSource = (AudioSource[])(object)new AudioSource[5];

	private int temp_i;

	private int temp_j;

	private void Start()
	{
		temp_i = 0;
		temp_j = 20;
		for (int i = 0; i < 5; i++)
		{
			sound[i] = Resources.Load<AudioClip>("Sounds/splash" + Convert.ToString(i));
			soundAudioSource[i] = base.gameObject.AddComponent<AudioSource>();
			soundAudioSource[i].clip = sound[i];
			soundAudioSource[i].playOnAwake = false;
			soundAudioSource[i].volume = 0.3f;
		}
	}

	private void FixedUpdate()
	{
		if (temp_j <= 20)
		{
			temp_j++;
		}
	}

	private void OnTriggerEnter2D(Collider2D Hit)
	{
		Rigidbody2D component = ((Component)(object)Hit).GetComponent<Rigidbody2D>();
		water_line component2 = base.transform.parent.GetComponent<water_line>();
		float y = component.velocity.y;
		float angularVelocity = component.angularVelocity;
		component2.Disable_Auto_Splash();
		y += (((Component)(object)Hit).transform.position.x - base.transform.position.x) * angularVelocity / 400f;
		if (((Component)(object)Hit).gameObject.CompareTag("Player") && temp_j >= 20)
		{
			temp_j = 0;
			if (y > 2f || y < -2f)
			{
				if (!soundAudioSource[0].isPlaying && !soundAudioSource[1].isPlaying && !soundAudioSource[2].isPlaying && !soundAudioSource[3].isPlaying && !soundAudioSource[4].isPlaying)
				{
					soundAudioSource[temp_i].Play();
				}
				temp_i++;
				temp_i %= 5;
			}
		}
		component2.Splash(base.transform.position.x, y / 20f);
	}
}
