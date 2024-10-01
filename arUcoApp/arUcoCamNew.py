import cv2
import numpy as np

# カメラの初期化
cap = cv2.VideoCapture(0)

# ArUcoマーカーの辞書とパラメータの設定
aruco = cv2.aruco
p_dict = aruco.getPredefinedDictionary(aruco.DICT_4X4_50)

# メイン処理
while True:
    # フレーム読み込み
    ret, frame = cap.read()

    if not ret:
        break

    # マーカー検出
    corners, ids, rejectedImgPoints = aruco.detectMarkers(frame, p_dict)

    # マーカーが検出されたか確認し、id 0,1,2,3のマーカーが全て揃っているか確認
    if ids is not None and set([0, 1, 2, 3]).issubset(ids.ravel()):
        # corners2を初期化して、マーカーごとに座標を格納
        corners2 = [None] * 4  # id 0, 1, 2, 3 に対応するリスト

        # 各idに対応するcornerを格納
        for i, c in zip(ids.ravel(), corners):
            if i in [0, 1, 2, 3]:  # id 0,1,2,3にのみ対応
                corners2[i] = c

        # 各マーカーの角を変換後の画像の四隅に対応させる
        m = np.empty((4, 2))  # 4つのマーカーに対して(x, y)の2つの座標
        m[0] = corners2[0][0][2]  # id 0の角2 → 変換後の左上
        m[1] = corners2[1][0][3]  # id 1の角3 → 変換後の右上
        m[2] = corners2[2][0][0]  # id 2の角0 → 変換後の右下
        m[3] = corners2[3][0][1]  # id 3の角1 → 変換後の左下

        # 変形後画像サイズ[px]
        width, height = (700, 700)

        marker_coordinates = np.float32(m)
        true_coordinates = np.float32([[0, 0], [width, 0], [width, height], [0, height]])

        # 透視変換行列を計算し、画像を変換
        trans_mat = cv2.getPerspectiveTransform(marker_coordinates, true_coordinates)
        img_trans = cv2.warpPerspective(frame, trans_mat, (width, height))

        # 変換後の画像を表示
        cv2.imshow('Transformed Image', img_trans)

    # 描画
    cv2.imshow('SDCs Marker Scan System', frame)

    # 'q'キーが押されたらループを終了
    if cv2.waitKey(1) & 0xFF == ord('q'):
        break

# カメラとウィンドウのリリース
cap.release()
cv2.destroyAllWindows()
