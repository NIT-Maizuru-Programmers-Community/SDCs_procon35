using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour
{
    [SerializeField]
    private Camera sceneCamera;

    private Vector3 lastPosition;

    [SerializeField]
    private LayerMask placementLayermask;

    public event Action OnClicked, OnExit;

    // メソッド名を正しく修正
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
            OnClicked?.Invoke();

        // InputManager.GetKeyDown ではなく Input.GetKeyDown を使用
        if (Input.GetKeyDown(KeyCode.Escape))
            OnExit?.Invoke();
    }

    public bool IsPointerOverUI()
        => EventSystem.current.IsPointerOverGameObject();

    // 修正版の GetSelectedMapPosition メソッド
    public Vector3 GetSelectedMapPosition()
    {
        // マウスのスクリーン座標を取得
        Vector3 mousePos = Input.mousePosition;

        // Z軸の値を適切な距離に変更（カメラからの距離を設定）
        mousePos.z = 10.0f; // カメラから適切な距離に設定

        // スクリーン座標をRayに変換
        Ray ray = sceneCamera.ScreenPointToRay(mousePos);
        RaycastHit hit;

        // Rayが地面などに当たったか確認
        if (Physics.Raycast(ray, out hit, 100, placementLayermask))
        {
            // オブジェクトの最後の位置を更新
            lastPosition = hit.point;

            // Y座標を固定して、オブジェクトが浮かないようにする
            lastPosition.y = 0.15f; // 0.15に設定して少し浮かせる

            // デバッグ用: Rayが飛んでいる位置を可視化
            Debug.DrawRay(ray.origin, ray.direction * 100, Color.red, 1f);
        }

        return lastPosition;
    }
}