import cv2
from cv2 import aruco
import numpy as np

# ArUcoマーカーの辞書とパラメータの設定
aruco_dict = aruco.getPredefinedDictionary(aruco.DICT_4X4_50)
parameters = aruco.DetectorParameters()

# カメラの初期化
cap = cv2.VideoCapture(0)

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

# 座標の範囲をクリップする関数
def clip_coordinate(value, min_value=0, max_value=6000):
    return max(min_value, min(value, max_value))

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
                reference_corners[marker_id[0]] = {
                    "LU": corners[i][0][0],  # 左上
                    "RU": corners[i][0][1],  # 右上
                    "RD": corners[i][0][2],  # 右下
                    "LD": corners[i][0][3],  # 左下
                }

                # ターミナルに基準マーカーの座標を表示
                print(f"Reference Marker ID: {marker_id[0]}")
                for label, corner in reference_corners[marker_id[0]].items():
                    print(f"  {label} Corner: x={corner[0]:.2f}, y={corner[1]:.2f}")
        
        # 基準マーカーがすべて検出されている場合、射影変換を用いて座標を変換
        if len(reference_corners) == 4:
            # 実際のマーカー座標
            actual_positions = np.array([reference_corners[i]["LU"] for i in reference_ids], dtype=np.float32)
            
            # 理想的なマーカー座標
            ideal_positions_array = np.array([ideal_positions[i] for i in reference_ids], dtype=np.float32)
            
            # 射影変換行列の計算
            transform_matrix = cv2.getPerspectiveTransform(actual_positions, ideal_positions_array)
            
            # 他のマーカーの座標を計算して表示
            for i, marker_id in enumerate(ids):
                if marker_id[0] not in reference_ids:
                    # 各コーナーの座標を計算
                    for j, label in enumerate(["LU", "RU", "RD", "LD"]):
                        corner = np.array([corners[i][0][j]], dtype=np.float32)
                        
                        # 座標変換
                        transformed_corner = cv2.perspectiveTransform(np.array([corner]), transform_matrix)[0][0]
                        
                        # 座標をクリップして表示
                        transformed_x = clip_coordinate(transformed_corner[0])
                        transformed_y = clip_coordinate(transformed_corner[1])
                        print(f"Marker ID: {marker_id[0]}, {label} Transformed Position: x={transformed_x:.2f}, y={transformed_y:.2f}")
                        
                        # フレームに変換された座標を表示
                        cv2.putText(frame, f"{label}: x={transformed_x:.2f}, y={transformed_y:.2f}", 
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
