using UnityEngine;
using System.Collections;

public class PlayerClient : MonoBehaviour
{
    private PlayerCore core;
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Animator anim;
    private AfterimageEffect afterimage;

    private float defaultGravityScale;
    private bool physicsGrounded;
    private bool wasGroundedLastFrame;

    void Awake()
    {
        core = new PlayerCore();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        afterimage = GetComponent<AfterimageEffect>();

        defaultGravityScale = rb.gravityScale;
    }

    void Start()
    {
        if (anim != null) anim.Update(0f);
    }

    void Update()
    {
        float input = Input.GetAxisRaw("Horizontal");
        core.SetMoveInput(input);

        if (Input.GetKeyDown(KeyCode.K))
        {
            int dir = sr.flipX ? -1 : 1;
            core.SetFaceDirection(dir);
            if (core.TryStartDash(out float dashX))
            {
                rb.linearVelocity = new Vector2(dashX, 0f);
                anim.SetTrigger("Dash");
                StartCoroutine(DelayedAfterimage());
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            core.RequestJump();
        }

        core.Tick(Time.deltaTime);

        anim.SetBool("isWalking", Mathf.Abs(input) > 0.1f);
        anim.SetBool("isJumping", core.IsJumping);
    }

    void FixedUpdate()
    {
        // 只有从离地到着地的瞬间才重置跳跃次数
        if (!core.IgnoreGravity)
        {
            bool justLanded = physicsGrounded && !wasGroundedLastFrame;

            if (justLanded)
            {
                core.SetGrounded(true);
                // 立刻强制播放 Idle 动画，消除延迟
                anim.Play("Idle", 0, 0f);
            }
            else if (!physicsGrounded)
            {
                core.SetGrounded(false);
            }
            // 一直着地时不重复调用
        }

        // 执行跳跃
        if (core.TryJump(out float jumpVel))
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpVel);
            physicsGrounded = false;   // 跳跃后立即取消着地标记
        }

        // 处理重力与移动
        if (core.IgnoreGravity)
        {
            rb.gravityScale = 0f;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
        }
        else
        {
            rb.gravityScale = defaultGravityScale;
            if (core.CanMove)
            {
                float moveX = core.GetMovementVelocityX();
                rb.linearVelocity = new Vector2(moveX, rb.linearVelocity.y);
            }
        }

        // 角色翻转
        float currentInput = core.MoveInput;
        if (currentInput > 0.1f)
        {
            sr.flipX = false;
            core.SetFaceDirection(1);
        }
        else if (currentInput < -0.1f)
        {
            sr.flipX = true;
            core.SetFaceDirection(-1);
        }

        // 保存着地状态，重置物理标记
        wasGroundedLastFrame = physicsGrounded;
        physicsGrounded = false;
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        // 上升阶段不认定为着地，避免起跳瞬间误判
        if (rb.linearVelocity.y > 0.1f)
            return;

        foreach (ContactPoint2D contact in collision.contacts)
        {
            if (contact.normal.y > 0.5f)
            {
                physicsGrounded = true;
                break;
            }
        }
    }

    IEnumerator DelayedAfterimage()
    {
        yield return new WaitForSeconds(0.03f);
        afterimage?.SpawnAfterimage();
    }
}