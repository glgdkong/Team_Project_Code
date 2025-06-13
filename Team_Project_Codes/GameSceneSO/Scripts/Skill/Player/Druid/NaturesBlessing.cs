using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NaturesBlessing", menuName = "SKill/Player/Druid/BuffSkill/NaturesBlessing")]
public class NaturesBlessing : BuffSkillSO
{
    [SerializeField] private AtkUp atkUp;
    [SerializeField] private CritUp critUp;
    public override void ApplySkillEffect(Character casterCharacter, Character targetCharacter, bool isCriticalHit = false, bool hitSuccess = true)
    {
        if (targetCharacter.IsDeath || !hitSuccess) { return; }
        atkUp.IsCurrentTurnGranted = (casterCharacter == targetCharacter);
        critUp.IsCurrentTurnGranted = (casterCharacter == targetCharacter);
        targetCharacter.StatusEffectHandler.UpdateOrAddEffect(atkUp, true);
        targetCharacter.StatusEffectHandler.UpdateOrAddEffect(critUp, true);
    }
}
