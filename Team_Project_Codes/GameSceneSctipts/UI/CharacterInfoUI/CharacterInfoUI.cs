using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterInfoUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI playerName;
    [SerializeField] private TextMeshProUGUI playerHealth;
    [SerializeField] private TextMeshProUGUI playerMana;
    [SerializeField] private TextMeshProUGUI playerSpeed;
    [SerializeField] private TextMeshProUGUI playerDef;
    [SerializeField] private TextMeshProUGUI playerEvd;
    [SerializeField] private TextMeshProUGUI playerAcc;
    [SerializeField] private TextMeshProUGUI playerPassive;
    [SerializeField] private TextMeshProUGUI playerInfo;
    [SerializeField] private GameObject playerSpawnParent;
    [SerializeField] private TextMeshProUGUI[] playerResistancesTexts;
    [SerializeField] private InformationSkillButtonUI[] informationSkillButtonUIs;
    [SerializeField] private FinalItemSlotView playerFirstItemSlot;
    [SerializeField] private FinalItemSlotView playerSecondItemSlot;
    [SerializeField] private GameObject inventoryToggleButton;
    [SerializeField] private SkillSelectionWarningUI skillSelectionWarningUI;


    private GameObject playerObject;

    [SerializeField] private Camera characterCamera;
    

    private PlayableCharacterSO playableCharacterSO;
    private PlayableCharacter playableCharacter;
    public PlayableCharacterSO PlayableCharacterSO  => playableCharacterSO;

    public FinalItemSlotView PlayerFirstItemSlot { get => playerFirstItemSlot; set => playerFirstItemSlot = value; }
    public FinalItemSlotView PlayerSecondItemSlot { get => playerSecondItemSlot; set => playerSecondItemSlot = value; }

    private bool isActive = false;

    private void Start()
    {
        if (gameObject.activeSelf && !isActive)
        {
            gameObject.SetActive(false);
        }
    }
    public void SetInfos(PlayableCharacterSO playableCharacterSO)
    {
        ItemStatType itemStatType = playableCharacterSO.TotalItemStat();

        isActive = true;
        this.playableCharacterSO = playableCharacterSO;
        playerName.text = playableCharacterSO.CharacterName;
        playerHealth.text = (playableCharacterSO.CharacterHp+itemStatType.Hp).ToString();
        playerMana.text =  (playableCharacterSO.Mana + itemStatType.Mana).ToString();
        playerSpeed.text = (playableCharacterSO.CharacterSpd + itemStatType.Speed).ToString();
        playerDef.text = (playableCharacterSO.CharacterDef + itemStatType.Defense).ToString();
        playerEvd.text = ((playableCharacterSO.CharacterEvd + itemStatType.Evade) * 100).ToString() + "%";
        playerAcc.text = ((playableCharacterSO.CharacterAcc + itemStatType.Accuracy) * 100).ToString()+"%";
        playerPassive.text = playableCharacterSO.PassiveSkillText;
        playerInfo.text = playableCharacterSO.CharacterInfoText;
        if (playerFirstItemSlot != null)
        {
            if(playableCharacterSO.FirstItem != null) playerFirstItemSlot.SlotData.CurrentItem = playableCharacterSO.FirstItem;
            playerFirstItemSlot.SlotData.ItemSlotType = ItemSlotType.CharacterEquip;
            playerFirstItemSlot.UpdateUI();
        }
        else
        {
            Debug.Log("아이템 슬롯 없음");
        }
        if (playerSecondItemSlot != null)
        {
            if (playableCharacterSO.SecondItem != null) playerSecondItemSlot.SlotData.CurrentItem = playableCharacterSO.SecondItem;
            playerSecondItemSlot.SlotData.ItemSlotType = ItemSlotType.CharacterEquip;
            playerSecondItemSlot.UpdateUI();
        }
        else
        {
            Debug.Log("아이템 슬롯 없음");
        }



        // 스킬과 저항 처리
        ProcessSkillAndResistance(playableCharacterSO, itemStatType);

        // 오브젝트 생성
        InstantiateCharacterObject(playableCharacterSO);

        inventoryToggleButton.SetActive(true);

        gameObject.SetActive(true);

    }
    public void SetInfos(PlayableCharacter playableCharacter, PlayableCharacterSO playableCharacterSO)
    {
        ItemStatType itemStatType = playableCharacterSO.TotalItemStat();
        isActive = true;
        this.playableCharacterSO = playableCharacterSO;
        this.playableCharacter = playableCharacter;
        playerName.text = playableCharacterSO.CharacterName;
        playerHealth.text = playableCharacter.CurrentHp +"/"+ (playableCharacterSO.CharacterHp + itemStatType.Hp).ToString();
        playerMana.text = playableCharacter.CurrentMana + "/" + (playableCharacterSO.Mana + itemStatType.Mana).ToString();
        playerSpeed.text = (playableCharacterSO.CharacterSpd + itemStatType.Speed).ToString();
        playerDef.text = (playableCharacterSO.CharacterDef + itemStatType.Defense).ToString();
        playerEvd.text = ((playableCharacterSO.CharacterEvd + itemStatType.Evade)* 100).ToString() + "%";
        playerAcc.text = ((playableCharacterSO.CharacterAcc + itemStatType.Accuracy) * 100).ToString() + "%";
        playerPassive.text = playableCharacterSO.PassiveSkillText;
        playerInfo.text = playableCharacterSO.CharacterInfoText;

        if (playerFirstItemSlot != null)
        {
            if (playableCharacterSO.FirstItem != null) playerFirstItemSlot.SlotData.CurrentItem = playableCharacterSO.FirstItem;
            //playerFirstItemSlot.GetComponent<Image>().raycastTarget = false;
            playerFirstItemSlot.SlotData.ItemSlotType = ItemSlotType.CharacterEquip;
            playerFirstItemSlot.UpdateUI();
        }
        else
        {
            Debug.Log("아이템 슬롯 없음");
        }
        if (playerSecondItemSlot != null)
        {
            if (playableCharacterSO.SecondItem != null) playerSecondItemSlot.SlotData.CurrentItem = playableCharacterSO.SecondItem;
            //playerSecondItemSlot.GetComponent<Image>().raycastTarget = false;
            playerSecondItemSlot.SlotData.ItemSlotType = ItemSlotType.CharacterEquip;
            playerSecondItemSlot.UpdateUI();
        }
        else
        {
            Debug.Log("아이템 슬롯 없음");
        }

        // 스킬과 저항 처리
        ProcessSkillAndResistance(playableCharacterSO, itemStatType);

        // 오브젝트 생성
        InstantiateCharacterObject(playableCharacterSO);

        inventoryToggleButton.SetActive(false);

        gameObject.SetActive(true);
    }

    // 스킬과 저항 처리
    private void ProcessSkillAndResistance(PlayableCharacterSO playableCharacterSO, ItemStatType itemStatType)
    {
        // 스킬 버튼과 캐릭터가 가지고 있는 스킬 갯수 비교
        int minCount = Mathf.Min(informationSkillButtonUIs.Length, playableCharacterSO.SkillInfoSOs.Count);
        // 작은 값 만큼 반복
        for (int i = 0; i < minCount; i++)
        {
            // 해당 버튼에 스킬정보와 캐릭터 정보 부여
            informationSkillButtonUIs[i].SetSkillInfos(playableCharacterSO.SkillInfoSOs[i], playableCharacterSO);
        }

        for (int i = 0; i < playableCharacterSO.Resistances.Length; i++)
        {
            float resValue = 0;
            resValue += playableCharacterSO.Resistances[i].ResistanceValue;

            if (itemStatType.ResistanceValues.ContainsKey(playableCharacterSO.Resistances[i].StatusType))
            {
                resValue += itemStatType.ResistanceValues[playableCharacterSO.Resistances[i].StatusType];
            }
            playerResistancesTexts[i].text = ((int)(resValue * 100)).ToString();
        }
    }

    // 오브젝트 생성
    private void InstantiateCharacterObject(PlayableCharacterSO playableCharacterSO)
    {
        playerObject = Instantiate(playableCharacterSO.CharacterPrefab, Vector3.zero, Quaternion.identity, playerSpawnParent.transform);
        playerObject.GetComponent<PlayableCharacter>().SetCharacterInfo(playableCharacterSO);
        playerObject.transform.localPosition = Vector3.zero;
    }


    public void DisableButton()
    {
        if(playableCharacterSO == null ||playableCharacterSO.SelectedSkillInfos.Count >= 5)
        {
            playableCharacterSO = null;
            foreach (InformationSkillButtonUI skillInfoUI in informationSkillButtonUIs)
            {
                skillInfoUI.DeletInfo();
            }
            gameObject.SetActive(false);
        }
        else
        {
            //Debug.Log("");
            skillSelectionWarningUI?.ShowSkillSelectionWarning();
        }
    }
    private void OnDisable()
    {
        if (playerObject != null)
        {
            Destroy(playerObject);
        }
        if (ItemSlotManager.Instance != null)
        {
            
            if (SceneManager.GetActiveScene() != SceneManager.GetSceneByName("GameScene")) ItemSlotManager.Instance.CloseInventory();
            ItemSlotManager.Instance.OpenCharacter = null;
            ItemSlotManager.Instance.CharacterInfoUI = null;
            /*
            PlayerFirstItemSlot.SlotData.ItemSlotType = ItemSlotType.CharacterEquip;
            PlayerSecondItemSlot.SlotData.ItemSlotType = ItemSlotType.CharacterEquip;
            PlayerFirstItemSlot.ViewReset();
            PlayerSecondItemSlot.ViewReset();
            RefreshInfoUI();
            */
        }
        else
        {
            Debug.Log("아이템 슬롯 매니저 없음");
        }
    }

    public void SkillIndexUpdate()
    {
        foreach (InformationSkillButtonUI skillInfoUI in informationSkillButtonUIs)
        {
            skillInfoUI.SkillIndexUpdate();
        }
    }

    private void OnEnable()
    {
        //열릴때 마다, 현재 존재하는 인벤토리 매니저에 현재 열려있는 캐릭터의 스크립터블 오브젝트를 보낸다.
        //자기 자신도 보낸다.
        if(ItemSlotManager.Instance != null)
        {
            //캐릭터 인포를 열었으니, 인벤토리도 같이 열어달라는 지시를 보낸다.
            if(SceneManager.GetActiveScene() != SceneManager.GetSceneByName("GameScene")) ItemSlotManager.Instance.OpenInventory();
            ItemSlotManager.Instance.OpenCharacter = PlayableCharacterSO;
            ItemSlotManager.Instance.CharacterInfoUI = this;
            playerFirstItemSlot.SlotData.ItemSlotType = ItemSlotType.CharacterEquip;
            playerSecondItemSlot.SlotData.ItemSlotType = ItemSlotType.CharacterEquip;
            playerFirstItemSlot.ViewReset();
            playerSecondItemSlot.ViewReset();
            RefreshInfoUI();
        }
        else
        {
            Debug.Log("아이템 슬롯 매니저 없음");
        }
    }

    //아이템 장착 시 새로고침용으로 만들었음.
    public void RefreshInfoUI()
    {
        if(playableCharacterSO == null) return;
        //혹시나 이걸 봤다면 정보 새로고침 함수좀....

        if (playerFirstItemSlot != null)
        {
            if (playableCharacterSO.FirstItem != null)
            {
                playerFirstItemSlot.SlotData.CurrentItem = playableCharacterSO.FirstItem;
            }
            playerFirstItemSlot.SlotData.ItemSlotType = ItemSlotType.CharacterEquip;
            playerFirstItemSlot.UpdateUI();
        }
        else
        {
            Debug.Log("아이템 슬롯 없음");
        }
        if (playerSecondItemSlot != null)
        {
            if (playableCharacterSO.SecondItem != null) playerSecondItemSlot.SlotData.CurrentItem = playableCharacterSO.SecondItem;
            playerSecondItemSlot.SlotData.ItemSlotType = ItemSlotType.CharacterEquip;
            playerSecondItemSlot.UpdateUI();
        }
        else
        {
            Debug.Log("아이템 슬롯 없음");
        }

        ItemStatType itemStatType = playableCharacterSO.TotalItemStat();

        isActive = true;
        playerName.text = playableCharacterSO.CharacterName;

        if (SceneManager.GetActiveScene().name == "GameScene")
        {
            playerHealth.text = playableCharacter.CurrentHp + "/" + (playableCharacterSO.CharacterHp + itemStatType.Hp).ToString();
            playerMana.text = playableCharacter.CurrentMana + "/" + (playableCharacterSO.Mana + itemStatType.Mana).ToString();
        }
        else
        {
            playerHealth.text = (playableCharacterSO.CharacterHp + itemStatType.Hp).ToString();
            playerMana.text = (playableCharacterSO.Mana + itemStatType.Mana).ToString();

        }
        
        playerSpeed.text = (playableCharacterSO.CharacterSpd + itemStatType.Speed).ToString();
        playerDef.text = (playableCharacterSO.CharacterDef + itemStatType.Defense).ToString();
        playerEvd.text = ((playableCharacterSO.CharacterEvd + itemStatType.Evade) * 100).ToString() + "%";
        playerAcc.text = ((playableCharacterSO.CharacterAcc + itemStatType.Accuracy) * 100).ToString() + "%";
        playerPassive.text = playableCharacterSO.PassiveSkillText;
        playerInfo.text = playableCharacterSO.CharacterInfoText;
        // 스킬과 저항 처리
        ProcessSkillAndResistance(playableCharacterSO, itemStatType);

        Debug.Log($"현재 캐릭터 장비창의 인식 상태 : 슬롯 1 :{playerFirstItemSlot.SlotData.ItemSlotType}, 슬롯 2 : {playerSecondItemSlot.SlotData.ItemSlotType}");

    }


}
