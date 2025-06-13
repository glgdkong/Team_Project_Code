using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guradian : PlayableCharacter
{
    [SerializeField] private Taunt taunt;
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
        HealHealth(2);
        float value = Random.value;
        if(value <= 0.03f)
        {
            taunt.IsCurrentTurnGranted = true;
            statusEffectHandler.UpdateOrAddEffect(taunt, true);
        }
    }
}
