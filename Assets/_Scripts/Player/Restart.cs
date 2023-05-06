using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 重生点
/// </summary>
public class Restart : MonoBehaviour
{
    /// <summary>
    /// 存档点触发器
    /// </summary>
    private BoxCollider2D trigger;
    /// <summary>
    /// 该存档点是否已被触发
    /// </summary>
    public bool hasTrigger = false;
    /// <summary>
    /// 重生点控制器，同时也是该脚本绑定GameObject的父物体
    /// </summary>
    private RestartController restartController;

    private void Start()
    {
        restartController = transform.parent.GetComponent<RestartController>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (hasTrigger) return;
        if (collision.CompareTag("Player"))  //仅限触发对象为玩家
        {
            hasTrigger = true;
            restartController.UpdataLastPoint(this);
        }
    }

    /// <summary>
    /// //重置该存档点
    /// </summary>
    public void ResetPoint()
    {
        hasTrigger = false;
    }


}
