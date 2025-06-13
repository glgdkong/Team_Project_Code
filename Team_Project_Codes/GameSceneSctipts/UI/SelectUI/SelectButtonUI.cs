using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectButtonUI : MonoBehaviour
{
    [SerializeField] private CharacterType characterType;
    [SerializeField] private Sprite[] sprites;
    [SerializeField] private Color[] colors;
    [SerializeField] private Image image;
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private SelectUI selectUI;
    public RectTransform RectTransform => rectTransform;

    public SelectUI SelectUI { get => selectUI; set => selectUI = value; }
    public CharacterType CharacterType => characterType;

    private void OnEnable()
    {
        image.sprite = sprites[1];
        image.color = colors[1];
    }
    public void MouseEnter()
    {
        image.sprite = sprites[0];
        image.color = colors[0];
        if (BattleManager.Instance.PlayerActionManager.SkillInfoSO != null)
        {
            selectUI.ActivateAreaTargets(this, BattleManager.Instance.PlayerActionManager.SkillInfoSO);
        }
    }
    public void PortraitMouseEnter()
    {
        image.sprite = sprites[0];
        image.color = colors[0];
    }
    public void Active()
    {
        image.sprite = sprites[0];
        image.color = colors[0];
    }


    public void MouseExit()
    {
        image.sprite = sprites[1];
        image.color = colors[1];
        if (BattleManager.Instance.PlayerActionManager.SkillInfoSO != null)
        {
            selectUI.DeactivateAreaTargets(this, BattleManager.Instance.PlayerActionManager.SkillInfoSO);
        }
    }
    public void PortraitMouseExit()
    {
        image.sprite = sprites[1];
        image.color = colors[1];
    }

    public void DeActiave()
    {
        image.sprite = sprites[1];
        image.color = colors[1];
    }
    

    private void OnDisable()
    {
        image.sprite = sprites[1];
        image.color = colors[1];
    }
}
