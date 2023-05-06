using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trick_StoneDoor : MonoBehaviour
{
    public Animator trickAnimator;
    public Animator stoneAnimator;


    private void Awake()
    {
        trickAnimator = transform.GetChild(0).GetComponent<Animator>();
        stoneAnimator = transform.GetChild(1).GetComponent<Animator>();
        if (trickAnimator) trickAnimator.enabled = false;
        if (stoneAnimator) stoneAnimator.enabled = false;
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.CompareTag("金"))
        {
            //触发动画
            trickAnimator.enabled = true;
            StartCoroutine(stoneDoorDown(1f));
            //将碰撞器去掉，不妨碍玩家
            transform.GetComponent<BoxCollider2D>().enabled = false;
        }
    }

    private IEnumerator stoneDoorDown(float _delaTime)
    {
        yield return new WaitForSeconds(_delaTime);
        stoneAnimator.enabled = true;
    }
}
