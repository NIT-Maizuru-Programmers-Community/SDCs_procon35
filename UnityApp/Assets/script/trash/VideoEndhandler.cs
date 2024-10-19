using UnityEngine;
using UnityEngine.Video;

public class VideoEndHandler : MonoBehaviour
{
    public VideoPlayer videoPlayer; // Video Player コンポーネント
    public Camera videoCamera; // 動画再生用のカメラ
    public Camera mainCamera; // 元のメインカメラ

    private bool videoEnded = false; // 動画の終了を追跡するフラグ

    void Start()
    {
        // 初期設定: メインカメラを無効化し、動画カメラを有効化
        mainCamera.enabled = false;
        videoCamera.enabled = true;

        // 動画の再生終了を検知するイベントを登録
        videoPlayer.loopPointReached += OnVideoEnd;
    }

    void Update()
    {
        // エンターキーが押されたときにカメラを切り替え
        if (Input.GetKeyDown(KeyCode.Return))
        {
            SwitchToMainCamera();
        }
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        videoEnded = true; // 動画が終了したことを記録
        SwitchToMainCamera();
    }

    void SwitchToMainCamera()
    {
        if (!videoEnded) return; // 動画が終了していない場合は実行しない

        // 動画カメラを無効化してメインカメラを有効化
        videoCamera.enabled = false;
        mainCamera.enabled = true;
    }
}

