import asyncio
import websockets
import json

# WebSocketサーバーの処理
async def server(websocket, path):
    print("Client connected")

    try:
        # クライアントからのメッセージを受信
        async for message in websocket:
            data = json.loads(message)
            print(f"Received JSON data: {data}")

            # クライアントにレスポンスを送信（オプション）
            response = json.dumps({"message": "Data received!"})
            await websocket.send(response)

    except websockets.ConnectionClosed as e:
        print("Client disconnected")

# WebSocketサーバーを起動
async def main():
    async with websockets.serve(server, "0.0.0.0", 5000):
        await asyncio.Future()  # 無限に待機

if __name__ == "__main__":
    asyncio.run(main())
