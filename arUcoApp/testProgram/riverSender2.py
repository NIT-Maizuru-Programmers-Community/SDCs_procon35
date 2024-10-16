import serial  # シリアル通信ライブラリ
import time
#import keyboard  # キー入力を監視するライブラリ

# Arduinoと通信するためのシリアルポート設定（適宜変更してください）
SERIAL_PORT = '/dev/ttyACM0'  # Arduinoが接続されているポート名
BAUD_RATE = 9600  # Arduinoと一致するボーレート

arduino = serial.Serial(SERIAL_PORT, BAUD_RATE, timeout=1)

while True:
    print("色を変化させます")
    time.sleep(1)
    arduino.write('1'.encode())
    print("色を変化させます")
    time.sleep(1)
    arduino.write('0'.encode())