using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackCanvas : MonoBehaviour
{
    /// <summary>
    /// 黑幕canvas
    /// </summary>
    public GameObject blackCanvas;
    /// <summary>
    /// 黑幕是否开启
    /// </summary>
    public bool isCanvasOn;
    public GameObject lightObject;
    public GameObject fireObject;
    public Material lightMaterial;
    public Material fireMaterial;
    private void Awake()
    {
        blackCanvas = transform.GetChild(0).gameObject;
        lightMaterial.SetFloat("_CircleHole_Size_1", 1.0f);
        lightObject.SetActive(true);
        fireObject.SetActive(false);
    }
                                                                                                                                       
    private void Start()
    {
        isCanvasOn = !DrawController.canDraw; 
        blackCanvas.SetActive(isCanvasOn);
    }
    private void Update()
    {
        if(isCanvasOn == DrawController.canDraw)
        {
            isCanvasOn = !isCanvasOn;
            blackCanvas.SetActive(isCanvasOn);
            if(isCanvasOn)  //调整黑幕
            {
                if (ObjectsController.isLight)
                {
                    if (!lightObject.activeSelf) lightObject.SetActive(true);//开辰
                    if (fireObject.activeSelf) fireObject.SetActive(false);
                }
                else
                {
                    if(lightObject.activeSelf) lightObject.SetActive(false);
                    if(!fireObject.activeSelf) fireObject.SetActive(true);
                }
            }
        }
    }
}
