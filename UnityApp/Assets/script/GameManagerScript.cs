using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;

public class GameManagerScript : MonoBehaviour
{
    
    
    public Camera mainCamera; // メインカメラ
    public Camera cameraKun; // カメラくん
    
    void Start()
    {   
     mainCamera.enabled = true;  // メインカメラを有効化
        cameraKun.enabled = true;  // カメラくんも無効化する（必要に応じて）
        //uiSet.SetActive(false);

        // 必要に応じてカーソルの設定を行う
        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
    }

 
}
