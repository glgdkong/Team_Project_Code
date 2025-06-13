using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryUI : MonoBehaviour
{
    public void NextStageButton()
    {
        // 대기 상태로
        //ChangeState(GameState.Stay);
        NodeMapSceneMoveManager nodeManager = GameObject.Find("NodeMapSceneMoveManagerObject").GetComponent<NodeMapSceneMoveManager>();
        switch (CharacterLists.mapModEnum)
        {
            case MapModEnum.LASTBOSS:
                Destroy(PartyManager.Instance.gameObject);
                NodeSceneStartManager.Defeat();
                //StageLoader.Instance.BattleTeamReset();
                //ChoiceCanvasScript.Instance.EventReset();
                SceneManager.LoadScene("CreditScene");
                break;
            default:
                //노드맵 씬으로 변경
                nodeManager.ShowNodeMap();
                nodeManager.SceneMoveNodeMap("minsudevelop");
                break;
        }
    }

    public void RequestRewardScreen()
    {
        NodeMapSceneMoveManager nodeManager = GameObject.Find("NodeMapSceneMoveManagerObject").GetComponent<NodeMapSceneMoveManager>();
        switch (CharacterLists.mapModEnum)
        {
            case MapModEnum.LASTBOSS:
                Destroy(PartyManager.Instance.gameObject);
                NodeSceneStartManager.Defeat();
                //StageLoader.Instance.BattleTeamReset();
                //ChoiceCanvasScript.Instance.EventReset();
                SceneManager.LoadScene("CreditScene");
                break;
                //이벤트로 전투들어가서 보상을 얻는 경우, 여기에서 수정해야 함. (현재 7개 확정)
            case MapModEnum.EVENT:
                ItemManager.Instance.RewardScreenOpen(9);
                ItemSlotManager.Instance.OpenInventory();
                break;
            case MapModEnum.MIDDLEBOSS:
                ItemManager.Instance.RewardScreenOpen(5,9);
                ItemSlotManager.Instance.OpenInventory();
                break;
            default:
                ItemManager.Instance.RewardScreenOpen(null);
                ItemSlotManager.Instance.OpenInventory();
                break;
        }
        gameObject.SetActive(false);

    }

}
