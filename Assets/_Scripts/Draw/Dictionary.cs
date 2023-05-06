using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Dictionary : MonoBehaviour
{
    #region 单例实现
    public static Dictionary Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else Destroy(gameObject);
        if(gestureDic.Count==0) InitGestureDic();   // 初始化系统笔画字典
        DontDestroyOnLoad(gameObject);
    }
    #endregion
    /// <summary>
    /// 玩家字典（类似背包）；通过名字来查看/修改玩家对于不同字的状态（unGet,get,disable）
    /// </summary>
    private Dictionary<string, Character> playerDic = new Dictionary<string, Character>();
    /// <summary>
    /// 系统字典(含所有字)；根据字查找对应的字的笔迹
    /// </summary>
    private static Dictionary<string, Gesture> gestureDic = new Dictionary<string, Gesture>();
    /// <summary>
    /// 添加新字到字典事件
    /// </summary>
    public static Action<string> AddPlayerDicAction;
    private void Start()
    {
        InitPlayerDic(GameManager.GetCurLevel());
    }

    /// <summary>
    /// 初始化笔迹字典
    /// </summary>
    private static void InitGestureDic()
    {
        //从文件中读取所有笔迹，生成对应的gesture并添加到字典中
        List<Gesture> gestureList = GestureIO.ReadAllTempGesture();
        foreach(Gesture gesture in gestureList)
        {
            gestureDic.Add(gesture.gestureName, gesture);   //初始化gestureDic     zhen
            //playerDic.Add(gesture.gestureName, new Character(gesture.gestureName));  //测试
        }
        Debug.Log("系统笔迹字典初始化完毕！");
    }

    /// <summary>
    /// 初始化玩家字典
    /// </summary>
    /// hhhhhhhh
    public void InitPlayerDic(int _level)
    {
        playerDic.Clear();
        //TODO：（根据存档）
        switch (_level)
        {
            case 1: //1
                break;
            case 2: //2
            case 3: //3
                playerDic.Add("火", new Character("火"));
                break;
            case 4: //3.1
                playerDic.Add("火", new Character("火"));
                playerDic.Add("木", new Character("木"));
                break;
            case 5: //3.2
            case 6: //4
            case 7: //5
                playerDic.Add("火", new Character("火"));
                playerDic.Add("木", new Character("木"));
                playerDic.Add("水", new Character("水"));
                break;
            case 8: //6
                playerDic.Add("火", new Character("火"));
                playerDic.Add("木", new Character("木"));
                playerDic.Add("水", new Character("水"));
                playerDic.Add("土", new Character("土"));
                break;
            case 9: //7
                playerDic.Add("火", new Character("火"));
                playerDic.Add("木", new Character("木"));
                playerDic.Add("水", new Character("水"));
                playerDic.Add("土", new Character("土"));
                playerDic.Add("烟", new Character("烟"));
                break;
            case 10: //8
                playerDic.Add("火", new Character("火"));
                playerDic.Add("木", new Character("木"));
                playerDic.Add("水", new Character("水"));
                playerDic.Add("土", new Character("土"));
                playerDic.Add("烟", new Character("烟"));
                playerDic.Add("金", new Character("金"));
                playerDic.Add("坎", new Character("坎"));
                break;
            case 11: //9
                playerDic.Add("巽", new Character("巽"));
                break;
            case 12: //10
            case 13: //11
                //不提供字
                break;
        }

        Debug.Log("玩家字典初始化（读取）完毕！");
    }

    public List<string> GetCurDicList()
    {
        List<string> curDicList = new List<string>();
        foreach(var item in playerDic)
        {
            curDicList.Add(item.Key);
        }
        return curDicList;
    }


    /// <summary>
    /// 对于某些特定的关卡，限定玩家的字典为某些指定字
    /// </summary>
    /// <param name="_characterList"></param>
    public void SetPlayerDic(List<string> _characterList)
    {
        playerDic.Clear();//先清空之前的字典
        foreach(var item in _characterList)
        {
            playerDic.Add(item, new Character(item));
        }
    }

    /// <summary>
    /// 查某个字的字结构体
    /// </summary>
    /// <param name="_name"></param>
    /// <returns></returns>
    public Character SearchCharacter(string _name)
    {
        Character character = new Character();
        return playerDic.TryGetValue(_name, out character) ? character : null;
    }

    /// <summary>
    /// 查某个字的笔迹
    /// </summary>
    /// <param name="_name"></param>
    /// <returns></returns>
    public static Gesture SearchGesture(string _name)
    {
        Gesture gesture = new Gesture();
        return gestureDic.TryGetValue(_name, out gesture) ? gesture : null;
    }
    
    /// <summary>
    /// 遍历字典获得所有字名
    /// </summary>
    /// <returns></returns>
    public void GetAllCharacterName(out List<string> _names)
    {
        _names = new List<string>();
        foreach (var item in gestureDic)
        {
            _names.Add(item.Key);
        }
    }
    public void GetPlayerDicCharacterName(out List<string> _names)
    {
        _names = new List<string>();
        foreach (var item in playerDic)
        {
            _names.Add(item.Key);
        }
    }


    /// <summary>
    /// 添加新的字（贴）到玩家的字典中
    /// </summary>
    /// <param name="_name"></param>
    public void AddPlayerDic(string _name)
    {
        playerDic.Add(_name, SearchCharacter(_name));
        //触发相关的订阅事件
        if (AddPlayerDicAction != null)
        {
            AddPlayerDicAction(_name);
        }
    }
}
