using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class NPC_Jin : MonoBehaviour
{
    /// <summary>
    /// 暂停面板背景材质
    /// </summary>
    [SerializeField] private Material blinker;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public IEnumerator Blinker1(float timelife)
    {
        yield return new WaitForSeconds(timelife);
        blinker.DOFloat(0, "_UnderlaySoftness", 0.5f).SetUpdate(true);
    }

    public IEnumerator Blinker2(float timelife)
    {
        yield return new WaitForSeconds(timelife);
        blinker.DOFloat(0.8f, "_UnderlaySoftness", 0.5f).SetUpdate(true);
    }

    private void OnEnable()
    {
        blinker.DOFloat(0.8f, "_UnderlaySoftness", 0.5f).SetUpdate(true);
        StartCoroutine(Blinker1(0.5f));
        StartCoroutine(Blinker2(1.0f));
        StartCoroutine(Blinker1(1.5f));
        StartCoroutine(Blinker2(2.0f));
        StartCoroutine(Blinker1(2.5f));
        StartCoroutine(Blinker2(3.0f));
        StartCoroutine(Blinker1(3.5f));
    }


}
