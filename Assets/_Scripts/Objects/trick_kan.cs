using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 用坎炸开地面的机关
/// </summary>
public class trick_kan : MonoBehaviour
{
    public Animator stoneAnimator;
    public CapsuleCollider2D cc;
    private void Awake()
    {
        stoneAnimator = transform.GetComponentInChildren<Animator>();
        if (stoneAnimator)
        {
            stoneAnimator.enabled = false;
            cc =transform.GetComponent<CapsuleCollider2D>();
        }
    }

    private void OnParticleCollision(GameObject other)
    {
        Debug.Log(other.name);
        if (other.CompareTag("坎"))
        {
            stoneAnimator.enabled = true;
            //cc.isTrigger = true;
            cc.enabled = false;
        }
    }
}
