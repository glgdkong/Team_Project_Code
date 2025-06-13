using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TargetInfoPrintUI : MonoBehaviour
{
    [SerializeField] private GameObject targetUI;
    [SerializeField] private Image hpImage;
    [SerializeField] private TextMeshProUGUI targetNameText;
    [SerializeField] private TextMeshProUGUI targetSpeedText;
    [SerializeField] private TextMeshProUGUI targetHpText;
    [SerializeField] private TextMeshProUGUI targetDefText;
    [SerializeField] private TextMeshProUGUI targetEvdText;
    [SerializeField] private TextMeshProUGUI[] targetResistancesTexts;
    private bool isActive = false;
    private Character target;
    private void Start()
    {
        if (targetUI.gameObject.activeSelf && !isActive)
        {
            targetUI.SetActive(false);
        }
    }
    public void TargetInfo(Character target)
    {
        this.target = target;
        if (this.target == null)
        {
            targetUI.SetActive(false);
        }
        else
        {
            isActive = true;
            targetUI.SetActive(true);
            targetNameText.text = this.target.CharacterInfo.CharacterName;
            targetSpeedText.text = this.target.CurrentSpeed.ToString();
            targetHpText.text = $"{(this.target.CurrentHp < 0 ? 0 : this.target.CurrentHp)}/{this.target.CharacterInfo.CharacterHp.ToString()}";
            targetDefText.text = $"{this.target.CharacterInfo.CharacterDef + this.target.StatusEffectHandler.DefB}";
            targetEvdText.text = $"{this.target.CharacterInfo.CharacterEvd * 100:0.##}%";
            hpImage.fillAmount = (float)(this.target.CurrentHp < 0 ? 0 : this.target.CurrentHp) / (float)this.target.CharacterInfo.CharacterHp;
            for (int i = 0; i < targetResistancesTexts.Length; i++) 
            {
                targetResistancesTexts[i].text = ((int)Mathf.Round(this.target.CharacterInfo.Resistances[i].ResistanceValue * 100)).ToString();
            }
        }
    }

    public void UpdateInfo(Character target)
    {
        if (this.target == target)
        {
            isActive = true;
            targetUI.SetActive(true);
            targetNameText.text = this.target.CharacterInfo.CharacterName;
            targetSpeedText.text = this.target.CurrentSpeed.ToString();
            targetHpText.text = $"{(this.target.CurrentHp < 0 ? 0 : this.target.CurrentHp)}/{this.target.CharacterInfo.CharacterHp.ToString()}";
            targetDefText.text = $"{this.target.CharacterInfo.CharacterDef + this.target.StatusEffectHandler.DefB}";
            targetEvdText.text = $"{this.target.CharacterInfo.CharacterEvd * 100:0.##}%";
            hpImage.fillAmount = (float)(this.target.CurrentHp < 0 ? 0 : this.target.CurrentHp) / (float)this.target.CharacterInfo.CharacterHp;
            for (int i = 0; i < targetResistancesTexts.Length; i++)
            {
                targetResistancesTexts[i].text = ((int)Mathf.Round(this.target.CharacterInfo.Resistances[i].ResistanceValue * 100)).ToString();
            }
        }
    }
}
