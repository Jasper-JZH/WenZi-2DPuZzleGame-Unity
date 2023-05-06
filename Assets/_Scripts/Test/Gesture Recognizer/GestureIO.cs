using System.Collections.Generic;
using System;
using UnityEngine;
using System.IO;
using LitJson;

public class GestureIO
{
    public static List<string> filePathList = new List<string>();

    /// <summary>
    /// 点云匹配，将新的笔迹与目标笔迹匹配，返回一个匹配程度（距离）
    /// </summary>
    /// <param name="newGesture">新笔迹</param>
    /// <param name="targetGesture">目标笔迹</param>
    /// <param name="n">点数</param>
    /// <returns>匹配程度（距离）</returns>
    public static float GestureMatch(in Gesture _newGesture, in Gesture _targetGesture, int n)
    {
        if(_targetGesture==null)
        {
            Debug.LogWarning("_targetGesture为空！");
            return -1f;
        }
        bool[] isMatch = new bool[n];   //标记targetGesture中的点是否已经被匹配
        Array.Clear(isMatch, 0, n);     //初始化isMatch
        float sum = 0f;     //匹配点距离的累加
        for(int i = 0; i < _newGesture.pointsArray.Length; i++)
        {
            //寻找最短距离点
            float minDis = float.MaxValue;
            int index = i;
            for(int j = 0; j < _targetGesture.pointsArray.Length; j++)
            {
                if(isMatch[j] == false) //只计算仍未被匹配的点
                {
                    float dis = Vector2.Distance(_newGesture.pointsArray[i].pos, _targetGesture.pointsArray[j].pos);
                    if(dis < minDis)
                    {
                        minDis = dis;
                        index = j;  //更新索引标记
                    }
                }
            }
            isMatch[index] = true;
            sum += minDis;
        }
        return sum / (n-1); //将距离和的平均值作为匹配程度
    }

    /// <summary>
    /// 创建一个可以保存成json的GestureObject
    /// </summary>
    /// <param name="_gesture"></param>
    /// <returns></returns>
    public static GestureObject CreateGestureObject(in Gesture _gesture)
    {
        GestureObject newGestureObekct = new GestureObject();
        newGestureObekct.pointsArray = (Point[])_gesture.pointsArray.Clone();
        //Array.Copy(_gesture.pointsArray, newGestureObekct.pointsArray, _gesture.pointsArray.Length);    //拷贝点集
        newGestureObekct.gestureName = _gesture.gestureName;
        return newGestureObekct;
    }


    /// <summary>
    /// 将笔画保存为json
    /// </summary>
    /// <param name="_gestureObject"></param>
    public static void SaveGestureObjectAsJson(in GestureObject _gestureObject)
    {
        //string jsonString = JsonUtility.ToJson(_gestureObject);
        string jsonString = JsonMapper.ToJson(_gestureObject);
        string filePath = Application.streamingAssetsPath + "/Gesture" + "/" + _gestureObject.gestureName + ".text";
        filePathList.Add(filePath); //加入文件路径表中
        StreamWriter sw = new StreamWriter(filePath);  //创建文件写指针
        sw.Write(jsonString);   //将json格式数据写入文件
        sw.Close();
        Debug.Log($"{_gestureObject.gestureName} 已保存到 {filePath}");
    }

    /// <summary>
    /// 读取所有模板笔迹文件到表中
    /// </summary>
    /// <returns></returns>
    public static List<Gesture> ReadAllTempGesture()
    {
        List<Gesture> tempGesureList = new List<Gesture>();
        string parentPath = Application.streamingAssetsPath + "/Gesture";
        string[] filesPath = Directory.GetFiles(parentPath);    //子目录下所有文档名

        foreach(string filePath in filesPath)
        {
            //排除掉.meta文件
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
            string jsonString = sr.ReadToEnd(); //从文件中读出json格式数据

            sr.Close();

            //解析Json 
            _gestureObject = new GestureObject();
            _gestureObject = JsonMapper.ToObject<GestureObject>(jsonString);
            //_gestureObject = JsonUtility.FromJson<GestureObject>(jsonString);
            //Debug.Log($"已根据文件{filePath}创建{_gestureObject.gestureName}");
        }
        else
        {
            Debug.LogWarning($"目标路径文件{filePath}不存在");
            _gestureObject = new GestureObject();
        }
    }
}
