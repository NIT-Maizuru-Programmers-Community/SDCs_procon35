using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class GameExitManager : MonoBehaviour
{
    public void QuitGame()
    {
        Debug.Log("Quit button pressed.");  // デバッグ用ログ

        // 実際のアプリケーションを終了
        Application.Quit();

        // エディタで動作している場合は再生モードを停止
        #if UNITY_EDITOR
        EditorApplication.isPlaying = false;
        #endif
    }
}

/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEnd : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // 初期設定などがあればここに記述
    }

    // Update is called once per frame
    void Update()
    {
        // 毎フレームの処理が必要であればここに記述
    }

    // ボタンを押したときに呼び出すメソッド
    public void QuitGame()
    {
        Debug.Log("Quit button pressed.");  // デバッグ用ログ
        Application.Quit();  // ゲームを終了
    }
}*/
