using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// 角色重生相关控制
/// </summary>
public class RestartController : MonoBehaviour
{
    /// <summary>
    /// 保存当前关卡的所有存档点
    /// </summary>
    [SerializeField] private List<Restart> restartPointsList = new List<Restart>();
    /// <summary>
    /// 最近经过的存档点
    /// </summary>
    [SerializeField] private Restart lastRestartPoint;
    /// <summary>
    /// 角色的引用
    /// </summary>
    [SerializeField] private Transform player;
    /// <summary>
    /// 关卡起点
    /// </summary>
    [SerializeField] private Vector3 StartPoint;
    /// <summary>
    /// 关卡终点
    /// </summary>
    [SerializeField] private GameObject EndPoint;

    /// <summary>
    /// 等待CG播放时的位置
    /// </summary>
    [SerializeField] private Vector3 waitPoint;
    public Action restartAction;


    private void Awake()
    {
        StartPoint = GameObject.Find("起点").transform.position;
        waitPoint = GameObject.Find("等待点").transform.position;
        EndPoint = GameObject.Find("终点");
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        player.position = waitPoint;
        //添加场景中所有重生点到链表中
        Restart[] points = transform.GetComponentsInChildren<Restart>();
        foreach(var point in points)
        {
            point.ResetPoint(); //重置该存档点
            restartPointsList.Add(point);
        }
    }

    /// <summary>
    /// 更新最新的存档点
    /// </summary>
    /// <param name="_newPoint"></param>
    public void UpdataLastPoint(Restart _newPoint)
    {
        lastRestartPoint = _newPoint;
        Debug.Log("存档点更新！");
    }

    /// <summary>
    /// 角色在最近存档点重生
    /// </summary>
    public void Restart()
    {
        ObjectsController.ClearAllObjects();
        if(restartAction!=null)restartAction();
        if (lastRestartPoint!=null)
        {
            player.position = lastRestartPoint.transform.position;
        }
        else //在初始点重生
        {
            player.position = StartPoint;
        }
    }
}
