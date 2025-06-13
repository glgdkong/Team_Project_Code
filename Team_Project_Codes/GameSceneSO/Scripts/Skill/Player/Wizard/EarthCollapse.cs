using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EarthCollapse", menuName = "SKill/Player/Wizard/StatusAttackSkill/EarthCollapse")]
public class EarthCollapse : StatusAttackSkillSO
{
    [SerializeField, Header("상태이상 부여 효과")] protected Stun stun;
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
