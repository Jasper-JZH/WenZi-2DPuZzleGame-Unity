using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    #region 单例实现
    public static SceneController Instance { get; private set; }
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

    /// <summary>
    /// 场景ID（与Unity的BuildingSet中的序号对应）
    /// </summary>
    public enum SceneID
    {
        CG,
        Menu,
        LoadingScene,
        Level_1,
        Level_2,
        Level_3,
        Level_4,
        Level_5,
        Level_6,
        Level_7,
        Level_8,
        Level_9,
        Level_10,
        Level_11,
        Level_12,
        Level_13,
        LoadingScene1,
        LoadingScene2,


        //
        End
    }

    /// <summary>
    /// 存放关卡场景的场景ID<关卡序号，关卡场景ID>
    /// </summary>
    private Dictionary<int, SceneID> levelScenesDic = new Dictionary<int, SceneID>
    {
        {1,SceneID.Level_1 },
        {2,SceneID.Level_2 },
        {3,SceneID.Level_3 },
        {4,SceneID.Level_4 },
        {5,SceneID.Level_5 },
        {6,SceneID.Level_6 },
        {7,SceneID.Level_7 },
        {8,SceneID.Level_8 },
        {9,SceneID.Level_9 },
        {10,SceneID.Level_10 },
        {11,SceneID.Level_11 },
        {12,SceneID.Level_12 },
        {13,SceneID.Level_13 }
        // {4,SceneID.Level_4 },...
    };

    #region private 成员

    private GameManager gameManager;


    #endregion

    #region public 成员

    /// <summary>
    /// 下一个场景的ID
    /// </summary>
    public int nextSceneIndex;


    #endregion

    #region private 方法
    private void Start()
    {
        gameManager = GameManager.Instance;
        /*gameManager.curSceneIndex = 0;*/
    }
    /// <summary>
    /// 所有场景切换均需调用该方法
    /// </summary>
    /// <param name="_targetSceneIndex">Unity给场景的编号</param>
    private void StartLoadingScene(int _targetSceneIndex)
    {
        nextSceneIndex = _targetSceneIndex; //记录目标场景索引
        // 先直接同步加载到过渡场景中，在从过渡场景中异步加载到目标场景
        if (nextSceneIndex == 3) //加载第一关
        {
            SceneManager.LoadScene((int)SceneID.LoadingScene2);
        }
        else if (nextSceneIndex == 11)
        {
            SceneManager.LoadScene((int)SceneID.LoadingScene1);
        }
        else
        {
            SceneManager.LoadScene((int)SceneID.LoadingScene);
        }
       
    }

    #endregion

    #region public 方法

    /// <summary>
    /// 进入指定关卡场景
    /// </summary>
    /// <param name="levelID">关卡序号</param>
    public void ToLevel(int _levelID)
    {
        StartLoadingScene((int)levelScenesDic[_levelID]);
    }

    /// <summary>
    /// 切换到菜单（开始）场景
    /// </summary>
    public void ToMenu()
    {
        StartLoadingScene((int)SceneID.Menu);
    }

    /// <summary>
    /// 切换到结束场景
    /// </summary>
    public void ToEnd()
    {
        StartLoadingScene((int)SceneID.End);
    }

    public IEnumerator LoadSceneCallBack(int _nextSceneIndex)
    {
        gameManager.curSceneIndex = _nextSceneIndex;
        Debug.Log($"当前场景索引为{_nextSceneIndex}");
        yield return null;
    }
    #endregion

}
