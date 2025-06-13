using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TauntStatusAttack", menuName = "SKill/Enemy/StatusAttackSkill/TauntStatusAttack")]
public class TauntStatusAttack : StatusAttackSkillSO
{
    [SerializeField] private Taunt taunt;
    public override void ApplySkillEffect(Character casterCharacter, Character targetCharacter, bool isCriticalHit = false, bool hitSuccess = true)
    {
        if (targetCharacter.IsDeath || !hitSuccess) { return; }
        chance = Random.value;
        if (chance < CalcEffectChance(taunt.StatusEffectType, targetCharacter, isCriticalHit)) 
        {
            targetCharacter.StatusEffectHandler.UpdateOrAddEffect(taunt, true);
        }
        else
        {
            targetCharacter.StatusEffectHandler.UpdateOrAddEffect(taunt, false);
        }
    }
}
