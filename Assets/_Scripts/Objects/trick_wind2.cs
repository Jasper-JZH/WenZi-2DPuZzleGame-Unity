using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trick_wind2 : MonoBehaviour
{
    public static bool hasWrite = false;
    public ParticleSystem wind;
    public PlayerMovement playerMovement;
    //public CapsuleCollider2D playerCapsuleCollider2D;

    private void Awake()
    {
        wind.Stop();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && hasWrite)
        {
            Debug.Log("触发风场机关");
            wind.Play();
            //修改角色重力，时间为3秒
            StartCoroutine(ChangeGravity());
        }
    }

    public IEnumerator ChangeGravity()
    {
        playerMovement.rb.gravityScale = -1f;
        yield return new WaitForSeconds(3f);
        playerMovement.rb.gravityScale = 1f;
    }

}
