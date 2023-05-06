using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 该脚本装配在每个图层Layer上来控制Layer的移动
/// </summary>
public class ParrallaxLayer : MonoBehaviour
{
    /// <summary>
    /// 视差控制系数，离相机最远的的图层为-1，最近的为1
    /// </summary>
    [Range(-1f,1f)]
    [SerializeField] private float parallaxAmount;

    private Vector2 newPosition;

    public void MoveLayer(Vector2 _cameraPosChange)
    {
        newPosition = transform.localPosition;
        newPosition.x += _cameraPosChange.x * parallaxAmount * 15f * Time.deltaTime;
        newPosition.y += _cameraPosChange.y * parallaxAmount * 5f * Time.deltaTime;
        //newPosition += _cameraPosChange * parallaxAmount * 40f * Time.deltaTime;
        transform.localPosition = newPosition;  //TODO:Lerp实现平滑移动
    }
}
