using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obj_yun : ObjectBase
{

    private void OnTriggerExit2D(Collider2D collision)
    {


        //玩家离开云，云消失
        if(collision.CompareTag("Player"))
        {
            ObjectsController.TryDestroyOneObject(gameObject);
        }
    }
}
