using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimationEvent : MonoBehaviour
{
    public bool eventCalled;
    public UnityEvent OnAnimEventTrigger;

    public void AttackEvent()
    {
        if (!eventCalled) return;
        OnAnimEventTrigger?.Invoke();
        eventCalled = false;
    }
    public void ResetEvent()
    {
        eventCalled = true;
    }
}
