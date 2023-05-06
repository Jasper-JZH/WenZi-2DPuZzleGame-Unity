using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 第四关的所有机关
/// </summary>
public class Trick_Level4 : MonoBehaviour
{
    //两盏待点亮的灯
    public Light light1;
    public Light light2;

    //一个斜坡
    public GameObject slope;

    //一个土堆
    public GameObject mound;
    private void Awake()
    {
        slope.SetActive(true);
        mound.SetActive(true);
    }

    private void Update()
    {
        Trick_1();
        Trick_2();
    }


    /// <summary>
    /// 斜坡机关
    /// </summary>
    public void Trick_1()
    {
        if (light1.Lighted)
        {
            slope.SetActive(false);
        }
    }

    /// <summary>
    /// 土堆机关
    /// </summary>
    public void Trick_2()
    {
        if (light1.Lighted && light2.Lighted)
        {
            mound.GetComponent<Animator>().enabled = true;
        }
    }
}
