using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillInfoPrintUI : MonoBehaviour
{
    [SerializeField] private RectTransform[] rebuildLayouts;
    [SerializeField] private TextMeshProUGUI skillName;
    [SerializeField] private TextMeshProUGUI skillManaCost;
    [SerializeField] private TextMeshProUGUI skillRangeType;
    [SerializeField] private TextMeshProUGUI skillDamage;
    [SerializeField] private TextMeshProUGUI SkillCritChance;
    [SerializeField] private TextMeshProUGUI SkillTooltip;
    [SerializeField] private Image skillImage;

    [SerializeField] private Image[] attackPosImg;
    [SerializeField] private GameObject enemyTargetPosObject;
    [SerializeField] private Image[] enemyTargetPosImg;
    [SerializeField] private GameObject playerTargetPosObject;
    [SerializeField] private GameObject selfTargetObject;

    [SerializeField] private TextMeshProUGUI impactValueTitle;

    [SerializeField] private Canvas canvas;
    [SerializeField] private GameObject infoUI;
    [SerializeField] private Vector2 setPos;
    private SkillInfoSO skillInfoSO;
    public SkillInfoSO SkillInfoSO => skillInfoSO;
    public GameObject InfoUI => infoUI;
    private RectTransform parentRectTransform;
    private RectTransform cavasRectTransform;
    private RectTransform infoUIRectTransform;
    private void Awake()
    {
        // 마우스 위치를 부모 RectTransform의 로컬 좌표로 변환
        parentRectTransform = GetComponent<RectTransform>();
        cavasRectTransform = canvas.GetComponent<RectTransform>(); 
        infoUIRectTransform = infoUI.GetComponent<RectTransform>();
    }
    private void Start()
    {
        if (infoUI.gameObject.activeSelf)
        {
            infoUI.gameObject.SetActive(false);
        }
    }
    private void LateUpdate()
    {
        if (infoUI.gameObject.activeSelf)
        {
            // 마우스 위치를 화면 좌표로 가져오기
            Vector3 mousePosition = Input.mousePosition;
            // 마우스 위치를 캔버스의 로컬 좌표로 변환
            Vector2 localPoint;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(cavasRectTransform, mousePosition, canvas.worldCamera, out localPoint);
            localPoint.y += setPos.y;

            // 부모 RectTransform의 크기 구하기
            Vector2 parentSize = parentRectTransform.rect.size;

            // UI의 크기 구하기 (RectTransform)
            Vector2 uiSize = infoUIRectTransform.rect.size;

            // UI가 부모의 범위를 벗어나지 않도록 좌우로 제한
            float halfWidth = (uiSize.x * infoUIRectTransform.pivot.x);
            localPoint.x = Mathf.Clamp(localPoint.x, (-parentSize.x / 2) + halfWidth, (parentSize.x / 2) - halfWidth);
            localPoint.x += setPos.x;
            // UI의 위치를 갱신
            infoUI.transform.localPosition = localPoint;
        }
    }
    private void OnDisable()
    {
        skillName.text = string.Empty;
        skillManaCost.text = string.Empty;
        skillDamage.text = string.Empty;
        SkillCritChance.text = string.Empty;
        SkillTooltip.text = string.Empty;
        skillInfoSO = null;
    }

    public void GetToolTipText(string text, SkillInfoSO skillInfoSO = null)
    {
        skillImage.sprite = skillInfoSO.SkillEImage;
        skillName.text = skillInfoSO.SkillName;
        skillManaCost.text = $"<color=white>마나 소모: </color>{skillInfoSO.ManaCost}";
        SkillCritChance.text = $"<color=#FFD500>{skillInfoSO.CritChance * 100:0.##}</color>%";
        SkillTooltip.text = skillInfoSO.SkillToolTip;
        for (int i = 0; i < skillInfoSO.CanUsePos.Length; i++)
        {
            attackPosImg[i].gameObject.SetActive(skillInfoSO.CanUsePos[i]);
        }
        switch (skillInfoSO.SkillType)
        {
            case SkillType.Attack:
                AttackSkillSO attackSkillSO = (AttackSkillSO)skillInfoSO;
                enemyTargetPosObject.SetActive(true);
                playerTargetPosObject.SetActive(false);
                selfTargetObject.SetActive(false);
                impactValueTitle.text = "피해";
                skillDamage.text = $"<color=#FF0000>{skillInfoSO.ImpactValue.x}</color> - <color=#FF0000>{skillInfoSO.ImpactValue.y}</color>";
                for (int i = 0; i < attackSkillSO.CanUsePos.Length; i++)
                {
                    enemyTargetPosImg[i].gameObject.SetActive(attackSkillSO.TargetPositions[i]);
                }
                break;
            case SkillType.Heal:
                enemyTargetPosObject.SetActive(false);
                playerTargetPosObject.SetActive(true);
                selfTargetObject.SetActive(false);
                impactValueTitle.text = "치유";
                HealSkillSO healSkillSO = (HealSkillSO)skillInfoSO;
                string hexColor = string.Empty;
                switch (healSkillSO.HealType)
                {
                    case HealType.Heath:
                        hexColor = "#41FF41";
                        break;
                    case HealType.Mana:
                        hexColor = "#437BFF";
                        break;
                    default:
                        break;
                }
                if (skillInfoSO.ImpactValue.x > 0)
                {
                    skillDamage.text = $"<color={hexColor}>{skillInfoSO.ImpactValue.x}</color> - <color={hexColor}>{skillInfoSO.ImpactValue.y}</color>";
                }
                else
                {
                    skillDamage.text = $"<color={hexColor}>{skillInfoSO.ImpactValue.y}</color>";
                }

                break;
            case SkillType.Buff:
                enemyTargetPosObject.SetActive(false);
                playerTargetPosObject.SetActive(true);
                selfTargetObject.SetActive(false);
                impactValueTitle.text = "버프";
                skillDamage.text = "-";
                SkillCritChance.text = "-";
                break;
            default:
                break;
        }
        switch (skillInfoSO.SkillRangeType)
        {
            case SkillRangeType.Single:
                skillRangeType.text = "<color=#FE332E>단일</color> 대상";
                break;
            case SkillRangeType.Area:
                skillRangeType.text = "<color=#FE332E>범위</color> 대상";
                break;
            case SkillRangeType.Self:
                skillRangeType.text = "<color=#FE332E>자기 자신</color>";
                enemyTargetPosObject.SetActive(false);
                playerTargetPosObject.SetActive(false);
                selfTargetObject.SetActive(true);
                break;
            default:
                break;
        }
        this.skillInfoSO = skillInfoSO;
        infoUI.gameObject.SetActive(true);
        foreach (RectTransform rebuildLayout in rebuildLayouts)
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(rebuildLayout); 
        }
    }

}
