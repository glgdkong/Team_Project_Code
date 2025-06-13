using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ResUpBuffSkill", menuName = "SKill/Enemy/BuffSkill/ResUpBuffSkill")]
public class ResUpBuffSkill : BuffSkillSO
{
    [SerializeField] private ResUp resUp;
    public override void ApplySkillEffect(Character casterCharacter, Character targetCharacter, bool isCriticalHit = false, bool hitSuccess = true)
    {
        resUp.IsCurrentTurnGranted = (casterCharacter == targetCharacter);
        targetCharacter.StatusEffectHandler.UpdateOrAddEffect(resUp, true);
    }

}
