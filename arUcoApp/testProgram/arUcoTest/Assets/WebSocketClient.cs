using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using System.Net.WebSockets;

public class WebSocketClient : MonoBehaviour
{
    private ClientWebSocket ws;
    public Text statusText;
    public Text receivedDataText; // 受信したデータを表示するテキスト
    public Button sendButton;

    async void Start()
    {
        // WebSocketのインスタンスを初期化
        ws = new ClientWebSocket();
        sendButton.onClick.AddListener(SendData);
        await Connect();

        // 接続が成功したらステータスを更新
        statusText.text = "WebSocket connected!";

        // サーバーからのデータ受信を開始
        ReceiveData();
    }

    // WebSocketサーバーへの接続
    async Task Connect()
    {
        try
        {
            Uri serverUri = new Uri("ws://153.121.41.11:5000");  // WebSocketサーバーのURIを指定
            await ws.ConnectAsync(serverUri, CancellationToken.None);
        }
        catch (Exception e)
        {
            statusText.text = "Connection failed: " + e.Message;
        }
    }

    // サーバーにデータを送信する
    public async void SendData()
    {
        if (ws.State == WebSocketState.Open)
        {
            string jsonData = "{\"sensor_id\": \"sensor_01\", \"temperature\": 23.5, \"humidity\": 60.2}";
            var bytes = Encoding.UTF8.GetBytes(jsonData);
            await ws.SendAsync(new ArraySegment<byte>(bytes), WebSocketMessageType.Text, true, CancellationToken.None);
            statusText.text = "Data sent: " + jsonData;
        }
        else
        {
            statusText.text = "WebSocket is not connected!";
        }
    }

    // サーバーからデータを受信する
    public async void ReceiveData()
    {
        var buffer = new byte[1024];
        while (ws.State == WebSocketState.Open)
        {
            try
            {
                WebSocketReceiveResult result = await ws.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                if (result.MessageType == WebSocketMessageType.Text)
                {
                    string receivedMessage = Encoding.UTF8.GetString(buffer, 0, result.Count);
                    receivedDataText.text = "Received: " + receivedMessage;
                    Debug.Log("Received: " + receivedMessage);
                }
            }
            catch (Exception e)
            {
                statusText.text = "Receive error: " + e.Message;
                break;
            }
        }
    }

    void OnDestroy()
    {
        if (ws != null)
        {
            ws.Dispose();
        }
    }
}
