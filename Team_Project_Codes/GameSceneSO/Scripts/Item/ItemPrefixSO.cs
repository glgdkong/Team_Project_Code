using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemPrefixInfo", menuName = "Item/Prefix")]
public class ItemPrefixSO : ScriptableObject
{
    [SerializeField, Header("접두사명")] private string prefixName;

    [SerializeField, Header("접두사의 아이템 유형")] private List<BaseItemEnum> availableBaseItem;
    //아이템 유형과 스프라이트를 쌍으로 매치.
    [SerializeField, Header("아이템 유형별 이미지")] private List<ItemSpriteAndImage> itemImage;

    [SerializeField, Header("접두사의 아이템 능력치 변화")] private List<StatModifierSO> modifiers;

    [SerializeField, Header("접두사가 붙었을 때의 설명"), TextArea] private string prefixDescription;

    public string PrefixName { get => prefixName; set => prefixName = value; }
    public List<BaseItemEnum> AvailableBaseItem { get => availableBaseItem; set => availableBaseItem = value; }
    public List<ItemSpriteAndImage> ItemImage { get => itemImage; set => itemImage = value; }
    public List<StatModifierSO> Modifiers { get => modifiers; set => modifiers = value; }
    public string PrefixDescription { get => prefixDescription; set => prefixDescription = value; }


    //베이스 아이템을 넣으면 스프라이트 반환
    public Sprite GetSpriteForType(BaseItemEnum type)
    {
        foreach (var item in itemImage)
        {
            if (item.baseItemType == type)
                return item.sprite;
        }
        return null;
    }

    //베이스 아이템의 이름을 넣으면 베이스 아이템의 이름을 설명에 추가함
    public string GetFormattedDescription(string baseItemName)
    {
        return PrefixDescription.Replace("{itemName}", baseItemName);
    }
}
