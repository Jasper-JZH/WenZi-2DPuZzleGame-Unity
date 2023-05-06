using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PausePanel : PanelBase
{

    /// <summary>
    /// 返回按钮
    /// </summary>
    [SerializeField] private Button returnButton;
    /// <summary>
    /// 返回主界面按钮
    /// </summary>
    [SerializeField] private Button toMenuButton;
    /// <summary>
    /// 暂停面板背景材质
    /// </summary>
    [SerializeField] private Material backGroundMat;
 
    private UIManager uIManager;

    private void Start()
    {
        uIManager = UIManager.Instance;
        //按钮事件绑定
        returnButton.onClick.AddListener(delegate ()
        {
            AudioManager.PlayerAudio(AudioName.Click, false);
            uIManager.gameManager.ContinueGame();
        });
        toMenuButton.onClick.AddListener(delegate ()
        {
            AudioManager.PlayerAudio(AudioName.Click, false);
            uIManager.gameManager.BackToMenu();
        });

    }

    public override void OnPanelShow()
    {
        this.gameObject.SetActive(true);
    }

    public override void OnPanelClose()
    {
        this.gameObject.SetActive(false);
    }

    //通过迭代器定义一个方法
    IEnumerator Delayed(int i)
    {
        //Debug.Log("开始");
        yield return new WaitForSeconds(0.01f);
        //Debug.Log("输出开始后三秒后执行我");
        backGroundMat.DOFloat(1, "_GhostFX_ClipRight_1", 1.0f).SetUpdate(true);
    }

    //在程序种调用协程
    private void OnEnable()
    {
        //StartCoroutine(Delayed(1));
        backGroundMat.DOFloat(1, "_GhostFX_ClipRight_1", 1.0f).SetUpdate(true);
        backGroundMat.DOFloat(1, "_SpriteFade", 3.0f).SetUpdate(true);
        //Debug.Log("OnEnable的_GhostFX_ClipRight_1  " + backGroundMat.GetFloat("_GhostFX_ClipRight_1"));
        //Debug.Log("OnDisable的_SpriteFade " + backGroundMat.GetFloat("_SpriteFade"));
    }

    private void OnDisable()
    {
        backGroundMat.DOFloat(0, "_GhostFX_ClipRight_1", .0001f).SetUpdate(true);
        backGroundMat.DOFloat(0, "_SpriteFade", .0001f).SetUpdate(true);
        //Debug.Log("OnDisable的_GhostFX_ClipRight_1  " + backGroundMat.GetFloat("_GhostFX_ClipRight_1"));
        //Debug.Log("OnDisable的_SpriteFade " + backGroundMat.GetFloat("_SpriteFade"));
    }
}
