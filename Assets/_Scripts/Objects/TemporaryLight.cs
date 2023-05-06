using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TemporaryLight : MonoBehaviour
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
        Debug.Log("碰到了火");
        if (lightObject.active == true && fireObject.active == false)
        {
            lightObject.SetActive(false);//开火
            fireObject.SetActive(true);
        }
        blackCanvas.fireMaterial.DOFloat(0.764f, "_CircleHole_Size_1", 1f);
        blackCanvas.fireMaterial.DOFloat(0.26f, "_CircleHole_Dist_1", 1f);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("离开了火");
        if (lightObject.active == false && fireObject.active == true)
        {
            lightObject.SetActive(true);//开辰
            fireObject.SetActive(false);
        }
        blackCanvas.fireMaterial.DOFloat(1f, "_CircleHole_Size_1", 1.8f);
        blackCanvas.fireMaterial.DOFloat(1f, "_CircleHole_Dist_1", 1.8f);
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (lightObject.active == true && fireObject.active == false)
        {
            lightObject.SetActive(false);//开火
            fireObject.SetActive(true);
        }
        blackCanvas.fireMaterial.SetFloat("_CircleHole_Size_1", 0.764f);
        blackCanvas.fireMaterial.SetFloat("_CircleHole_Dist_1", 0.26f);
    }

}
