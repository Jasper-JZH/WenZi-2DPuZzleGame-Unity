using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//������/�ʻ����࣬��ʾÿһ�ݱʻ��ļ���
//���н��ʻ�����Ϊ���Ʋ�Ԥ����ķ���
//���洢��Щ�ʻ�ת���ɵĵ��Ƽ���һ��Point[]��
public class Gesture
{
    public Point[] pointsArray = null;      //��űʻ��㼯
    public string gestureName = "";         //ʶ������ıʻ�����
    public static int SAMPLE_POINT_NUM = 64;      //ͳһ�ز�������
    //ʹ�õ㼯points������һ���ʻ�����������Ԥ����
    public Gesture(Point[] _pointsArray, string _gestureName)
    {
        gestureName = _gestureName;
        //�Դ���ĵ㼯Ԥ����
        Scale(ref _pointsArray);
        TranslateToOrigin(ref _pointsArray);
        pointsArray = ReSample(_pointsArray, SAMPLE_POINT_NUM);
    }
    /// <summary>
    /// �չ��캯��
    /// </summary>
    public Gesture() { }

    public Gesture(GestureObject _gestureObject)
    {
        pointsArray = _gestureObject.pointsArray;
        gestureName = _gestureObject.gestureName;
    }

    /// <summary>
    /// ���ŵ㼯��[0,1]X[0,1]��С�Ķ�άƽ��ռ���
    /// </summary>
    /// <param name="_pointsArray">�����ŵ㼯������</param>
    /// <returns></returns>
    private void Scale(ref Point[] _pointsArray)
    {
        //��ȡԭʼ�㼯�ı߽�
        Vector2 minXY = new Vector2(float.MaxValue, float.MaxValue);
        Vector2 maxXY = new Vector2(float.MinValue, float.MinValue);

        foreach (var point in _pointsArray)
        {
            if (point.pos.x < minXY.x) minXY.x = point.pos.x;
            if (point.pos.y < minXY.y) minXY.y = point.pos.y;
            if (point.pos.x > maxXY.x) maxXY.x = point.pos.x;
            if (point.pos.y > maxXY.y) maxXY.y = point.pos.y;
        }

        //���㼯���ŵ�[0,1]X[0,1]��С�Ķ�ά�ռ���
        float scale = Mathf.Max(maxXY.x - minXY.x, maxXY.y - minXY.y);
        for(int i = 0; i < _pointsArray.Length; i++)
        {
            Vector2 newPos = new Vector2(_pointsArray[i].pos.x - minXY.x, _pointsArray[i].pos.y - minXY.y) / scale;
            //Vector2 newPos = new Point(_pointsArray[i].pos / scale, _pointsArray[i].strokeID);
            _pointsArray[i].pos = newPos;
        }
        //return newpointsArray;
    }

    /// <summary>
    /// ���㼯��ƽ�Ƶ�������ԭ��Ϊ����
    /// </summary>
    /// <param name="_pointsArray">��ƽ�Ƶ㼯������</param>
    /// <returns></returns>
    private void TranslateToOrigin(ref Point[] _pointsArray)
    {
        //���㵱ǰ������
        Vector2 centroid = Vector2.zero;
        foreach(var point in _pointsArray)
        {
            //����ÿ���㣬�ۼ�x,y
            centroid.x += point.pos.x;
            centroid.y += point.pos.y;
        }
        centroid /= _pointsArray.Length;    //ƽ��ֵ��Ϊ����

        //ƽ��
        //Point[] newPointsArray = new Point[_pointsArray.Length];
        for (int i = 0; i < _pointsArray.Length; i++)
        {
            _pointsArray[i].pos -= centroid;
        }
        //return newPointsArray;
    }

    /// <summary>
    /// ��λ�á���������ĵ㼯�ز������㼯����n���Ⱦ��
    /// </summary>
    /// <param name="pointsArray">���ز����㼯������</param>
    /// <param name="n">�ز�������</param>
    /// <returns></returns>
    private Point[] ReSample(Point[] _pointsArray, int _n)
    {
        //�����������࣬������ѵ�ǰ�ıʼ���n���㻮�ֳ�n-1�Σ�ÿ�εĳ���
        //�ۼ�����ʼ����ܳ���
        float pathLength = 0f;
        for (int i = 1; i < _pointsArray.Length; i++)
        {
            //�ж��������Ƿ���ͬһ�ʻ��ϵĵ�
            if(_pointsArray[i].strokeID == _pointsArray[i-1].strokeID)
            {
                pathLength += Vector2.Distance(_pointsArray[i].pos,_pointsArray[i-1].pos);
            }
        }
        //�����������
        float I = pathLength / (_n - 1);

        //���±���ԭʼ�����㣬��������n�����ҵ�֮����ΪI���ز���
        float D = 0;    //��ʾ��ǰ�������ĵ�����һ�����ز�����ѡ���µ㼯���ĵ�ľ���
        float d = 0;    //��ʾP[i]��P[i-1]�ľ���
        Point[] newPointsArray = new Point[_n];
        newPointsArray[0] = _pointsArray[0];    //����һ��������µ㼯
        int pointsCount = 1;    //��¼�µ㼯�еĵ���
        for (int i = 1; i < _pointsArray.Length; i++)
        {
            if(_pointsArray[i-1].strokeID == _pointsArray[i].strokeID)
            {
                d = Vector2.Distance(_pointsArray[i].pos, _pointsArray[i - 1].pos);
                if (D + d >= I)  //�����ǰ�ۻ������Ѿ�������������I��˵��P[i]��P[i-1]�����ٻ��ֵ��һ���µ�����µ㼯��
                {
                    Point firstPoint = _pointsArray[i - 1]; //��ǲ�ֵ�����еĵ�һ����
                    while(D + d >= I)   //���ܻ��ж���µ����
                    {
                        float k = Mathf.Clamp((I - D) / d, 0.0f, 1.0f); //�����ֵ����k,��ȷ�� (0 <= k <= 1)
                        Point newPoint = new Point(Vector2.Lerp(_pointsArray[i - 1].pos, _pointsArray[i].pos, k), _pointsArray[i].strokeID);   //��ֵ�õ��µ�
                        newPointsArray[pointsCount++] = newPoint;
                        firstPoint = newPointsArray[pointsCount - 1];   //���±��
                        d = D + d - I;  //����"dʣ�µ�"
                        D = 0;  //����D
                    }
                    D = d;  //�˳�while˵����ʣ�µġ�d < I����ʱ��d�Ĵ�С�������µ㼯���һ���㵽P[i]�ľ���
                }
                else D += d;
            }
        }
        //���һ������ܲ����� D+d>=I ������������û���������ˣ����Եò���ȷ���µ㼯�����㹻
        if(pointsCount == _n - 1)
        {
            newPointsArray[pointsCount++] = _pointsArray[_pointsArray.Length - 1];
        }
        return newPointsArray;

    }

    //Debug
    public void PrintGesture(Gesture _gesture)
    {
        Debug.Log($"�ʼ���Ϊ\"{_gesture.gestureName}\"");
        Point point;
        for (int i = 0; i < _gesture.pointsArray.Length; i++)
        {
            point = _gesture.pointsArray[i];
            Debug.Log($"��{i}���㣬���ڵ�{point.strokeID}��������Ϊ{point.pos}");
        }
    }
}
