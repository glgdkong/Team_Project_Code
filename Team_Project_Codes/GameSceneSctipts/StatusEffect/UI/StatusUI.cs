using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class StatusUI : MonoBehaviour
{
    [SerializeField] protected Image statusImage;

    [SerializeField] protected StatusEffectBase statusEffect;
    private RectTransform rectTransform;
    public StatusEffectBase StatusEffect => statusEffect;
    protected StatusInfoUI statusInfoUI;

    protected void Start()
    {
        statusInfoUI = GameObject.FindWithTag("StatusInfoUI").GetComponent<StatusInfoUI>();
        rectTransform = GetComponent<RectTransform>();
    }

    public virtual void ClearInfo(StatusEffectBase statusEffectBase)
    {
        if (statusInfoUI.InfoUI.gameObject.activeSelf && statusInfoUI.StatusUI == this)
        {
            statusInfoUI.InfoUI.gameObject.SetActive(false);
        };
        if (gameObject.activeSelf)
        {
            gameObject.SetActive(false);
            rectTransform.SetAsLastSibling();
        }
        statusEffect = null;
    }
    public abstract void SetEffectInfo(StatusEffectBase statusEffectBase);
    
    public virtual void EnableInfo()
    {
        statusInfoUI.GetToolTipText(statusEffect.StatusInfo(), this);
        statusInfoUI.InfoUI.SetActive(true);
    }
    public virtual void DisableInfo()
    {
        statusInfoUI.InfoUI.SetActive(false);
    }

    protected virtual void OnDisable()
    {
        if(statusInfoUI.InfoUI != null)
        if (statusInfoUI.InfoUI.activeSelf)
        {
            if(statusInfoUI.StatusUI == this)
            {
                DisableInfo();
            }
        }
    }
}
