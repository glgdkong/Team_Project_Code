using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InformationSkillButtonUI : MonoBehaviour
{
    private CharacterInfoUI characterInfoUI;
    // 스킬 이미지
    [SerializeField] private Image skillIcon;
    // 스킬 선택 표시 이미지
    private SpriteState spriteState;
    [SerializeField] private GameObject skillSelectedIcon;
    [SerializeField] private SkillInfoSO skillInfoSO;
    [SerializeField] private Button skillButton;
    [SerializeField] private TextMeshProUGUI skillIndex;
    // 스킬 선택 여부
    [SerializeField] private bool isSelected = false;
    private PlayableCharacterSO playableCharacterSO;
    private SkillInfoPrintUI skillInfoPrintUI;

    //private Sk
    private void Awake()
    {
        skillInfoPrintUI = GetComponentInParent<SkillInfoPrintUI>();
        characterInfoUI = GetComponentInParent<CharacterInfoUI>();
    }

    public void SetSkillInfos(SkillInfoSO skillInfoSO, PlayableCharacterSO playableCharacterSO)
    {
        skillIcon.gameObject.SetActive(true);
        this.playableCharacterSO = playableCharacterSO;
        this.skillInfoSO = skillInfoSO;
        skillIcon.sprite = this.skillInfoSO.SkillEImage;
        spriteState.disabledSprite = this.skillInfoSO.SkillDImage;
        skillButton.spriteState = spriteState;
        if(playableCharacterSO.SelectedSkillInfos.Count > 0)
        {
            foreach (SkillInfoSO skillInfo in playableCharacterSO.SelectedSkillInfos)
            {
                if(skillInfo == this.skillInfoSO)
                {
                    SkillSelected();
                }
            }
        }
    }

    private void SkillSelected()
    {
        isSelected = true;
        skillSelectedIcon.gameObject.SetActive(isSelected);
        SkillIndexUpdate();
    }
    private void SkillDeSelected()
    {
        isSelected = false;
        skillSelectedIcon.gameObject.SetActive(isSelected);
        skillIndex.text = string.Empty;
    }

    public void SkillIndexUpdate()
    {
        if(skillSelectedIcon.gameObject.activeSelf)
        { 
            skillIndex.text = (playableCharacterSO.SelectedSkillInfos.IndexOf(skillInfoSO) + 1).ToString(); 
        }
    }

    public void SkillSelect()
    {
        if (SceneManager.GetActiveScene().name == "GameScene")
        {
            return;
        }
        if (!isSelected)
        {
            if (characterInfoUI.PlayableCharacterSO.SelectedSkillInfos.Count < 5)
            {
                characterInfoUI.PlayableCharacterSO.SelectedSkillInfos.Add(skillInfoSO);
                characterInfoUI.SkillIndexUpdate();
                SkillSelected();
            }
        }
        else
        {
            characterInfoUI.PlayableCharacterSO.SelectedSkillInfos.Remove(skillInfoSO);
            characterInfoUI.SkillIndexUpdate();
            SkillDeSelected();
        }
    }

    public void DeletInfo()
    {
        skillIcon.gameObject.SetActive(false);
        playableCharacterSO = null;
        skillInfoSO = null;
        skillIcon.sprite = null;
        SkillDeSelected();
    }

    public virtual void EnableInfo()
    {
        skillInfoPrintUI.GetToolTipText(skillInfoSO.SkillToolTip, skillInfoSO);
    }
    public virtual void DisableInfo()
    {
        skillInfoPrintUI.InfoUI.SetActive(false);
    }
}
