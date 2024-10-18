import serial  # シリアル通信ライブラリ
import time
import keyboard  # キー入力を監視するライブラリ

# Arduinoと通信するためのシリアルポート設定（適宜変更してください）
SERIAL_PORT = '/dev/cu.usbmodem1101'  # Arduinoが接続されているポート名
BAUD_RATE = 9600  # Arduinoと一致するボーレート

# シリアル通信を開始
try:
    arduino = serial.Serial(SERIAL_PORT, BAUD_RATE, timeout=1)
    print("Arduinoに接続しました")
except Exception as e:
    print(f"Arduinoに接続できませんでした: {e}")
    exit()

# Arduinoに命令を送信する関数
def send_command(command):
    arduino.write(command.encode())  # コマンドをバイトに変換して送信
    print(f"送信: {command}")

def main():
    print("Rキーで赤色、Bキーで青色に切り替えます。終了するにはCtrl+Cを押してください。")

    try:
        while True:
            if keyboard.is_pressed('r'):  # Rキーが押されたら赤色に変更
                send_command('1')
                time.sleep(0.2)  # 誤作動防止のため少し待機

            elif keyboard.is_pressed('b'):  # Bキーが押されたら青色に変更
                send_command('0')
                time.sleep(0.2)  # 誤作動防止のため少し待機

    except KeyboardInterrupt:
        print("プログラムを終了します")

    finally:
        arduino.close()  # シリアル通信を終了

# メインプログラムを実行
if __name__ == "__main__":
    main()