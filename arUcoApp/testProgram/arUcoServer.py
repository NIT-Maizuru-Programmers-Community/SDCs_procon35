from flask import Flask
import socketio

# FlaskとSocket.IOサーバーを初期化
app = Flask(__name__)
sio = socketio.Server()
app.wsgi_app = socketio.WSGIApp(sio, app.wsgi_app)

# クライアントからの接続が確立したときに呼び出されるイベント
@sio.event
def connect(sid, environ):
    print('Client connected:', sid)

# クライアントが送信したJSONデータを受け取るイベント
@sio.event
def json_event(sid, data):
    print('Received JSON from client:', data)

    # クライアントにレスポンスを送信（オプション）
    sio.emit('response_event', {'message': 'Data received!'}, room=sid)

# クライアントが切断したときに呼び出されるイベント
@sio.event
def disconnect(sid):
    print('Client disconnected:', sid)

if __name__ == '__main__':
    app.run(host='153.121.41.11', port=5000)
