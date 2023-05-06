using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obj_wood : ObjectBase
{
    /// <summary>
    /// 是否碰到火焰在燃烧中
    /// </summary>
    private bool burning;
    public Vector2 force = Vector2.zero;
    /// <summary>
    /// 刚体（如果有）
    /// </summary>
    [SerializeField] private Rigidbody2D rb;

    [Header("树")]
    /// <summary>
    /// 树生长的动画
    /// </summary>
    public Animation growingAnim;
    /// <summary>
    /// 燃烧动画
    /// </summary>
    public Animator burningAnimator;
    /// <summary>
    /// 是否为树
    /// </summary>
    public bool tree = false;
    /// <summary>
    /// 是否种在土上
    /// </summary>
    private bool onEarth = false;
    /// <summary>
    /// 土的layer
    /// </summary>
    public LayerMask earthLayer;

    /// <summary>
    /// 消失的动画
    /// </summary>
    [SerializeField] private Animator Disappear;

    public Obj_wood(string _objectName, float _life) : base(_objectName, _life)
    {
        Debug.Log("Obj_wood有参构造！");
    }

    protected override void Awake()
    {
        if (life > 0)
            StartCoroutine(LifeTimer(life));
        //base.Awake();
        //if (burningAnim) burningAnim.Stop();
    }

    private void OnDestroy()
    {
        AudioManager.StopAudio(AudioName.Water3);
    }

    /// <summary>
    /// 燃烧计时，达到时间时木头被销毁
    /// </summary>
    /// <param name="_time"></param>
    /// <returns></returns>
    protected IEnumerator Burning(float _time)
    {
        float animTime = 1.1f;  //保证有1.1秒的燃烧过程
        _time = Mathf.Clamp(_time -= animTime, 0f, _time);

        while (burning && _time > 0)
        {
            _time -= Time.deltaTime;
            yield return null;
        }

        if (_time <= 0) //销毁木头
        {
            //燃烧动画
            if (burningAnimator) burningAnimator.enabled = true;
            yield return new WaitForSeconds(animTime);
            if (life > 0) //玩家生成的木
            {
                ObjectsController.TryDestroyOneObject(this.gameObject);
            }
            else Destroy(this.gameObject);
        }

    }

    private IEnumerator Floating(float _time)
    {
        AudioManager.PlayerAudio(AudioName.Water3, false);
        float timer = 0f;
        while (timer < _time)
        {
            timer += Time.deltaTime;
            rb.AddForce(force, ForceMode2D.Force);
            yield return null;
        }

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case "火":     //火碰到木头，木头被烧毁
                {
                    burning = true;
                    Debug.Log($"{gameObject.name}碰到火焰触发器");
                    StartCoroutine(Burning(2f));
                }break;
            case "火1":     //火碰到木头，木头被烧毁
                {
                    burning = true;
                    Debug.Log($"{gameObject.name}碰到火焰触发器");
                    StartCoroutine(Burning(0.5f));
                }
                break;
            case "Water":   //漂浮
                {
                    rb.constraints = RigidbodyConstraints2D.FreezePositionY;
                    StartCoroutine(Floating(20f));
                }
                break;
        }
    }

    //射线检测土
    protected override void Update()
    {
        base.Update();
        if(tree)
        {
            RaycastHit2D hitInfo = new RaycastHit2D();
            hitInfo = Physics2D.Raycast(transform.position, new Vector2(0f, -1f), earthLayer);
            Debug.DrawRay(transform.position, new Vector2(0f, -1f), Color.red);
            if (!onEarth && hitInfo.collider.CompareTag("土"))
            {
                Debug.Log("树种上了");
                onEarth = true;
            }
            else if (onEarth && !hitInfo.collider.CompareTag("土"))
            {
                Debug.Log("土消失了，树也跟着消失");
                ObjectsController.TryDestroyOneObject(gameObject);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("火"))
        {
            //当火和木头接触超过2秒，则销毁木头
            burning = false;
            Debug.Log($"{gameObject.name}离开火焰");
        }
    }

    private void OnParticleCollision(GameObject other)
    {
        Debug.Log(other.name);
        if (other.CompareTag("水"))
        { 
            if (growingAnim) growingAnim.Play();
        }
    }

    public override IEnumerator LifeTimer(float _lifeTime)
    {
        float Timer = 0f;
        while(Timer < _lifeTime)
        {
            Timer += Time.deltaTime;
            if(tree == false && Timer > _lifeTime - 3)
            {
                Disappear.enabled = true;
            }
            yield return null;
        }
        if(tree != true)
        {
            isDied = true;
        }
    }


    protected void DestroyOneObjectTree()
    {
        ObjectsController.TryDestroyOneObject(gameObject);
    }
}
