using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Video : MonoBehaviour
{
    public Transform obj;
    public Transform start;
    public Transform end;
    public float time;

    private void Start()
    {
        obj.position = start.position;
    }
    private void Update()
    {
        if (Input.GetKey(KeyCode.P))
        {
            Debug.Log("开始移动");
            obj.DOMoveX(end.position.x, time);
        }
        if(Input.GetKey(KeyCode.R))
        {
            obj.position = start.position;
        }
    }
}
