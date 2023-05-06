using UnityEngine;
using System.IO;
using LitJson;


/// <summary>
/// 整个游戏最高权限管理者，管理游戏的状态切换，场景切换，游戏阶段
/// </summary>
public partial class GameManager : MonoBehaviour
{
    #region 单例实现
    public static GameManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else Destroy(gameObject);
        MyGameState = GameState.GameInit;   //设置游戏状态为初始化
        DontDestroyOnLoad(gameObject);
    }
    #endregion

    #region 游戏阶段
    private enum GamePhase
    {
        Begin,      // 处于开始场景
        Level,      // 处于某一关卡中
        //PassLevel,  // 通过某一关卡
        End         // 游戏结束
    }

    /// <summary>
    /// m_gamePhase的对外接口
    /// </summary>
    private GamePhase MyGamePhase
    {
        get { return m_gamePhase; }
        set
        {
            m_gamePhase = value;
            switch(m_gamePhase)
            {
                case GamePhase.Begin:
                    sceneController.ToMenu();   //跳转到开始场景
                    break;
                case GamePhase.Level:       
                    sceneController.ToLevel(curLevel);  //跳转到指定关卡场景
                    break;
                case GamePhase.End:         
                    sceneController.ToEnd();    //跳转到结束场景
                    break;
            }
        }
    }
    #endregion


    #region private成员
    /// <summary>
    /// 游戏状态
    /// </summary>
    private static GameState m_gameState;
    /// <summary>
    /// 游戏阶段
    /// </summary>
    private static GamePhase m_gamePhase;
    /// <summary>
    /// 场景控制器
    /// </summary>
    private SceneController sceneController;
    /// <summary>
    /// 关卡数
    /// </summary>
    public static int levelNum = 13;
    /// <summary>
    /// 当前关卡
    /// </summary>
    private static int curLevel;
    /// <summary>
    /// 游戏存档文件路径
    /// </summary>
    private static string path = Application.streamingAssetsPath + "/save.text";
    /// <summary>
    /// 游戏存档
    /// </summary>
    private Save gameSave;

    private UIManager uIManager;

    #endregion

    #region public成员
    /// <summary>
    /// 标记关卡解锁状态 true:已解锁 false:未解锁
    /// </summary>
    public bool[] levelsStateArray;

    public int curSceneIndex
    {
        get
        {
            return m_curSceneIndex;
        }
        set
        {
            m_curSceneIndex = value;
            switch(m_curSceneIndex)
            {
                case 1: //Menu
                    {
                        uIManager.BackToMenu();
/*                        Debug.Log("case 0: //Menu");*/
                    }
                    break;
                default: // Level
                    {
                        uIManager.Inlevel = true;
                    }
                    break;
            }
        }
    }

    private int m_curSceneIndex = 0;

    #endregion

    #region private方法

    private void Start()
    {
        sceneController = SceneController.Instance;
        uIManager = UIManager.Instance;
        
        //ToGamePhase(GamePhase.Begin);      
        //ToGameState(GameState.GameRun);   
    }
    private void Update()
    {
        //根据当前游戏状态，有不同的实时响应
        if(MyGameState == GameState.GameRun)    //正常游玩状态时
        {
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                ToGameState(GameState.GamePause);    //状态转换
            }
        }
        else if (MyGameState == GameState.GamePause)    //暂停状态时
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                ToGameState(GameState.GameRun);    //状态转换
            }
        }
        else if (MyGameState == GameState.GameView)
        {
            //关闭玩家控制权限
            //... 
        }
    }
    /// <summary>
    /// 游戏阶段跳转
    /// </summary>
    /// <param name="_nextPhase"></param>
    private void ToGamePhase(GamePhase _nextPhase)
    {
        MyGamePhase = _nextPhase;
    }

    /// <summary>
    /// 读取游戏存档
    /// </summary>
    private void ReadArchive()
    {
        //TODO：读取Json文件
        if(File.Exists(path))
        {
            StreamReader sr = new StreamReader(path);
            string jsonString = sr.ReadToEnd();
            sr.Close();
            gameSave = new Save();
            gameSave = JsonMapper.ToObject<Save>(jsonString);
            Debug.Log("读取存档文件成功！");
        }
        else
        {
            Debug.LogWarning($"目标路径文件{path}不存在");
            gameSave = new Save(1);
        }
    }
    /// <summary>
    /// 存档
    /// </summary>
    private void SaveArchive()
    {
        Save save = new Save(curLevel);
        string jsonString = JsonMapper.ToJson(save);
        //string path = Application.streamingAssetsPath + "/save.text";
        StreamWriter sw = new StreamWriter(path);
        sw.Write(jsonString);
        sw.Close();
        Debug.Log($"游戏存档已更新！");
    }
    #endregion

    #region public方法
    /// <summary>
    /// 进入关卡
    /// </summary>
    /// <param name="level"></param>
    public void EnterLevel(int _level)
    {
        if (_level < 1 || _level > levelsStateArray.Length)     // 非法检测
            return;
        curLevel = _level;

        if (levelsStateArray[curLevel - 1])       // 判断目标关卡是否已解锁
        {
            ToGamePhase(GamePhase.Level);       // 更新游戏阶段
            ToGameState(GameState.GameRun);   
        }
    }
    /// <summary>
    /// 玩家通过关卡时调用
    /// </summary>
    public void PassLevel()
    {
        //播放通过关卡时的相关提示（ 动效、音效...）
        //...

        //修改当前关卡的UI图标等
        //...

        //解锁下一关卡或通关
        if (curLevel >= levelNum)    //通关
        {
            curLevel = 1;   
            SaveArchive();  //存档
            BackToMenu();
        }
        else  //解锁下一关卡
        {
            levelsStateArray[curLevel] = true;  //数组索引比实际level小1
            curLevel = Mathf.Clamp(++curLevel, 0, levelNum);
            //导出测试用！！！
            /*            if (curLevel > 8)
                        {

                        }*/

            //实际使用只保留下面这两行
            SaveArchive();  //存档
            ToGamePhase(GamePhase.Level);
            ToGameState(GameState.GameRun);
        }
        //实际使用要用到下面这行
        //ToGameState(GameState.GameRun);
    }

    /// <summary>
    /// 回到菜单
    /// </summary>
    public void BackToMenu()
    {
        ToGameState(GameState.GameRun);
        ToGamePhase(GamePhase.Begin);
    }

    public static int GetCurLevel()
    {
        return curLevel;
    }

    public void ContinueGame()
    {
        ToGameState(GameState.GameRun);
    }

    public void QuitGame()
    {
        ToGameState(GameState.GameQuit);
    }
    /// <summary>
    /// 控制鼠标的显示
    /// </summary>
    public static void ShowCursor(bool _show)
    {
        Cursor.visible = _show;
    }
    #endregion
}
