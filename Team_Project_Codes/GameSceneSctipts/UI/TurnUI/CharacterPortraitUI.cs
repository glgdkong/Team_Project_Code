using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class CharacterPortraitUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image characterImage;
    [SerializeField] private CharacterInfoSO characterInfo;
    [SerializeField] private Character character; 
    [SerializeField] private SelectUI characterSelect;
    [SerializeField] private RectTransform rectTransform;

    public Image CharacterImage => characterImage;

    public CharacterInfoSO CharacterInfo  => characterInfo;
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }
    public void ShowCharacterImage(Character character)
    {
        gameObject.SetActive(true);
        characterInfo = character.CharacterInfo;
        this.character = character;
        characterImage.sprite = character.CharacterInfo.CharacterImage;
    }

    public void DisableUI()
    {
        characterImage.sprite = null;
        characterInfo = null;
        character = null;
        gameObject.SetActive(false);
    }
    public void DeletInfo()
    {
        characterInfo = null;
        gameObject.SetActive(false);
        rectTransform.SetAsLastSibling();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        characterSelect.DisplayCharLocation(character);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        characterSelect.DisableDisplayCharLocation();
    }
}
