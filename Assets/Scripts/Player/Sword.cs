using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Animator playerAnimetor;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private ActiveWeapon activeWeapon;
    [SerializeField] private Transform slashAnimSpawn;
    [SerializeField] private SelfDestroy slashPrefab;
    [SerializeField] private SelfDestroy chargedSlashPrefab;
    [SerializeField] private Transform weaponCollider;
    [SerializeField] private Transform chargedWeaponCollider;
    [SerializeField] private Transform chargeParticleSpawnPoint;
    [SerializeField] private GameObject chargingParticle;
    [SerializeField] private GameObject chargedParticle;
    [SerializeField] private float attackCooldown = 1f;
    [SerializeField] private float chargeAttackTime = 2f;
    [SerializeField] private List<AudioSource> swingSounds;
    [SerializeField] private List<AudioSource> chargedSwingSounds;
    [SerializeField] private AudioSource chargingUpSound;
    [SerializeField] private GameObject weaponVisuals;

    private bool attackButtonDown = false;
    private bool isAttacking = false;
    private bool isAttackCharged = false;
    private bool isChargeAttacking = false;
    private GameObject chargedParticlesInstance;

    private const string swingDownAniState = "SwingDown";
    private const string swingUpAniState = "SwingUp";
    private const string chargedSwingDownAniState = "ChargedSwingDown";
    private const string chargedSwingUpAniState = "ChargedSwingUp";
    private const string attackAniState = "Attack";

    private PlayerControls playerControls;

    private SelfDestroy slashAnim;

    private IEnumerator chargeRoutine;
    private IEnumerator resetChargeRoutine;

    private void Awake()
    {
        playerControls = new PlayerControls();
        weaponCollider.gameObject.SetActive(false);
        chargedWeaponCollider.gameObject.SetActive(false);
        weaponVisuals.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    private void PlayRandomSwing()
    {
        var randomIndex = UnityEngine.Random.Range(0, swingSounds.Count);

        swingSounds[randomIndex].Play();
    }

    private void PlayRandomChargedSwing()
    {
        var randomIndex = UnityEngine.Random.Range(0, chargedSwingSounds.Count);

        chargedSwingSounds[randomIndex].Play();
    }

    private void Start()
    {
        playerControls.Combat.Attack.started += _ => Attack();
        playerControls.Combat.Attack.canceled += _ => Released();
    }

    private IEnumerator ResetEveryThingAfter2Seconds()
    {
        yield return new WaitForSeconds(3.5f);
        ChargeAttackDone();
    }

    private void Released()
    {
        if(resetChargeRoutine != null)
        {
            StopCoroutine(resetChargeRoutine);
        }
        
        resetChargeRoutine = ResetEveryThingAfter2Seconds();
        StartCoroutine(resetChargeRoutine);
        chargedWeaponCollider.gameObject.SetActive(false);
        chargingUpSound.Stop();
        if (chargedParticlesInstance)
        {
            Destroy(chargedParticlesInstance);
        }

        if (chargeRoutine != null)
        {
            StopCoroutine(chargeRoutine);
        }

        if (isChargeAttacking)
        {
            return;
        }

        if (!isAttackCharged)
        {
            weaponVisuals.gameObject.SetActive(false);
            return;
        }
        isAttackCharged = false;

        ExecuteChargeAttack();
    }

    private void Update()
    {
        MouseFollowWithOffset();
        //Attack();
    }

    public void SwingUpFlipAnimation()
    {
        if(slashAnim == null)
        {
            return;
        }
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

    private void ChargeAttackDone()
    {
        chargedWeaponCollider.gameObject.SetActive(false);
        isChargeAttacking = false;
        weaponVisuals.gameObject.SetActive(false);
    }

    private void ExecuteChargeAttack()
    {
        playerAnimetor.Play(attackAniState);
        PlayRandomChargedSwing();
        chargedWeaponCollider.gameObject.SetActive(true);
        isChargeAttacking = true;
        animator.SetTrigger("chargeAttack");
        slashAnim = Instantiate(chargedSlashPrefab, slashAnimSpawn.position, Quaternion.identity);
        slashAnim.transform.parent = transform.parent;
        slashAnim.gameObject.GetComponent<SpriteRenderer>().flipX = playerController.FacingLeft;
    }

    private void Attack()
    {
        weaponVisuals.gameObject.SetActive(true);
        if (isAttacking || isChargeAttacking)
        {
            return;
        }
        isAttacking = true;
        chargedWeaponCollider.gameObject.SetActive(false);

        animator.SetTrigger("attack");

        if(slashAnim != null)
        {
            slashAnim.DestrotSelf();
        }

        weaponCollider.gameObject.SetActive(true);

        slashAnim = Instantiate(slashPrefab, slashAnimSpawn.position, Quaternion.identity);
        slashAnim.transform.parent = transform.parent;
        slashAnim.gameObject.GetComponent<SpriteRenderer>().flipX = playerController.FacingLeft;
        playerAnimetor.Play(attackAniState);
        PlayRandomSwing();
        StartCoroutine(ResetAttackCooldown());

        if(chargeRoutine != null)
        {
            StopCoroutine(chargeRoutine);
        }

        chargeRoutine = ChargingRoutine();

        StartCoroutine(chargeRoutine);  
    }

    private IEnumerator ChargingRoutine()
    {
        chargingUpSound.Play();
        yield return new WaitForSeconds(chargeAttackTime / 3);
        Instantiate(chargingParticle, chargeParticleSpawnPoint.position, Quaternion.identity); 
        yield return new WaitForSeconds(chargeAttackTime / 3);
        Instantiate(chargingParticle, chargeParticleSpawnPoint.position, Quaternion.identity);
        yield return new WaitForSeconds(chargeAttackTime / 3);
        chargedParticlesInstance = Instantiate(chargedParticle, chargeParticleSpawnPoint.position, Quaternion.identity);
        chargedParticlesInstance.transform.parent = transform;
        isAttackCharged = true; 
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
            chargedWeaponCollider.transform.rotation = Quaternion.Euler(new Vector3(0, -180, angle));
        }
        else
        {
            activeWeapon.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
            weaponCollider.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
            chargedWeaponCollider.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }
    }
}
