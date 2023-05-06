using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 物体基类，所有字生成的物体均需继承该类
/// </summary>
public class ObjectBase : MonoBehaviour
{
    /// <summary>
    /// 提供无参构造函数
    /// </summary>
    protected ObjectBase() { }
    /// <summary>
    /// 有参构造
    /// </summary>
    /// <param name="_name"></param>
    protected ObjectBase(string _name, float _life)
    {
        objectName = _name;
        life = _life;
        isDied = false;
        //Debug.Log("ObjectBase有参构造！");
    }
    /// <summary>
    /// 物体对应的字名
    /// </summary>
    [SerializeField] protected string objectName;
    /// <summary>
    /// 物体的生命周期（从生成的一刻开始计时）(-1则表示无生命周期)
    /// </summary>
    [SerializeField] protected float life;
    /// <summary>
    /// 角色生命周期结束
    /// </summary>
    [SerializeField] protected bool isDied;

    protected PlayerMovement player;

    protected virtual void Awake()
    {
        if(life > 0)
            StartCoroutine(LifeTimer(life));
        player = GameObject.FindWithTag("Player").GetComponent<PlayerMovement>();
    }

    protected virtual void Update()
    {
        if (isDied)
        {
            //销毁
            ObjectsController.TryDestroyOneObject(gameObject);
            //Destroy(this.gameObject);
        }
    }

    /// <summary>
    /// 瞬间的碰撞检测
    /// </summary>
    /// <param name="collision"></param>
    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {

    }

    /// <summary>
    /// 持续停留的碰撞检测
    /// </summary>
    /// <param name="collision"></param>
    protected virtual void OnCollisionStay2D(Collision2D collision)
    {
        
    }

    

    public virtual IEnumerator LifeTimer(float _lifeTime)
    {
        float Timer = 0f;
        while (Timer < _lifeTime)
        {
            Timer += Time.deltaTime;
            yield return null;
        }
        Debug.Log("Timer：" + Timer);
        isDied = true;
    }
}
