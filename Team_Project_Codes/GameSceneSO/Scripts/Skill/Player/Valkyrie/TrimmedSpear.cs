using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TrimmedSpear", menuName = "SKill/Player/Valkyrie/BuffSkill/TrimmedSpear")]
public class TrimmedSpear : BuffSkillSO
{
    [SerializeField] private CritUp critUp;
    public override void ApplySkillEffect(Character casterCharacter, Character targetCharacter, bool isCriticalHit = false, bool hitSuccess = true)
    {
        critUp.IsCurrentTurnGranted = (casterCharacter == targetCharacter);
        targetCharacter.StatusEffectHandler.UpdateOrAddEffect(critUp, true);
    }
}
