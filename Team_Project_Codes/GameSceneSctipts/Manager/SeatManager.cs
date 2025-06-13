using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class SeatManager : MonoBehaviour
{
    [SerializeField] private Transform[] playerPositions;
    [SerializeField] private Transform[] enemyPositions;
    [SerializeField] private float moveTime;

    public Transform[] PlayerPositions => playerPositions;
    public Transform[] EnemyPositions => enemyPositions; 
    // 캐릭터 배치 초기화
    public void InitializePositions<T>(List<T> characters) where T : Character
    {
        for (int i = 0; i < characters.Count; i++)
        {
            Transform[] positions = (characters[i].CharacterInfo.CharacterType == CharacterType.Player) ? playerPositions : enemyPositions;  // 이넘을 사용하여 구분
            characters[i].transform.position = positions[i].position;  // 자리 배치
            characters[i].CurrentPositionIndex = i;  // 캐릭터 위치 인덱스 저장
        }
    }


    // 캐릭터 이동 처리
    // 이동할 캐릭터와 이동할 위치 인덱스
    public bool MoveCharacterToSlot<T>(T character, int targetSlotIndex) where T : Character
    {
        int currentSlotIndex = -1;
        Transform[] targetPositions = null; // 위치 배열
        List<T> characterList = null;

        // 캐릭터 유형에 따라 플레이어나 적의 위치 배열을 설정
        switch (character.CharacterInfo.CharacterType)
        {
            case CharacterType.Player:
                targetPositions = playerPositions;
                characterList = (List<T>)(object)BattleManager.Instance.PlayerCharacterList;
                break;

            case CharacterType.Enemy:
                targetPositions = enemyPositions;
                characterList = (List<T>)(object)BattleManager.Instance.EnemyCharacterList;
                break;

            default:
                return false; // 알 수 없는 캐릭터 유형
        }
        // 현재 캐릭터가 있는 슬롯 인덱스를 찾기
        currentSlotIndex = characterList.IndexOf(character);
        // 이동할 슬롯과 현재 슬롯이 같다면
        if (currentSlotIndex == targetSlotIndex)
        {
            // 이동 실패
            return false;
        }
        else
        {
            int moveIndex;
            // 자리 이동하기 전에 밀어낼 캐릭터들 처리
            if (currentSlotIndex < targetSlotIndex) // 앞에서 뒤로 이동
            {
                moveIndex = Mathf.Min(targetSlotIndex, characterList.Count-1);
                for (int i = currentSlotIndex; i < moveIndex; i++)
                {
                    int pos = i + 1 >= characterList.Count ? characterList.Count - 1 : i + 1;
                    characterList[i] = characterList[pos];
                }
                characterList[moveIndex] = character;
            }
            else // 뒤에서 앞으로 이동
            {
                moveIndex = Mathf.Max(targetSlotIndex, 0);
                for (int i = currentSlotIndex; i >= moveIndex; i--)
                {
                    int pos = i - 1 < 0 ? 0 : i - 1;
                    characterList[i] = characterList[pos];
                }
                characterList[moveIndex] = character;
            }
            // 이동 애니메이션을 위한 코루틴 호출
            StartCoroutine(MoveCharactersCoroutine(characterList, targetPositions, moveTime)); // 여러 캐릭터 이동 애니메이션 실행


           
            return true;
        }

    }
    // 캐릭터 이동 처리
    // 이동할 캐릭터와 이동할 위치 인덱스
    public void ForceMoveCharacterToSlot(Character character, int moveValue, bool isMovingBackward) 
    {
        if (character.IsDeath) { return; }
        int moveIndex;
        switch (character)
        {
            case PlayableCharacter playableCharacter:
                moveIndex = isMovingBackward ? playableCharacter.CurrentPositionIndex + moveValue : playableCharacter.CurrentPositionIndex - moveValue;
                MoveCharacterToSlot(playableCharacter, moveIndex);
                break;
            case EnemyCharacter enemyCharacter:
                moveIndex = isMovingBackward ? enemyCharacter.CurrentPositionIndex + moveValue : enemyCharacter.CurrentPositionIndex - moveValue;
                MoveCharacterToSlot(enemyCharacter, moveIndex);
                break;
            default:
                break;
        }


    }
    // 여러 캐릭터가 동시에 이동하는 코루틴
    public IEnumerator MoveCharactersCoroutine<T>(List<T> characters, Transform[] targetPositions, float duration) where T : Character
    {
        float elapsedTime = 0f;
        List<Vector3> startPositions = new List<Vector3>();

        // 각 캐릭터의 시작 위치 저장
        foreach (var character in characters)
        {
            startPositions.Add(character.transform.position);
        }

        // 여러 캐릭터들을 부드럽게 이동시키는 애니메이션
        while (elapsedTime < duration)
        {
            for (int i = 0; i < characters.Count; i++)
            {
                // 각 캐릭터를 Lerp로 부드럽게 이동
                characters[i].transform.position = Vector3.Lerp(startPositions[i], targetPositions[i].position, elapsedTime / duration);
            }

            elapsedTime += Time.deltaTime;
            yield return null; // 다음 프레임까지 대기
        }

        InitializePositions(characters);
    }

    public void CharacterDeath<T>(T character) where T : Character
    {
        Transform[] targetPositions = null; // 위치 배열
        List<T> characterList = null;

        // 캐릭터 유형에 따라 플레이어나 적의 위치 배열을 설정
        switch (character.CharacterInfo.CharacterType)
        {
            case CharacterType.Player:
                targetPositions = playerPositions;
                characterList = (List<T>)(object)BattleManager.Instance.PlayerCharacterList;
                break;

            case CharacterType.Enemy:
                targetPositions = enemyPositions;
                characterList = (List<T>)(object)BattleManager.Instance.EnemyCharacterList;
                break;

            default:
                break; // 알 수 없는 캐릭터 유형
        }

        StartCoroutine(MoveCharactersCoroutine(characterList, targetPositions, moveTime));
    }
}
