public class PlayerCore
{
    // 移动
    public float moveSpeed = 5f;
    private float moveInput;

    // 冲刺
    public float dashSpeed = 20f;
    public float dashDuration = 0.15f;
    public float dashCooldown = 1f;
    public float secondDashWindow = 2f;

    // 跳跃
    public float jumpForce = 10f;
    public int maxJumps = 2;

    // 状态
    public bool CanMove { get; private set; } = true;
    public bool CanDash { get; private set; } = true;
    public int FaceDirection { get; private set; } = 1;
    public bool IsJumping { get; private set; }

    // 冲刺期间无视重力（不再额外延长）
    public bool IgnoreGravity => !CanMove;

    // 冲刺内部
    private float dashTimer;
    private float dashCooldownTimer;
    private int dashCount;
    private float secondDashWindowTimer;

    // 跳跃内部
    private int jumpCount;
    private bool jumpRequested;
    private bool isGrounded;

    // 输入接口
    public void SetMoveInput(float input) => moveInput = input;
    public float MoveInput => moveInput;

    public void RequestJump() => jumpRequested = true;

    public void SetGrounded(bool grounded)
    {
        isGrounded = grounded;
        if (grounded)
        {
            jumpCount = 0;
            IsJumping = false;
        }
    }

    public void Tick(float deltaTime)
    {
        // 冲刺位移计时
        if (!CanMove)
        {
            dashTimer -= deltaTime;
            if (dashTimer <= 0f)
                CanMove = true;
        }
        else
        {
            // 第一次冲刺后窗口倒计时（允许第二次冲刺）
            if (dashCount == 1)
            {
                if (secondDashWindowTimer <= 0f)
                    secondDashWindowTimer = secondDashWindow;

                secondDashWindowTimer -= deltaTime;
                if (secondDashWindowTimer <= 0f)
                    EnterDashCooldown();
            }
        }

        // 冲刺冷却计时
        if (!CanDash)
        {
            dashCooldownTimer -= deltaTime;
            if (dashCooldownTimer <= 0f)
            {
                CanDash = true;
                dashCount = 0;
            }
        }
    }

    public float GetMovementVelocityX()
    {
        if (!CanMove) return 0f;
        return moveInput * moveSpeed;
    }

    public bool TryStartDash(out float dashVelocityX)
    {
        dashVelocityX = 0f;
        if (!CanDash || !CanMove || dashCount >= 2)
            return false;

        CanMove = false;
        dashTimer = dashDuration;
        dashVelocityX = FaceDirection * dashSpeed;

        if (dashCount == 0)
        {
            dashCount = 1;
            secondDashWindowTimer = 0f;
        }
        else if (dashCount == 1)
        {
            dashCount = 2;
            EnterDashCooldown();
        }

        return true;
    }

    public bool TryJump(out float jumpVelocityY)
    {
        jumpVelocityY = 0f;
        if (!jumpRequested) return false;
        jumpRequested = false;
        if (jumpCount >= maxJumps) return false;

        jumpCount++;
        IsJumping = true;
        jumpVelocityY = jumpForce;
        return true;
    }

    public void SetFaceDirection(int direction) => FaceDirection = direction;

    private void EnterDashCooldown()
    {
        CanDash = false;
        dashCooldownTimer = dashCooldown;
        secondDashWindowTimer = 0f;
    }
}