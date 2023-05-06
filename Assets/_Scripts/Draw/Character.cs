using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 每个字的结构体，存储字的基本信息和玩家对该字的获取状态等
/// </summary>
public class Character
{
    /// <summary>
    /// 什么字
    /// </summary>
    public string name;
    /// <summary>
    /// 当前字的状态
    /// </summary>
    private State characterState;


    /// <summary>
    /// 空构造函数
    /// </summary>
    public Character() { }
    /// <summary>
    /// 构造函数
    /// </summary>
    public Character(string _characterName)  
    {
        name = _characterName;
        characterState = State.unGet;
    }


    public State CharacterState
    {
        get { return characterState; }
        set
        {
            characterState = value;
            switch(characterState)
            {
                case State.unGet:

                    break;
                case State.get:

                    break;
                case State.disable:

                    break;
            }
        }
    }
    public enum State
    {
        unGet,  // 未获取
        get,    // 已获取且可使用
        disable // 已获取但禁用
    }

    public void ChangeState(State _nextState)
    {
        if (_nextState == CharacterState) return;
        CharacterState = _nextState;
    }


}
