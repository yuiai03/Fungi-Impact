using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using UnityEngine;
using static EventManager;

public class FungusController : MonoBehaviour
{
    public bool isAttacking;
    public bool isDashing;
    public bool isDied;
    public bool isDying;
    public bool isDashCoolingDown;

    public Vector2 MoveDirection { get => moveDirection; }
    private Vector2 moveDirection;
    public Vector2 LastDirection { get => lastDirection; }
    private Vector2 lastDirection;
    public Vector2 AttackDirection { get => attackDirection; }
    private Vector2 attackDirection;

    private Animator animator;
    private Rigidbody2D rb2d;
    private FungusHealth fungusHealth;
    private FungusStamina fungusStamina;
    private FungusAttack fungusAttack;
    private AnimationEvent animationEvent;
    private CapsuleCollider2D capsuleCollider2D;

    private FungusInfoReader fungusInfo;
    private CameraCollider cameraCollider => CameraCollider.instance;
    private GameplayManager gameplayController => GameplayManager.Instance;
    private FungusManager playerManager => FungusManager.Instance;

    private Coroutine fungusDieCoroutine;
    private Coroutine stopDashCoroutine;
    private Coroutine dashCoolDownCoroutine;
    private void Awake()
    {
        EventManager.onFungusDie += OnFungusDie;
        EventManager.onSwitchFungus += OnSwitchFungus;

    }
    private void OnDestroy()
    {
        EventManager.onFungusDie += OnFungusDie;
        EventManager.onSwitchFungus -= OnSwitchFungus;

    }
    void OnFungusDie()
    {
        if (gameObject.activeSelf)
        {
            if (fungusDieCoroutine != null) StopCoroutine(fungusDieCoroutine);
            fungusDieCoroutine = StartCoroutine(FungusDieCoroutine());
        }
    }
    private void Start()
    {
        animationEvent = GetComponentInChildren<AnimationEvent>();
        animator = GetComponentInChildren<Animator>();
        fungusStamina = GetComponent<FungusStamina>();
        fungusAttack = GetComponent<FungusAttack>();
        fungusHealth = GetComponent<FungusHealth>();
        fungusInfo = GetComponent<FungusInfoReader>();
        rb2d = GetComponent<Rigidbody2D>();
        capsuleCollider2D = GetComponent<CapsuleCollider2D>();

        SetAnimEvent();
    }
    private void Update()
    {
        UpdateInput();
        UpdateAnimation();
    }
    void UpdateInput()
    {
        if (isDying || isDied)
        {
            return;
        }
        if (isAttacking)
        {
            rb2d.velocity = new Vector2(moveDirection.x, moveDirection.y);
            return;
        }
        Move();
        Dash();
        Attack();
    }
    void SetAnimEvent()
    {
        animationEvent.OnStartAnimEvent.AddListener(() => InteractSlotState(false));
        animationEvent.OnStartAnimEvent.AddListener(() => AttackingState(true));

        animationEvent.OnActionAnimEvent.AddListener(() => AttackingState(false));
        animationEvent.OnActionAnimEvent.AddListener(() => InteractSlotState(true));
        animationEvent.OnActionAnimEvent.AddListener(() => fungusAttack.AttackAction());
    }
    void UpdateAnimation()
    {

        animator.SetFloat("MoveX", moveDirection.x);
        animator.SetFloat("MoveY", moveDirection.y);

        animator.SetFloat("LastMoveX", lastDirection.x);
        animator.SetFloat("LastMoveY", lastDirection.y);

        animator.SetFloat("AttackX", attackDirection.x);
        animator.SetFloat("AttackY", attackDirection.y);

        animator.SetFloat("MoveSpeed", moveDirection.magnitude);
        animator.SetBool("Move", moveDirection != Vector2.zero);


    }
    void Move()
    {
        if (CanMove())
        {
            float pressMoveX = Input.GetAxisRaw("Horizontal");
            float pressMoveY = Input.GetAxisRaw("Vertical");
            if ((pressMoveX == 0 && pressMoveY == 0) && (moveDirection.x != 0 || moveDirection.y != 0))
            {
                SetLastDirection(moveDirection);
            }

            rb2d.velocity = new Vector2(pressMoveX * fungusInfo.FungusData.moveSpeed, pressMoveY * fungusInfo.FungusData.moveSpeed);
            SetMoveDirection(new Vector2(pressMoveX, pressMoveY).normalized);
        }
    }
    void Dash()
    {
        bool pressDash = Input.GetMouseButtonDown(1);
        if (pressDash && CanDash())
        {
            if (moveDirection == Vector2.zero)
            {
                if(lastDirection.x == 0 && lastDirection.y == 0) lastDirection.y = -1;

                SetMoveDirection(new Vector2(lastDirection.x, lastDirection.y));
            }
            rb2d.velocity = moveDirection.normalized * fungusInfo.FungusData.dashForce;

            playerManager.ConsumeStamina(fungusInfo.FungusData.dashStamina);

            if (stopDashCoroutine != null) StopCoroutine(stopDashCoroutine);
            stopDashCoroutine = StartCoroutine(StopDashCoroutine());
        }

    }
    IEnumerator StopDashCoroutine()
    {
        isDashing = true;
        InteractSlotState(false);
        yield return new WaitForSeconds(fungusInfo.FungusData.dashTime);
        isDashing = false;
        InteractSlotState(true);
        playerManager.UpdateStamina();

        if (dashCoolDownCoroutine != null) StopCoroutine(dashCoolDownCoroutine);
        dashCoolDownCoroutine = StartCoroutine(DashCoolDownCoroutine());
    }

    IEnumerator DashCoolDownCoroutine()
    {
        isDashCoolingDown = true;
        yield return new WaitForSeconds(PlayerConfig.dashCoolDown);
        isDashCoolingDown = false;
    }
    public void Attack()
    {
        bool pressAttack = Input.GetMouseButtonDown(0);
        if (pressAttack && CanAttack())
        {
            if (cameraCollider.CheckTargetDetection() != null)
            {
                Vector3 direction;
                direction = (cameraCollider.GetTargetTransform().position - transform.position).normalized;
                SetAttackDirection(direction);
            }
            else
            {
                if (moveDirection.x == 0 && moveDirection.y == 0)
                {
                    SetAttackDirection(lastDirection);
                }
                else if(moveDirection.x !=0 || moveDirection.y != 0)
                {
                    SetAttackDirection(moveDirection);
                    SetLastDirection(moveDirection);
                }
            }
            animator.SetTrigger("Attack");
        }
    }
    public Vector3 DirectionAttackWithOutTarget()
    {
        Vector3 direction;
        if (lastDirection.x == 0 && lastDirection.y == 0) direction = new Vector2(0, -1);

        else if (moveDirection.x == 0 && moveDirection.y == 0)
            direction = new Vector2(lastDirection.x, lastDirection.y);

        else direction = moveDirection;

        return direction;
    }

    public void SetMoveDirection(Vector2 direction) => moveDirection = direction;
    public void SetLastDirection(Vector2 direction) => lastDirection = direction;
    public void SetAttackDirection(Vector2 direction) => attackDirection = direction;
    bool CanMove() => !isDashing;
    bool CanAttack() => !isAttacking && !isDashing;
    bool CanDash()
    {
        return  StaminaEnough() && !isDashing && !isAttacking && !isDashCoolingDown;
    }
    bool StaminaEnough()
    {
        return playerManager.GetCurrentStamina() >= fungusInfo.FungusData.dashStamina;
    }
    void OnSwitchFungus(FungusInfoReader oldFungusInfo, FungusInfoReader newFungusInfo, FungusCurrentStatusHUD fungusCurrentStatusHUD)
    {
        GetNewPosition(oldFungusInfo.transform.position);
    }

    void GetNewPosition(Vector2 newPos)
    {
        this.transform.position = newPos;
    }
    public void InteractSlotState(bool state)
    {
        if (playerManager.IsRecoveringInteractSlot()) return;
            
        playerManager.InteractSlotState(state);
    }
    public void AttackingState(bool state) => isAttacking = state;
    public IEnumerator FungusDieCoroutine()
    {
        DyingState(true);
        DyingFungusState();

        int fungusAliveIndex = playerManager.GetFungusAliveIndex();
        if (fungusAliveIndex >= 0)
        {
            yield return new WaitForSeconds(PlayerConfig.dyingWaitTime);

            DyingState(false);

            DieState(true);

            playerManager.SwitchFungus(fungusAliveIndex);
        }
        else
        {
            Debug.Log("Game over");
        }
    }
    void DyingState(bool state) => isDying = state;
    void DyingFungusState()
    {
       
        SetMoveDirection(Vector2.zero);
        rb2d.velocity = moveDirection;
        
        capsuleCollider2D.enabled = false;
        
        animator.SetBool("Die", isDying);

    }
    void ResetFungusState()
    {
        isDying = false;
        isDashing = false;
        capsuleCollider2D.enabled = true;

        DieState(false);
    }
    public FungusHealth GetFungusHealth()
    {
        return fungusHealth;
    }
    public void DieState(bool state)
    {
        isDashing = true;
    }
    public bool IsDying()
    {
        return isDying;
    }
    public bool IsAttacking()
    {
        return isAttacking;
    }
}
