from flask import Flask
from flask_socketio import SocketIO, emit

app = Flask(__name__)
socketio = SocketIO(app)

@socketio.on('json_event')
def handle_json_event(json_data):
    print("Received JSON Data:")
    for key, value in json_data.items():
        print(f'{key}: {value}')  # 各キーとその値を表示
    
    # 受信したデータを含む応答をクライアントに送信
    emit('response_event', {'data': 'JSON received', 'received_data': json_data})

if __name__ == '__main__':
    socketio.run(app, host='0.0.0.0', port=5000)
