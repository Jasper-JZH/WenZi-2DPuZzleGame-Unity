using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//“手势/笔画”类，表示每一份笔画的集合
//含有将笔画生成为点云并预处理的方法
//并存储这些笔画转化成的点云集（一个Point[]）
public class Gesture
{
    public Point[] pointsArray = null;      //存放笔画点集
    public string gestureName = "";         //识别出来的笔画集名
    public static int SAMPLE_POINT_NUM = 64;      //统一重采样点数
    //使用点集points来创建一个笔画集，并进行预处理
    public Gesture(Point[] _pointsArray, string _gestureName)
    {
        gestureName = _gestureName;
        //对传入的点集预处理
        Scale(ref _pointsArray);
        TranslateToOrigin(ref _pointsArray);
        pointsArray = ReSample(_pointsArray, SAMPLE_POINT_NUM);
    }
    /// <summary>
    /// 空构造函数
    /// </summary>
    public Gesture() { }

    public Gesture(GestureObject _gestureObject)
    {
        pointsArray = _gestureObject.pointsArray;
        gestureName = _gestureObject.gestureName;
    }

    /// <summary>
    /// 缩放点集到[0,1]X[0,1]大小的二维平面空间中
    /// </summary>
    /// <param name="_pointsArray">待缩放点集的引用</param>
    /// <returns></returns>
    private void Scale(ref Point[] _pointsArray)
    {
        //获取原始点集的边界
        Vector2 minXY = new Vector2(float.MaxValue, float.MaxValue);
        Vector2 maxXY = new Vector2(float.MinValue, float.MinValue);

        foreach (var point in _pointsArray)
        {
            if (point.pos.x < minXY.x) minXY.x = point.pos.x;
            if (point.pos.y < minXY.y) minXY.y = point.pos.y;
            if (point.pos.x > maxXY.x) maxXY.x = point.pos.x;
            if (point.pos.y > maxXY.y) maxXY.y = point.pos.y;
        }

        //将点集缩放到[0,1]X[0,1]大小的二维空间中
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
    /// 将点集合平移到以坐标原点为形心
    /// </summary>
    /// <param name="_pointsArray">待平移点集的引用</param>
    /// <returns></returns>
    private void TranslateToOrigin(ref Point[] _pointsArray)
    {
        //计算当前的形心
        Vector2 centroid = Vector2.zero;
        foreach(var point in _pointsArray)
        {
            //遍历每个点，累加x,y
            centroid.x += point.pos.x;
            centroid.y += point.pos.y;
        }
        centroid /= _pointsArray.Length;    //平均值即为形心

        //平移
        //Point[] newPointsArray = new Point[_pointsArray.Length];
        for (int i = 0; i < _pointsArray.Length; i++)
        {
            _pointsArray[i].pos -= centroid;
        }
        //return newPointsArray;
    }

    /// <summary>
    /// 对位置“规整”后的点集重采样，点集保留n个等距点
    /// </summary>
    /// <param name="pointsArray">待重采样点集的引用</param>
    /// <param name="n">重采样点数</param>
    /// <returns></returns>
    private Point[] ReSample(Point[] _pointsArray, int _n)
    {
        //计算期望点间距，即如果把当前的笔迹用n个点划分成n-1段，每段的长度
        //累加求出笔迹的总长度
        float pathLength = 0f;
        for (int i = 1; i < _pointsArray.Length; i++)
        {
            //判断两个点是否是同一笔画上的点
            if(_pointsArray[i].strokeID == _pointsArray[i-1].strokeID)
            {
                pathLength += Vector2.Distance(_pointsArray[i].pos,_pointsArray[i-1].pos);
            }
        }
        //计算期望点距
        float I = pathLength / (_n - 1);

        //重新遍历原始采样点，根据需求（n个点且点之间间距为I）重采样
        float D = 0;    //表示当前遍历到的点与上一个被重采样（选入新点集）的点的距离
        float d = 0;    //表示P[i]和P[i-1]的距离
        Point[] newPointsArray = new Point[_n];
        newPointsArray[0] = _pointsArray[0];    //将第一个点放入新点集
        int pointsCount = 1;    //记录新点集中的点数
        for (int i = 1; i < _pointsArray.Length; i++)
        {
            if(_pointsArray[i-1].strokeID == _pointsArray[i].strokeID)
            {
                d = Vector2.Distance(_pointsArray[i].pos, _pointsArray[i - 1].pos);
                if (D + d >= I)  //如果当前累积距离已经大于期望距离I，说明P[i]和P[i-1]间至少会插值出一个新点放入新点集中
                {
                    Point firstPoint = _pointsArray[i - 1]; //标记插值计算中的第一个点
                    while(D + d >= I)   //可能会有多个新点出现
                    {
                        float k = Mathf.Clamp((I - D) / d, 0.0f, 1.0f); //计算插值参数k,需确保 (0 <= k <= 1)
                        Point newPoint = new Point(Vector2.Lerp(_pointsArray[i - 1].pos, _pointsArray[i].pos, k), _pointsArray[i].strokeID);   //插值得到新点
                        newPointsArray[pointsCount++] = newPoint;
                        firstPoint = newPointsArray[pointsCount - 1];   //更新标记
                        d = D + d - I;  //计算"d剩下的"
                        D = 0;  //重置D
                    }
                    D = d;  //退出while说明“剩下的”d < I，此时的d的大小可理解成新点集最后一个点到P[i]的距离
                }
                else D += d;
            }
        }
        //最后一个点可能不满足 D+d>=I 的条件，但是没有其它点了，所以得补上确保新点集数量足够
        if(pointsCount == _n - 1)
        {
            newPointsArray[pointsCount++] = _pointsArray[_pointsArray.Length - 1];
        }
        return newPointsArray;

    }

    //Debug
    public void PrintGesture(Gesture _gesture)
    {
        Debug.Log($"笔迹名为\"{_gesture.gestureName}\"");
        Point point;
        for (int i = 0; i < _gesture.pointsArray.Length; i++)
        {
            point = _gesture.pointsArray[i];
            Debug.Log($"第{i}个点，属于第{point.strokeID}画，坐标为{point.pos}");
        }
    }
}
