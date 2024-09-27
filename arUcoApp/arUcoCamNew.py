import cv2
from cv2 import aruco
import numpy as np

# カメラの初期化
cap = cv2.VideoCapture(2)

# ArUcoマーカーの辞書とパラメータの設定
p_dict = aruco.getPredefinedDictionary(aruco.DICT_4X4_50)

# メイン処理
while True:
    # フレーム読み込み
    ret, frame = cap.read()

    if not ret:
        break

    # マーカー検出
    corners, ids, rejectedImgPoints = aruco.detectMarkers(frame, p_dict)

    # マーカーが検出されたか確認
    if ids is not None and len(ids) == 4:
        # 時計回りで左上から順にマーカーの中心座標をmに格納
        m = np.empty((4, 2))  # 4つのマーカーに対してそれぞれ2つのパラメータを与える(x,y)
        for i, c in zip(ids.ravel(), corners):
            m[i] = c[0].mean(axis=0)

        # 変換後画像サイズ[px]
        width, height = (400, 400)

        marker_cordinates = np.float32(m)
        true_cordinates = np.float32([[0, 0], [width, 0], [width, height], [0, height]])
        trans_mat = cv2.getPerspectiveTransform(marker_cordinates, true_cordinates)
        img_trans = cv2.warpPerspective(frame, trans_mat, (width, height))

        # 変換後の画像を表示（必要に応じて）
        cv2.imshow('Transformed Image', img_trans)

    # 描画
    cv2.imshow('SDCs Marker Scan System', frame)
    
    # 'q'キーが押されたらループを終了
    if cv2.waitKey(1) & 0xFF == ord('q'):
        break

# カメラとウィンドウのリリース
cap.release()
cv2.destroyAllWindows()
