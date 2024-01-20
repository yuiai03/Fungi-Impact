using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.VisualScripting;
using UnityEngine;
using static EventManager;

public class FungusController : MonoBehaviour
{

    #region Variables

    public bool IsDashCoolingDown { get; set; }
    public bool IsDied { get; set; }
    public bool IsDashing { get; set; }

    public bool IsPressDash { get; set; }

    public bool IsUsingSkill { get; set; }
    public bool IsPressNA;
    public bool IsNA_ing { get; set; }
    public bool CanNA => !IsNA_ing;

    public bool IsPressES;
    public bool IsES_ing { get; set; }
    public bool CanES => !IsES_ing && !IsDashing;

    public bool IsPressEB;
    public bool IsEB_ing { get; set; }
    public bool CanEB => !IsEB_ing && !IsDashing;

    public Vector2 MoveDirection { get; set; }
    public Vector2 LastDirection { get; set; }
    public Vector2 AttackDirection { get; set; }

    protected bool CanDash => StaminaEnough && !IsDashing && !IsNA_ing && !IsDashCoolingDown;
    protected bool CanMove => !IsDashing;
    protected bool StaminaEnough => fungusManager.fungusStamina.CurrentStamina >= FungusInfo.FungusData.dashStamina;

    public Rigidbody2D rb2d { get; private set; }
    public CapsuleCollider2D capsuleCollider2D { get; private set; }
    public AnimationEvent animationEvent { get; private set; }
    public CharacterAnimator fungusAnimator { get; private set; }

    public FungusHealth FungusHealth { get; private set; }
    public FungusStamina FungusStamina { get; private set; }
    public FungusInfoReader FungusInfo { get; private set; }
    public FungusSkill FungusSkill { get; private set; }


    public Action OnStopDashEvent;


    private Coroutine fungusDieCoroutine;
    private Coroutine stopDashCoroutine;
    private Coroutine dashCoolDownCoroutine;

    protected CameraCollider cameraCollider => CameraCollider.Instance;
    protected FungusManager fungusManager => FungusManager.Instance;
    protected GameplayManager gameplayController => GameplayManager.Instance;
    #endregion


    protected virtual void Awake()
    {
        _GetComponents();
        _ListenEvents();
    }
    protected void OnDestroy()
    {
        _RemoveEvents();
    }
    protected void OnEnable()
    {
        IsDashCoolingDown = false;
        IsDashing = false;
    }
    protected void Update()
    {
        UpdateInput();
        UpdateAnimation();
    }
    protected virtual void _GetComponents()
    {
        FungusStamina = GetComponent<FungusStamina>();
        FungusHealth = GetComponent<FungusHealth>();
        FungusInfo = GetComponent<FungusInfoReader>();
        FungusSkill = GetComponent<FungusSkill>();
        
        capsuleCollider2D = GetComponent<CapsuleCollider2D>();

        rb2d = GetComponent<Rigidbody2D>();

        animationEvent = GetComponentInChildren<AnimationEvent>();
        fungusAnimator = GetComponentInChildren<CharacterAnimator>();
    }
    protected virtual void _ListenEvents()
    {
        EventManager.onFungusDie += OnDied;

        animationEvent.OnStartAttackEvent.AddListener(() => OnStartAttack());
        animationEvent.OnEndAttackEvent.AddListener(() => OnEndAttack());
    }

    protected virtual void OnStartAttack()
    {
        fungusManager.InteractSlotState(false);
        IsNA_ing = true;
        IsUsingSkill = true;
    }

    protected virtual void OnEndAttack()
    {
        fungusManager.InteractSlotState(true);
        FungusSkill.NA_Skill();
        IsNA_ing = false;
        IsUsingSkill = false;
    }
    protected virtual void _RemoveEvents() { }

    protected virtual void UpdateInput()
    {
        if (IsDied) return;

        if (IsNA_ing)
        {
            rb2d.velocity = new Vector2(MoveDirection.x, MoveDirection.y);
            return;
        }

        Move();
        Dash();
        Skill();
    }
    protected virtual void UpdateAnimation()
    {
        fungusAnimator.SetMoveDirection(MoveDirection);

        fungusAnimator.SetLastMoveDirection(LastDirection);

        fungusAnimator.SetAttackDirection(AttackDirection);

        fungusAnimator.SetMoveState(MoveDirection != Vector2.zero);
    }

    protected virtual void Move()
    {
        float pressMoveX = Input.GetAxisRaw("Horizontal");
        float pressMoveY = Input.GetAxisRaw("Vertical");
        if (CanMove)
        {
            if ((pressMoveX == 0 && pressMoveY == 0) && (MoveDirection.x != 0 || MoveDirection.y != 0))
            {
                LastDirection = MoveDirection;
            }

            float moveSpeed = FungusInfo.FungusData.moveSpeed;
            rb2d.velocity = new Vector2(pressMoveX * moveSpeed, pressMoveY * moveSpeed);
            MoveDirection = new Vector2(pressMoveX, pressMoveY).normalized;
            fungusAnimator.SetMoveDirection(MoveDirection);
        }
    }
    protected virtual void Dash()
    {
        IsPressDash = Input.GetMouseButtonDown(1);
        if (IsPressDash && CanDash)
        {
            if (MoveDirection == Vector2.zero)
            {
                if(LastDirection.x == 0 && LastDirection.y == 0) LastDirection = new Vector2(0, -1);
                MoveDirection = LastDirection;
            }
            rb2d.velocity = MoveDirection.normalized * FungusInfo.FungusData.dashForce;

            fungusManager.fungusStamina.CurrentStamina -= FungusInfo.FungusData.dashStamina;

            if (stopDashCoroutine != null) StopCoroutine(stopDashCoroutine);
            stopDashCoroutine = StartCoroutine(StopDashCoroutine());
        }

    }
    IEnumerator StopDashCoroutine()
    {
        IsDashing = true;
        fungusManager.InteractSlotState(false);

        yield return new WaitForSeconds(FungusInfo.FungusData.dashTime);

        IsDashing = false;
        fungusManager.InteractSlotState(true);

        if (dashCoolDownCoroutine != null) StopCoroutine(dashCoolDownCoroutine);
        dashCoolDownCoroutine = StartCoroutine(DashCoolDownCoroutine());
    }

    IEnumerator DashCoolDownCoroutine()
    {
        IsDashCoolingDown = true;
        yield return new WaitForSeconds(PlayerConfig.dashCoolDown);       
        IsDashCoolingDown = false;
    }


    protected virtual void Skill()
    {
        NA(); ES(); EB();
    }

    void NA()
    {
        IsPressNA = Input.GetMouseButtonDown(0);

        if (IsPressNA && CanNA)
        {
            if (cameraCollider.CheckTargetDetection() != null)
            {
                Vector3 direction;
                direction = (cameraCollider.GetTargetTransform().position - transform.position).normalized;
                AttackDirection = direction;
            }
            else
            {
                if (MoveDirection.x == 0 && MoveDirection.y == 0)
                {
                    AttackDirection = LastDirection;
                }
                else if (MoveDirection.x != 0 || MoveDirection.y != 0)
                {
                    AttackDirection = LastDirection = MoveDirection;
                }
            }
            fungusAnimator.SetTriggerAttack();
        }
    }


    void ES()
    {
        IsPressES = Input.GetKeyDown(KeyCode.E);

        if (IsPressES && CanES)
        {
            Debug.Log("ES");
        }
    }

    void EB()
    {
        IsPressEB = Input.GetKeyDown(KeyCode.Q);

        if (IsPressEB && CanEB)
        {
            Debug.Log("EB");
        }
    }
    public Vector3 DirectionAttackWithOutTarget()
    {
        Vector3 direction;
        if (LastDirection.x == 0 && LastDirection.y == 0) direction = new Vector2(0, -1);

        else if (MoveDirection.x == 0 && MoveDirection.y == 0)
            direction = new Vector2(LastDirection.x, LastDirection.y);

        else direction = MoveDirection;

        return direction;
    }
    public IEnumerator FungusDieCoroutine()
    {

        IsDied = true;
        DiedState();

        int fungusAliveIndex = fungusManager.GetFungusAliveIndex();
        if (fungusAliveIndex >= 0)
        {
            yield return new WaitForSeconds(PlayerConfig.dyingWaitTime);

            fungusManager.SwitchFungus(fungusAliveIndex);
        }
        else
        {
            Debug.Log("Game over");
        }
    }


    public void DiedState()
    {
        MoveDirection = Vector2.zero;
        rb2d.velocity = MoveDirection;    

        capsuleCollider2D.enabled = false;
        fungusAnimator.SetTriggerDie();

    }
    public void ResetState()
    {
        IsDashing = false;
        IsDied = false;
        capsuleCollider2D.enabled = true;
    }


    //Event
    protected virtual void OnDied()
    {
        if (gameObject.activeSelf)
        {
            if (fungusDieCoroutine != null) StopCoroutine(fungusDieCoroutine);
            fungusDieCoroutine = StartCoroutine(FungusDieCoroutine());
        }
    }

}
