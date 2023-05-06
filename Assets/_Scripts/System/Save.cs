using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 存档
/// </summary>

[System.Serializable]
public class Save
{
    public int level;   //记录玩家玩到第几关    

    public Save() { }

    public Save(int _level)
    {
        level = _level;
    }

}
