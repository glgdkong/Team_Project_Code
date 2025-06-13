using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DeathCross", menuName = "SKill/Enemy/BossSkill/AttackSkill/DeathCross")]
public class DeathCross : StatusAttackSkillSO
{
    [SerializeField] private Stun stun;
    [SerializeField] private Bleed bleed;
    public override void ApplySkillEffect(Character casterCharacter, Character targetCharacter, bool isCriticalHit = false, bool hitSuccess = true)
    {
        if(targetCharacter.IsDeath || !hitSuccess) { return; }
        chance = Random.value;
        if (chance < CalcEffectChance(stun.StatusEffectType, targetCharacter, isCriticalHit)) 
        {
            targetCharacter.StatusEffectHandler.UpdateOrAddEffect(stun, true);
        }
        else
        {
            targetCharacter.StatusEffectHandler.UpdateOrAddEffect(stun, false);
        }
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
    }

}
