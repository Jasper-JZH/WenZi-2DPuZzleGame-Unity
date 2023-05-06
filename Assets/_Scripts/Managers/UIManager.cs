using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class UIManager : MonoBehaviour
{
    #region 单例实现
    public static UIManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }
    #endregion


    public GameManager gameManager;
    /// <summary>
    /// 菜单面板
    /// </summary>
    [SerializeField] private MenuPanel menuPanel;
    /// <summary>
    /// 设置面板
    /// </summary>
    [SerializeField] private SetPanel setPanel;
    /// <summary>
    /// 暂停面板
    /// </summary>
    [SerializeField] private PausePanel pausePanel;
    /// <summary>
    /// 开发者面板
    /// </summary>
    [SerializeField] private DeveloperPanel developerPanel;
    /// <summary>
    /// 对话面板 TODO:
    /// </summary>
    [SerializeField] private DialogePanel dialogePanel;

    private DrawController drawController;

    /// <summary>
    /// 面板类型
    /// </summary>
    public enum Panels
    {
        Menu,
        Set,
        Developer,
        Pause,
        Dialoge
    };

    /// <summary>
    /// 是否在关卡中
    /// </summary>
    private  bool inLevel;
    public bool Inlevel
    {
        get { return inLevel; }
        set
        {
            inLevel = value;
            if(inLevel == true)
            {
                StartCoroutine(FindDrawController());
            }
        }
    }
    /// <summary>
    /// 动态绑定当前场景的DrawController
    /// </summary>
    /// <returns></returns>
    private IEnumerator FindDrawController()
    {
        yield return new WaitForSeconds(.5f);
        drawController = GameObject.Find("Cameras").GetComponent<DrawController>();
        if (drawController == null) Debug.LogWarning("未找到DrawController");
    }

    /// <summary>
    /// 将要对话的npc的名字
    /// </summary>
    private string npcName;

    private void Start()
    {
        gameManager = GameManager.Instance;
        ShowPanel(Panels.Menu);
        inLevel = false;
    }

    public void SelectLevel(int level)
    {
        gameManager.EnterLevel(level);
    }

    public void BackToMenu()
    {
        ShowPanel(Panels.Menu);
    }


    /// <summary>
    /// 暂停时UI相关
    /// </summary>
    public void Pause()
    {
        ShowPanel(Panels.Pause);
    }

    /// <summary>
    /// 继续时UI相关
    /// </summary>
    public void Continue()
    {
        ClosePanel(Panels.Pause);
    }

    /// <summary>
    /// 打开目标面板
    /// </summary>
    /// <param name="_targetPanel"></param>
    public void ShowPanel(Panels _targetPanel)
    {
        switch (_targetPanel)
        {
            case Panels.Menu:
                {
                    menuPanel.OnPanelShow();
                    setPanel.OnPanelClose();
                    developerPanel.OnPanelClose();
                }
                break;
            case Panels.Set:
                {
                    setPanel.OnPanelShow();
                    menuPanel.OnPanelClose();
                }
                break;
            case Panels.Developer:
                {
                    developerPanel.OnPanelShow();
                    menuPanel.OnPanelClose();

                }
                break;
            case Panels.Pause:
                {
                    if (!inLevel) break;
                    pausePanel.OnPanelShow();
                }
                break;
            case Panels.Dialoge:
                {
                    if (!inLevel) break;
                    dialogePanel.NewDialoge(npcName);
                }
                break;
        }
    }

    /// <summary>
    /// 关闭目标面板
    /// </summary>
    /// <param name="_targetPanel"></param>
    public void ClosePanel(Panels _targetPanel)
    {
        switch (_targetPanel)
        {
            case Panels.Menu:
                menuPanel.OnPanelClose();
                break;
            case Panels.Set:
                {
                    setPanel.OnPanelClose();
                    menuPanel.OnPanelShow();
                }
                break;
            case Panels.Developer:
                {
                    developerPanel.OnPanelClose();
                    menuPanel.OnPanelShow();
                }
                break;
            case Panels.Pause:
                {
                    if (!inLevel) break;
                    pausePanel.OnPanelClose();
                }
                break;
            case Panels.Dialoge:
                {
                   /* if (!inLevel) break;
                    dialogePanel.OnPanelClose();*/
                }
                break;
        }
    }

    public void NewNPCDialoge(string _name)
    {
        npcName = _name;
        ShowPanel(Panels.Dialoge);
        //同时静止玩家移动
        drawController.playerMovement.LockControl(true);
    }

    public void EndDialoge(string _name)
    {
        if(_name == "火2" || _name == "师")
        {
            //恢复控制权
            drawController.playerMovement.LockControl(false);
        }
        else
        {
            drawController.DrawOnce(npcName);
            //恢复控制权
            drawController.playerMovement.LockControl(false);
        }  
    }


}
