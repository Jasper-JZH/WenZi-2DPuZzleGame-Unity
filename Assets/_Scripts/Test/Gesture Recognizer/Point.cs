using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//”点“类，表示点云中每一个点的数据结构
[System.Serializable]
public class Point
{
    public Vector2 pos;  //线段上每个结点的坐标
    public int strokeID;    //该点所属于的笔画的ID

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="_pos"></param>
    /// <param name="_strokeID"></param>
    public Point(Vector2 _pos,int _strokeID)   
    {
        pos = _pos;
        strokeID = _strokeID;
    }

    public Point() { }
}
