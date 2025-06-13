using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DuelCry", menuName = "SKill/Player/Guardian/AttackSkill/DuelCry")]
public class DuelCry : BuffSkillSO
{
    [SerializeField, Header("도발 수치")] private Taunt taunt;

    public override void ApplySkillEffect(Character casterCharacter, Character targetCharacter, bool isCriticalHit = false, bool hitSuccess = true)
    {
        taunt.IsCurrentTurnGranted = true;
        casterCharacter.StatusEffectHandler.UpdateOrAddEffect(taunt, true);
    }
}
