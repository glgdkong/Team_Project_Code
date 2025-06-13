using RPGCharacterAnims.Actions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "IceSphere", menuName = "SKill/Player/Wizard/AttackSkill/IceSphere")]
public class IceSphere : StatusAttackSkillSO
{
    [SerializeField,Header("빙결 부여값")] private Freeze freeze;
    public override void ApplySkillEffect(Character casterCharacter, Character targetCharacter, bool isCriticalHit = false, bool hitSuccess = true)
    {
        if (targetCharacter.IsDeath || !hitSuccess) { return; }
        chance = Random.value;
        if (chance < CalcEffectChance(freeze.StatusEffectType, targetCharacter, isCriticalHit))
        {
            targetCharacter.StatusEffectHandler.UpdateOrAddEffect(freeze, true);
        }
        else
        {
            targetCharacter.StatusEffectHandler.UpdateOrAddEffect(freeze, false);
        }
    }
}
