using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 适用于第5,6关的机关
/// </summary>
public class Trick_Level5 : MonoBehaviour
{
    //一盏待点亮的灯
    public Light light1;

    //三个平台
    public List<GameObject> platforms = new List<GameObject>();

    public GameObject platform;

    private void Awake()
    {
        platform.SetActive(true);
        platform.GetComponent<Animator>().enabled = false;
    }

    private void Update()
    {
        Trick_1();
    }

    /// <summary>
    /// 平台机关
    /// </summary>
    public void Trick_1()
    {
        if (light1.Lighted)
        {
            platform.GetComponent<Animator>().enabled = true;
        }
    }
}
