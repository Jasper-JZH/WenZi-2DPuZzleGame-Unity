using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    #region ����ʵ��
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
    /// ����ID����Unity��BuildingSet�е���Ŷ�Ӧ��
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
    /// ��Źؿ������ĳ���ID<�ؿ���ţ��ؿ�����ID>
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

    #region private ��Ա

    private GameManager gameManager;


    #endregion

    #region public ��Ա

    /// <summary>
    /// ��һ��������ID
    /// </summary>
    public int nextSceneIndex;


    #endregion

    #region private ����
    private void Start()
    {
        gameManager = GameManager.Instance;
        /*gameManager.curSceneIndex = 0;*/
    }
    /// <summary>
    /// ���г����л�������ø÷���
    /// </summary>
    /// <param name="_targetSceneIndex">Unity�������ı��</param>
    private void StartLoadingScene(int _targetSceneIndex)
    {
        nextSceneIndex = _targetSceneIndex; //��¼Ŀ�곡������
        // ��ֱ��ͬ�����ص����ɳ����У��ڴӹ��ɳ������첽���ص�Ŀ�곡��
        if (nextSceneIndex == 3) //���ص�һ��
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

    #region public ����

    /// <summary>
    /// ����ָ���ؿ�����
    /// </summary>
    /// <param name="levelID">�ؿ����</param>
    public void ToLevel(int _levelID)
    {
        StartLoadingScene((int)levelScenesDic[_levelID]);
    }

    /// <summary>
    /// �л����˵�����ʼ������
    /// </summary>
    public void ToMenu()
    {
        StartLoadingScene((int)SceneID.Menu);
    }

    /// <summary>
    /// �л�����������
    /// </summary>
    public void ToEnd()
    {
        StartLoadingScene((int)SceneID.End);
    }

    public IEnumerator LoadSceneCallBack(int _nextSceneIndex)
    {
        gameManager.curSceneIndex = _nextSceneIndex;
        Debug.Log($"��ǰ��������Ϊ{_nextSceneIndex}");
        yield return null;
    }
    #endregion

}
