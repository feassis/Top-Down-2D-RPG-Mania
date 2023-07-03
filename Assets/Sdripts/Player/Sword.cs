using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private ActiveWeapon activeWeapon;
    [SerializeField] private Transform slashAnimSpawn;
    [SerializeField] private SelfDestroy slashPrefab;
    [SerializeField] private Transform weaponCollider;
    [SerializeField] private float attackCooldown = 1f; 

    private bool attackButtonDown = false;
    private bool isAttacking = false;

    private PlayerControls playerControls;

    private SelfDestroy slashAnim;
    
    private void Awake()
    {
        playerControls = new PlayerControls();
        weaponCollider.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    private void Start()
    {
        playerControls.Combat.Attack.started += _ => Attack();
    }

    private void Update()
    {
        MouseFollowWithOffset();
        //Attack();
    }

    public void SwingUpFlipAnimation()
    {
        slashAnim.gameObject.transform.rotation = Quaternion.Euler(new Vector3(180, 0, 0));
    }

    private void StartAttacking()
    {
        attackButtonDown = true;
    }

    private void StopAttacking()
    {
        attackButtonDown = false;
    }

    private void Attack()
    {
        if (isAttacking)
        {
            return;
        }
        isAttacking = true;

        animator.SetTrigger("attack");

        if(slashAnim != null)
        {
            slashAnim.DestrotSelf();
        }

        weaponCollider.gameObject.SetActive(true);

        slashAnim = Instantiate(slashPrefab, slashAnimSpawn.position, Quaternion.identity);
        slashAnim.transform.parent = transform.parent;
        slashAnim.gameObject.GetComponent<SpriteRenderer>().flipX = playerController.FacingLeft;
        StartCoroutine(ResetAttackCooldown());
    }

    private IEnumerator ResetAttackCooldown()
    {
        yield return new WaitForSeconds(attackCooldown);
        isAttacking = false;
    }

    public void DoneAttacking()
    {
        weaponCollider.gameObject.SetActive(false);
    }

    private void MouseFollowWithOffset()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 playerScreenPoint = Camera.main.WorldToScreenPoint(playerController.transform.position);

        float angle = Mathf.Atan2(mousePos.y, mousePos.x)*Mathf.Rad2Deg;

        if(mousePos.x < playerScreenPoint.x)
        {
            activeWeapon.transform.rotation = Quaternion.Euler(new Vector3(0, -180, angle));
            weaponCollider.transform.rotation = Quaternion.Euler(new Vector3(0, -180, angle));
        }
        else
        {
            activeWeapon.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
            weaponCollider.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }
    }
}
