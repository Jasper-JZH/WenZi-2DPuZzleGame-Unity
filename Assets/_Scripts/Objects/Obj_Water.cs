using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obj_Water : ObjectBase
{
    protected override void Awake()
    {
        base.Awake();
        AudioManager.PlayerAudio(AudioName.Water,false);
    }

    private void OnDestroy()
    {
        AudioManager.StopAudio(AudioName.Water);
    }
}
