using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StatusEffectHandler : MonoBehaviour
{
    [SerializeReference]private List<StatusEffectBase> statusEffects = new List<StatusEffectBase>();
    [SerializeField] private StatusEffectParticleHandler particleHandler;
    public List<StatusEffectBase> StatusEffects => statusEffects;
    public StatusEffectParticleHandler ParticleHandler => particleHandler;
    protected CharacterUI characterUI;
    protected Character character;
    public Character Character => character;

    protected WaitForSeconds delay = new WaitForSeconds(1f);

    private float atkB = 0;
    private float evdB = 0;
    private float critB = 0;
    private int defB = 0;
    public float AtkB=> atkB; 
    public float EvdB => evdB; 
    public float CritB  => critB; 
    public int DefB  => defB; 

    private void Awake()
    {
        character = GetComponent<Character>();
        characterUI = GetComponentInChildren<CharacterUI>();
        particleHandler = GetComponentInChildren<StatusEffectParticleHandler>();
    }


    public void UpdateOrAddEffect(StatusEffectBase effect, bool isGranted, bool showlog = true)
    {   
        if(isGranted)
        {
            switch (effect)
            {
                case BuffEffect buff:
                    buff.AddEffect(buff, this);
                    break;

                case DebuffEffect debuff:
                    debuff.AddEffect(debuff, this);
                    break;

                case StatusEffect otherStatus:
                    otherStatus.AddEffect(otherStatus, this);
                    break;

                default:
                    Debug.LogError($"알 수 없는 상태이상 타입: {effect.GetType()}");
                    break;
            }
            if(showlog)
            BattleManager.Instance.HitLogManager.ShowDamageLog(transform, effect, isGranted);
        }
        else
        {
            if(showlog)
            BattleManager.Instance.HitLogManager.ShowDamageLog(transform, effect, isGranted);
        }
    }

    public void RegisterNewEffect<T>(T effect) where T : StatusEffectBase
    {
        statusEffects.Add(effect);
        characterUI.AddStatusUI(effect);
    }


    public void RemoveStatus<T>(T effect) where T : StatusEffectBase
    {
        statusEffects.Remove(effect);
        characterUI.RemoveStatus(effect);
    }



    public bool HasSpecificEffect<T>(T effectType) where T : Enum
    {
        /*Debug.Log(effectType.ToString());
        Debug.Log(statusEffects.Any(effect =>
            (effect is BuffEffect buff && EqualityComparer<T>.Default.Equals((T)(object)buff.BuffType, effectType)) ||
            (effect is DebuffEffect debuff && EqualityComparer<T>.Default.Equals((T)(object)debuff.DebuffType, effectType)) ||
            (effect is StatusEffect status && EqualityComparer<T>.Default.Equals((T)(object)status.StatusEffectType, effectType)) ||
            (effect is StatusEffect status2 && EqualityComparer<T>.Default.Equals((T)(object)status2.StatusType, effectType))
        ));*/
        return statusEffects.Any(effect =>
            (effect is BuffEffect buff && effectType is BuffType buffType && buff.BuffType.Equals(buffType)) ||
            (effect is DebuffEffect debuff && effectType is DebuffType debuffType && debuff.DebuffType.Equals(debuffType)) ||
            (effect is StatusEffect status && effectType is StatusEffectType statusEffectType && status.StatusEffectType.Equals(statusEffectType)) ||
            (effect is StatusEffect status2 && effectType is StatusType statusType && status2.StatusType.Equals(statusType)|| 
            (effect is StatusEffectBase baseStatus && effectType is EffectCategory category && baseStatus.Category.Equals(category))));
    }

    public StatusEffectBase GetSpecificEffect<T>(T effectType) where T : Enum
    {
        return statusEffects.Find(effect =>
            (effect is BuffEffect buff && effectType is BuffType buffType && buff.BuffType.Equals(buffType)) ||
            (effect is DebuffEffect debuff && effectType is DebuffType debuffType && debuff.DebuffType.Equals(debuffType)) ||
            (effect is StatusEffect status && effectType is StatusEffectType statusEffectType && status.StatusEffectType.Equals(statusEffectType)) ||
            (effect is StatusEffect status2 && effectType is StatusType statusType && status2.StatusType.Equals(statusType) ||
            (effect is StatusEffectBase baseStatus && effectType is EffectCategory category && baseStatus.Category.Equals(category))));
    }



    public IEnumerator ApplyEffect<T>(T effectType) where T : Enum
    {
        List<StatusEffectBase> useStatus = statusEffects.FindAll(effect =>
            (effect is BuffEffect buff && effectType is BuffType buffType && buff.BuffType.Equals(buffType)) ||
            (effect is DebuffEffect debuff && effectType is DebuffType debuffType && debuff.DebuffType.Equals(debuffType)) ||
            (effect is StatusEffect status && effectType is StatusEffectType statusEffectType && status.StatusEffectType.Equals(statusEffectType)) ||
            (effect is StatusEffect status2 && effectType is StatusType statusType && status2.StatusType.Equals(statusType) ||
            (effect is StatusEffectBase baseStatus && effectType is EffectCategory category && baseStatus.Category.Equals(category))));
        // 삭제할 항목들을 모을 리스트
        List<StatusEffectBase> effectsToRemove = new List<StatusEffectBase>();

        foreach (StatusEffectBase effect in useStatus)
        {
            effect.ActivateEffect(character);
            if (effect.Count <= 0)
            {
                effectsToRemove.Add(effect);
            }
            if (character.IsDeath)
            {
                yield break;
            }
            yield return delay;
        }

        // 삭제할 항목들을 한 번에 처리
        foreach (StatusEffectBase effect in effectsToRemove)
        {
            RemoveStatus(effect);
        }

    }

    public void DecreaseCount<T>(T effectType) where T : Enum
    {
        List<StatusEffectBase> useStatus = statusEffects.FindAll(effect =>
            (effect is BuffEffect buff && effectType is BuffType buffType && buff.BuffType.Equals(buffType)) ||
            (effect is DebuffEffect debuff && effectType is DebuffType debuffType && debuff.DebuffType.Equals(debuffType)) ||
            (effect is StatusEffect status && effectType is StatusEffectType statusEffectType && status.StatusEffectType.Equals(statusEffectType)) ||
            (effect is StatusEffect status2 && effectType is StatusType statusType && status2.StatusType.Equals(statusType) ||
            (effect is StatusEffectBase baseStatus && effectType is EffectCategory category && baseStatus.Category.Equals(category))));
        // 삭제할 항목들을 모을 리스트
        List<StatusEffectBase> effectsToRemove = new List<StatusEffectBase>();

        foreach (StatusEffectBase effect in useStatus)
        {
            effect.ActivateEffect(character);
            if (effect.Count <= 0)
            {
                effectsToRemove.Add(effect);
            }
        }

        // 삭제할 항목들을 한 번에 처리
        foreach (StatusEffectBase effect in effectsToRemove)
        {
            RemoveStatus(effect);
        }
    }

    public void DeleteStatusEffect<T>(T effectType) where T : Enum
    {
        List<StatusEffectBase> effectsToRemove = statusEffects.FindAll(effect =>
            (effect is BuffEffect buff && effectType is BuffType buffType && buff.BuffType.Equals(buffType)) ||
            (effect is DebuffEffect debuff && effectType is DebuffType debuffType && debuff.DebuffType.Equals(debuffType)) ||
            (effect is StatusEffect status && effectType is StatusEffectType statusEffectType && status.StatusEffectType.Equals(statusEffectType)) ||
            (effect is StatusEffect status2 && effectType is StatusType statusType && status2.StatusType.Equals(statusType) ||
            (effect is StatusEffectBase baseStatus && effectType is EffectCategory category && baseStatus.Category.Equals(category))));


        // 삭제할 항목들을 한 번에 처리
        foreach (StatusEffectBase effect in effectsToRemove)
        {
            RemoveStatus(effect);
        }
    }
    // StatusEffectHandler 클래스 내부
    public void ModifyBuffValues(float atk = 0, float evd = 0, float crit = 0, int def = 0)
    {
        atkB += atk;
        evdB += evd;
        critB += crit;
        defB += def;
    }
}
