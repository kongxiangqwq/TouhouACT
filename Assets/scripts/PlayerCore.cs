public class PlayerCore
{
    public float moveSpeed = 5f;
    public float rollSpeed = 20f;
    public float rollDuration = 0.15f;
    public float rollCooldown = 1f;

    public float moveInput;
    public bool CanMove { get; private set; } = true;
    public bool CanRoll { get; private set; } = true;
    public int FaceDirection { get; private set; } = 1;

    private float rollTimer;
    private float rollCooldownTimer;

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

    public float GetMovementVelocityX()
    {
        if (!CanMove) return 0f;
        return moveInput * moveSpeed;
    }

    public bool TryStartRoll(out float rollVelocityX)
    {
        rollVelocityX = 0f;
        if (!CanRoll || !CanMove) return false;

        CanMove = false;
        CanRoll = false;
        rollTimer = rollDuration;
        rollCooldownTimer = rollCooldown;
        rollVelocityX = FaceDirection * rollSpeed;
        return true;
    }

    public void SetFaceDirection(int direction)
    {
        FaceDirection = direction;
    }
}