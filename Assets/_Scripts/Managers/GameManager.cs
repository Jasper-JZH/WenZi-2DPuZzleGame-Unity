using UnityEngine;
using System.IO;
using LitJson;


/// <summary>
/// ������Ϸ���Ȩ�޹����ߣ�������Ϸ��״̬�л��������л�����Ϸ�׶�
/// </summary>
public partial class GameManager : MonoBehaviour
{
    #region ����ʵ��
    public static GameManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else Destroy(gameObject);
        MyGameState = GameState.GameInit;   //������Ϸ״̬Ϊ��ʼ��
        DontDestroyOnLoad(gameObject);
    }
    #endregion

    #region ��Ϸ�׶�
    private enum GamePhase
    {
        Begin,      // ���ڿ�ʼ����
        Level,      // ����ĳһ�ؿ���
        //PassLevel,  // ͨ��ĳһ�ؿ�
        End         // ��Ϸ����
    }

    /// <summary>
    /// m_gamePhase�Ķ���ӿ�
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
                    sceneController.ToMenu();   //��ת����ʼ����
                    break;
                case GamePhase.Level:       
                    sceneController.ToLevel(curLevel);  //��ת��ָ���ؿ�����
                    break;
                case GamePhase.End:         
                    sceneController.ToEnd();    //��ת����������
                    break;
            }
        }
    }
    #endregion


    #region private��Ա
    /// <summary>
    /// ��Ϸ״̬
    /// </summary>
    private static GameState m_gameState;
    /// <summary>
    /// ��Ϸ�׶�
    /// </summary>
    private static GamePhase m_gamePhase;
    /// <summary>
    /// ����������
    /// </summary>
    private SceneController sceneController;
    /// <summary>
    /// �ؿ���
    /// </summary>
    public static int levelNum = 13;
    /// <summary>
    /// ��ǰ�ؿ�
    /// </summary>
    private static int curLevel;
    /// <summary>
    /// ��Ϸ�浵�ļ�·��
    /// </summary>
    private static string path = Application.streamingAssetsPath + "/save.text";
    /// <summary>
    /// ��Ϸ�浵
    /// </summary>
    private Save gameSave;

    private UIManager uIManager;

    #endregion

    #region public��Ա
    /// <summary>
    /// ��ǹؿ�����״̬ true:�ѽ��� false:δ����
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

    #region private����

    private void Start()
    {
        sceneController = SceneController.Instance;
        uIManager = UIManager.Instance;
        
        //ToGamePhase(GamePhase.Begin);      
        //ToGameState(GameState.GameRun);   
    }
    private void Update()
    {
        //���ݵ�ǰ��Ϸ״̬���в�ͬ��ʵʱ��Ӧ
        if(MyGameState == GameState.GameRun)    //��������״̬ʱ
        {
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                ToGameState(GameState.GamePause);    //״̬ת��
            }
        }
        else if (MyGameState == GameState.GamePause)    //��ͣ״̬ʱ
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                ToGameState(GameState.GameRun);    //״̬ת��
            }
        }
        else if (MyGameState == GameState.GameView)
        {
            //�ر���ҿ���Ȩ��
            //... 
        }
    }
    /// <summary>
    /// ��Ϸ�׶���ת
    /// </summary>
    /// <param name="_nextPhase"></param>
    private void ToGamePhase(GamePhase _nextPhase)
    {
        MyGamePhase = _nextPhase;
    }

    /// <summary>
    /// ��ȡ��Ϸ�浵
    /// </summary>
    private void ReadArchive()
    {
        //TODO����ȡJson�ļ�
        if(File.Exists(path))
        {
            StreamReader sr = new StreamReader(path);
            string jsonString = sr.ReadToEnd();
            sr.Close();
            gameSave = new Save();
            gameSave = JsonMapper.ToObject<Save>(jsonString);
            Debug.Log("��ȡ�浵�ļ��ɹ���");
        }
        else
        {
            Debug.LogWarning($"Ŀ��·���ļ�{path}������");
            gameSave = new Save(1);
        }
    }
    /// <summary>
    /// �浵
    /// </summary>
    private void SaveArchive()
    {
        Save save = new Save(curLevel);
        string jsonString = JsonMapper.ToJson(save);
        //string path = Application.streamingAssetsPath + "/save.text";
        StreamWriter sw = new StreamWriter(path);
        sw.Write(jsonString);
        sw.Close();
        Debug.Log($"��Ϸ�浵�Ѹ��£�");
    }
    #endregion

    #region public����
    /// <summary>
    /// ����ؿ�
    /// </summary>
    /// <param name="level"></param>
    public void EnterLevel(int _level)
    {
        if (_level < 1 || _level > levelsStateArray.Length)     // �Ƿ����
            return;
        curLevel = _level;

        if (levelsStateArray[curLevel - 1])       // �ж�Ŀ��ؿ��Ƿ��ѽ���
        {
            ToGamePhase(GamePhase.Level);       // ������Ϸ�׶�
            ToGameState(GameState.GameRun);   
        }
    }
    /// <summary>
    /// ���ͨ���ؿ�ʱ����
    /// </summary>
    public void PassLevel()
    {
        //����ͨ���ؿ�ʱ�������ʾ�� ��Ч����Ч...��
        //...

        //�޸ĵ�ǰ�ؿ���UIͼ���
        //...

        //������һ�ؿ���ͨ��
        if (curLevel >= levelNum)    //ͨ��
        {
            curLevel = 1;   
            SaveArchive();  //�浵
            BackToMenu();
        }
        else  //������һ�ؿ�
        {
            levelsStateArray[curLevel] = true;  //����������ʵ��levelС1
            curLevel = Mathf.Clamp(++curLevel, 0, levelNum);
            //���������ã�����
            /*            if (curLevel > 8)
                        {

                        }*/

            //ʵ��ʹ��ֻ��������������
            SaveArchive();  //�浵
            ToGamePhase(GamePhase.Level);
            ToGameState(GameState.GameRun);
        }
        //ʵ��ʹ��Ҫ�õ���������
        //ToGameState(GameState.GameRun);
    }

    /// <summary>
    /// �ص��˵�
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
    /// ����������ʾ
    /// </summary>
    public static void ShowCursor(bool _show)
    {
        Cursor.visible = _show;
    }
    #endregion
}
