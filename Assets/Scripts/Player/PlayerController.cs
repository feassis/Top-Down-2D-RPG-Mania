using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Singleton<PlayerController>
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float dashDistance = 4f;
    [SerializeField] private float dashCooldown = 1.5f;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Transform trail;
    [SerializeField] private Knockback knockback;

    private PlayerControls playerControl;
    private Vector2 movement;
    private bool facingLeft = false;
    private float moveSpeed;
    private bool isDashing = false;
    private bool isOnDashingCooldown = false;

    private List<IInteractable> interactables = new List<IInteractable>();

    public bool FacingLeft { get { return facingLeft; } set { facingLeft = value; } }

    protected override void Awake()
    {
        base.Awake();
        playerControl = new PlayerControls();
        moveSpeed = speed;
    }

    private void Start()
    {
        playerControl.Combat.Dash.performed += _ => Dash();
        playerControl.Actions.Interactable.performed += _ => TryInteract();
    }

    private void TryInteract()
    {
        if(interactables.Count <= 0)
        {
            return;
        }

        interactables[0].Interact();
        interactables.Remove(interactables[0]);
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
        if (isDashing)
        {
            Vector2 dashMoveDir = movement.normalized * dashDistance;
            Vector3 dashMovePosition =
                transform.position + new Vector3(dashMoveDir.x, dashMoveDir.y, 0);
            rb.MovePosition(dashMovePosition);
            isDashing = false;
            trail.gameObject.SetActive(false);
            return;
        }

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
        
        trail.gameObject.SetActive(true);

        StartCoroutine(ResetDash());
    }

    private IEnumerator ResetDash()
    {
        yield return new WaitForSeconds(dashCooldown);
        isOnDashingCooldown = false;
    }

    public void AddInteractable(IInteractable interactable)
    {
        interactables.Add(interactable);
    }

    public void RemoveInteractable(IInteractable interactable)
    {
        interactables.Remove(interactable);
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
