using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpearSlam", menuName = "SKill/Player/Valkyrie/AttackSkill/SpearSlam")]
public class SpearSlam : StatusAttackSkillSO
{
    [SerializeField] private Stun stun;
    public override void ApplySkillEffect(Character casterCharacter, Character targetCharacter, bool isCriticalHit = false, bool hitSuccess = true)
    {
        if (targetCharacter.IsDeath || !hitSuccess) { return; }
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
    }
}
