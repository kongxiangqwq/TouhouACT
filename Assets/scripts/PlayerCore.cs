public class PlayerCore
{
    
    public float moveSpeed = 5f;
    private float moveInput;

    
    public float dashSpeed = 20f;
    public float dashDuration = 0.15f;
    public float dashCooldown = 1f;
    public float secondDashWindow = 2f;

    
    public float jumpForce = 10f;
    public int maxJumps = 2;

    
    public bool CanMove { get; private set; } = true;
    public bool CanDash { get; private set; } = true;
    public int FaceDirection { get; private set; } = 1;
    public bool IsJumping { get; private set; }

    
    private float dashTimer;
    private float dashCooldownTimer;
    private int dashCount;
    private float secondDashWindowTimer;

    
    private int jumpCount;
    private bool jumpRequested;
    private bool isGrounded;

    
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
        
        if (!CanMove)
        {
            dashTimer -= deltaTime;
            if (dashTimer <= 0f)
                CanMove = true;
        }
        else
        {
            
            if (dashCount == 1)
            {
                if (secondDashWindowTimer <= 0f)
                    secondDashWindowTimer = secondDashWindow;

                secondDashWindowTimer -= deltaTime;
                if (secondDashWindowTimer <= 0f)
                    EnterDashCooldown();
            }
        }

        
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