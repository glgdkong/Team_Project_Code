using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Soulflare", menuName = "SKill/Player/Druid/AttackSkill/Soulflare")]
public class Soulflare : StatusAttackSkillSO
{
    [SerializeField, Header("화상 부여 값")] protected Burn burn;

    public override void ApplySkillEffect(Character casterCharacter, Character targetCharacter, bool isCriticalHit = false, bool hitSuccess = true)
    {
        if (targetCharacter.IsDeath || !hitSuccess) { return; }
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
    }
}
