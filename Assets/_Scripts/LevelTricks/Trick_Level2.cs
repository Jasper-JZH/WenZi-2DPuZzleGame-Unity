using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 第二关的所有机关
/// </summary>
public class Trick_Level2 : MonoBehaviour
{
    //四盏待点亮的灯
    public Light light1;
    public Light light2;
    public Light light3;
    public Light light4;

    //一个平台
    public GameObject platform;

    //两个斜坡
    public GameObject slope1;
    public GameObject slope2;

    private void Awake()
    {
        platform.SetActive(false);
        slope1.SetActive(false);
        slope2.SetActive(false);
    }

    private void Update()
    {
        Trick_1();
        Trick_2();
        Trick_3();
    }


    /// <summary>
    /// 平台机关
    /// </summary>
    public void Trick_1()
    {
        if(light1.Lighted)
        {
            platform.SetActive(true);
        }
    }

    /// <summary>
    /// 斜坡机关1
    /// </summary>
    public void Trick_2()
    {
        if(light2.Lighted && light3.Lighted)
        {
            slope1.SetActive(true);
        }
    }

    /// <summary>
    /// 斜坡机关2
    /// </summary>
    public void Trick_3()
    {
        if (light2.Lighted && light3.Lighted && light4.Lighted)
        {
            slope2.SetActive(true);
        }
    }
}
