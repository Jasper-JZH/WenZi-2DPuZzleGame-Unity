using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Obj_Fire : ObjectBase
{
    /// <summary>
    /// 该火焰是否会伤到玩家
    /// </summary>
    [SerializeField] private bool attack = false;   //默认不会

    private BlackCanvas blackCanvas;
    private bool hasBlackCanvas = false;
    protected override void Awake()
    {
        base.Awake();
        if(GameManager.GetCurLevel()==10)
        {
            blackCanvas = GameObject.FindObjectOfType<BlackCanvas>();
            hasBlackCanvas = blackCanvas ? true : false;
        }
        //音效
        AudioManager.PlayerAudio(AudioName.Burn, false);
    }
    private void Start()
    {
        if(hasBlackCanvas)
        {
            StartCoroutine(Lighting(life));
        }
    }

    private void OnDestroy()
    {
        //音效
        AudioManager.StopAudio(AudioName.Burn);
    }
    protected override void Update()
    {
        base.Update();
        //火焰跟随玩家
        if(!attack) FollowPlayer();
    }

    private void FollowPlayer()
    {
        //插值跟随
        transform.position = player.transform.position;
    }

    protected void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log($"{gameObject.name}碰到{collision.collider.name}");
        base.OnCollisionEnter2D(collision);
        if(collision.collider.CompareTag("Player") && attack)
        {
            //玩家受伤死亡
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.name);
        if (collision.CompareTag("Water") || collision.CompareTag("水"))
        {
            //火被水熄灭
            ObjectsController.TryDestroyOneObject(gameObject);
        }
    }

    private void OnParticleCollision(GameObject other)
    {
        Debug.Log(other.name);
        if(other.CompareTag("水"))
        { //火被水熄灭
            if (life < 0)
            {
                Destroy(this.gameObject);
            }
            else ObjectsController.TryDestroyOneObject(gameObject);
        }
    }
    private void OnParticleTrigger()
    {
        Debug.Log("OnParticleTrigger");
    }

    public IEnumerator Lighting(float _time)
    {
        float timer1 = 0f;  //用于延迟启动变亮的计时器
        float fadeTime = 2f;
        bool lighting = false;
        while (timer1 < 1f)
        {
            timer1 += Time.deltaTime;
            yield return null;
        }
        blackCanvas.fireMaterial.DOFloat(0.764f, "_CircleHole_Size_1", 1f);
        blackCanvas.fireMaterial.DOFloat(0.26f, "_CircleHole_Dist_1", 1f);
        yield return new WaitForSeconds(Mathf.Clamp(_time - fadeTime, 0, _time));
        //等到离光消失还有fadeTime时，开始变暗
        blackCanvas.fireMaterial.DOFloat(1f, "_CircleHole_Size_1", 1.8f);
        blackCanvas.fireMaterial.DOFloat(1f, "_CircleHole_Dist_1", 1.8f);
    }
}
