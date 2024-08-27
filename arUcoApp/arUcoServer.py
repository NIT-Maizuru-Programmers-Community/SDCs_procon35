from flask import Flask
from flask_socketio import SocketIO, emit

app = Flask(__name__)
socketio = SocketIO(app)

@socketio.on('json_event')
def handle_json_event(json_data):
    print(f'Received JSON: {json_data}')
    emit('response_event', {'data': 'JSON received', 'received_data': json_data})

if __name__ == '__main__':
    socketio.run(app, host='0.0.0.0', port=5000)
