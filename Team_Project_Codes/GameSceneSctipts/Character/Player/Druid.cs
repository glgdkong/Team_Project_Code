using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Druid : PlayableCharacter
{
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
        HealMana(1);
        HealHealth(1);
    }
}
