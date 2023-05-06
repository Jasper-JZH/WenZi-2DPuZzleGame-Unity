using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
public class DrawController : MonoBehaviour
{
    #region 单例实现
    public static DrawController Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else Destroy(gameObject);


        //其它初始化
        blackFade.SetActive(false);
        playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        isDrawing = canDraw = false;
        isCreate = true;
        DrawInit();
        //初始化相机
        camera3D.gameObject.SetActive(false);
        curCamera = Camera.main;
    }
    #endregion

    public GameObject blackFade;

    private Dictionary dictionary;

    private GameManager gameManager;
    /// <summary>
    /// 绘制的面板
    /// </summary>
    [SerializeField] private GameObject drawPlane;
    /// <summary>
    /// 角色控制引用
    /// </summary>
    public PlayerMovement playerMovement;
    /// <summary>
    /// 实物控制器的引用
    /// </summary>
    private ObjectsController objectsController;

    [SerializeField] private GestureSelect gestureSelect;

    [SerializeField] private Transform pen;

    [Header("Cameras相机相关")]
    /// <summary>
    /// 当前使用的相机
    /// </summary>
    [SerializeField] private Camera curCamera;
    /// <summary>
    /// CG画面相机
    /// </summary>
    //[SerializeField] private Camera cgCamera;
    /// <summary>
    /// 2D游戏画面相机
    /// </summary>
    [SerializeField] private Camera camera2D;
    /// <summary>
    /// 3D写字画面相机
    /// </summary>
    [SerializeField] private Camera camera3D;
    /// <summary>
    /// 只捕获笔迹的相机
    /// </summary>
    [SerializeField] private Camera strokeRenderCamera;
    /// <summary>
    /// 只捕获笔的相机
    /// </summary>
    [SerializeField] private Camera penRenderCamera;


    [Header("Draw绘制相关")]
    /// <summary>
    /// 当前绘制了的笔画数
    /// </summary>
    private int strokeCount = 0;
    /// <summary>
    /// 是否正在绘制（按下鼠标）
    /// </summary>
    private bool isDrawing;
    /// <summary>
    /// 是都能够绘制（打开了绘制面板）
    /// </summary>
    public static bool canDraw;
    /// <summary>
    /// 鼠标是否在可绘制的面板上
    /// </summary>
    [SerializeField] private bool isMouseOn;
    /// <summary>
    /// 鼠标发出射线的检测结果
    /// </summary>
    private RaycastHit mouseHit;
    /// <summary>
    /// 可绘制面板
    /// </summary>
    [SerializeField] private LayerMask drawPanelLayer;
    /// <summary>
    /// 笔模型动画机
    /// </summary>
    [SerializeField] private Animator penAnimator;
    /// <summary>
    /// 鼠标在屏幕上的坐标(自定义z轴)
    /// </summary>
    private Vector3 mousePosition;
    /// <summary>
    /// 笔刷预制体
    /// </summary>
    [SerializeField] private Transform strokePrefab;
    /// <summary>
    /// 识别成功后是否要生成实物
    /// </summary>
    private bool isCreate;
    /// <summary>
    /// 特殊情况（火）
    /// </summary>
    private int specify = 0;


    [Header("Stroke笔画相关")]
    /// <summary>
    ///  笔画生成lineRender集
    /// </summary>
    private List<LineRenderer> lineRenderList = new List<LineRenderer>();
    /// <summary>
    /// 当前笔画的LineRenderer(每连续的一笔一个LineRenderer)
    /// </summary>
    private LineRenderer curLineRenderer;
    /// <summary>
    /// curLineRenderer的顶点数
    /// </summary>
    private int vertexCount;
    /// <summary>
    /// 笔迹点的生成位置
    /// </summary>
    private Vector3 linePointPositon;
    /// <summary>
    /// 渲染游戏画面的平面
    /// </summary>
    [SerializeField] private Renderer renderPlane;
    /// <summary>
    ///  点集
    /// </summary>
    private List<Point> pointList = new List<Point>();

    [Header("Gesture笔迹相关")]
    /// <summary>
    ///  当前要识别的目标笔迹
    /// </summary>
    public Gesture curTargetGesture;


    [Header("编辑模式使用")]
    public InputField tempGestureName;
    private bool AddingTemp = false;

    private int CurLevel;

    private void Start()
    {
        //tempGestureName.gameObject.SetActive(true);

        //单例引用初始化
        objectsController = ObjectsController.Instance;
        gameManager = GameManager.Instance;
        dictionary = Dictionary.Instance;
        gestureSelect = GestureSelect.Instance;
        pen.gameObject.SetActive(true);
        CurLevel = GameManager.GetCurLevel();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C) && isCreate && CurLevel!= 13) //绘制模式开关
        {
            SwitchDrawState();
        }

        //绘制面板开启时
        if (canDraw)
        {
            //可绘制时相关实时更新
            OnDrawState();
            if (CheckInDrawArea())   //确保笔刷在画板上才可以绘制
            {
                if (Input.GetMouseButtonDown(0))    //开始新的一笔
                {
                    isDrawing = true;
                    strokeCount++;  //笔画数累加
                    StartNewStroke();
                    //生成一个新的lineRenderer
                    //写字音效
                    AudioManager.PlayerAudio(AudioName.Brush, false);

                }
                if (Input.GetMouseButton(0) && isDrawing == true) // 绘制中，记录点
                {

                    //记录笔迹中的点
                    mousePosition = Input.mousePosition;
                    linePointPositon = curCamera.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, 9f));

                    curLineRenderer.positionCount = ++vertexCount;
                    curLineRenderer.SetPosition(vertexCount - 1, linePointPositon);
                    pointList.Add(new Point((Vector2)linePointPositon, strokeCount));
                }
                if (Input.GetMouseButtonUp(0))   // 结束当前这一笔
                {
                    isDrawing = false;
                    //写字音效
                    AudioManager.StopAudio(AudioName.Brush);
                }

            }


            if (Input.GetMouseButtonDown(1)) // 右键鼠标笔迹识别
            {
                isDrawing = false;
#if UNITY_EDITOR
                AddingTemp = Input.GetKey(KeyCode.T) ? true : false;
#endif
                GenerateNewGesture();


            }
            if (Input.GetKeyDown(KeyCode.F)) // F键擦除笔迹重写
            {
                isDrawing = false;
                //重置笔迹集、点集
                ResetLineRenderList();
            }
        }
    }
    /// <summary>
    /// 初始化
    /// </summary>
    private void DrawInit()
    {
        //初始化时默认（隐藏）写字面板
        drawPlane.SetActive(false);
        pen.gameObject.SetActive(false);
    }

    /// <summary>
    /// 开关绘制模式时的更新
    /// </summary>
    private void OnDrawStateChange(bool _canDraw)
    {
        isDrawing = false;
        //冻结或解冻角色
        playerMovement.FreezePlayer(_canDraw ? true : false);
        //启用状态更新
        drawPlane.SetActive(_canDraw);

        strokeRenderCamera.gameObject.SetActive(_canDraw);
        penRenderCamera.gameObject.SetActive(_canDraw);
        //画板旋转动画
        drawPlane.transform.DOLocalRotate(new Vector3(_canDraw ? 60 : 90, 180, 0), .5f, RotateMode.Fast).SetUpdate(true);
        //桌板旋转音效
        if (_canDraw) AudioManager.PlayerAudio(AudioName.Plane, false);
        if (_canDraw)
        {
            //笔下落动画
            Vector3 localPos = pen.parent.InverseTransformPoint(GetFixedMousePosition());
            pen.localPosition = new Vector3(localPos.x, 1f, localPos.z);
            pen.DOLocalMoveY(localPos.y, .3f).SetUpdate(true);
            //渲染画面色调的渐变
            renderPlane.material.DOFloat(1, "SepiaAmount", .5f).SetUpdate(true);
            ResetLineRenderList();
        }
        else
        {
            ResetLineRenderList();
            //渲染画面色调重置
            renderPlane.material.DOFloat(0, "SepiaAmount", .5f).SetUpdate(true);
        }
    }

    /// <summary>
    /// 转换主相机（在2D和3D相机间转换）
    /// </summary>
    private void ChangeCamera(Camera _targetCamera)
    {
        if (_targetCamera == curCamera) return;
        //切换AudioListener
        curCamera.GetComponent<AudioListener>().enabled = false;
        _targetCamera.gameObject.SetActive(true);
        _targetCamera.GetComponent<AudioListener>().enabled = true;
        curCamera.gameObject.SetActive(false);
        curCamera = _targetCamera;
    }

    /// <summary>
    /// 鼠标位置更新，笔模型动画等（绘制模式下每帧调用）
    /// </summary>
    private void OnDrawState()
    {
        //笔模型跟随鼠标位置
        pen.position = Vector3.Lerp(pen.position, GetFixedMousePosition(), 0.5f);

        if (penAnimator != null && penAnimator.gameObject.activeSelf)
        {
            penAnimator.SetFloat("X", Mathf.Lerp(penAnimator.GetFloat("X"), Input.GetAxis("Mouse X") * 1, .07f));
            penAnimator.SetFloat("Y", Mathf.Lerp(penAnimator.GetFloat("Y"), Input.GetAxis("Mouse Y") * 1, .07f));
            penAnimator.SetBool("isDrawing", isDrawing);
        }
    }
    /*    /// <summary>
        /// 限制模型(物体)的位置在屏幕画面内
        /// </summary>
        private void ClampPosition(Transform _obj)
        {
            Vector3 newPostion = curCamera.WorldToViewportPoint(_obj.position);
            // 限制位置在0,1
            Mathf.Clamp01(newPostion.x);
            Mathf.Clamp01(newPostion.y);
            _obj.position = curCamera.ViewportToScreenPoint(newPostion);
        }
    */
    /// <summary>
    /// 笔画生成和记录（每新的一笔时调用）
    /// </summary>
    private void StartNewStroke()
    {
        vertexCount = 0;
        Transform tempStroke = Instantiate(strokePrefab, transform.position, transform.rotation) as Transform;
        curLineRenderer = tempStroke.GetComponent<LineRenderer>();
        lineRenderList.Add(curLineRenderer);
    }

    /// <summary>
    /// 重置笔迹集、点集
    /// </summary>
    private void ResetLineRenderList()
    {
        pointList.Clear();
        strokeCount = 0;
        foreach (LineRenderer line in lineRenderList)
        {
            line.positionCount = 0;
            Destroy(line.gameObject);
        }
        lineRenderList.Clear();
    }

    /// <summary>
    /// 获取修正过z坐标的鼠标世界坐标
    /// </summary>
    /// <returns></returns>
    private Vector3 GetFixedMousePosition()
    {
        Vector3 fixedMousePosition = Input.mousePosition;
        fixedMousePosition.z = 0.58f;
        return curCamera.ScreenToWorldPoint(fixedMousePosition);
    }

    /// <summary>
    /// 将点集生成为gesture(待识别的笔迹)
    /// </summary>
    private void GenerateNewGesture()
    {
#if UNITY_EDITOR
        if (AddingTemp) //开发模式，导入模板到文件
        {
            Debug.LogWarning("现在是导入模板模式");
            string NewTempGestureName = tempGestureName.text.ToString();
            Gesture newGesture = new Gesture(pointList.ToArray(), NewTempGestureName);
            //将新笔迹存为Json文件
            GestureIO.SaveGestureObjectAsJson(GestureIO.CreateGestureObject(newGesture));
            return;
        }
#endif
        if (strokeCount > 1)    //生成临时笔迹并识别与目标的匹配程度
        {
            Gesture newGesture = new Gesture(pointList.ToArray(), "newGesture");

            //调用笔迹匹配方法
            float matchScore = GestureIO.GestureMatch(newGesture, curTargetGesture, Gesture.SAMPLE_POINT_NUM);
            Debug.Log($"匹配分数为 {matchScore}");
            if (matchScore < 0)//非法分数
            {
                ResetLineRenderList();
                return;
            }
            //判断否要生成实体
            if (isCreate)
            {
                if (objectsController.TryInstantiateOneObject(curTargetGesture.gestureName, in matchScore))//生成实体
                {
                    SwitchDrawState(false);
                }
                else ResetLineRenderList();
            }
            else if (objectsController.Judge(matchScore))  //不用生成实体
            {
                //添加该字到玩家字帖
                dictionary.AddPlayerDic(curTargetGesture.gestureName);
                isCreate = true;    //重置
                SwitchDrawState(false);
                if (specify == 1)  //特殊处理
                {
                    specify = 2;
                    UIManager.Instance.NewNPCDialoge("火2");
                }

            }
            else
            {
                ResetLineRenderList();
            }
        }
    }

    /// <summary>
    /// 检测鼠标是否在可绘制的面板区域
    /// </summary>
    private bool CheckInDrawArea()
    {
        if (Physics.Raycast(curCamera.ScreenPointToRay(Input.mousePosition), out mouseHit, 10f, drawPanelLayer.value))
        {
            return true;
        }
        else return false;
    }

    /// <summary>
    /// 只描一遍字帖，不可选字，不生成实物
    /// </summary>
    public void DrawOnce(string _string)
    {
        if (_string == "火" && specify == 0)
        {
            specify = 1;
        }
        SwitchDrawState();
        if (!gestureSelect) gestureSelect = GestureSelect.Instance;
        gestureSelect.selectedGestureName = _string;
        //显示要写的字，更新匹配目标
        StartCoroutine(gestureSelect.ChangeShowCharacter(_string));
        curTargetGesture = Dictionary.SearchGesture(_string);
        isCreate = false;
    }

    private void SwitchDrawState(bool _state)
    {
        canDraw = _state;
        ChangeCamera(canDraw ? camera3D : camera2D);
        OnDrawStateChange(canDraw);
    }

    private void SwitchDrawState()
    {
        canDraw = !canDraw;
        ChangeCamera(canDraw ? camera3D : camera2D);
        OnDrawStateChange(canDraw);
    }

    /// <summary>
    /// 更换字帖对象
    /// </summary>
    /// <param name="_target"></param>
    public void ChangeTarget(Gesture _target)
    {
        curTargetGesture = _target;
        ResetLineRenderList();
    }

    /// <summary>
    /// 播放过场黑幕动画
    /// </summary>
    public static void PlayBlackAnimation()
    {
        Instance.blackFade.SetActive(true);
    }

/*    public static IEnumerator PlayBlackAnimation()
    {
        Instance.blackFade.SetActive(true);
        yield return new WaitForSeconds(3f);
        Instance.blackFade.SetActive(false);
    }*/
}
