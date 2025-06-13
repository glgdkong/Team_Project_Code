using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DefeatUI : MonoBehaviour
{
    public void ReturnToMainMenuButton()
    {
        Destroy(PartyManager.Instance.gameObject);
        NodeSceneStartManager.Defeat();
        //StageLoader.Instance.BattleTeamReset();
        //ChoiceCanvasScript.Instance.EventReset();
        SceneManager.LoadScene("StartScene");
    }

    public void ExitButton()
    {
#if UNITY_EDITOR
        // 에디터에서 게임을 종료
        EditorApplication.isPlaying = false;
#else
        // 빌드본에서는 게임을 종료
        Application.Quit();
#endif
    }
}
