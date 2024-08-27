import cv2
from cv2 import aruco
import numpy as np

# ArUco辞書の設定
aruco_dict = aruco.getPredefinedDictionary(aruco.DICT_4X4_50)
aruco_params = aruco.DetectorParameters()

# カメラのキャプチャ
cap = cv2.VideoCapture(2)  # カメラIDが0の場合

while True:
    ret, frame = cap.read()
    if not ret:
        break

    # ArUcoマーカーを検出
    corners, ids, rejected = aruco.detectMarkers(frame, aruco_dict, parameters=aruco_params)
    
    # マーカーが検出された場合
    if ids is not None:
        # 各マーカーの四隅の座標を描画
        for corner in corners:
            int_corners = np.int0(corner)
            cv2.polylines(frame, int_corners, True, (0, 255, 0), 2)

        # 四隅のマーカーのIDを特定し、対応する座標を取得
        corner_ids = [0, 1, 2, 3]  # 左上、右上、右下、左下
        corner_positions = []

        for id in corner_ids:
            if id in ids:
                index = np.where(ids == id)[0][0]
                corner_positions.append(corners[index][0])
            else:
                print(f"ID {id} のマーカーが見つかりませんでした。")

        # 座標を表示
        if len(corner_positions) == 4:
            for pos in corner_positions:
                for point in pos:
                    cv2.putText(frame, f'({int(point[0])}, {int(point[1])})', 
                                (int(point[0]), int(point[1])),
                                cv2.FONT_HERSHEY_SIMPLEX, 0.5, (255, 0, 0), 2)

            # 四隅の座標が揃っている場合、内部に配置されたマーカーを表示
            # マーカーIDが四隅に含まれない場合の処理
            for i in range(len(ids)):
                if ids[i] not in corner_ids:
                    center = np.mean(corners[i][0], axis=0)
                    cv2.putText(frame, f'ID: {ids[i][0]}', 
                                (int(center[0]), int(center[1])),
                                cv2.FONT_HERSHEY_SIMPLEX, 0.5, (0, 0, 255), 2)

    # フレームを表示
    cv2.imshow('ArUco Detection', frame)

    # 'q'キーで終了
    if cv2.waitKey(1) & 0xFF == ord('q'):
        break

cap.release()
cv2.destroyAllWindows()
