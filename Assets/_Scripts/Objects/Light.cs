using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//放在火台上实现点灯机关
public class Light : MonoBehaviour
{
    /// <summary>
    /// 子物体火
    /// </summary>
    public GameObject fire;
    /// <summary>
    /// 已经被点亮
    /// </summary>
    public bool Lighted;   
    /// <summary>
    /// 正在点亮中
    /// </summary>
    private bool isLighting;

    private void Awake()
    {
        fire.SetActive(false);
        isLighting = false;
        Lighted = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.name);
        if (collision.CompareTag("火"))
        {
            if (Lighted) return;
            isLighting = true;
            StartCoroutine(Lighting(1.5f));
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("火"))
        {
            if (Lighted) return;
            isLighting = false;
        }
    }

    //计时点灯
    public IEnumerator Lighting(float _time)
    {
        while (isLighting && _time > 0)
        {
            _time -= Time.deltaTime;
            yield return null;
        }
        if (_time <= 0)
        {
            //点亮成功
            fire.SetActive(true);
            Lighted = true;
        }
    }
}
