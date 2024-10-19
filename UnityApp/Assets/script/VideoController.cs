//10月17日 夜時点で正しく動作
using UnityEngine;
using UnityEngine.UI;      // RawImage のために必要
using UnityEngine.Video;   // VideoPlayer のために必要
using System.Collections.Generic;  // List を使用するために必要

public class VideoController : MonoBehaviour
{
    public CityManager cityManager;
    public VideoPlayer videoPlayer;        // VideoPlayer コンポーネントの参照
    public RawImage rawImage;              // 動画を表示するための RawImage の参照
    public List<VideoClip> videoClips;     // 再生する動画のリスト

    private void Start()
    {
        // VideoPlayer または RawImage が設定されていない場合、エラーメッセージを出力
        if (videoPlayer == null)
        {
            Debug.LogError("VideoPlayer が設定されていません。Inspector で設定してください。");
        }
        
        if (rawImage == null)
        {
            Debug.LogError("RawImage が設定されていません。Inspector で設定してください。");
        }
        else
        {
            rawImage.gameObject.SetActive(false);  // 初期状態で RawImage を非表示にする
        }

        if (videoPlayer != null)
        {
            // 動画の再生終了イベントを登録
            videoPlayer.loopPointReached += OnVideoEnd;
        }
    }



    public void PlayVideo(int index)
    {
        // 指定されたインデックスがリスト範囲内か確認
        if (index >= 0 && index < videoClips.Count)
        {
            if (videoPlayer != null && rawImage != null)
            {
                // 前のフレームが表示されるのを防ぐために停止
                videoPlayer.Stop();
                rawImage.gameObject.SetActive(true);  // RawImage を表示

                // 選択された動画を VideoPlayer に設定して準備
                videoPlayer.clip = videoClips[index];
                videoPlayer.Prepare();

                // 準備が完了したら動画を再生
                videoPlayer.prepareCompleted += OnPrepareCompleted;
            }
            else
            {
                Debug.LogError("VideoPlayer または RawImage が設定されていません。");
            }
        }
        else
        {
            Debug.LogError("指定されたインデックスがリストの範囲外です。");
        }
    }

    private void OnPrepareCompleted(VideoPlayer vp)
    {
        // 動画の準備が完了したら再生を開始
        videoPlayer.Play();
        Debug.Log("動画を再生しています。");

        // 再度 OnPrepareCompleted が呼ばれないようにイベントから削除
        videoPlayer.prepareCompleted -= OnPrepareCompleted;
    }

    // 動画の再生終了時に呼び出されるメソッド
    private void OnVideoEnd(VideoPlayer vp)
    {
        Debug.Log("動画が終了しました。RawImage を非表示にします。");
        rawImage.gameObject.SetActive(false);  // RawImage を非表示
        if(cityManager.state>4){
        cityManager.Mokuhyo();
        }
    }
}


/*
using UnityEngine;
using UnityEngine.UI;      // RawImage のために必要
using UnityEngine.Video;   // VideoPlayer のために必要
using System.Collections.Generic;  // List を使用するために必要

public class MultiVideoPlayer : MonoBehaviour
{
    public VideoPlayer videoPlayer;        // VideoPlayer コンポーネントの参照
    public RawImage rawImage;              // 動画を表示するための RawImage の参照
    public List<VideoClip> videoClips;     // 再生する動画のリスト

    private void Start()
    {
        // VideoPlayer または RawImage が設定されていない場合、エラーメッセージを出力
        if (videoPlayer == null)
        {
            Debug.LogError("VideoPlayer が設定されていません。Inspector で設定してください。");
        }
        
        if (rawImage == null)
        {
            Debug.LogError("RawImage が設定されていません。Inspector で設定してください。");
        }
        else
        {
            rawImage.gameObject.SetActive(false);  // 初期状態で RawImage を非表示にする
        }

        if (videoPlayer != null)
        {
            // 動画の再生終了イベントを登録
            videoPlayer.loopPointReached += OnVideoEnd;
        }
    }

    private void Update()
    {
        // キーボード入力によって異なる動画を再生
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            PlayVideo(0);  // 1キーでリストの最初の動画を再生
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            PlayVideo(1);  // 2キーでリストの2番目の動画を再生
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            PlayVideo(2);  // 3キーでリストの3番目の動画を再生
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            PlayVideo(3);  // 4キーでリストの3番目の動画を再生
        }
    }

    private void PlayVideo(int index)
    {
        // 指定されたインデックスがリスト範囲内か確認
        if (index >= 0 && index < videoClips.Count)
        {
            if (videoPlayer != null && rawImage != null)
            {
                // 選択された動画を VideoPlayer に設定して再生開始
                videoPlayer.clip = videoClips[index];
                rawImage.gameObject.SetActive(true);  // RawImage を表示
                videoPlayer.Play();                   // 動画を再生
                Debug.Log("動画 " + (index + 1) + " を再生しています。");
            }
            else
            {
                Debug.LogError("VideoPlayer または RawImage が設定されていません。");
            }
        }
        else
        {
            Debug.LogError("指定されたインデックスがリストの範囲外です。");
        }
    }

    // 動画の再生終了時に呼び出されるメソッド
    private void OnVideoEnd(VideoPlayer vp)
    {
        Debug.Log("動画が終了しました。RawImage を非表示にします。");
        rawImage.gameObject.SetActive(false);  // RawImage を非表示
    }
}
*/


/*
using UnityEngine;
using UnityEngine.UI;      // RawImage のために必要
using UnityEngine.Video;   // VideoPlayer のために必要

public class SimpleVideoTest : MonoBehaviour
{
    public VideoPlayer videoPlayer;    // VideoPlayer コンポーネントの参照
    public RawImage rawImage;          // 動画を表示するための RawImage の参照

    private void Start()
    {
        // VideoPlayer または RawImage が設定されていない場合、エラーメッセージを出力
        if (videoPlayer == null)
        {
            Debug.LogError("VideoPlayer が設定されていません。Inspector で設定してください。");
        }
        
        if (rawImage == null)
        {
            Debug.LogError("RawImage が設定されていません。Inspector で設定してください。");
        }
        else
        {
            rawImage.gameObject.SetActive(false);  // 初期状態で RawImage を非表示にする
        }

        if (videoPlayer != null)
        {
            // 動画の再生終了イベントを登録
            videoPlayer.loopPointReached += OnVideoEnd;
        }
    }

    private void Update()
    {
        // キーボードの「1」キーが押されたとき、動画の再生を開始
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Debug.Log("1キーが押されました。動画再生を開始します。");
            PlayVideo();
        }
    }

    private void PlayVideo()
    {
        // VideoPlayer と RawImage が設定されていれば再生開始
        if (videoPlayer != null && rawImage != null)
        {
            rawImage.gameObject.SetActive(true);    // RawImage を表示
            videoPlayer.Play();                     // 動画を再生
            Debug.Log("動画を再生しています。");
        }
        else
        {
            Debug.LogError("VideoPlayer または RawImage が設定されていません。");
        }
    }

    // 動画の再生終了時に呼び出されるメソッド
    private void OnVideoEnd(VideoPlayer vp)
    {
        Debug.Log("動画が終了しました。RawImage を非表示にします。");
        rawImage.gameObject.SetActive(false);  // RawImage を非表示
    }
}*/

/*
using UnityEngine;
using UnityEngine.Video;

public class SimpleVideoTest : MonoBehaviour
{
    public VideoPlayer videoPlayer;  // VideoPlayer コンポーネントの参照

    private void Start()
    {
        // VideoPlayer が設定されていない場合、エラーメッセージを出力
        if (videoPlayer == null)
        {
            Debug.LogError("VideoPlayer が設定されていません。Inspector で設定してください。");
        }
        else
        {
            // 初期状態で VideoPlayer を非表示にする
            videoPlayer.gameObject.SetActive(false);

            // 動画の再生終了イベントを登録
            videoPlayer.loopPointReached += OnVideoEnd;
        }
    }

    private void Update()
    {
        // キーボードの「1」キーが押されたとき、動画の再生を開始
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Debug.Log("1キーが押されました。動画再生を開始します。");
            PlayVideo();
        }
    }

    private void PlayVideo()
    {
        // VideoPlayer が設定されていれば再生開始
        if (videoPlayer != null)
        {
            videoPlayer.gameObject.SetActive(true);  // VideoPlayerを表示
            videoPlayer.Play();  // 動画を再生
            Debug.Log("動画を再生しています。");
        }
        else
        {
            Debug.LogError("VideoPlayer が設定されていません。");
        }
    }

    // 動画の再生終了時に呼び出されるメソッド
    private void OnVideoEnd(VideoPlayer vp)
    {
        Debug.Log("動画が終了しました。VideoPlayer を非表示にします。");
        videoPlayer.gameObject.SetActive(false);  // VideoPlayer を非表示
    }
}*/


/*
using UnityEngine;
using UnityEngine.Video;

public class SimpleVideoTest : MonoBehaviour
{
    public VideoPlayer videoPlayer;  // VideoPlayer コンポーネントの参照

    private void Start()
    {
        // VideoPlayer が設定されていない場合、エラーメッセージを出力
        if (videoPlayer == null)
        {
            Debug.LogError("VideoPlayer が設定されていません。Inspector で設定してください。");
        }
        
    }

    private void Update()
    {
        Debug.Log("Update メソッドが動作しています"); // 毎フレーム確認用のログ

        // キーボードの「1」キーが押されたとき、動画の再生を開始
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Debug.Log("1キーが押されました。動画再生を開始します。");
            PlayVideo();
        }
    }

    private void PlayVideo()
    {
        // VideoPlayer が設定されていれば再生開始
        if (videoPlayer != null)
        {
            videoPlayer.gameObject.SetActive(true);  // VideoPlayerを表示
            videoPlayer.Play();  // 動画を再生
            Debug.Log("動画を再生しています。");
        }
        else
        {
            Debug.LogError("VideoPlayer が設定されていません。");
        }
    }
}
*/