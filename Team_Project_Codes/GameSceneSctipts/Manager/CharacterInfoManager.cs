using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CharacterInfoManager : MonoBehaviour
{
    [SerializeField] private CharacterInfoUI characterInfoUI;
    private GameObject target;
    private PlayableCharacter playableCharacter;

    private void Update()
    {
        if(BattleManager.Instance.TurnManager.GameState == GameState.Win || BattleManager.Instance.TurnManager.GameState == GameState.Lose) 
        {
            if (characterInfoUI.gameObject.activeSelf)
            {
                characterInfoUI.DisableButton();
            }
            return; 
        }
        if (!characterInfoUI.gameObject.activeSelf)
        {
            if (Input.GetMouseButtonDown(1))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);  // 마우스 위치에서 레이 생성
                RaycastHit hit;  // 충돌 정보를 담을 변수
                GameObject previousTarget = target;  // 이전 타겟 저장
                if (Physics.Raycast(ray, out hit))  // 레이가 충돌했는지 검사
                {
                    target = hit.collider.gameObject;  // 타겟 변수에 저장
                }
                else // 충돌한 오브젝트가 없다면
                {
                    target = null; // null 처리
                }

                if (target != null && target.CompareTag("Player"))
                {
                    //if (RewardScreenUIScript.Instance.IsRewardOpen()) return;
                    playableCharacter = target.GetComponent<PlayableCharacter>();
                    characterInfoUI.SetInfos(playableCharacter, (PlayableCharacterSO)playableCharacter.CharacterInfo);
                }
            }
        }
    }
}
