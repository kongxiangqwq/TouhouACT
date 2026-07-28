using UnityEngine;

public class PlayerClient : MonoBehaviour
{
    private PlayerCore core;
    private SpriteRenderer sr;
    private Animator anim;

    void Awake()
    {
        core = GetComponent<PlayerCore>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        float input = Input.GetAxisRaw("Horizontal");
        core.moveInput = input;

        if (Input.GetKeyDown(KeyCode.K))
        {
            float dir = sr.flipX ? -1f : 1f;
            if (core.TryStartRoll(dir))
            {
                anim.SetTrigger("Roll");
            }
        }

        core.Tick(Time.deltaTime);
        anim.SetBool("isWalking", Mathf.Abs(input) > 0.1f);
    }

    void FixedUpdate()
    {
        core.ApplyMovement();

        float input = core.moveInput;
        if (input > 0.1f)
            sr.flipX = false;
        else if (input < -0.1f)
            sr.flipX = true;
    }
}