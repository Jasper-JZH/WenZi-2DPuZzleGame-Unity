using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trick_wind : MonoBehaviour
{
    public static bool hasWrite = false;
    public List<ParticleSystem> winds;
    public CapsuleCollider2D playerCapsuleCollider2D;

    private void Awake()
    {
        //foreach (var item in winds)
        //{
        //    item.Stop();
        //}
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && hasWrite)
        {
            Debug.Log("触发风场机关");
            //foreach (var item in winds)
            //{
            //    item.Play();
            //}
            playerCapsuleCollider2D = collision.GetComponent<CapsuleCollider2D>();
            playerCapsuleCollider2D.isTrigger = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (playerCapsuleCollider2D.isTrigger == true)
            {
                playerCapsuleCollider2D.isTrigger = false;
            }
        }
    }
}
