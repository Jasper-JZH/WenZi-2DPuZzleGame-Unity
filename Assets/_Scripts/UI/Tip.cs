using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tip : MonoBehaviour
{
    [SerializeField] GameObject tip;
    public Animation anim;
    private void Awake()
    {
        tip.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //显示箭头动画
            tip.SetActive(true);
            anim.Play();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            anim.Stop();
            //显示箭头动画
            tip.SetActive(false);
        }
    }
}
