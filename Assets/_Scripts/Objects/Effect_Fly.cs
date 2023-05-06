using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect_Fly : MonoBehaviour
{
    public static bool hasWrite = false;
    public Rigidbody2D playerRB;
    public PlayerMovement playerMovement;
    public Vector2 flyForce;
    private void Start()
    {
        hasWrite = false;
    }

    private void Update()
    {
        if(hasWrite)
        {
            playerMovement.rb.gravityScale = 0.2f;
            playerMovement.fallFactor = 2f;
            if (Input.GetKey(KeyCode.Space))
            {
                playerRB.AddForce(flyForce, ForceMode2D.Impulse);
            }
        }
    }
}
