using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Boat : MonoBehaviour
{

    private Vector2 oriPos;
    public RestartController restartController;
    private void Awake()
    {
        oriPos = transform.position;
        restartController.restartAction += ReSetBoat;
    }

    public void ReSetBoat()
    {
        transform.position = oriPos;
    }
}
