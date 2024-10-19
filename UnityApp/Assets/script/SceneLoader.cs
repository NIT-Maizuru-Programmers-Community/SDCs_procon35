using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    public Button startButton;  // ボタンをInspectorで設定
    public Button EndtoStartButton;

    void Start()
    {
        // ボタンのクリックイベントを設定
        if (startButton != null)
        {
            startButton.onClick.AddListener(OnStartButtonClicked);
        }
        if(EndtoStartButton != null)
        {   
            EndtoStartButton.onClick.AddListener(OnStartButtonClicked);
        }
        else
        {
            Debug.LogWarning("Start button is not assigned in the Inspector.");
        }
    }

    public void OnStartButtonClicked()
    {
        Debug.Log("Start button clicked.");  // デバッグ用のログ
        SceneManager.LoadScene("GameStartScene");
    }
}


/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneLoader : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
*/