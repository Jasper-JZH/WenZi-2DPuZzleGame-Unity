using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 炸开石头才显示金
/// </summary>
public class NPC_JinHide : MonoBehaviour
{
    public GameObject jinNPC;
    public GameObject stone;
    private void Awake()
    {
        jinNPC.SetActive(false);
    }

    private void Update()
    {
        if(!stone)
        {
            jinNPC.SetActive(true);
        }
    }
}
