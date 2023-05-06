using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
/// <summary>
/// 视错觉控制器
/// </summary>
public class ParallaxController : MonoBehaviour
{
    /// <summary>
    /// 相机移动委托
    /// </summary>
    public Action<Vector2> onCameraMove;
    /// <summary>
    /// 旧相机位置
    /// </summary>
    private Vector2 oldCameraPos;
    /// <summary>
    /// 存放所有层
    /// </summary>
    private List<ParrallaxLayer> parallaxLayersList = new List<ParrallaxLayer>();
    /// <summary>
    /// 相机
    /// </summary>
    Camera camera;

    private void Start()
    {
        camera = Camera.main;
        onCameraMove += MoveLayrs;  //订阅事件
        FindLayers();   //找到所有层，初始化“层表”parallaxLayersList
        oldCameraPos = camera.transform.position;
    }

    /// <summary>
    /// 相机应使用FixedUpdate来更新
    /// </summary>
    private void FixedUpdate()
    {
        //如果相机发生移动才更新
        if(camera.transform.position.x !=oldCameraPos.x || camera.transform.position.y != oldCameraPos.y)
        {
            if(onCameraMove!=null)
            {
                Vector2 cameraPosChange = (Vector2)camera.transform.position - oldCameraPos;
                onCameraMove(cameraPosChange);  //触发事件，使所有层跟着相机移动
            }
        }
        oldCameraPos = camera.transform.position;
    }

    /// <summary>
    /// 找到所有层
    /// </summary>
    private void FindLayers()
    {
        parallaxLayersList.Clear();

        for(int i = 0; i < transform.childCount; i++)
        {
            ParrallaxLayer layer = transform.GetChild(i).GetComponent<ParrallaxLayer>();
            if (layer!=null)
            {
                parallaxLayersList.Add(layer);
            }
        }
    }

    private void MoveLayrs(Vector2 _cameraPosChange)
    {
        foreach(var layer in parallaxLayersList)
        {
            layer.MoveLayer(_cameraPosChange);
        }
    }
}
