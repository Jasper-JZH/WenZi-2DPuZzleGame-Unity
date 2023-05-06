using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obj_smoke : ObjectBase
{
    public Obj_smoke(string _objectName, float _life) : base(_objectName, _life)
    {
        Debug.Log("Obj_smoke有参构造！");
    }
    protected override void Awake()
    {
        base.Awake();
        //音效
        //AudioManager.PlayerAudio(AudioName.Burn, false);
    }
}
