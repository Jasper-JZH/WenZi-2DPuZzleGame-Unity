using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempPen : MonoBehaviour
{
    [Header("LineRenderer")]
    public List<LineRenderer> lineList = new List<LineRenderer>();
    [SerializeField] private LineRenderer curline;
    [SerializeField] private LineRenderer tempLine;  //��ˢ����
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
        if (Input.GetKeyDown(KeyCode.C)) //��ʼ����
        {
            //��յ㼯
            pointList.Clear();
            //���lineList
            count = 0;
            foreach(var line in lineList)
            {
                line.positionCount = 0;
                Destroy(line.gameObject);
            }
            lineList.Clear();
            //���ñʻ���
            strokeCount = 0;
            //�������
            canDraw = true;
            //�򿪻��壬��ʾ�ʵ�ģ��
            penModel.gameObject.SetActive(true);
        }
        if(Input.GetKeyUp(KeyCode.C))   //��������
        {
            //��ֹ����
            canDraw = false;
            //���ر�ģ��
            penModel.gameObject.SetActive(false);
            //���ôλ��Ƶĵ㼯����Ϊ�ʼ���Gesture��
            if (strokeCount > 0)
            {
                Gesture newGesture = new Gesture(pointList.ToArray(), "test Gesture 1");
                newGesture.PrintGesture(newGesture);
                //���±ʼ���ΪJson�ļ�
                GestureIO.SaveGestureObjectAsJson(GestureIO.CreateGestureObject(newGesture));
                //��ӡ�����ȡ��json�ļ�
                GestureObject tempGestureObejct;
                GestureIO.ReadGestureObjectFromJson(out tempGestureObejct, Application.streamingAssetsPath +"/Gesture" + "/test Gesture 1.text");
                Gesture tempGesture = new Gesture(tempGestureObejct);
                tempGesture.PrintGesture(tempGesture);
            }

            //���ñʼ�ƥ�䷽��
            //......
        }
        if(Input.GetMouseButtonDown(0) && canDraw == true) //��ʼ�µ�һ��
        {
            isDrawing = true;
            strokeCount++;
            count = 0;  //����lineRenderer������
            //����һ���µ�lineRenderer
            curline = Instantiate(tempLine, transform.position, transform.rotation);
            //curline = GetComponent<LineRenderer>();
            lineList.Add(curline);
        }
        if(Input.GetMouseButton(0) && isDrawing == true) //�����У���¼��
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 2.0f;
            penModel.position = Vector3.Lerp(penModel.position, mousePosition, 0.5f);

            lineList[strokeCount - 1].positionCount = ++count;
            lineList[strokeCount - 1].SetPosition(count - 1, mousePosition);
            pointList.Add(new Point(mousePosition, strokeCount));   //��¼�ʼ��еĵ�
        }
        if(Input.GetMouseButtonUp(0))   //������ǰ��һ��
        {
            isDrawing = false;
        }
    }
}
