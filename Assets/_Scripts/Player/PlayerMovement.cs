using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("预设运动参数")]
    /// <summary>
    /// 跳跃系数
    /// </summary>
    [SerializeField] private float jumpForce;
    /// <summary>
    /// 移动速度 
    /// </summary>
    [SerializeField] private float moveSpeed;
    /// <summary>
    /// 降落系数，用于优化跳跃手感
    /// </summary>
   public float fallFactor;
    /// <summary>
    /// 短跳系数，用于优化跳跃手感
    /// </summary>
    [SerializeField] private float shortJumpFactor;

    [Header("组件引用")]
    /// <summary>
    /// 角色刚体
    /// </summary>
    public Rigidbody2D rb;
    /// <summary>
    /// 角色动画机
    /// </summary>
    private Animator playerAnimator;

    /// <summary>
    /// 水平方向输入值
    /// </summary>
    private float HmoveInput;
    /// <summary>
    /// ”平台“层标记
    /// </summary>
    [SerializeField] private LayerMask layerMask;
    /// <summary>
    /// 着地检查点，配合Physics2D.OverlapBox()使用
    /// </summary>
    [SerializeField] private Transform groundCheckPoint;
    /// <summary>
    /// 着地检测盒大小
    /// </summary>
    [SerializeField] private Vector2 checkBoxSize;
    /// <summary>
    /// 是否在地面上
    /// </summary>
    [SerializeField] private bool onGround;


    [Header("玩家状态相关")]
    /// <summary>
    /// 角色是否死亡
    /// </summary>
    [SerializeField] private bool isDeath;
    /// <summary>
    /// 死线，当角色y坐标低于该值时，判定玩家死亡
    /// </summary>
    [SerializeField] private float deathLine;
    /// <summary>
    /// 角色是否被冻结,即角色定在原位置.冻结时为0，解冻时为1)
    /// </summary>
    [SerializeField] private float frozen;
    /// <summary>
    /// 玩家是否可以操控角色(可操作时为1，不可操作时为0)
    /// </summary>
    public float controllable;
    /// <summary>
    /// 重生控制器
    /// </summary>
    private RestartController restartController;
    /// <summary>
    /// 是否站在土上
    /// </summary>
    public static bool onEarth;

    private void Awake()
    {
        playerAnimator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        restartController = GameObject.FindGameObjectWithTag("Respawn").GetComponent<RestartController>();
        if(GameManager.GetCurLevel() == 1)
        StartCoroutine(FreezePlayerForSeconds(17f));
    }

    private void Start()
    {
        isDeath = onGround = onEarth = false;
        frozen = controllable = 1f;       
    }

    private void Update()
    {
        DeathCheck();           //死亡检测
        //跳跃检测
        if (Input.GetKeyDown(KeyCode.Space) && onGround)         //获取跳跃移动输入
        {
            rb.velocity = Vector2.up * jumpForce * controllable;
        }
        HmoveInput = Input.GetAxis("Horizontal") * moveSpeed * controllable;   //获取水平移动输入
        rb.velocity = new Vector2(HmoveInput, rb.velocity.y) * frozen;

        Flip();                 //反转角色（如果需要）
        CheckOnGround();        //检测角色是否着地
        JumpFix();              //修正跳跃手感
        UpdateAnimatorData();

    }
    /// <summary>
    /// 当角色方向调换时，反转角色
    /// </summary>
    private void Flip()
    {
        if(rb.velocity.x < 0 && transform.eulerAngles.y < 90f)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
        }
        else if(rb.velocity.x > 0 && transform.eulerAngles.y > 90f)
            transform.eulerAngles = new Vector3(0, 0, 0);
    }
    /// <summary>
    /// 检测角色是否在地面上
    /// </summary>
    private void CheckOnGround()
    {
        //获取角色着地检测碰撞物
        Collider2D collider = Physics2D.OverlapBox(groundCheckPoint.position, checkBoxSize, 0, layerMask);
        if (collider != null)
        {
            if(onEarth)onEarth = false;
            if(!onGround) onGround = true;
            //if ((collider.CompareTag("木") || collider.CompareTag("土")) && rb.velocity.y < 0f)   //跳到木板上时y速度减为0
            if (rb.velocity.y < 0f)   //速度减为0
            {
                rb.velocity = new Vector2(rb.velocity.x, 0f);
            }
            else if (collider.CompareTag("土") && !onEarth)    //站在土上时才可以生成树
            {
                onEarth = true;
            }
        }
        else
        {
            if (onEarth) onEarth = false;
            onGround = false;
        }
        playerAnimator.SetBool("onGround", onGround);
    }

    /// <summary>
    /// 修正跳跃手感
    /// </summary>
    private void JumpFix()
    {
        if(rb.velocity.y  < 0)   //下落时调整重力大小，使角色下落加快
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * fallFactor * frozen * Time.deltaTime;
        }
        //else if(rb.velocity.y > 0 && )    //TODO:小跳
    }

    /// <summary>
    /// 可视化着地检测框
    /// </summary>
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(groundCheckPoint.position, checkBoxSize);
        Gizmos.color = Color.red;
    }
    /// <summary>
    /// 更新动画机相关参数
    /// </summary>
    private void UpdateAnimatorData()
    {
        playerAnimator.SetBool("onGround", onGround);
        playerAnimator.SetFloat("velocityX", Mathf.Abs(rb.velocity.x));
        playerAnimator.SetFloat("velocityY", rb.velocity.y);
    }

    /// <summary>
    /// 检测玩家是否死亡
    /// </summary>
    private void DeathCheck()
    {
        
        if(transform.position.y < deathLine)
        {
            isDeath = true;
            controllable = 0f;
            restartController.Restart();
            //重置角色速度
            rb.velocity = Vector2.zero;
            StartCoroutine(RecoverControl(1f));
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("火1"))
        {
            Debug.Log("被烧死了");
            isDeath = true;
            controllable = 0f;
            restartController.Restart();
            //重置角色速度
            rb.velocity = Vector2.zero;
            StartCoroutine(RecoverControl(1f));
        }
    }

    /// <summary>
    /// 延迟一段时间后恢复玩家对角色的控制权
    /// </summary>
    /// <returns></returns>
    private IEnumerator RecoverControl(float _second)
    {
        yield return new WaitForSeconds(_second);
        controllable = 1f;
    }

    /// <summary>
    /// 冻结角色
    /// </summary>
    public void FreezePlayer(bool _frozen)
    {
        if (_frozen)
        {
            frozen = controllable = 0f;  //冻结时不能控制
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }
        else
        {
            frozen = controllable = 1f;
            rb.constraints = RigidbodyConstraints2D.None;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
        
        playerAnimator.speed = frozen;//暂停/启动角色动画
    } 

    public void LockControl(bool _lock)
    {
        controllable = _lock ? 0f : 1f;
    }

    private IEnumerator FreezePlayerForSeconds(float _seconds)
    {
        FreezePlayer(true);
        yield return new WaitForSeconds(_seconds);
        FreezePlayer(false);
        restartController.Restart();
        //重置角色速度
        rb.velocity = Vector2.zero;
    }
}
