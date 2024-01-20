using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimationEvent : MonoBehaviour
{
    [SerializeField] private bool eventCalled;


    public UnityEvent OnStartAttackEvent;
    public UnityEvent OnEndAttackEvent;

    private void Awake()
    {

    }
    public void OnStartAttack()
    {
        OnStartAttackEvent?.Invoke();
        eventCalled = true;
    }

    public void OnEndAttack()
    {
        if (!eventCalled) return;

        OnEndAttackEvent?.Invoke();
        eventCalled = false;
    }
}
