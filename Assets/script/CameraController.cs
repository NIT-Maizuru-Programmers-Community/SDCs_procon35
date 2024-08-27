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
