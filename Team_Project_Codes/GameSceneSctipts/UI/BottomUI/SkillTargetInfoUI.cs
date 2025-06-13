using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SkillTargetInfoUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI acc;
    [SerializeField] private TextMeshProUGUI damage;
    [SerializeField] private TextMeshProUGUI crit;
    [SerializeField] private TextMeshProUGUI manaCost;

    private PlayerActionUI playerActionUI;
    private SkillInfoSO skillInfoSO;
    private EnemyCharacter enemyCharacter;

    public PlayerActionUI PlayerActionUI { get => playerActionUI; set => playerActionUI = value; }

    private void PrintTotalSkillInfo()
    {
        if (skillInfoSO == null || !gameObject.activeSelf) return;
        switch (skillInfoSO.SkillType)
        {
            case SkillType.Attack:
                AttackSkillSO attackSkillSO = (AttackSkillSO)skillInfoSO;   
                if (enemyCharacter == null || !attackSkillSO.TargetPositions[enemyCharacter.CurrentPositionIndex])
                {
                    PrintNullSkillInfo();
                    manaCost.text = $"<color=#437BFF>{skillInfoSO.ManaCost}</color>";
                }
                else
                {
                    ItemStatType itemStatType = BattleManager.Instance.PlayerActionManager.PlayableCharacter.PlayableCharacterSO.TotalItemStat();
                    float totalAcc = (BattleManager.Instance.PlayerActionManager.PlayableCharacter.CharacterInfo.CharacterAcc - enemyCharacter.CharacterInfo.CharacterEvd + itemStatType.Accuracy);
                    totalAcc = totalAcc >= 1f ? 1f : totalAcc;
                    totalAcc = totalAcc <= 0f ? 0.05f : totalAcc;
                    acc.text = $"<color=#FFC000>{totalAcc * 100:0.##}</color>%";
                    Vector2 totalDamage = BattleManager.Instance.ComputeSkillDamage(BattleManager.Instance.PlayerActionManager.PlayableCharacter, skillInfoSO, enemyCharacter);
                    damage.text =$"<color=#FE332E>{(int)totalDamage.x}</color>" +"~"+$"<color=#FE332E>{(int)totalDamage.y}</color>";
                    float totalCirt = skillInfoSO.CritChance + (BattleManager.Instance.PlayerActionManager.PlayableCharacter.StatusEffectHandler.CritB)+ itemStatType.Critical;
                    crit.text = (totalCirt * 100 > 100) ? "<color=#FFD500>100</color>%" : $"<color=#FFD500>{totalCirt * 100:0.##}</color>%";
                    manaCost.text = $"<color=#437BFF>{skillInfoSO.ManaCost}</color>";
                }
                break;
            default:
                gameObject.SetActive(false);
                break;
        }
    }
    private void PrintNullSkillInfo()
    {
        acc.text = "-";
        damage.text = "-";
        crit.text = "-";
        manaCost.text = "-";
    }

    public void GetSkillInfo(SkillInfoSO skillInfoSO)
    {
        if(skillInfoSO == null)
        {
            this.skillInfoSO = skillInfoSO;
            PrintNullSkillInfo();
            gameObject.SetActive(false);
            return;
        }
        this.skillInfoSO = skillInfoSO;
        switch (skillInfoSO.SkillType)
        {
            case SkillType.Attack:
                gameObject.SetActive(true);
                break;
            default:
                gameObject.SetActive(false);
                break;
        }
        PrintTotalSkillInfo();
    }

    public void GetEnemyInfo(EnemyCharacter enemyCharacter) 
    {
        this.enemyCharacter = enemyCharacter;
        PrintTotalSkillInfo();
    }

}
