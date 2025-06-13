using System;
using UnityEngine;
public enum EffectCategory
{
    Buff,      // 긍정적 상태이상 (강화)
    Debuff,    // 부정적 상태이상 (약화)
    Status     // 상태이상 (출혈, 중독 등)
}
[Serializable]
public abstract class StatusEffectBase
{
    [SerializeField, Header("횟수")] protected int count;
    public int Count { get => count; set => count = value; }
    protected bool isCriticalHit;
    public bool IsCriticalHit { get => isCriticalHit; set => isCriticalHit = value; }
    protected  bool isCurrentTurnGranted;
    public bool IsCurrentTurnGranted { get => isCurrentTurnGranted; set => isCurrentTurnGranted = value; }
    public virtual EffectCategory Category { get; }
   
    public virtual void ActivateEffect(Character character) {;}
    public virtual void AddEffect(StatusEffectBase effectBase, StatusEffectHandler statusEffectHandler) 
    {

    }
    public virtual string StatusInfo(){return null; }

    public abstract string ShowGrantSuccessMessage();
    public abstract string ShowGrantFailureMessage();
}
