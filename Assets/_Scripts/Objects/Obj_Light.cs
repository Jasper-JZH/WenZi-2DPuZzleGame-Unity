using System.Collections;
using UnityEngine;
using DG.Tweening;

public class Obj_Light : ObjectBase
{
    private BlackCanvas blackCanvas;

    protected override void Awake()
    {
        base.Awake();
        blackCanvas = GameObject.FindObjectOfType<BlackCanvas>();
    }

    private void Start()
    {
        StartCoroutine(Lighting(life));
    }

    private void FollowPlayer()
    {
        //插值跟随
        transform.position = player.transform.position;
    }

    protected override void Update()
    {
        base.Update();
        FollowPlayer();
    }

    public IEnumerator Lighting(float _time)
    {
        float timer1 = 0f;  //用于延迟启动变亮的计时器
        float fadeTime = 3f;
        bool lighting = false;
        while (timer1 < 1f)
        {
            timer1 += Time.deltaTime;
            yield return null;
        }
        blackCanvas.lightMaterial.DOFloat(0f, "_CircleHole_Size_1", 1f);
        yield return new WaitForSeconds(Mathf.Clamp(_time - fadeTime, 0, _time));
        //等到离光消失还有fadeTime时，开始变暗
        blackCanvas.lightMaterial.DOFloat(1f, "_CircleHole_Size_1", 2.8f);
    }
}
