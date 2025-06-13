using RPGCharacterAnims.Actions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StunStatusAttack", menuName = "SKill/Enemy/StatusAttackSkill/StunStatusAttack")]
public class StunStatusAttack : StatusAttackSkillSO
{
    [SerializeField] private Stun stun; 
    public override void ApplySkillEffect(Character casterCharacter, Character targetCharacter, bool isCriticalHit = false, bool hitSuccess = true)
    {
        if (targetCharacter.IsDeath || !hitSuccess) { return; }
        chance = Random.value;
        if (chance < CalcEffectChance(stun.StatusEffectType, targetCharacter, isCriticalHit))
        {
            targetCharacter.StatusEffectHandler.UpdateOrAddEffect(stun, true);
        }
        else
        {
            targetCharacter.StatusEffectHandler.UpdateOrAddEffect(stun, false);
        }
    }
}
