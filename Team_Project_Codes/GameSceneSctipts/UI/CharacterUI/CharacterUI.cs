using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public abstract class CharacterUI : MonoBehaviour
{
    [SerializeField] protected Image healthBar;
    [SerializeField] protected RectTransform statusEffectPos;
    [SerializeField] protected RectTransform debuffAndBuffEffectPos;
    [SerializeField] protected GameObject statusEffectUI;
    [SerializeField] protected GameObject debuffAndBuffEffectEffectUI;
    [SerializeField] protected List<StatusEffectUI> statusEffectUIs = new List<StatusEffectUI>();
    [SerializeField] protected List<BuffDebuffUI> debuffAndBuffEffectUIs = new List<BuffDebuffUI>();
    [SerializeField] protected ResUpUI resistanceEffectUI;
    [SerializeField] private Image turnDisplay;

    protected void Start()
    {
        turnDisplay.gameObject.SetActive(false);
    }

    public void AddStatusUI(StatusEffectBase effect)
    {
        // 상태이상 타입에 따른 UI 추가
        if (effect is BuffEffect buffEffect)
        {
            switch (buffEffect.BuffType)
            {
                case BuffType.ATK_Up:
                case BuffType.DEF_Up:
                case BuffType.Crit_Up:
                case BuffType.Dodge_Up:
                    BuffDebuffUI debuffAndBuffEffect = GetReusableUI(debuffAndBuffEffectUIs);
                    if (debuffAndBuffEffect == null)
                    {
                        debuffAndBuffEffect = Instantiate(debuffAndBuffEffectEffectUI, debuffAndBuffEffectPos).GetComponent<BuffDebuffUI>();
                        debuffAndBuffEffect.SetEffectInfo(buffEffect);
                        debuffAndBuffEffectUIs.Add(debuffAndBuffEffect);
                    }
                    else
                    {
                        debuffAndBuffEffect.SetEffectInfo(buffEffect);
                        debuffAndBuffEffect.gameObject.SetActive(true); 
                    }
                    break;
                case BuffType.Resistance_Up:
                    if (!resistanceEffectUI.gameObject.activeSelf)
                    {
                        resistanceEffectUI.SetEffectInfo(buffEffect);
                        resistanceEffectUI.gameObject.SetActive(true);
                    }
                    else
                    {
                        resistanceEffectUI.SetEffectInfo(buffEffect);
                    }
                    break;
                default:
                    break;
            }
        }
        else if (effect is StatusEffect statusEffect)
        {
            switch (statusEffect.StatusEffectType)
            {
                case StatusEffectType.Bleed:
                case StatusEffectType.Poison:
                case StatusEffectType.Burn:
                case StatusEffectType.Stun:
                case StatusEffectType.Freeze:
                case StatusEffectType.Electrocute:

                    StatusEffectUI statusEffectUI = GetReusableUI(statusEffectUIs);
                    if (statusEffectUI == null)
                    {
                        // 일반 상태이상 효과 처리
                        statusEffectUI = Instantiate(this.statusEffectUI, statusEffectPos).GetComponent<StatusEffectUI>();
                        statusEffectUI.SetEffectInfo(statusEffect);
                        statusEffectUIs.Add(statusEffectUI);
                    }
                    else
                    {
                        statusEffectUI.SetEffectInfo(statusEffect);
                        
                        statusEffectUI.gameObject.SetActive(true);
                    }
                    break;
                case StatusEffectType.Weakness:
                    if (statusEffect is DebuffEffect debuffEffect)
                    {
                        BuffDebuffUI debuffAndBuffEffect = GetReusableUI(debuffAndBuffEffectUIs);
                        if (debuffAndBuffEffect == null)
                        {
                            debuffAndBuffEffect = Instantiate(debuffAndBuffEffectEffectUI, debuffAndBuffEffectPos).GetComponent<BuffDebuffUI>();
                            debuffAndBuffEffect.SetEffectInfo(debuffEffect);
                            debuffAndBuffEffectUIs.Add(debuffAndBuffEffect);
                        }
                        else
                        {
                            debuffAndBuffEffect.SetEffectInfo(debuffEffect);
                        }
                        debuffAndBuffEffect.gameObject.SetActive(true);
                    }
                    break;
                default:
                    break;
            }
        }
    }
    private T GetReusableUI<T>(List<T> uiList) where T : StatusUI
    {
        return uiList.FirstOrDefault(ui => !ui.gameObject.activeSelf && ui.StatusEffect == null);
    }

    public virtual void UpdateCharacterInfo(float amount)
    {
        healthBar.fillAmount = amount;
    }

    public virtual void RemoveStatus(StatusEffectBase effect)
    {
        // 상태이상 타입에 따른 UI 추가
        if (effect is ResUp resbuffEffect)
        {
            resistanceEffectUI.ClearInfo(resbuffEffect);
        }
        else
        {
            switch (effect)
            {
                case BuffEffect removedBuffEffect:
                    foreach (BuffDebuffUI buffDebuffUI in debuffAndBuffEffectUIs)
                    {
                        if (!buffDebuffUI.gameObject.activeSelf)
                        {
                            continue;
                        }
                        else
                        {
                            if ((buffDebuffUI.StatusEffect is BuffEffect currentBuffEffect) && currentBuffEffect.BuffType == removedBuffEffect.BuffType)
                            {
                                if (debuffAndBuffEffectUIs.Remove(buffDebuffUI))
                                {
                                    //Debug.Log("제거 성공 및 새로 추가");
                                    debuffAndBuffEffectUIs.Add(buffDebuffUI);

                                }
                                buffDebuffUI.ClearInfo(removedBuffEffect);
                                break;

                            }
                        }
                    }
                    break;
                case DebuffEffect debuffEffect:
                    foreach (BuffDebuffUI buffDebuffUI in debuffAndBuffEffectUIs)
                    {
                        if (!buffDebuffUI.gameObject.activeSelf)
                        {
                            continue;
                        }
                        else
                        {
                            if ((buffDebuffUI.StatusEffect is DebuffEffect currentDeBuffEffect) && currentDeBuffEffect.DebuffType == debuffEffect.DebuffType)
                            {
                                if (debuffAndBuffEffectUIs.Remove(buffDebuffUI))
                                {
                                    //Debug.Log("제거 성공 및 새로 추가");
                                    debuffAndBuffEffectUIs.Add(buffDebuffUI);

                                }
                                buffDebuffUI.ClearInfo(debuffEffect);
                                break;
                            }
                        }
                    }
                    break;
                case StatusEffect statusEffect:
                    foreach (StatusEffectUI statusEffectUI in statusEffectUIs)
                    {
                        if (!statusEffectUI.gameObject.activeSelf)
                        {
                            continue;
                        }
                        else
                        {
                            if ((statusEffectUI.StatusEffect is StatusEffect currentStatusEffect) && currentStatusEffect.StatusEffectType == statusEffect.StatusEffectType)
                            {
                                if (statusEffectUIs.Remove(statusEffectUI))
                                {
                                    //Debug.Log("제거 성공 및 새로 추가");
                                    statusEffectUIs.Add(statusEffectUI);

                                }
                                statusEffectUI.ClearInfo(statusEffect);
                                break;
                            }
                        }
                    }
                    break;
                default:
                    break;
            }
        }
    }

    public void ActivateTurnDisplay()
    {
        turnDisplay.gameObject.SetActive(true);
    }
    public void DeactivateTurnDisplay()
    {
        turnDisplay.gameObject.SetActive(false);
    }
}
