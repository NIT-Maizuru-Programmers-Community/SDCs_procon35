using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using System.Net.WebSockets;

public class WebSocketClient : MonoBehaviour
{
    // WebSocketオブジェクト
    private ClientWebSocket ws;
    
    // VPSのWebSocketサーバーのURI
    private Uri serverUri = new Uri("ws://153.121.41.11:8765"); // VPSのIPアドレスを指定

    // StartメソッドはUnityが最初に呼び出す
    async void Start()
    {
        // WebSocketオブジェクトを初期化
        ws = new ClientWebSocket();
        
        // サーバーに接続
        await ConnectToServer();

        // JSONデータをリクエストし、受信
        await RequestJsonData();
    }

    // サーバーへ接続するメソッド
    private async Task ConnectToServer()
    {
        try
        {
            // WebSocketでサーバーに接続
            await ws.ConnectAsync(serverUri, CancellationToken.None);
            Debug.Log("Connected to server");
        }
        catch (Exception e)
        {
            Debug.LogError($"Connection failed: {e.Message}");
        }
    }

    // サーバーからJSONデータをリクエストし、受信するメソッド
    private async Task RequestJsonData()
    {
        try
        {
            // リクエストメッセージを送信
            byte[] requestBytes = Encoding.UTF8.GetBytes("Requesting data");
            await ws.SendAsync(new ArraySegment<byte>(requestBytes), WebSocketMessageType.Text, true, CancellationToken.None);

            // サーバーからのレスポンスを受信
            var buffer = new byte[1024]; // バッファサイズは適宜調整
            var result = await ws.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            string jsonResponse = Encoding.UTF8.GetString(buffer, 0, result.Count);

            // 受信したデータをログに出力
            Debug.Log("Received JSON data: " + jsonResponse);

            // 必要に応じて、受信したデータをUnityのオブジェクトやUIに反映
            ProcessJsonData(jsonResponse);
        }
        catch (Exception e)
        {
            Debug.LogError($"Error receiving data: {e.Message}");
        }
    }

    // 受信したJSONデータを処理するメソッド
    private void ProcessJsonData(string jsonData)
    {
        // JSONデータを解析して、Unity内で必要な処理を行う
        Debug.Log("Processing JSON data: " + jsonData);
        // ここにJSONデータの解析処理を追加
    }

    // アプリケーション終了時にWebSocket接続を閉じる
    private void OnApplicationQuit()
    {
        if (ws != null && ws.State == WebSocketState.Open)
        {
            ws.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closed by client", CancellationToken.None);
        }
    }
}
