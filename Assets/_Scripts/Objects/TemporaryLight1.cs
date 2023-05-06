using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TemporaryLight1 : MonoBehaviour
{
    [SerializeField]  private BlackCanvas blackCanvas;

    public GameObject lightObject;
    public GameObject fireObject;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("碰到了辰");
        if (lightObject.active == false && fireObject.active == true)
        {
            lightObject.SetActive(true);//开辰
            fireObject.SetActive(false);
        }
        blackCanvas.lightMaterial.DOFloat(0f, "_CircleHole_Size_1", 1f);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(lightObject.active == true && fireObject.active == false)
        {
            lightObject.SetActive(false);//开火
            fireObject.SetActive(true);
        }
       
        blackCanvas.lightMaterial.DOFloat(1f, "_CircleHole_Size_1", 2.8f);
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (lightObject.active == false && fireObject.active == true)
        {
            lightObject.SetActive(true);//开辰
            fireObject.SetActive(false);
        }
        blackCanvas.lightMaterial.SetFloat("_CircleHole_Size_1", 0f);
    }

}
