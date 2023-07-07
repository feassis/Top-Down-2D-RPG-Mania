using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float dashSpeed = 4f;
    [SerializeField] private float dashDuration = 0.5f;
    [SerializeField] private float dashCooldown = 1.5f;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Transform trail;
    [SerializeField] private Knockback knockback;

    public static PlayerController Instance;

    private PlayerControls playerControl;
    private Vector2 movement;
    private bool facingLeft = false;
    private float moveSpeed;
    private bool isDashing = false;
    private bool isOnDashingCooldown = false;

    public bool FacingLeft { get { return facingLeft; } set { facingLeft = value; } }

    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        playerControl = new PlayerControls();
        moveSpeed = speed;
    }

    private void OnDestroy()
    {
        if(Instance == this)
        {
            Instance = null;
        }
    }

    private void Start()
    {
        playerControl.Combat.Dash.performed += _ => Dash();
    }

    private void OnEnable()
    {
        playerControl.Enable();
    }

    private void OnDisable()
    {
        playerControl.Disable();
    }

    private void PlayerInput()
    {
        movement = playerControl.Movement.Move.ReadValue<Vector2>();

        animator.SetFloat("moveX", movement.x);
        animator.SetFloat("moveY", movement.y);
    }

    private void Update()
    {
        PlayerInput();
    }

    private void FixedUpdate()
    {
        Move();
        AdjustPlayerFacingDirection();
    }

    public void KnockbackPlayer(Vector3 DamageSource, float knockbackForce)
    {
        knockback.GetknockedBack(DamageSource, knockbackForce);
    }

    private void Move()
    {
        if (knockback.GettingKnockedBack)
        {
            return;
        }

        Vector2 moveDir = movement.normalized * moveSpeed * Time.fixedDeltaTime;
        Vector3 movePosition = 
            transform.position + new Vector3(moveDir.x, moveDir.y, 0);
        rb.MovePosition(movePosition);
    }

    private void Dash()
    {
        if (isDashing || isOnDashingCooldown)
        {
            return;
        }

        isDashing = true;
        isOnDashingCooldown = true;
        moveSpeed *= dashSpeed;
        trail.gameObject.SetActive(true);

        StartCoroutine(ResetDash());
    }

    private IEnumerator ResetDash()
    {
        yield return new WaitForSeconds(dashDuration);
        moveSpeed = speed;
        isDashing = false;
        trail.gameObject.SetActive(false);
        yield return new WaitForSeconds(dashCooldown - dashDuration);
        isOnDashingCooldown = false;
    }



    private void AdjustPlayerFacingDirection()
    {
        var mousePos = Input.mousePosition;
        var playerPosOnCamera = Camera.main.WorldToScreenPoint(transform.position);
        if (playerPosOnCamera.x > mousePos.x)
        {
            spriteRenderer.flipX = true;
            FacingLeft = true;
        }
        else
        {
            spriteRenderer.flipX = false;
            FacingLeft = false;
        }
    }
}
