from flask import Flask
from flask_socketio import SocketIO, emit

app = Flask(__name__)
socketio = SocketIO(app)

# クライアントが接続したときの処理
@socketio.on('connect')
def handle_connect():
    print('Client connected')
    emit('response_event', {'data': 'Connected to server'})

# クライアントがメッセージを送信したときの処理
@socketio.on('json_event')
def handle_json_event(json_data):
    print(f'Received JSON: {json_data}')
    # 必要な処理をここで行います
    emit('response_event', {'data': 'JSON received', 'received_data': json_data})

# クライアントが切断したときの処理
@socketio.on('disconnect')
def handle_disconnect():
    print('Client disconnected')

if __name__ == '__main__':
    # WebSocketサーバをポート5000で実行
    socketio.run(app, host='0.0.0.0', port=5000)
