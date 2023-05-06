using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gonggaoban : MonoBehaviour
{
    //挂载到公告板画布
    [SerializeField] Camera mainCam;
    [SerializeField] bool revert;

    void Update()
    {

        if (mainCam)
        {
            Vector3 camPos = mainCam.transform.position;
            Vector3 vector = camPos - transform.position;
            vector.y = 0;
            if (vector.magnitude >= 0.5f)
            {
                transform.rotation = Quaternion.LookRotation((revert ? -1 : 1) * vector);
            }
        }
    }


}
