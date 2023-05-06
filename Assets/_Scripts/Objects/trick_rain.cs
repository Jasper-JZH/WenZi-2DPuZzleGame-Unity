using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trick_rain : MonoBehaviour
{
    public List<ParticleSystem> rains;
    public ParticleSystem singleRain;
    public static bool hasWrite;
    private bool isRaining;
    private void Awake()
    {
        hasWrite = false;
        isRaining = true;
        singleRain.Stop();
    }

    private void Update()
    {
      if(hasWrite && isRaining)  //玩家写了雨字，则关闭特效
        {
            foreach(var rain in rains)
            {
                rain.Stop();
            }
            isRaining = false;
            //播放云朵下的雨
            singleRain.Play();
        }

    }
}
