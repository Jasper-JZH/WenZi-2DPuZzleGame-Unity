using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//点灯机关
public class trick_TwoLight : MonoBehaviour
{
    /// <summary>
    /// 灯火1
    /// </summary>
    public Light light1;
    /// <summary>
    /// 灯火2
    /// </summary>
    public Light light2;
    /// <summary>
    /// 斜坡机关
    /// </summary>
    public GameObject road;

    /// <summary>
    /// 机关已经被打开
    /// </summary>
    private bool opened;
    private void Awake()
    {
        road.SetActive(false);
        opened = false;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (opened) return;
        //当两个灯火都点亮时，开启斜坡
        if (collision.CompareTag("Player") && light1.Lighted && light2.Lighted)
        {
            road.SetActive(true);
            opened = true;
        }
    }
}
