import cv2
from cv2 import aruco
import numpy as np

# ArUcoマーカーの辞書とパラメータの設定
aruco_dict = aruco.getPredefinedDictionary(aruco.DICT_4X4_50)
parameters = aruco.DetectorParameters()

# カメラの初期化
cap = cv2.VideoCapture(2)

# 基準マーカーのIDとその座標
reference_ids = [0, 1, 2, 3]
reference_corners = {}

# 座標の最大値
max_coordinate = 6000

# 理想的なマーカー位置 (IDごと)
ideal_positions = {
    0: (0, 0),  # 左上
    1: (0, max_coordinate),  # 左下
    2: (max_coordinate, max_coordinate),  # 右下
    3: (max_coordinate, 0)  # 右上
}

while True:
    # フレームの読み込み
    ret, frame = cap.read()
    if not ret:
        break
    
    # グレースケール変換
    gray = cv2.cvtColor(frame, cv2.COLOR_BGR2GRAY)
    
    # ArUcoマーカーの検出
    corners, ids, _ = aruco.detectMarkers(gray, aruco_dict, parameters=parameters)
    
    # マーカーが検出された場合
    if ids is not None:
        # 検出されたマーカーのIDをフレームに描画
        aruco.drawDetectedMarkers(frame, corners, ids)
        
        # 基準マーカーの座標を取得
        for i, marker_id in enumerate(ids):
            if marker_id[0] in reference_ids:
                reference_corners[marker_id[0]] = corners[i][0][0]  # 左上の座標を保存
                
        # 基準マーカーがすべて検出されている場合、射影変換を用いて座標を変換
        if len(reference_corners) == 4:
            # 実際のマーカー座標
            actual_positions = np.array([reference_corners[i] for i in reference_ids], dtype=np.float32)
            
            # 理想的なマーカー座標
            ideal_positions_array = np.array([ideal_positions[i] for i in reference_ids], dtype=np.float32)
            
            # 射影変換行列の計算
            transform_matrix = cv2.getPerspectiveTransform(actual_positions, ideal_positions_array)
            
            # 他のマーカーの座標を計算して表示
            for i, marker_id in enumerate(ids):
                if marker_id[0] not in reference_ids:
                    # マーカーの左上の実座標を取得
                    corner = np.array([corners[i][0][0]], dtype=np.float32)
                    
                    # 座標変換
                    transformed_corner = cv2.perspectiveTransform(np.array([corner]), transform_matrix)[0][0]
                    
                    # ターミナルに座標を表示
                    print(f"Marker ID: {marker_id[0]}, Transformed Position: x={transformed_corner[0]:.2f}, y={transformed_corner[1]:.2f}")
                    
                    # フレームに変換された座標を表示
                    cv2.putText(frame, f"x={transformed_corner[0]:.2f}, y={transformed_corner[1]:.2f}", 
                                (int(corner[0][0]), int(corner[0][1] - 10)), 
                                cv2.FONT_HERSHEY_SIMPLEX, 0.5, (255, 0, 0), 2, cv2.LINE_AA)
    
    # フレームを表示
    cv2.imshow('ArUco Marker Detection', frame)
    
    # 'q'キーが押されたらループを終了
    if cv2.waitKey(1) & 0xFF == ord('q'):
        break

# カメラとウィンドウのリリース
cap.release()
cv2.destroyAllWindows()
