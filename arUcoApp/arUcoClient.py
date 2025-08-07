import cv2
import numpy as np
import asyncio
import websockets
import json

# カメラの初期化
cap = cv2.VideoCapture(2)

# ArUcoマーカーの辞書とパラメータの設定
aruco = cv2.aruco
p_dict = aruco.getPredefinedDictionary(aruco.DICT_4X4_50)

# WebSocketでデータを送信する関数
async def send_data(marker_data):
    # WebSocketサーバーのURI
    uri = "ws://153.121.41.11:9980"

    async with websockets.connect(uri) as websocket:
        # データをJSON形式にシリアライズして送信
        await websocket.send(json.dumps(marker_data))
        print("Data sent to server")

        # サーバーからのレスポンスを受信
        response = await websocket.recv()
        print(f"Received response: {response}")

# メイン処理
while True:
    # フレーム読み込み
    ret, frame = cap.read()

    if not ret:
        break

    # 画像をグレースケールに変換
    gray = cv2.cvtColor(frame, cv2.COLOR_BGR2GRAY)

    # ガウシアンブラーを適用してノイズを軽減
    gray = cv2.GaussianBlur(gray, (5, 5), 0)

    # マーカー検出
    corners, ids, rejectedImgPoints = aruco.detectMarkers(gray, p_dict)

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
        width, height = (500, 500)

        marker_coordinates = np.float32(m)
        true_coordinates = np.float32([[0, 0], [width, 0], [width, height], [0, height]])

        # 透視変換行列を計算し、画像を変換
        trans_mat = cv2.getPerspectiveTransform(marker_coordinates, true_coordinates)
        img_trans = cv2.warpPerspective(frame, trans_mat, (width, height))

        # 変換後の画像で再度マーカー検出
        trans_corners, trans_ids, _ = aruco.detectMarkers(img_trans, p_dict)

        # マーカーのデータを格納するリスト
        marker_data = []

        # 変換後の画像にマーカーが存在すれば、そのIDと左上の座標を表示
        if trans_ids is not None:
            aruco.drawDetectedMarkers(img_trans, trans_corners, trans_ids)
            for i in range(len(trans_ids)):
                c = trans_corners[i][0]
                # 左上の座標を取得
                top_left_corner = (int(c[0][0]), int(c[0][1]))
                # IDを画像上に表示
                cv2.putText(img_trans, f'ID: {trans_ids[i][0]}', top_left_corner,
                            cv2.FONT_HERSHEY_SIMPLEX, 1, (0, 255, 0), 2, cv2.LINE_AA)
                # 左上の座標を画像上に表示
                cv2.putText(img_trans, f'Pos: {top_left_corner}', (top_left_corner[0], top_left_corner[1] + 30),
                            cv2.FONT_HERSHEY_SIMPLEX, 0.8, (255, 0, 0), 2, cv2.LINE_AA)
                
                # ターミナルにIDと左上の座標を表示
                print(f"ID: {trans_ids[i][0]}, Top-left corner: {top_left_corner}")

                # マーカーのデータをリストに追加
                marker_data.append({
                    "id": int(trans_ids[i][0]),
                    "x": top_left_corner[0],
                    "y": top_left_corner[1]
                })

        else:
            # マーカーが存在しない場合、Noneを送信
            marker_data = None
            print("No markers detected.")

        # WebSocketでデータを送信
        asyncio.run(send_data(marker_data))

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
