using UnityEngine;

public class PlayerClient : MonoBehaviour
{
    private PlayerCore core;
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Animator anim;

    void Awake()
    {
        core = new PlayerCore();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        float input = Input.GetAxisRaw("Horizontal");
        core.SetMoveInput(input);   

        if (Input.GetKeyDown(KeyCode.K))
        {
            int dir = sr.flipX ? -1 : 1;
            core.SetFaceDirection(dir);

            if (core.TryStartRoll(out float rollX))
            {
                rb.linearVelocity = new Vector2(rollX, rb.linearVelocity.y);
                anim.SetTrigger("Roll");
            }
        }

        core.Tick(Time.deltaTime);
        anim.SetBool("isWalking", Mathf.Abs(input) > 0.1f);
    }

    void FixedUpdate()
    {
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