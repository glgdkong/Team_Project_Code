using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SuperDuperGeneral2", menuName = "SKill/Test/SuperDuperGeneral2")]
public class SuperDuperGeneral2 : BuffSkillSO
{
    [SerializeField] private CritUp critUp;
    [SerializeField] private AtkUp atkUp;
    [SerializeField] private DefUp defUp;
    [SerializeField] private DodgeUp dodgeUp;
    public override void ApplySkillEffect(Character casterCharacter, Character targetCharacter, bool isCriticalHit = false, bool hitSuccess = true)
    {
        critUp.IsCurrentTurnGranted = (casterCharacter == targetCharacter);
        targetCharacter.StatusEffectHandler.UpdateOrAddEffect(critUp, true);
        atkUp.IsCurrentTurnGranted = (casterCharacter == targetCharacter);
        targetCharacter.StatusEffectHandler.UpdateOrAddEffect(atkUp, true);
        defUp.IsCurrentTurnGranted = (casterCharacter == targetCharacter);
        targetCharacter.StatusEffectHandler.UpdateOrAddEffect(defUp, true);
        dodgeUp.IsCurrentTurnGranted = (casterCharacter == targetCharacter);
        targetCharacter.StatusEffectHandler.UpdateOrAddEffect(dodgeUp, true);
    }
}
