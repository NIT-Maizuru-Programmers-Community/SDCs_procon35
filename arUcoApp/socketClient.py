import socketio

# Socket.IOクライアントを初期化
sio = socketio.Client()

# サーバーに接続
sio.connect('http://153.121.41.11:5000')

# サーバーからのメッセージを受け取るイベントハンドラ
@sio.on('response_event')
def on_message(data):
    print('Received from server:', data)

# 送信するJSONデータを準備
data = {
    "sensor_id": "sensor_01",
    "temperature": 23.5,
    "humidity": 60.2
}

# サーバーにデータを送信
sio.emit('json_event', data)

# 接続を閉じる
sio.disconnect()
