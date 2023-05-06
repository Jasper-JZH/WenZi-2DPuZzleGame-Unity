using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 烟雾NPC特殊脚本
/// </summary>
public class NPC_smoke : MonoBehaviour
{
    private NPC npc;
    /// <summary>
    /// 烟雾躲开的动画
    /// </summary>
    //public Animation anim;
    public Animator animator;
    /// <summary>
    /// 已经躲开player一次
    /// </summary>
    private bool hasRanaway;
    /// <summary>
    /// 逃跑时的烟雾特效
    /// </summary>
    private ParticleSystem RanawayAnim;
    private void Awake()
    {
        npc = transform.GetComponent<NPC>();
        npc.enabled = false;
        RanawayAnim = transform.GetChild(3).GetComponent<ParticleSystem>();
        RanawayAnim.Stop();
        //anim = transform.GetComponent<Animation>();
        hasRanaway = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !hasRanaway)
        {
            RanawayAnim.Play();
            animator.SetTrigger("Touch");
            //anim.Play();
            hasRanaway = true;
            npc.enabled = true;
        }
    }
}
