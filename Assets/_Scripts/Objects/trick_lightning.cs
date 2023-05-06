using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// 打雷机关
/// </summary>
public class trick_lightning : MonoBehaviour
{
    /// <summary>
    /// 是否已经写了震字
    /// </summary>
    public static bool hasWrite;
    /// <summary>
    /// 记录
    /// </summary>
    public static int count;
    /// <summary>
    /// 闪电特效
    /// </summary>
    public ParticleSystem lightning;
    /// <summary>
    /// 石头贴图列表
    /// </summary>
    public List<Sprite> stoneSpriteList;
    /// <summary>
    /// 石头当前贴图
    /// </summary>
    public Sprite curSprite;
    /// <summary>
    /// 要替换贴图的山
    /// </summary>
    public GameObject mountain;
    public GameObject road;
    /// <summary>
    /// 神物材质
    /// </summary>
    [SerializeField] private Animator ShenWu;
    /// <summary>
    /// 关闭黑幕
    /// </summary>
    [SerializeField] private GameObject HeiMu;
    // public Animator roadAnimator;
    private void Awake()
    {
        curSprite = stoneSpriteList[0];
        //roadAnimator.enabled = false;
        road.SetActive(false);
        count = 0;
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && hasWrite)
        {
            count++;
            Debug.Log("触发闪电机关");
            if(count == 1)
            {
                StartCoroutine(Lightning1(2f));
            }
            if (count == 2)
            {
                StartCoroutine(Lightning2(2f));
            }
            if (count == 3)
            {
                StartCoroutine(Lightning3(2f));
                transform.GetComponent<BoxCollider2D>().enabled = false;
            }
            hasWrite = false;
        }
    }
    /// <summary>
    /// 闪电效果
    /// </summary>
    /// <param name="_timeGap">闪电的间隔时间</param>
    /// <returns></returns>
    public IEnumerator Lightning1(float _timeGap)
    {
        //播放闪电特效
        lightning.Play();
        //替换石头贴图
        curSprite = stoneSpriteList[1];
        mountain.GetComponent<SpriteRenderer>().sprite = curSprite;
        //神物出现,黑幕关闭
        HeiMu.SetActive(false);
        ShenWu.enabled = true;
        yield return new WaitForSeconds(_timeGap);
        //神物消失,黑幕开启
        ShenWu.enabled = false;
        HeiMu.SetActive(true);
    }

    public IEnumerator Lightning2(float _timeGap)
    {
        //播放闪电特效
        lightning.Play();
        //替换石头贴图
        curSprite = stoneSpriteList[2];
        mountain.GetComponent<SpriteRenderer>().sprite = curSprite;
        //神物出现,黑幕关闭
        HeiMu.SetActive(false);
        ShenWu.enabled = true;
        yield return new WaitForSeconds(_timeGap);
        //神物消失,黑幕开启
        ShenWu.enabled = false;
        HeiMu.SetActive(true);
    }

    public IEnumerator Lightning3(float _timeGap)
    {
        //播放闪电特效
        lightning.Play();
        //替换石头贴图
        curSprite = stoneSpriteList[3];
        mountain.GetComponent<SpriteRenderer>().sprite = curSprite;
        //神物出现,黑幕关闭
        HeiMu.SetActive(false);
        ShenWu.enabled = true;
        yield return new WaitForSeconds(_timeGap);
        road.SetActive(true);
        //神物消失,黑幕开启
        ShenWu.enabled = false;
        HeiMu.SetActive(true);
    }
}
