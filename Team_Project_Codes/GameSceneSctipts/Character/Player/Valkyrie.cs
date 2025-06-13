using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Valkyrie : PlayableCharacter
{
    [SerializeField] private AtkUp atkUp;
    protected override IEnumerator TurnActionCoroutine()
    {
        PassiveSkill();
        yield return null;
        BattleManager.Instance.PlayerActionManager.PlayerTurn(this);


        while (!playerAction)
        {
            yield return null;
            //yield return null;
        }
    }

    protected override void PassiveSkill()
    {
        float value = Random.value;
        if (value <= 0.12f)
        {
            atkUp.IsCurrentTurnGranted = true;
            statusEffectHandler.UpdateOrAddEffect(atkUp, true);
        }
    }
}
