using System;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

public partial class GameManager : MonoBehaviour
{
    #region 游戏状态
    public enum GameState
    {
        GameInit,   //初始化
        GameView,   //观看状态：游戏运行中但玩家无法操作
        GameRun,    //游玩状态：游戏运行中且玩家可操作
        GamePause,  //暂停
        GameQuit    //退出
    }

    /// <summary>
    /// m_gameState的对外接口
    /// </summary>
    public GameState MyGameState
    {
        get { return m_gameState; }
        set
        {
            m_gameState = value;
            switch(m_gameState) //切换状态
            {
                case GameState.GameInit:
                    Init();
                    break;
                case GameState.GameView:
                    View();
                    break;
                case GameState.GameRun:
                    Run();
                    break;
                case GameState.GamePause:
                    Pause();
                    break;
                case GameState.GameQuit:
                    Quit();
                    break;
            }
        }
    }
    /// <summary>
    /// 游戏状态转换
    /// </summary>
    /// <param name="_nextGameState"></param>
    private void ToGameState(GameState _nextGameState)
    {
        if (_nextGameState == MyGameState) return;
        MyGameState = _nextGameState;
    }

    #endregion

    /// <summary>
    /// 游戏初始化
    /// </summary>
    private void Init()
    {
        Debug.Log("Init!");
        //TODO:存档读取
        ReadArchive();
        if(Dictionary.Instance) Dictionary.Instance.InitPlayerDic(GameManager.GetCurLevel());
        //关卡初始化
        levelsStateArray = new bool[levelNum];
        Array.Clear(levelsStateArray, 0, levelNum);
        for(int i = 0; i < levelsStateArray.Length; i++)
        {
            levelsStateArray[i] = i < gameSave.level;
        }
        curLevel = gameSave.level;

        ShowCursor(true);
    }

    /// <summary>
    /// 切换到（观看）状态，玩家无法操作
    /// </summary>
    private void View()
    {
        Debug.Log("View!");
        //关闭玩家控制权限
    }

    /// <summary>
    /// 切换到运行状态
    /// </summary>
    private void Run()
    {
        Debug.Log("Run!");
        Time.timeScale = 1.0f;
        uIManager.Continue();   //关闭暂停面板
        ShowCursor(false);
    }

    /// <summary>
    /// 切换到暂停状态
    /// </summary>
    private void Pause()
    {
        Debug.Log("Pause!");
        Time.timeScale = 0.0f;
        uIManager.Pause();      //显示暂停面板
        ShowCursor(true);
    }

    private void Quit()
    {
        SaveArchive();  //存档
#if UNITY_EDITOR
        Debug.Log("Quit!");
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
