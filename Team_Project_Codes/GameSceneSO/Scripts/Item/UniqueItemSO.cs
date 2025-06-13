using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UniqueItemInfo", menuName = "Item/UniqueItem")]
public class UniqueItemSO : ScriptableObject
{
    [SerializeField, Header("아이템 유형")] private BaseItemEnum baseItemType;
    [SerializeField, Header("유니크 아이템 이름")] private string uniqueItemName;
    [SerializeField, Header("유니크 아이템 능력치 변화")] private List<StatModifierSO> modifiers;
    [SerializeField, Header("유니크 아이템의 스프라이트")] private Sprite uniqueItemSprite;
    [SerializeField, Header("유니크 아이템의 설명"), TextArea] private string uniqueItemDescription;

    public BaseItemEnum BaseItemType { get => baseItemType; set => baseItemType = value; }
    public string UniqueItemName { get => uniqueItemName; set => uniqueItemName = value; }
    public List<StatModifierSO> Modifiers { get => modifiers; set => modifiers = value; }
    public Sprite UniqueItemSprite { get => uniqueItemSprite; set => uniqueItemSprite = value; }
    public string UniqueItemDescription { get => uniqueItemDescription; set => uniqueItemDescription = value; }
}

