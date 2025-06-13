using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetManager : MonoBehaviour
{
    [SerializeField] private TargetInfoPrintUI targetUIManager;
    private GameObject target;

    public GameObject Target { get => target; set => target = value; }
    public TargetInfoPrintUI TargetUIManager { get => targetUIManager; set => targetUIManager = value; }

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);  // 마우스 위치에서 레이 생성
        RaycastHit hit;  // 충돌 정보를 담을 변수
        GameObject previousTarget = target;  // 이전 타겟 저장
        if (Physics.Raycast(ray, out hit))  // 레이가 충돌했는지 검사
        {
            if (hit.collider.gameObject.CompareTag("Enemy")) // 충돌한 게임오브젝트의 태그가 에너미라면
            {
                target = hit.collider.gameObject;  // 타겟 변수에 저장
            }
            else // 아니라면
            {
                target = null; // 타겟 널처리
            }
        }
        else // 충돌한 오브젝트가 없다면
        { 
            target = null; // null 처리
        }




        // 타겟이 변경되었을 때만 UI 업데이트
        if (target != previousTarget)
        {
            // 타겟 게임오브젝트가 캐릭터 컴포넌트를 가지고 있다면
            // 타겟 UI매니저에 정보 전달
            targetUIManager.TargetInfo(target?.GetComponent<Character>());

            BattleManager.Instance.PlayerActionManager.PlayerActionUI.SkillTargetInfoUI.GetEnemyInfo(target?.GetComponent<EnemyCharacter>());
        }
    }
}
