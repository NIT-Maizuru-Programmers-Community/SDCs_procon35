using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class GameStartManager : MonoBehaviour
{
    public Button startButton; // スタートボタン
    public GameObject uiSet; // 背景画像とボタンを含むUIセット
    public VideoPlayer videoPlayer; // 動画再生用のVideoPlayer

    void Start()
    {
        // 初期設定: スタート画面 (UIセット) を表示
        if (uiSet != null)
        {
            uiSet.SetActive(true); // 最初はUIセットが表示された状態
        }

        // 動画を非アクティブにする (スタートボタンが押されたら再生するため)
        if (videoPlayer != null)
        {
            videoPlayer.gameObject.SetActive(false); // 最初は非表示にする
        }

        // ボタンのクリックイベントを設定
        if (startButton != null)
        {
            startButton.onClick.AddListener(OnStartButtonClicked);
        }

        // 動画の再生終了を検知するイベントを登録
        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached += OnVideoEnd;
        }
    }

    public void OnStartButtonClicked()
    {
        Debug.Log("Start button clicked.");  // デバッグ用のログ

        // UIセットを非表示にする
        if (uiSet != null)
        {
            uiSet.SetActive(false); // UIを非表示にする
        }

        // 動画をアクティブにして再生開始
        if (videoPlayer != null)
        {
            videoPlayer.gameObject.SetActive(true); // 動画オブジェクトを表示
            videoPlayer.Play(); // 動画を再生
        }
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        // 動画が終了したらメインシーンに移動
        SceneManager.LoadScene("SampleScene"); // SampleSceneはメインシーンの名前に変更してください
    }
}    

/*
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class GameStartManager : MonoBehaviour
{
    public Button startButton; // スタートボタン
    public GameObject uiSet; // 背景画像とボタンを含むUIセット
    public VideoPlayer videoPlayer; // 動画再生用のVideoPlayer

    void Start()
    {
        // 初期設定: スタート画面 (UIセット) を表示
        if (uiSet != null)
        {
            uiSet.SetActive(true); // 最初はUIセットが表示された状態
        }

        // 動画を非アクティブにする (スタートボタンが押されたら再生するため)
        if (videoPlayer != null)
        {
            videoPlayer.gameObject.SetActive(false); // 最初は非表示にする
        }

        // ボタンのクリックイベントを設定
        if (startButton != null)
        {
            startButton.onClick.AddListener(OnStartButtonClicked);
        }

        // 動画の再生終了を検知するイベントを登録
        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached += OnVideoEnd;
        }
    }

    private void Update()
    {
        // キーボードのPキーでシーンを直接ロード
        if (Input.GetKeyDown(KeyCode.P))
        {
            SceneManager.LoadScene("SampleScene");      
        }

        // スペースキーで動画をスキップしてシーンをロード
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SkipVideo();
        }
    }

    public void OnStartButtonClicked()
    {
        Debug.Log("Start button clicked.");  // デバッグ用のログ

        // UIセットを非表示にする
        if (uiSet != null)
        {
            uiSet.SetActive(false); // UIを非表示にする
        }

        // 動画をアクティブにして再生開始
        if (videoPlayer != null)
        {
            videoPlayer.gameObject.SetActive(true); // 動画オブジェクトを表示
            videoPlayer.Play(); // 動画を再生
        }
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        // 動画が終了したらメインシーンに移動
        LoadMainScene();
    }

    // 動画をスキップしてシーンをロードするメソッド
    public void SkipVideo()
    {
        Debug.Log("Video skipped."); // デバッグ用のログ

        if (videoPlayer != null && videoPlayer.isPlaying)
        {
            videoPlayer.Stop(); // 動画を停止
        }

        LoadMainScene();
    }

    // メインシーンをロードするメソッド
    public void LoadMainScene()
    {
        SceneManager.LoadScene("SampleScene"); // SampleSceneはメインシーンの名前に変更してください
    }
}*/



/*
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class GameStartManager : MonoBehaviour
{
    public Button startButton; // スタートボタン
    public GameObject uiSet; // 背景画像とボタンを含むUIセット
    public VideoPlayer videoPlayer; // 動画再生用のVideoPlayer

    void Start()
    {
        // 初期設定: スタート画面 (UIセット) を表示
        if (uiSet != null)
        {
            uiSet.SetActive(true); // 最初はUIセットが表示された状態
        }

        // 動画を非アクティブにする (スタートボタンが押されたら再生するため)
        if (videoPlayer != null)
        {
            videoPlayer.gameObject.SetActive(false); // 最初は非表示にする
        }

        // ボタンのクリックイベントを設定
        if (startButton != null)
        {
            startButton.onClick.AddListener(OnStartButtonClicked);
        }

        // 動画の再生終了を検知するイベントを登録
        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached += OnVideoEnd;
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            SceneManager.LoadScene("SampleScene");      
        }
    }

    public void OnStartButtonClicked()
    {
        Debug.Log("Start button clicked.");  // デバッグ用のログ

        // UIセットを非表示にする
        if (uiSet != null)
        {
            uiSet.SetActive(false); // UIを非表示にする
        }

        // 動画をアクティブにして再生開始
        if (videoPlayer != null)
        {
            videoPlayer.gameObject.SetActive(true); // 動画オブジェクトを表示
            videoPlayer.Play(); // 動画を再生
        }
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        // 動画が終了したらメインシーンに移動
        SceneManager.LoadScene("SampleScene"); // SampleSceneはメインシーンの名前に変更してください
    }
}*/


/*
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class GameStartManager : MonoBehaviour
{
    public Button startButton; // スタートボタン
    public GameObject uiSet; // 背景画像とボタンを含むUIセット
    public VideoPlayer videoPlayer; // 動画再生用のVideoPlayer

    void Start()
    {
        // 初期設定: UIセットを無効化
        if (uiSet != null)
        {
            uiSet.SetActive(false);
        }

        // ボタンのクリックイベントを設定
        if (startButton != null)
        {
            startButton.onClick.AddListener(OnStartButtonClicked);
        }

        // 動画の再生終了を検知するイベントを登録
        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached += OnVideoEnd;
        }
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        // 動画が終了したらUIセットを表示
        if (uiSet != null)
        {
            uiSet.SetActive(true);
        }
    }

    public void OnStartButtonClicked()
    {
        Debug.Log("Start button clicked.");  // デバッグ用のログ
        // メインシーンに移動
        SceneManager.LoadScene("SampleScene"); // SampleSceneはメインシーンの名前に変更してください
    }
}
*/