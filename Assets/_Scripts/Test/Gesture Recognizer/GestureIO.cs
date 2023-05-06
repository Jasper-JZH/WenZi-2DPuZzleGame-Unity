using System.Collections.Generic;
using System;
using UnityEngine;
using System.IO;
using LitJson;

public class GestureIO
{
    public static List<string> filePathList = new List<string>();

    /// <summary>
    /// ����ƥ�䣬���µıʼ���Ŀ��ʼ�ƥ�䣬����һ��ƥ��̶ȣ����룩
    /// </summary>
    /// <param name="newGesture">�±ʼ�</param>
    /// <param name="targetGesture">Ŀ��ʼ�</param>
    /// <param name="n">����</param>
    /// <returns>ƥ��̶ȣ����룩</returns>
    public static float GestureMatch(in Gesture _newGesture, in Gesture _targetGesture, int n)
    {
        if(_targetGesture==null)
        {
            Debug.LogWarning("_targetGestureΪ�գ�");
            return -1f;
        }
        bool[] isMatch = new bool[n];   //���targetGesture�еĵ��Ƿ��Ѿ���ƥ��
        Array.Clear(isMatch, 0, n);     //��ʼ��isMatch
        float sum = 0f;     //ƥ��������ۼ�
        for(int i = 0; i < _newGesture.pointsArray.Length; i++)
        {
            //Ѱ����̾����
            float minDis = float.MaxValue;
            int index = i;
            for(int j = 0; j < _targetGesture.pointsArray.Length; j++)
            {
                if(isMatch[j] == false) //ֻ������δ��ƥ��ĵ�
                {
                    float dis = Vector2.Distance(_newGesture.pointsArray[i].pos, _targetGesture.pointsArray[j].pos);
                    if(dis < minDis)
                    {
                        minDis = dis;
                        index = j;  //�����������
                    }
                }
            }
            isMatch[index] = true;
            sum += minDis;
        }
        return sum / (n-1); //������͵�ƽ��ֵ��Ϊƥ��̶�
    }

    /// <summary>
    /// ����һ�����Ա����json��GestureObject
    /// </summary>
    /// <param name="_gesture"></param>
    /// <returns></returns>
    public static GestureObject CreateGestureObject(in Gesture _gesture)
    {
        GestureObject newGestureObekct = new GestureObject();
        newGestureObekct.pointsArray = (Point[])_gesture.pointsArray.Clone();
        //Array.Copy(_gesture.pointsArray, newGestureObekct.pointsArray, _gesture.pointsArray.Length);    //�����㼯
        newGestureObekct.gestureName = _gesture.gestureName;
        return newGestureObekct;
    }


    /// <summary>
    /// ���ʻ�����Ϊjson
    /// </summary>
    /// <param name="_gestureObject"></param>
    public static void SaveGestureObjectAsJson(in GestureObject _gestureObject)
    {
        //string jsonString = JsonUtility.ToJson(_gestureObject);
        string jsonString = JsonMapper.ToJson(_gestureObject);
        string filePath = Application.streamingAssetsPath + "/Gesture" + "/" + _gestureObject.gestureName + ".text";
        filePathList.Add(filePath); //�����ļ�·������
        StreamWriter sw = new StreamWriter(filePath);  //�����ļ�дָ��
        sw.Write(jsonString);   //��json��ʽ����д���ļ�
        sw.Close();
        Debug.Log($"{_gestureObject.gestureName} �ѱ��浽 {filePath}");
    }

    /// <summary>
    /// ��ȡ����ģ��ʼ��ļ�������
    /// </summary>
    /// <returns></returns>
    public static List<Gesture> ReadAllTempGesture()
    {
        List<Gesture> tempGesureList = new List<Gesture>();
        string parentPath = Application.streamingAssetsPath + "/Gesture";
        string[] filesPath = Directory.GetFiles(parentPath);    //��Ŀ¼�������ĵ���

        foreach(string filePath in filesPath)
        {
            //�ų���.meta�ļ�
            if (filePath.Substring(filePath.Length - 4) == "meta") continue;
            //string filePath = parentPath + "/" + fileName + ".txt";
            GestureObject tempGestureObject;
            ReadGestureObjectFromJson(out tempGestureObject, filePath);
            tempGesureList.Add(new Gesture(tempGestureObject));
        }
        return tempGesureList;
    }

    public static void ReadGestureObjectFromJson(out GestureObject _gestureObject ,string filePath)
    {
        if(File.Exists(filePath))
        {
            StreamReader sr = new StreamReader(filePath);
            string jsonString = sr.ReadToEnd(); //���ļ��ж���json��ʽ����

            sr.Close();

            //����Json 
            _gestureObject = new GestureObject();
            _gestureObject = JsonMapper.ToObject<GestureObject>(jsonString);
            //_gestureObject = JsonUtility.FromJson<GestureObject>(jsonString);
            //Debug.Log($"�Ѹ����ļ�{filePath}����{_gestureObject.gestureName}");
        }
        else
        {
            Debug.LogWarning($"Ŀ��·���ļ�{filePath}������");
            _gestureObject = new GestureObject();
        }
    }
}
