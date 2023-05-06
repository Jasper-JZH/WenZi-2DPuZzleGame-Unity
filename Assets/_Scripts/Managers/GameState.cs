using System;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

public partial class GameManager : MonoBehaviour
{
    #region ��Ϸ״̬
    public enum GameState
    {
        GameInit,   //��ʼ��
        GameView,   //�ۿ�״̬����Ϸ�����е�����޷�����
        GameRun,    //����״̬����Ϸ����������ҿɲ���
        GamePause,  //��ͣ
        GameQuit    //�˳�
    }

    /// <summary>
    /// m_gameState�Ķ���ӿ�
    /// </summary>
    public GameState MyGameState
    {
        get { return m_gameState; }
        set
        {
            m_gameState = value;
            switch(m_gameState) //�л�״̬
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
    /// ��Ϸ״̬ת��
    /// </summary>
    /// <param name="_nextGameState"></param>
    private void ToGameState(GameState _nextGameState)
    {
        if (_nextGameState == MyGameState) return;
        MyGameState = _nextGameState;
    }

    #endregion

    /// <summary>
    /// ��Ϸ��ʼ��
    /// </summary>
    private void Init()
    {
        Debug.Log("Init!");
        //TODO:�浵��ȡ
        ReadArchive();
        if(Dictionary.Instance) Dictionary.Instance.InitPlayerDic(GameManager.GetCurLevel());
        //�ؿ���ʼ��
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
    /// �л������ۿ���״̬������޷�����
    /// </summary>
    private void View()
    {
        Debug.Log("View!");
        //�ر���ҿ���Ȩ��
    }

    /// <summary>
    /// �л�������״̬
    /// </summary>
    private void Run()
    {
        Debug.Log("Run!");
        Time.timeScale = 1.0f;
        uIManager.Continue();   //�ر���ͣ���
        ShowCursor(false);
    }

    /// <summary>
    /// �л�����ͣ״̬
    /// </summary>
    private void Pause()
    {
        Debug.Log("Pause!");
        Time.timeScale = 0.0f;
        uIManager.Pause();      //��ʾ��ͣ���
        ShowCursor(true);
    }

    private void Quit()
    {
        SaveArchive();  //�浵
#if UNITY_EDITOR
        Debug.Log("Quit!");
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
