import cv2
from cv2 import aruco
import numpy as np

# ArUcoマーカーの辞書とパラメータの設定
aruco_dict = aruco.getPredefinedDictionary(cv2.aruco.DICT_4X4_50)
parameters = aruco.DetectorParameters()

# カメラの初期化
cap = cv2.VideoCapture(0)

# マーカーIDを記憶するリスト
detected_ids = []

while True:
    # フレームの読み込み
    ret, frame = cap.read()
    if not ret:
        break
    
    # グレースケール変換
    gray = cv2.cvtColor(frame, cv2.COLOR_BGR2GRAY)
    
    # ArUcoマーカーの検出
    corners, ids, rejectedImgPoints = aruco.detectMarkers(gray, aruco_dict, parameters=parameters)
    
    # マーカーが検出された場合
    if ids is not None:
        # 検出されたマーカーのIDをフレームに描画
        aruco.drawDetectedMarkers(frame, corners, ids)
        
        # 新しいマーカーIDをリストに追加
        for id in ids:
            if id[0] not in detected_ids:
                detected_ids.append(id[0])
                
        # 検出されたマーカーIDを表示
        ids_text = ', '.join(map(str, detected_ids))
        cv2.putText(frame, f"Detected IDs: {ids_text}", (10, 30), cv2.FONT_HERSHEY_SIMPLEX, 1, (0, 255, 0), 2, cv2.LINE_AA)
    
    # フレームを表示
    cv2.imshow('ArUco Marker Detection', frame)
    
    # 'q'キーが押されたらループを終了
    if cv2.waitKey(1) & 0xFF == ord('q'):
        break

# カメラとウィンドウのリリース
cap.release()
cv2.destroyAllWindows()
