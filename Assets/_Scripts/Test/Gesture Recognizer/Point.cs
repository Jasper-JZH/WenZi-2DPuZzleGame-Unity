using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//���㡰�࣬��ʾ������ÿһ��������ݽṹ
[System.Serializable]
public class Point
{
    public Vector2 pos;  //�߶���ÿ����������
    public int strokeID;    //�õ������ڵıʻ���ID

    /// <summary>
    /// ���캯��
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
