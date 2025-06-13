using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class TurnUI : MonoBehaviour
{
    [SerializeField] private TurnManager turnManager; // 턴 매니저 
    [SerializeField] private ScrollRect scrollRect;   // 턴 UI 표시 스크롤 렉트
    [SerializeField] private RectTransform content;   // 캐릭터 초상화 부모 게임오브젝트
    [SerializeField] private GameObject characterPortrai;  // 캐릭터 초상화 게임오브젝트
    [SerializeField] private float scrollSpeed;     // 스크롤 속도 15
    [SerializeField] private List<CharacterPortraitUI> characterPortraitUIs; // 캐릭터 초상화 UI 리스트
    [SerializeField] private float portraitSize;
    [SerializeField] private float spacingSize;
    [SerializeField] private TextMeshProUGUI roundCount;

    public void PrintTurnUI(LinkedList<Character> turnQueue)
    {
        List<Character> characters = new List<Character>(turnQueue);
        
        for (int i = 0; i < characters.Count; i++)
        {
            AddTurnUI(characters[i]);
        }
    }

    public void NextTurnUpdate(Character currentCharacter)
    {
        // 이동해야 할 값 계산
        float moveAmount = portraitSize + spacingSize;
        /*
        // ScrollRect의 Content를 위로 이동
        Vector2 newPosition = scrollRect.content.anchoredPosition;
        newPosition.y += moveAmount;*/

        // ScrollRect의 Content를 옆으로 이동
        Vector2 newPosition = scrollRect.content.anchoredPosition;
        newPosition.x += moveAmount;

        // 부드럽게 이동 (Lerp 사용)
        StopAllCoroutines();
        StartCoroutine(SmoothScroll(newPosition, currentCharacter));

    }


    private IEnumerator SmoothScroll(Vector2 targetPos, Character currentCharacter)
    {
        while (Vector2.Distance(scrollRect.content.anchoredPosition, targetPos) > 0.1f)
        {
            scrollRect.content.anchoredPosition = Vector2.Lerp(scrollRect.content.anchoredPosition, targetPos, Time.deltaTime * scrollSpeed);
            yield return null;
        }

        // 최종 목표 위치에 정확히 도달하게 함
        scrollRect.content.anchoredPosition = targetPos;
        CharacterPortraitUI characterPortraitUI = characterPortraitUIs.Find(characterPortraitUI => characterPortraitUI.CharacterInfo == currentCharacter.CharacterInfo);
        /*if (characterPortraitUI == null) Debug.LogWarning("초상화 오류");
        else Debug.Log("초상화 찾음");*/
        characterPortraitUI.DeletInfo(); 
        if (characterPortraitUIs.Remove(characterPortraitUI))
        {
            characterPortraitUIs.Add(characterPortraitUI); // 맨 뒤로 이동
        }

        //scrollRect.verticalNormalizedPosition = 1f; // 최상단  
        scrollRect.horizontalNormalizedPosition = 1f; // 최상단  
        
    }

    public void ResetTurnUI()
    {
        foreach (CharacterPortraitUI characterPortraitUI in characterPortraitUIs)
        {
            characterPortraitUI.DisableUI();
        }
        //scrollRect.verticalNormalizedPosition = 1f; // 최상단  
        scrollRect.horizontalNormalizedPosition = 1f; // 최상단  
    }
    public void AddTurnUI(Character character, bool isLast = false)
    {
        // 1. 기존의 빈 슬롯을 찾아 재사용
        foreach (CharacterPortraitUI characterPortraitUI in characterPortraitUIs)
        {
            if (!characterPortraitUI.gameObject.activeSelf || characterPortraitUI.CharacterInfo == null) // 비활성화된 슬롯 발견
            {
                characterPortraitUI.ShowCharacterImage(character);
                /*
                if (isLast)
                {
                    characterPortraitUI.GetComponent<RectTransform>().SetAsLastSibling();
                }*/
                return;
            }
        }
        // 2. 모든 슬롯이 차 있으면 새로 생성 후 리스트에 추가
        CharacterPortraitUI newcharacterPortraitUI = Instantiate(characterPortrai, content).GetComponent<CharacterPortraitUI>();
        newcharacterPortraitUI.ShowCharacterImage(character);
        characterPortraitUIs.Add(newcharacterPortraitUI);

    }

    public void DisableCharacterPortrait(Character character)
    {
        characterPortraitUIs.Find(characterPortraitUI => characterPortraitUI.CharacterInfo == character.CharacterInfo).gameObject.SetActive(false);
    }

    public void CurrentRoundUpdate(int roundCount) 
    {
        this.roundCount.text = roundCount + " 라운드";
    }


}
