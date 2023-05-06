using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndPoint : MonoBehaviour
{
    /// <summary>
    /// 是否等待
    /// </summary>
    public bool wait;
    /// <summary>
    /// 等待时间
    /// </summary>
    [SerializeField] private float waitTime;
    /// <summary>
    /// 是否需要按下E键
    /// </summary>
    public bool needPress;

    private void Awake()
    {
        waitTime = 3f;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!needPress)
        {
            if (wait)
            {
                StartCoroutine(PassLevel(waitTime));
            }
            else
            {
                StartCoroutine(PassLevel(waitTime));
            }
            Debug.Log("过关！");
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(needPress && Input.GetKey(KeyCode.E))
        {
            StartCoroutine(PassLevel(waitTime));
            Debug.Log("前往下一场景");
        }
    }

    /// <summary>
    /// 延迟一段时间后切换到下一关（场景）
    /// </summary>
    /// <param name="_waitSeconds"></param>
    /// <returns></returns>
    private IEnumerator PassLevel(float _waitSeconds)
    {
        DrawController.PlayBlackAnimation();
        //清除玩家身上的东西
        ObjectsController.ClearAllObjects();
        yield return new WaitForSeconds(_waitSeconds);
        Dictionary.Instance.InitPlayerDic(GameManager.GetCurLevel()+1);    //更新玩家的字典，为下一关做准备
        GameManager.Instance.PassLevel();
    }
}
