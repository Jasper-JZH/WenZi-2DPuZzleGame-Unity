using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect_Boom : MonoBehaviour
{
    public BoxCollider2D boxCollider;
    /// <summary>
    /// 标记可炸毁的土层
    /// </summary>
    public LayerMask earthLayer;

    [SerializeField] private Vector2 checkBoxSize;

    private void OnParticleCollision(GameObject other)
    {
        Debug.Log(other.name);
        if (other.CompareTag("烟"))
        {
            //播放爆炸效果
            Debug.Log("发生爆炸");
            Boom();
        }
    }

    //发生爆炸后的响应
    private void Boom()
    {
        //获取角色周围标记为”土“的物体，如果有土则会被炸毁
        Collider2D collider = Physics2D.OverlapBox(transform.position, checkBoxSize, 0, earthLayer);
        if(collider)
        {
            GameObject boomedObject = collider.gameObject;
            Debug.Log($"{boomedObject}被炸毁");
            if (!ObjectsController.TryDestroyOneObject(boomedObject))
            {
                Destroy(boomedObject);
            }
        }
    }

    public void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, checkBoxSize);
        Gizmos.color = Color.red;
    }
}
