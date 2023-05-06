using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class NPC : MonoBehaviour
{
    private UIManager uIManager;
    /// <summary>
    /// 该npc的名字
    /// </summary>
    [SerializeField] private string npcName;
    /// <summary>
    /// 提示的UI/文字
    /// </summary>
    [SerializeField] private Transform tip;

    /// <summary>
    /// 播放animator
    /// </summary>
    [SerializeField] private Animator Gradual_change;

    /// <summary>
    /// 对话提示动画效果
    /// </summary>
    private ParticleSystem dialogeTip;

    private TextMeshProUGUI tipTMP;

    private bool hasInterated;

    private void Awake()
    {
        uIManager = UIManager.Instance;
        tipTMP = tip.GetChild(0).GetComponent<TextMeshProUGUI>();
        if(transform.childCount>1)
        dialogeTip = transform.GetChild(2).GetComponent<ParticleSystem>();
        if (dialogeTip) dialogeTip.Stop();
        if (tipTMP!=null)
        {
            tipTMP.fontMaterial.SetColor("_FaceColor", new Color(255f, 255f, 255f, 0f));
            tipTMP.fontMaterial.SetColor("_OutlineColor", new Color(0f, 0f, 0f, 0f));
            tipTMP.fontMaterial.SetFloat("_OutlineWidth", 0.06f);
        }
        hasInterated = false;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(hasInterated)
        {
            if(dialogeTip)dialogeTip.Stop();
        }
        if (Input.GetKey(KeyCode.E) && !hasInterated)
        {
            hasInterated = true;
            ShowTip(false);
            //打开对话面板
            uIManager.NewNPCDialoge(npcName);
        }
    }

    //显示按钮提示UI
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (hasInterated) return;
        if (collision.CompareTag("Player")) 
        {
            if (dialogeTip) dialogeTip.Play();
            ShowTip(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (dialogeTip) dialogeTip.Stop();
            ShowTip(false);
        }
        if (hasInterated)
        {
            //已经交互过后，npc会在玩家离开后消失
            //TODO:逐渐透明的效果
            if(Gradual_change)
            Gradual_change.enabled = true;
        }

    }

    private void ShowTip(bool _show)
    {
        if(_show)
        {
            tipTMP.fontMaterial.DOColor(new Color(255f, 255f, 255f, 255f), "_FaceColor", 0.3f).SetUpdate(true);
            tipTMP.fontMaterial.DOColor(new Color(0f, 0f, 0f, 255f), "_OutlineColor", 0.3f).SetUpdate(true);
            tip.DOLocalMoveY(tip.localPosition.y + 3f, 0.3f).SetUpdate(true);
        }
        else
        {
            
            tipTMP.fontMaterial.DOColor(new Color(255f, 255f, 255f, 0f), "_FaceColor", 0.1f).SetUpdate(true);
            tipTMP.fontMaterial.DOColor(new Color(0f, 0f, 0f, 0f), "_OutlineColor", 0.1f).SetUpdate(true);
            tip.DOLocalMoveY(tip.localPosition.y - 3f, 0.2f).SetUpdate(true);
        }
    }

    private void Disappear()
    {
        Debug.Log("已经销毁");
        gameObject.SetActive(false);
    }
}
