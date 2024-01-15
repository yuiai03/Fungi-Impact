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
    public bool isDying;
    [SerializeField] private float countRecoveryDashTime = 0;

    [SerializeField] private Vector2 moveDirection;
    [SerializeField] private Vector2 lastDirection;
    [SerializeField] private Vector2 attackDirection;

    private Animator animator;
    private Rigidbody2D rb2d;
    private FungusHealth fungusHealth;
    private FungusStamina fungusStamina;
    private FungusAttack fungusAttack;
    private AnimationEvent animationEvent;

    private FungusInfoReader fungusInfo;
    private CameraCollider cameraCollider => CameraCollider.instance;
    private GameplayManager gameplayController => GameplayManager.Instance;

    private Coroutine fungusDieCoroutine;
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
        fungusDieCoroutine = StartCoroutine(FungusDieCoroutine());
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

        SetAnimEvent();
    }
    private void Update()
    {
        UpdateInput();
        UpdateAnimation();
        UpdateRecoveryDashTime();

        CheckState();
    }
    void UpdateInput()
    {
        if (isDying)
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
    void CheckState()
    {
        if (fungusStamina == null) return;

        gameplayController.UpdateStaminaState();
        gameplayController.UpdateStaminaBar();
        gameplayController.UpdateStaminaBarState();
    }
    void SetAnimEvent()
    {
        animationEvent.OnStartAnimEvent.AddListener(() => InteractSlotState(false));
        animationEvent.OnStartAnimEvent.AddListener(() => AttackingState(true));

        animationEvent.OnActionAnimEvent.AddListener(() => InteractSlotState(true));
        animationEvent.OnActionAnimEvent.AddListener(() => AttackingState(false));
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
            isDashing = true;
            countRecoveryDashTime = 0;
            if (moveDirection == Vector2.zero)
            {
                if(lastDirection.x == 0 && lastDirection.y == 0) lastDirection.y = -1;

                SetMoveDirection(new Vector2(lastDirection.x, lastDirection.y));
            }
            rb2d.velocity = moveDirection.normalized * fungusInfo.FungusData.dashForce;

            gameplayController.ConsumeStamina(fungusInfo.FungusData.dashStamina);
            StartCoroutine(StopDash());
        }

    }
    IEnumerator StopDash()
    {
        yield return new WaitForSeconds(fungusInfo.FungusData.dashTime);
        isDashing = false;
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
    public Vector3 SetDirectionAttackWithOutTarget()
    {
        Vector3 direction;
        if (lastDirection.x == 0 && lastDirection.y == 0) direction = new Vector2(0, -1);

        else if (moveDirection.x == 0 && moveDirection.y == 0)
            direction = new Vector2(lastDirection.x, lastDirection.y);

        else direction = moveDirection;

        return direction;
    }

    void SetMoveDirection(Vector2 direction) => moveDirection = direction;
    void SetLastDirection(Vector2 direction) => lastDirection = direction;
    void SetAttackDirection(Vector2 direction) => attackDirection = direction;
    bool CanMove() => !isDashing;
    bool CanAttack() => !isAttacking && !isDashing;
    bool CanDash()
    {
        return gameplayController.GetCurrentStamina() >= fungusInfo.FungusData.dashStamina && 
            !isDashing && !isAttacking && countRecoveryDashTime >= PlayerConfig.dashRecoveryWaitTime;
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
        gameplayController.InteractSlotState(state);
    }
    public void AttackingState(bool state)
    {
        isAttacking = state;
    }

    public IEnumerator FungusDieCoroutine()
    {
        DyingFungusState();

        int fungusAliveIndex = gameplayController.GetFungusAliveIndex();
        if (fungusAliveIndex >= 0)
        {
            yield return new WaitForSeconds(PlayerConfig.dyingWaitTime);
            ResetFungusState();

            gameplayController.SwitchFungus(fungusAliveIndex);
        }
        else
        {
            Debug.Log("Game over");
        }
    }
    void DyingFungusState()
    {
        isDying = true;
        fungusInfo.FungusData.health = 0;
        moveDirection = Vector2.zero;
        rb2d.velocity = moveDirection;
    }
    void ResetFungusState()
    {
        isDying = false;
        isAttacking = false;
        isDashing = false;
    }
    public FungusHealth GetFungusHealth()
    {
        return fungusHealth;
    }
    void UpdateRecoveryDashTime()
    {
        if(!isDashing && countRecoveryDashTime < PlayerConfig.dashRecoveryWaitTime)
        countRecoveryDashTime += Time.deltaTime;
    }
}
