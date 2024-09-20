import cv2
import numpy as np

# ArUcoマーカーの辞書を読み込み
aruco_dict = cv2.aruco.getPredefinedDictionary(cv2.aruco.DICT_4X4_50)

# ArUcoマーカー検出パラメータを設定
parameters = cv2.aruco.DetectorParameters()

# カメラからの映像をキャプチャ
cap = cv2.VideoCapture(0)

# 目標とする鳥瞰図のサイズ
bird_eye_width = 6000
bird_eye_height = 6000

# 四隅のターゲット座標（鳥瞰図上での座標）
bird_eye_points = np.array([
    [0, 0],  # 左上 (ID=0)
    [bird_eye_width, 0],  # 右上 (ID=1)
    [bird_eye_width, bird_eye_height],  # 右下 (ID=2)
    [0, bird_eye_height]  # 左下 (ID=3)
], dtype=np.float32)

while True:
    ret, frame = cap.read()
    if not ret:
        break

    # グレースケールに変換
    gray = cv2.cvtColor(frame, cv2.COLOR_BGR2GRAY)

    # ArUcoマーカーを検出
    corners, ids, _ = cv2.aruco.detectMarkers(gray, aruco_dict, parameters=parameters)

    if ids is not None and len(ids) >= 4:
        # 検出した四隅の座標を取得（順番をIDに基づいて並び替える）
        field_points = np.zeros((4, 2), dtype=np.float32)
        for i, marker_id in enumerate([0, 1, 2, 3]):
            idx = np.where(ids == marker_id)[0]
            if len(idx) > 0:
                field_points[i] = corners[idx[0]][0][0]

        # 射影変換行列を計算
        homography_matrix, _ = cv2.findHomography(field_points, bird_eye_points)

        # 鳥瞰画像に変換
        bird_eye_view = cv2.warpPerspective(frame, homography_matrix, (bird_eye_width, bird_eye_height))

        # 鳥瞰画像上に他のArUcoマーカーをプロット
        for i in range(len(ids)):
            if ids[i] not in [0, 1, 2, 3]:  # 四隅以外のマーカー
                marker_center = np.mean(corners[i][0], axis=0).reshape(1, 1, 2)
                transformed_center = cv2.perspectiveTransform(marker_center, homography_matrix)
                x, y = transformed_center[0][0]
                print(f"ID: {ids[i][0]} -> 鳥瞰座標: ({x:.2f}, {y:.2f})")

                # 鳥瞰図にプロット
                cv2.circle(bird_eye_view, (int(x), int(y)), 10, (0, 0, 255), -1)

        # 鳥瞰図を表示
        cv2.imshow('Bird Eye View', bird_eye_view)

    # オリジナル映像を表示
    cv2.imshow('Original', frame)

    # 'q'キーが押されたら終了
    if cv2.waitKey(1) & 0xFF == ord('q'):
        break

cap.release()
cv2.destroyAllWindows()
