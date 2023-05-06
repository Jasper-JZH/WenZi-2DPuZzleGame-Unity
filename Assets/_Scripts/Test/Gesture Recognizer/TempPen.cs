using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempPen : MonoBehaviour
{
    [Header("LineRenderer")]
    public List<LineRenderer> lineList = new List<LineRenderer>();
    [SerializeField] private LineRenderer curline;
    [SerializeField] private LineRenderer tempLine;  //笔刷配置
    [SerializeField] private int count;
    [SerializeField] private List<Point> pointList = new List<Point>();

    [Header("Pen")]
    [SerializeField] private Transform penModel;

    [Header("Draw")]
    [SerializeField] private bool isDrawing;
    [SerializeField] private bool canDraw;
    [SerializeField] private int strokeCount = 0;
    private void Awake()
    {
        penModel = transform.GetChild(0);
        tempLine.startColor = Color.green;
        tempLine.endColor = Color.red;
        tempLine.startWidth = 0.3f;
        tempLine.endWidth = 0.3f;
    }

    private void Start()
    {
        count = 0;
        penModel.gameObject.SetActive(false);
        canDraw = false;
        isDrawing = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C)) //开始绘制
        {
            //清空点集
            pointList.Clear();
            //清空lineList
            count = 0;
            foreach(var line in lineList)
            {
                line.positionCount = 0;
                Destroy(line.gameObject);
            }
            lineList.Clear();
            //重置笔画数
            strokeCount = 0;
            //允许绘制
            canDraw = true;
            //打开画板，显示笔的模型
            penModel.gameObject.SetActive(true);
        }
        if(Input.GetKeyUp(KeyCode.C))   //结束绘制
        {
            //禁止绘制
            canDraw = false;
            //隐藏笔模型
            penModel.gameObject.SetActive(false);
            //将该次绘制的点集生成为笔迹（Gesture）
            if (strokeCount > 0)
            {
                Gesture newGesture = new Gesture(pointList.ToArray(), "test Gesture 1");
                newGesture.PrintGesture(newGesture);
                //将新笔迹存为Json文件
                GestureIO.SaveGestureObjectAsJson(GestureIO.CreateGestureObject(newGesture));
                //打印输出读取的json文件
                GestureObject tempGestureObejct;
                GestureIO.ReadGestureObjectFromJson(out tempGestureObejct, Application.streamingAssetsPath +"/Gesture" + "/test Gesture 1.text");
                Gesture tempGesture = new Gesture(tempGestureObejct);
                tempGesture.PrintGesture(tempGesture);
            }

            //调用笔迹匹配方法
            //......
        }
        if(Input.GetMouseButtonDown(0) && canDraw == true) //开始新的一笔
        {
            isDrawing = true;
            strokeCount++;
            count = 0;  //重置lineRenderer顶点数
            //生成一个新的lineRenderer
            curline = Instantiate(tempLine, transform.position, transform.rotation);
            //curline = GetComponent<LineRenderer>();
            lineList.Add(curline);
        }
        if(Input.GetMouseButton(0) && isDrawing == true) //绘制中，记录点
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 2.0f;
            penModel.position = Vector3.Lerp(penModel.position, mousePosition, 0.5f);

            lineList[strokeCount - 1].positionCount = ++count;
            lineList[strokeCount - 1].SetPosition(count - 1, mousePosition);
            pointList.Add(new Point(mousePosition, strokeCount));   //记录笔迹中的点
        }
        if(Input.GetMouseButtonUp(0))   //结束当前这一笔
        {
            isDrawing = false;
        }
    }
}
