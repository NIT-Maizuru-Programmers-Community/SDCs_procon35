import asyncio
import websockets
import json

async def send_data():
    # WebSocketサーバーのURI
    uri = "ws://153.121.41.11:5000"
    t = "maizuru"

    async with websockets.connect(uri) as websocket:
        # 送信するデータをJSON形式で準備
        data = {
            "sensor_id": "sensor_01",
            "temperature": 23.5,
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
