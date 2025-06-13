using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SkillInfoUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI skillNameText;
    [SerializeField] private TextMeshProUGUI skillEffect;
    [SerializeField] private TextMeshProUGUI skillText;

    public void PrintSkillInfo(SkillInfoSO skillInfo)
    {
        if (skillInfo == null) 
        {
            skillNameText.text = string.Empty;
            skillEffect.text = string.Empty;
            skillText.text = string.Empty;
            gameObject.SetActive(false);
            return;
        }
        skillNameText.text = skillInfo.SkillName;
        switch (skillInfo.SkillType)
        {
            case SkillType.Attack:
                AttackSkillSO attackSkillSO = (AttackSkillSO)skillInfo;
                skillEffect.text = "피해량: <color=#FE332E>" + attackSkillSO.ImpactValue.x + "</color> ~ <color=#FE332E>" + attackSkillSO.ImpactValue.y + "</color> 치명타율: <color=#FFD500>" + ((attackSkillSO.CritChance * 100 > 100) ? "100</color>%" : $"{attackSkillSO.CritChance * 100:0.##}</color>%") + " 마나 소모: <color=#437BFF>" + attackSkillSO.ManaCost+"</color>";
                break;
            case SkillType.Heal:
            case SkillType.Buff:
                skillEffect.text = "마나소모: <color=#437BFF>" + skillInfo.ManaCost+ "</color>";
                break;
            default:
                break;
        }
        skillText.text = skillInfo.SkillToolTip;
    }

    public void DisableSkillInfo()
    {
        skillEffect.text = null;
        skillText.text = null;
    }
}
