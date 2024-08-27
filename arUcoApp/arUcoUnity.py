import websocket

# WebSocketサーバーのURLを設定
VPS_IP = "153.121.41.11"  # ここにVPSのIPアドレスを記入
url = f"ws://{VPS_IP}:5000/socket.io/?EIO=4&transport=websocket"

def on_message(ws, message):
    print("Received:", message)

def on_error(ws, error):
    print("Error:", error)

def on_close(ws, close_status_code, close_msg):
    print("WebSocket closed")

def on_open(ws):
    print("WebSocket connected")
    # ここにサーバーに送信したいメッセージやデータがあれば記述できます。
    # ws.send("Your message here")

# WebSocketオブジェクトを作成し、コールバックを設定
ws = websocket.WebSocketApp(url,
                            on_message=on_message,
                            on_error=on_error,
                            on_close=on_close)
ws.on_open = on_open

# WebSocket接続を開始
ws.run_forever()
