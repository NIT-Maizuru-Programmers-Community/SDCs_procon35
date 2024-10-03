#------
#ArUcoマーカー読み込み/送信用
#------

import asyncio
import websockets
import json

async def send_data():
    # WebSocketサーバーのURI
    uri = "ws://153.121.41.11:5000"
    t = "Kakuyu"

    async with websockets.connect(uri) as websocket:
        # 送信するデータをJSON形式で準備
        data = {
            "id": 5,
            "x": 330,
            "y": 220,
            "humidity": 60.2,
            "city": t
        }

        # データをJSON形式にシリアライズして送信
        await websocket.send(json.dumps(data))
        print("Data sent to server")

        # サーバーからのレスポンスを受信
        response = await websocket.recv()
        print(f"Received response: {response}")

# メイン関数を実行
asyncio.run(send_data())
