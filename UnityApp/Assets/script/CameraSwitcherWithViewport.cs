using UnityEngine;

public class CameraSwitcherWithViewport : MonoBehaviour
{
    // メインカメラとサブカメラをインスペクタで設定
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Camera subCamera;

    // サブカメラが全画面表示されているかどうかのフラグ
    private bool isSubCameraFullScreen = false;

    private void Start()
    {
        // 最初はメインカメラが全画面、サブカメラが左下に小さく表示される
        mainCamera.rect = new Rect(0, 0, 1, 1);  // 全画面
        subCamera.rect = new Rect(0.75f, 0.05f, 0.2f, 0.2f);  // 左下に小さく表示
    }

    private void Update()
    {
        // Cキーでサブカメラを全画面に切り替える
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (isSubCameraFullScreen)
            {
                // サブカメラを左下に戻す
                mainCamera.rect = new Rect(0, 0, 1, 1);  // メインカメラを全画面
                subCamera.rect = new Rect(0.75f, 0.05f, 0.2f, 0.2f);  // サブカメラを左下に
            }
            else
            {
                // サブカメラを全画面にするが、メインカメラを完全に消さない
                mainCamera.rect = new Rect(0, 0, 1, 1);  // メインカメラはそのまま
                subCamera.rect = new Rect(0, 0, 1, 1);  // サブカメラを全画面表示
            }

            // フラグを反転させる
            isSubCameraFullScreen = !isSubCameraFullScreen;
        }
    }
}

/*
using UnityEngine;

public class CameraSwitcherWithViewport : MonoBehaviour
{
    // メインカメラとサブカメラをインスペクタで設定
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Camera subCamera;

    // サブカメラが全画面表示されているかどうかのフラグ
    private bool isSubCameraFullScreen = false;

    private void Start()
    {
        // 最初はメインカメラが全画面、サブカメラが左下に小さく表示される
        mainCamera.rect = new Rect(0, 0, 1, 1);  // 全画面
        subCamera.rect = new Rect(0.75f, 0.05f, 0.2f, 0.2f);  // 左下に小さく表示
    }

    private void Update()
    {
        // Cキーでサブカメラを全画面に切り替える
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (isSubCameraFullScreen)
            {
                // サブカメラを左下に戻す
                mainCamera.rect = new Rect(0, 0, 1, 1);  // メインカメラを全画面
                subCamera.rect = new Rect(0.75f, 0.05f, 0.2f, 0.2f);  // サブカメラを左下に
            }
            else
            {
                // サブカメラを全画面にする
                mainCamera.rect = new Rect(0, 0, 0, 0);  // メインカメラを非表示（全画面外）
                subCamera.rect = new Rect(0, 0, 1, 1);  // サブカメラを全画面表示
            }

            // フラグを反転させる
            isSubCameraFullScreen = !isSubCameraFullScreen;
        }
    }
}
*/
