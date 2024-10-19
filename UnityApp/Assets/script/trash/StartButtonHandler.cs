using UnityEngine;
using UnityEngine.SceneManagement;

public class StartButtonHandler : MonoBehaviour
{
    public void OnStartButtonClicked()
    {
        // "MainScene" はゲームのメインシーン名に置き換えてください
        SceneManager.LoadScene("MainScene");
    }
}
