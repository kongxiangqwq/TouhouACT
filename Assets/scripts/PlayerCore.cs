using UnityEngine;

public class PlayerCore : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rollSpeed = 20f;
    public float rollDuration = 0.15f;
    public float rollCooldown = 1f;

    [HideInInspector] public float moveInput;

    public bool CanMove { get; private set; } = true;
    public bool CanRoll { get; private set; } = true;

    private float rollTimer;
    private float rollCooldownTimer;
    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Tick(float deltaTime)
    {
        if (!CanMove)
        {
            rollTimer -= deltaTime;
            if (rollTimer <= 0f) CanMove = true;
        }
        if (!CanRoll)
        {
            rollCooldownTimer -= deltaTime;
            if (rollCooldownTimer <= 0f) CanRoll = true;
        }
    }

    public void ApplyMovement()
    {
        if (!CanMove) return;
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
    }

    public bool TryStartRoll(float direction)
    {
        if (!CanRoll || !CanMove) return false;

        CanMove = false;
        CanRoll = false;
        rollTimer = rollDuration;
        rollCooldownTimer = rollCooldown;
        rb.linearVelocity = new Vector2(direction * rollSpeed, rb.linearVelocity.y);
        return true;
    }
}