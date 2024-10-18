#------
#JSONファイルをVPS->Unityへ送信
#------

import asyncio
import websockets
import json

async def send_json(websocket, path):
    try:
        # クライアントからメッセージを受け取る（リクエストがあるまで待機）
        request = await websocket.recv()
        print(f"Received request: {request}")

        # data.jsonの内容を読み込み
        with open('data.json', 'r') as file:
            data = json.load(file)

        # JSONデータをクライアントに送信
        await websocket.send(json.dumps(data))
        print("Data sent")
    except Exception as e:
        print(f"Error: {e}")

# WebSocketサーバーを起動
start_server = websockets.serve(send_json, "153.121.41.11", 8765)

asyncio.get_event_loop().run_until_complete(start_server)
print("Server started")
asyncio.get_event_loop().run_forever()
