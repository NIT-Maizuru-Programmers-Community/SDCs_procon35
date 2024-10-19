using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float speed = 5.0f;  // カメラの移動速度
    public float mouseSensitivity = 2.0f;  // マウスの感度
    private float pitch = 0.0f;  // 垂直方向の角度 (X軸)
    private float yaw = 0.0f;  // 水平方向の角度 (Y軸)
    private float fixedY = 1.0f;  // Y軸を1に固定
    private bool isCameraControlEnabled = true;  // カメラ操作の有効/無効フラグ

    void Start()
    {
        LockCursor();  // カーソルを非表示＆画面中央に固定

        // 初期回転
        pitch = 0.0f; yaw = 0.0f;
        transform.eulerAngles = new Vector3(pitch, yaw, 0.0f);
        transform.position = new Vector3(transform.position.x, fixedY, transform.position.z);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleCameraControl();  // Escapeキーでカーソル表示とカメラ操作の切替
        }

        if (isCameraControlEnabled)
        {
            // カメラ移動（右クリックで前進）
            float moveForward = Input.GetMouseButton(1) ? speed * Time.deltaTime : 0.0f;
            transform.Translate(0, 0, moveForward);

            // カメラ回転
            yaw += Input.GetAxis("Mouse X") * mouseSensitivity;
            pitch -= Input.GetAxis("Mouse Y") * mouseSensitivity;
            pitch = Mathf.Clamp(pitch, -90f, 90f);

            transform.eulerAngles = new Vector3(pitch, yaw, 0.0f);
        }

        // Y軸を固定
        Vector3 position = transform.position;
        position.y = fixedY;
        transform.position = position;
    }

    void ToggleCameraControl()
    {
        isCameraControlEnabled = !isCameraControlEnabled;

        if (isCameraControlEnabled)
        {
            LockCursor();  // カメラ操作を有効にし、カーソルを非表示＆ロック
        }
        else
        {
            UnlockCursor();  // カメラ操作を無効にし、カーソルを表示＆ロック解除
        }
    }

    void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}

/*
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float speed = 5.0f;  // カメラの移動速度
    public float mouseSensitivity = 2.0f;  // マウスの感度

    private float pitch = 0.0f;  // 垂直方向の角度 (X軸)
    private float yaw = 0.0f;  // 水平方向の角度 (Y軸)
    private float fixedY = 1.0f;  // Y軸を1に固定

    void Start()
    {
        // マウスカーソルを画面中央にロック
        Cursor.lockState = CursorLockMode.Locked;

        // 初期回転をX=0, Y=0, Z=0に設定
        pitch = 0.0f;  // X軸の回転を0度にリセット
        yaw = 0.0f;    // Y軸の回転を0度にリセット

        // 初期回転を適用
        transform.eulerAngles = new Vector3(pitch, yaw, 0.0f);

        // Y位置を1に設定
        Vector3 position = transform.position;
        position.y = fixedY;
        transform.position = position;
    }

    void Update()
    {
        // カメラの移動 (右クリックで前進)
        float moveForward = 0.0f;

        if (Input.GetMouseButton(1))  // 右クリックで前進
        {
            moveForward = speed * Time.deltaTime;  // 前進
        }

        // カメラの移動を適用 (Z軸方向に移動、Y軸は1に固定)
        transform.Translate(0, 0, moveForward);

        // 高さ(Y軸)を1に固定
        Vector3 position = transform.position;
        position.y = fixedY;
        transform.position = position;

        // カメラの回転 (マウスでの視点移動)
        yaw += Input.GetAxis("Mouse X") * mouseSensitivity;  // 水平方向 (Y軸)
        pitch -= Input.GetAxis("Mouse Y") * mouseSensitivity;  // 垂直方向 (X軸)
        pitch = Mathf.Clamp(pitch, -90f, 90f);  // 垂直方向の角度を制限

        // 回転を適用 (Z軸の回転は無視)
        transform.eulerAngles = new Vector3(pitch, yaw, 0.0f);
    }
}*/

/*using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float speed = 5.0f;  // カメラの移動速度
    public float mouseSensitivity = 2.0f;  // マウスの感度

    private float pitch = 0.0f;  // 垂直方向の角度 (X軸)
    private float yaw = 0.0f;  // 水平方向の角度 (Y軸)

    void Start()
    {
        // マウスカーソルを画面中央にロック
        Cursor.lockState = CursorLockMode.Locked;

        // 初期回転をX=0, Y=0, Z=0に設定
        pitch = 0.0f;  // X軸の回転を0度にリセット
        yaw = 0.0f;    // Y軸の回転を0度にリセット

        // 初期回転を適用
        transform.eulerAngles = new Vector3(pitch, yaw, 0.0f);
    }

    void Update()
    {
        // カメラの移動 (右クリックで前進)
        float moveForward = 0.0f;

        if (Input.GetMouseButton(1))  // 右クリックで前進
        {
            moveForward = speed * Time.deltaTime;  // 前進
        }

        // カメラの移動を適用 (Z軸方向に移動、Y軸は変えない)
        transform.Translate(0, 0, moveForward);  // 前進のためにZ軸方向に移動

        // カメラの回転 (マウスでの視点移動)
        yaw += Input.GetAxis("Mouse X") * mouseSensitivity;  // 水平方向 (Y軸)
        pitch -= Input.GetAxis("Mouse Y") * mouseSensitivity;  // 垂直方向 (X軸)
        pitch = Mathf.Clamp(pitch, -90f, 90f);  // 垂直方向の角度を制限

        // 回転を適用 (Z軸の回転は無視)
        transform.eulerAngles = new Vector3(pitch, yaw, 0.0f);
    }
}*/










/*
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float speed = 5.0f;  // カメラの移動速度
    public float mouseSensitivity = 2.0f;  // マウスの感度

    private float pitch = 0.0f;  // 垂直方向の角度
    private float yaw = 0.0f;  // 水平方向の角度

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;  // マウスカーソルを画面中央にロック
    }

    void Update()
    {
        // カメラの移動
        float moveForward = Input.GetAxis("Vertical") * speed * Time.deltaTime;
        float moveRight = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        transform.Translate(moveRight, 0, moveForward);

        // カメラの回転
        yaw += Input.GetAxis("Mouse X") * mouseSensitivity;
        pitch -= Input.GetAxis("Mouse Y") * mouseSensitivity;
        pitch = Mathf.Clamp(pitch, -90f, 90f);  // 垂直方向の角度を制限

        transform.eulerAngles = new Vector3(pitch, yaw, 0.0f);
    }
}
*/