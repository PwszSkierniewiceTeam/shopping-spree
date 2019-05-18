using Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacterController : MonoBehaviour
{
    [SerializeField]
    public float MovementSpeed = 60f;
    [SerializeField]
    public float JumpForce = 200f;
    [SerializeField]
    private Transform[] groundPoints;
    [SerializeField]
    private Transform ceilingPoint;
    [SerializeField]
    private LayerMask whatIsGround;
    [SerializeField]
    private Rigidbody2D playerRigidbody;
    [SerializeField]
    private Collider2D crouchDisableCollider;

    public bool CanMove { get; set; } = false;
    public bool CanJump { get; set; } = false;
    public bool CanDoubleJump { get; set; } = true;
    public bool CanBeAirControlled { get; set; } = true;
    public bool CanCrouch { get; set; } = false;
    private bool allowCharacterControll = false;
    
    private bool crouch = false;
    private float groundedRadius = .1f;
    private float ceilingRadius = .1f;
    private bool isGrounded = true;
    private bool wasGrounded = true;
    private int jumpCount = 0;
    private GamepadInput gamepadInput;
    private bool facingRight = true;
    private float horizontal;
    private bool jump = false;
        
    private void Update()
    {
        if (allowCharacterControll)
        { 
            horizontal = gamepadInput.GetJoystickAxis(GamepadJoystick.LeftJoystickHorizontal);


            if (gamepadInput.IsDown(GamepadButton.ButtonA))
            {
                jump = true;
            }

            if (gamepadInput.IsDown(GamepadButton.ButtonB))
            {
                crouch = true;
            }
            else if (gamepadInput.IsUp(GamepadButton.ButtonB))
            {
                crouch = false;
            }
        }
    }

    void FixedUpdate()
    {
        Move(horizontal, MovementSpeed, crouch);
    }

    public void SetInputSource(GamepadInput gamepadInput)
    {
        this.gamepadInput = gamepadInput;
        allowCharacterControll = true;
    }


    private void Move(float horizontal, float movementSpeed, bool crouch)
    {
        isGrounded = IsGrounded();
        if (isGrounded == true && wasGrounded == false)
        {
            jumpCount = 0;
        }

        if (!crouch)
        {
            crouch = HaveToCrouch();
        }
        if (crouch)
        {
            movementSpeed *= 0.5f;
            crouchDisableCollider.enabled = false;
        }
        else
        {
            crouchDisableCollider.enabled = true;
        }

        if (CanMove && (isGrounded || CanBeAirControlled))
        {
            if (horizontal > 0 && !facingRight || horizontal < 0 && facingRight)
            {
                Flip();
            }
            playerRigidbody.velocity = new Vector2(horizontal * movementSpeed * Time.fixedDeltaTime, playerRigidbody.velocity.y);
        }

        if (CanJump && jump && (isGrounded || CanDoubleJump == true && jumpCount < 2))
        {
            jump = false;
            jumpCount++;
            if (isGrounded == false)
            {
                wasGrounded = false;
            }
            playerRigidbody.AddForce(new Vector2(0, JumpForce));
            isGrounded = false;
        }
    }

    private void Flip()
    {
        facingRight = !facingRight;

        var scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    private bool IsGrounded()
    {
        if (playerRigidbody.velocity.y <= 0)
        {
            foreach (var point in groundPoints)
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(point.position, groundedRadius, whatIsGround);

                foreach (var collider in colliders)
                {
                    if (collider.gameObject != gameObject)
                    {
                        return true;
                    }
                }

            }
        }
        return false;
    }

    private bool HaveToCrouch()
    {
        if (Physics2D.OverlapCircle(ceilingPoint.position, ceilingRadius, whatIsGround))
        {
            return true;
        }
        return false;
    }
}
