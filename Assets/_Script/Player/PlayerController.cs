using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public bool isAttacking;
    public bool isDashing;
    public bool isDying;

    [SerializeField] private Vector2 moveDirection;
    [SerializeField] private Vector2 lastDirection;
    [SerializeField] private Vector2 attackDirection;

    private Animator animator;
    private Rigidbody2D rb2d;
    private PlayerStamina playerStamina;
    private PlayerAttack playerAttack;
    private AnimationEvent animationEvent;

    private PlayerInfoReader playerInfo;
    private CameraCollider cameraCollider => CameraCollider.instance;
    private GameplayController gameplayController => GameplayController.instance;

    private Coroutine fungusDieCoroutine;
    private void Awake()
    {
        EventManager.onFungusDie += OnFungusDie;
    }
    private void OnDestroy()
    {
        EventManager.onFungusDie += OnFungusDie;
    }
    void OnFungusDie()
    {
        fungusDieCoroutine = StartCoroutine(FungusDieCoroutine());
    }
    private void Start()
    {
        animationEvent = GetComponentInChildren<AnimationEvent>();
        animator = GetComponentInChildren<Animator>();
        playerStamina = GetComponent<PlayerStamina>();
        playerAttack = GetComponent<PlayerAttack>();
        playerInfo = GetComponent<PlayerInfoReader>();
        rb2d = GetComponent<Rigidbody2D>();

        SetAnimEvent();

    }
    private void Update()
    {
        UpdateInput();
        UpdateAnimation();

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
        Debug.Log("Inputing");
        Move();
        Dash();
        Attack();
    }
    void CheckState()
    {
        playerStamina.UpdateStaminaState();
        playerStamina.UpdateStaminaBar();
        playerStamina.UpdateStaminaBarState();
    }
    void SetAnimEvent()
    {
        animationEvent.OnStartAnimEvent.AddListener(() => InteractSlotState(false));
        animationEvent.OnStartAnimEvent.AddListener(() => AttackingState(true));

        animationEvent.OnActionAnimEvent.AddListener(() => InteractSlotState(true));
        animationEvent.OnActionAnimEvent.AddListener(() => AttackingState(false));
        animationEvent.OnActionAnimEvent.AddListener(() => playerAttack.AttackAction());
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

            rb2d.velocity = new Vector2(pressMoveX * playerInfo.PlayerData.moveSpeed, pressMoveY * playerInfo.PlayerData.moveSpeed);
            SetMoveDirection(new Vector2(pressMoveX, pressMoveY).normalized);
        }
    }
    void Dash()
    {
        bool pressDash = Input.GetMouseButtonDown(1);
        if (pressDash && CanDash())
        {
            isDashing = true;
            if (moveDirection == Vector2.zero)
            {
                if(lastDirection.x == 0 && lastDirection.y == 0) lastDirection.y = -1;

                SetMoveDirection(new Vector2(lastDirection.x, lastDirection.y));
            }
            rb2d.velocity = moveDirection.normalized * playerInfo.PlayerData.dashForce;

            playerStamina.ConsumeStamina(playerInfo.PlayerData.dashStamina);
            StartCoroutine(StopDash());
        }

    }
    IEnumerator StopDash()
    {
        yield return new WaitForSeconds(playerInfo.PlayerData.dashTime);
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
    bool CanDash() => playerStamina.stamina >= playerInfo.PlayerData.dashStamina && !isDashing && !isAttacking;
    
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
        playerInfo.PlayerData.health = 0;
        moveDirection = Vector2.zero;
        rb2d.velocity = moveDirection;
        Debug.Log("Dying");
    }
    void ResetFungusState()
    {
        isDying = false;
        isAttacking = false;
        isDashing = false;

        Debug.Log("Alive");
    }
}
