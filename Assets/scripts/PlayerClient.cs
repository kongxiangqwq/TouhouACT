using UnityEngine;

public class PlayerClient : MonoBehaviour
{
    private PlayerCore core;
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Animator anim;
    private AfterimageEffect afterimage;   

    void Awake()
    {
        core = new PlayerCore();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        afterimage = GetComponent<AfterimageEffect>();
    }

    void Update()
    {
        float input = Input.GetAxisRaw("Horizontal");
        core.SetMoveInput(input);

        bool grounded = Mathf.Abs(rb.linearVelocity.y) < 0.01f;
        core.SetGrounded(grounded);

        if (Input.GetKeyDown(KeyCode.K))
        {
            int dir = sr.flipX ? -1 : 1;
            core.SetFaceDirection(dir);
            if (core.TryStartDash(out float dashX))
            {
                rb.linearVelocity = new Vector2(dashX, rb.linearVelocity.y);
                anim.SetTrigger("Dash");
                Debug.Log("Dash triggered");
                afterimage?.SpawnAfterimage();   
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
        if (core.TryJump(out float jumpVel))
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpVel);
        }

        if (core.CanMove)
        {
            float moveX = core.GetMovementVelocityX();
            rb.linearVelocity = new Vector2(moveX, rb.linearVelocity.y);
        }

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
    }
}