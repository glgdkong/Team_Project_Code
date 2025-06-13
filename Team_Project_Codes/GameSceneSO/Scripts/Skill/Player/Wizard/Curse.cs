using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Curse", menuName = "SKill/Player/Wizard/AttackSkill/Curse")]
public class Curse : StatusAttackSkillSO
{
    [SerializeField] protected DefDown defDown;

    public override void ApplySkillEffect(Character casterCharacter, Character targetCharacter, bool isCriticalHit = false, bool hitSuccess = true)
    {
        if (targetCharacter.IsDeath || !hitSuccess) { return; }
        chance = Random.value;
        if (chance < CalcEffectChance(defDown.StatusEffectType, targetCharacter, isCriticalHit))
        {
            targetCharacter.StatusEffectHandler.UpdateOrAddEffect(defDown, true);
        }
        else
        {
            targetCharacter.StatusEffectHandler.UpdateOrAddEffect(defDown, false);
        }
    }

}
