using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellBlade : PlayableCharacter
{
    private bool isBuffed = false;
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
        switch (currentPositionIndex)
        {
            case 0:
            case 1:
                if (isBuffed)
                {
                    return;
                }
                else
                {
                    statusEffectHandler.ModifyBuffValues(crit: 0.02f);
                    isBuffed = true;
                }
                break;
            case 2:
            case 3:
                if (isBuffed)
                {
                    statusEffectHandler.ModifyBuffValues(crit: -0.02f);
                    isBuffed = false;
                }
                HealMana(2);
                break;
        }
    }
}