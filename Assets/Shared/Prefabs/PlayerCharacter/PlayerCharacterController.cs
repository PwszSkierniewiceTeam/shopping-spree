using Core;
using System;
using Shared.Prefabs.PlayerCharacter;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerCharacterController : MonoBehaviour
{
    [SerializeField]
    public float MovementSpeed = 60f;
    [SerializeField]
    public float JumpForce = 200f;
    [SerializeField]
    private Transform groundPoint;
    [SerializeField]
    private Transform ceilingPoint;
    [SerializeField]
    private LayerMask whatIsGround;
    [SerializeField]
    private Rigidbody2D playerRigidbody;
    [SerializeField]
    private Collider2D crouchDisableCollider;
    [SerializeField]
    private float throwForce = 1500f;

    public bool CanMove { get; set; } = false;
    public bool CanJump { get; set; } = false;
    public bool CanDoubleJump { get; set; } = true;
    public bool CanBeAirControlled { get; set; } = true;
    public bool CanCrouch { get; set; } = false;
    public bool CanThrowStuff { get; set; } = false;
    public bool CanThrowMoreThanOneThing { get; set; } = false;
    public bool DestroyThrowableAfterItStops { get; set; } = true;
    public bool IsThrowPowerFromButtonHold { get; set; } = true;

    private bool allowCharacterControll = false;
    private bool crouch = false;
    private float groundedRadius = .1f;
    private float ceilingRadius = .1f;
    public bool isGrounded { get; private set; } = true;
    private bool wasGrounded = true;
    private int jumpCount = 0;
    private GamepadInput gamepadInput;
    private bool facingRight = true;
    private float horizontal;
    public bool jump { get; private set; } = false;

    private Vector3 direction = new Vector3();
    private bool throwObject = false;
    private float power;
    public GameObject ThrowableObject { get; set; }
    public List<GameObject> Throwables { get; private set; } = new List<GameObject>();
    private void Update()
    {
        if (DestroyThrowableAfterItStops && Throwables.Any())
        {
            var thr = Throwables.Where(t => !t.GetComponent<Rigidbody2D>().IsAwake()).ToList();
            thr.ForEach(t =>
            {
                Throwables.Remove(t);
                Destroy(t);
            });
        }

        if (allowCharacterControll)

        {
            horizontal = gamepadInput.GetJoystickAxis(GamepadJoystick.LeftJoystickHorizontal);



            if (gamepadInput.IsDown(GamepadButton.ButtonX))
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
            direction.Set(gamepadInput.GetJoystickAxis(GamepadJoystick.RightJoystickHorizontal), gamepadInput.GetJoystickAxis(GamepadJoystick.RightJoystickVertical), 0);

            if (gamepadInput.IsDown(GamepadButton.RBumper))
            {
                if (IsThrowPowerFromButtonHold)
                {
                    StartGatheringPower();
                }
                else
                {
                    power = throwForce;
                    throwObject = true;
                }
            }

            if (gamepadInput.IsPressed(GamepadButton.RBumper))
            {
                if (IsThrowPowerFromButtonHold)
                {
                    ContinueGatheringPower();
                }
            }

            if (gamepadInput.IsUp(GamepadButton.RBumper))
            {
                throwObject = true;
            }
        }
    }
    private void ContinueGatheringPower()
    {
        power += 10;
    }

    private void StartGatheringPower()
    {
        power = 1000;
    }
    private void launchObject()
    {

        if (!CanThrowMoreThanOneThing && Throwables.Any())
        {
            return;
        }
        var offset = new Vector3(0.1f, 0, 0);
        if (!facingRight)
        {
            offset.x = -0.1f;
        }
        var throwable = Instantiate(ThrowableObject, transform.position + offset, Quaternion.identity);
        Throwables.Add(throwable);
        throwable.GetComponent<Rigidbody2D>().AddForce(direction * power);
        power = default;
    }

    void FixedUpdate()
    {
        Move(horizontal, MovementSpeed, crouch);
        if (CanThrowStuff && throwObject && ThrowableObject != null)
        {
            launchObject();
            throwObject = false;
        }
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
            Collider2D[] colliders = Physics2D.OverlapCircleAll(groundPoint.position, groundedRadius, whatIsGround);

            foreach (var collider in colliders)
            {
                if (collider.gameObject != gameObject)
                {
                    return true;
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
