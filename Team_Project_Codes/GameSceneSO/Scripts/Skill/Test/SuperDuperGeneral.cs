using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SuperDuperGeneral", menuName = "SKill/Test/SuperDuperGeneral")]
public class SuperDuperGeneral : StatusAttackSkillSO
{
    [SerializeField] private Bleed bleed;
    [SerializeField] private Poison poison;
    [SerializeField] private Burn burn;
    [SerializeField] private Stun stun;
    [SerializeField] private Freeze freeze;
    [SerializeField] private Electrocute electrocute;
    [SerializeField] private DefDown defDown;
    [SerializeField] private AtkDown atkDown;
    [SerializeField] private Slow slow;
    public override void ApplySkillEffect(Character casterCharacter, Character targetCharacter, bool isCriticalHit = false, bool hitSuccess = true)
    {
        if (targetCharacter.IsDeath || !hitSuccess) { return; }

        chance = Random.value;
        if (chance < CalcEffectChance(bleed.StatusEffectType, targetCharacter, isCriticalHit))
        {
            bleed.IsCriticalHit = isCriticalHit;
            targetCharacter.StatusEffectHandler.UpdateOrAddEffect(bleed, true);
        }
        else
        {
            targetCharacter.StatusEffectHandler.UpdateOrAddEffect(bleed, false);
        }

        chance = Random.value;
        if (chance < CalcEffectChance(poison.StatusEffectType, targetCharacter, isCriticalHit))
        {
            poison.IsCriticalHit = isCriticalHit;
            targetCharacter.StatusEffectHandler.UpdateOrAddEffect(poison, true);
        }
        else
        {
            targetCharacter.StatusEffectHandler.UpdateOrAddEffect(poison, false);
        }

        chance = Random.value;
        if (chance < CalcEffectChance(burn.StatusEffectType, targetCharacter, isCriticalHit))
        {
            burn.IsCriticalHit = isCriticalHit;
            targetCharacter.StatusEffectHandler.UpdateOrAddEffect(burn, true);
        }
        else
        {
            targetCharacter.StatusEffectHandler.UpdateOrAddEffect(burn, false);
        }

        chance = Random.value;
        if (chance < CalcEffectChance(stun.StatusEffectType, targetCharacter, isCriticalHit))
        {
            stun.IsCriticalHit = isCriticalHit;
            targetCharacter.StatusEffectHandler.UpdateOrAddEffect(stun, true);
        }
        else
        {
            targetCharacter.StatusEffectHandler.UpdateOrAddEffect(stun, false);
        }

        chance = Random.value;
        if (chance < CalcEffectChance(freeze.StatusEffectType, targetCharacter, isCriticalHit))
        {
            freeze.IsCriticalHit = isCriticalHit;
            targetCharacter.StatusEffectHandler.UpdateOrAddEffect(freeze, true);
        }
        else
        {
            targetCharacter.StatusEffectHandler.UpdateOrAddEffect(freeze, false);
        }

        chance = Random.value;
        if (chance < CalcEffectChance(electrocute.StatusEffectType, targetCharacter, isCriticalHit))
        {
            electrocute.IsCriticalHit = isCriticalHit;
            targetCharacter.StatusEffectHandler.UpdateOrAddEffect(electrocute, true);
        }
        else
        {
            targetCharacter.StatusEffectHandler.UpdateOrAddEffect(electrocute, false);
        }

        chance = Random.value;
        if (chance < CalcEffectChance(defDown.StatusEffectType, targetCharacter, isCriticalHit))
        {
            defDown.IsCriticalHit = isCriticalHit;
            targetCharacter.StatusEffectHandler.UpdateOrAddEffect(defDown, true);
        }
        else
        {
            targetCharacter.StatusEffectHandler.UpdateOrAddEffect(defDown, false);
        }

        chance = Random.value;
        if (chance < CalcEffectChance(atkDown.StatusEffectType, targetCharacter, isCriticalHit))
        {
            atkDown.IsCriticalHit = isCriticalHit;
            targetCharacter.StatusEffectHandler.UpdateOrAddEffect(atkDown, true);
        }
        else
        {
            targetCharacter.StatusEffectHandler.UpdateOrAddEffect(atkDown, false);
        }

        chance = Random.value;
        if (chance < CalcEffectChance(slow.StatusEffectType, targetCharacter, isCriticalHit))
        {
            slow.IsCriticalHit = isCriticalHit;
            targetCharacter.StatusEffectHandler.UpdateOrAddEffect(slow, true);
        }
        else
        {
            targetCharacter.StatusEffectHandler.UpdateOrAddEffect(slow, false);
        }

    }

}
