using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 管理和控制字对应实物的生成
/// </summary>
public class ObjectsController : MonoBehaviour
{
    #region 单例实现
    public static ObjectsController Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else Destroy(gameObject);
        dictionary = Dictionary.Instance;
        InitPrefabsDic();
        DontDestroyOnLoad(gameObject);
    }
    #endregion

    private void Start()
    {
        //清空场景中已有的物体（重置objectsList）
        if (objectsList.Count > 0)
        {
            foreach(GameObject item in objectsList)
            {
                Destroy(item.gameObject);
            }
        }
        objectsList.Clear();
    }
    private Dictionary dictionary;

    private GameManager gameManager;
    /// <summary>
    /// 实物字典；可根据字的名字查找对应实物的预制体
    /// </summary>
    private Dictionary<string, GameObject> objectsPrefabsDic = new Dictionary<string, GameObject>();
    /// <summary>
    /// 存放场景中已经实例化的字（物体）
    /// </summary>
    public static List<GameObject> objectsList = new List<GameObject>();
    /// <summary>
    /// 生成的所有实物的父物体
    /// </summary>
    private Transform mObjectParent;

    /// <summary>
    /// 写的是否为辰
    /// </summary>
    public static bool isLight = false;

    [SerializeField] private Transform objectParents
    {
        get
        {
            if (!mObjectParent)
                mObjectParent = GameObject.Find("ObjectsParent").GetComponent<Transform>();
            return mObjectParent;
        }
        set { mObjectParent = value; }
    }
    /// <summary>
    /// 玩家的引用
    /// </summary>
    /// 
    private Transform mPlauyer;
    [SerializeField] private Transform player
    {
        get
        {
            if (!mPlauyer)
                mPlauyer = GameObject.FindWithTag("Player").GetComponent<Transform>();
            return mPlauyer;
        }
        set { mPlauyer = value; }
    }
    /// <summary>
    /// 判定是否可生成的分数
    /// </summary>
    private static float Score = 0.2f;

    /// <summary>
    /// 加载资源，初始化实物字典
    /// </summary>
    private void InitPrefabsDic()
    {
        List<string> names;
        dictionary.GetAllCharacterName(out names);
        string parentPath = "ObjectsPrefabs/";   //父路径
        foreach (string name in names)
        {
            string path = parentPath + name;
            objectsPrefabsDic.Add(name, Resources.Load(path) as GameObject);
            Debug.Log($"成功初始化{name}的预制体");
        }
        //特殊
        string[] specialNames = new string[] { "水3", "木3", "木5", "火3","火6","土6","木6","水6"};
        foreach(var name in specialNames)
        {
            objectsPrefabsDic.Add(name, Resources.Load(parentPath + name) as GameObject);
            Debug.Log($"成功初始化{name}的预制体");
        }
    }

    /// <summary>
    /// 获取指定物体的预制体
    /// </summary>
    /// <param name="_name"></param>
    /// <returns></returns>
    private GameObject GetPrefab(string _name)
    {
        GameObject prefab;
        if (objectsPrefabsDic.TryGetValue(_name, out prefab))
        {
            return prefab;
        }
        else
        {
            return null;
        } 
    }

    /// <summary>
    /// 实例化一个物体
    /// </summary>
    /// <param name="_playerPosition"></param>
    /// <param name="_prefabs"></param>
    public void InstantiateOneObject(GameObject _prefabs,Vector2 _offset,Quaternion _rotation)
    {
        if(_prefabs==null)
        {
            Debug.LogWarning("尝试实例化时_prefab为空");
            return;
        }
        TryDestroyOneObject(_prefabs);   //若物体已存在则销毁旧的
        Vector2 pos = new Vector2(player.position.x, player.position.y) + _offset;
       // AudioManager.PlayerAudio(AudioName.ObjectInstantiate, false);    //音效
        objectsList.Add(Instantiate(_prefabs, pos, _rotation, objectParents));

    }

    /// <summary>
    /// 判定是否通过识别
    /// </summary>
    /// <param name=""></param>
    /// <returns></returns>
    public bool Judge(float _score)
    {
        if(_score < 0)  //说明分数有问题
        {
            Debug.LogWarning("无效的分数，请确保已选择字帖再尝试识别！");
            return false;
        }
        //分数低于预设的分数时，通过识别
        return _score <= Score ? true : false;
    }
    /// <summary>
    /// 尝试生成实物
    /// </summary>
    public bool TryInstantiateOneObject(string _name, in float _score)
    {
        bool isSmall =false;
        GameObject prefab;
        Quaternion rotation = Quaternion.identity;
        Vector2 offset = new Vector2(0f, -2f);
        //选择特殊的预制体
        switch (GameManager.GetCurLevel())
        {
            case 1:
            case 2:
            case 3:
            case 4:
                if(_name=="木")
                {
                    _name += "3";
                }
                break;
            case 5:
            case 6:
                break;
            case 7:
                if (_name == "木" && PlayerMovement.onEarth)
                {
                    _name += "5";
                    offset = new Vector2(0, -4.5f);
                }
                break;
            case 8:
                break;
            case 9:
                {
                    if(_name=="金")
                    {
                        offset = new Vector2(3f, 5f);
                    }
                    else if (_name == "坎")
                    {
                        offset = new Vector2(0f, -4f);
                    }
                    else if (_name == "火")
                    {
                        _name += "6";
                    }
                    else if (_name == "土")
                    {
                        _name += "6";
                    }
                    else if (_name == "木")
                    {
                        _name += "6";
                    }
                    else if (_name == "水")
                    {
                        _name += "6";
                    }
                }
                break;
            case 10:
                {
                    //if(isLight)isLight = false;
                    if(_name=="辰")
                    {
                        isLight = true;
                    }
                    else if(_name =="坤")
                    {
                        offset = new Vector2(0f, -3f);
                    }
                    else if(_name=="震")
                    {
                        trick_lightning.hasWrite = true;
                    }
                    else if(_name=="巽")
                    {
                        trick_wind.hasWrite = true;
                    }
                }
                break;
            case 11:
                {
                    if (_name == "巽") trick_wind2.hasWrite = true;
                    else if (_name == "飞") Effect_Fly.hasWrite = true;
                }
                break;
            case 12:
                if (_name == "云")
                {
                    offset = new Vector2(0f, -2f);

                }
                else if(_name == "雨")
                {
                    trick_rain.hasWrite = true;
                }
                break;


        }
        prefab = GetPrefab(_name);
        if (Judge(_score))
        {
            if (isSmall)
            {
                InstantiateOneObject(prefab, offset, rotation);
            }
            else
            {
                InstantiateOneObject(prefab, offset, rotation);
            }
            return true;
        }
        else
        {
            //生成失败相关提示（动效，音效）
            Debug.Log("未通过识别，请重试！");
            return false;
        }
    }

    public static void ClearAllObjects()
    {
        for(int i = objectsList.Count - 1; i >=  0; i--)
        {
            TryDestroyOneObject(objectsList[i]);
        }
        //objectsList.Clear();
    }

    public static bool TryDestroyOneObject(GameObject _object)
    {
        string tag = _object.tag;
        GameObject clearTarget = null;
        foreach (var item in objectsList)
        {
            if (item!=null && item.CompareTag(tag))
            {
                if (item.CompareTag("辰"))
                {
                    isLight = false;
                }
                clearTarget = item;
                objectsList.Remove(item);
                break;
            }
        }
        if (clearTarget)
        {
            Destroy(clearTarget);
            return true;
        }
        else return false;
    }
}
