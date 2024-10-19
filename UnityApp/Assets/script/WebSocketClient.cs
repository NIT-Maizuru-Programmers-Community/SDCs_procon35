using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using System.Net.WebSockets;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;  // Listを使うために追加

public class WebSocketClient : MonoBehaviour
{
    private ClientWebSocket ws;
    private Uri serverUri = new Uri("ws://153.121.41.11:8765");

    public ObjectPlacer objectplacer;  // Inspectorでアタッチする

    // ObjectPlacerの参照 (Prefabを配置するためのスクリプト)
    public ObjectPlacer objectPlacer;

    async void Start()
    {
        ws = new ClientWebSocket();
        await ConnectToServer();  // サーバーに接続

        // 10秒ごとにリクエストを送信するコルーチンを開始
        StartCoroutine(RequestDataPeriodically());
    }

    // 一定間隔でリクエストを送信するコルーチン
    private IEnumerator RequestDataPeriodically()
    {
        while (true)
        {
            // 非同期メソッドを呼び出し、Taskの完了を待つ
            var requestTask = RequestJsonData();
            while (!requestTask.IsCompleted) // タスクの完了を待機
            {
                yield return null;
            }

            // 1秒待つ
            yield return new WaitForSeconds(1);
        }
    }

    // サーバーへ接続するメソッド
    private async Task ConnectToServer()
    {
        try
        {
            await ws.ConnectAsync(serverUri, CancellationToken.None);
            Debug.Log("Connected to server");
        }
        catch (Exception e)
        {
            Debug.LogError($"Connection failed: {e.Message}");
        }
    }

    // サーバーからJSONデータをリクエストして受信するメソッド
    private async Task RequestJsonData()
    {
        try
        {
            // WebSocketの状態を確認
            Debug.Log("Current WebSocket state: " + ws.State);

            // WebSocketがOpen状態であるか確認
            if (ws.State == WebSocketState.Open)
            {
                // リクエストメッセージを送信
                byte[] requestBytes = Encoding.UTF8.GetBytes("Requesting data");
                await ws.SendAsync(new ArraySegment<byte>(requestBytes), WebSocketMessageType.Text, true, CancellationToken.None);

                // サーバーからのレスポンスを受信
                var buffer = new byte[10240];
                var result = await ws.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                string jsonResponse = Encoding.UTF8.GetString(buffer, 0, result.Count);

                // 受信データをログに表示
                Debug.Log("Received JSON data: " + jsonResponse);

                // 受信データを処理
                ProcessJsonData(jsonResponse);
            }
            else if (ws.State == WebSocketState.CloseReceived)
            {
                // WebSocketがサーバーから接続終了を受信している場合、手動で接続を閉じる
                Debug.LogWarning("WebSocket state is CloseReceived. Closing connection...");
                await ws.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing after receiving CloseReceived", CancellationToken.None);
                await ReconnectToServer();
            }
            else
            {
                Debug.LogWarning("WebSocket is not open. Current state: " + ws.State + ". Attempting to reconnect...");
                await ReconnectToServer();
            }
        }
        catch (WebSocketException we)
        {
            Debug.LogError($"WebSocket exception: {we.Message}");
            await ReconnectToServer();  // WebSocket例外が発生した場合は再接続を試みる
        }
        catch (Exception e)
        {
            Debug.LogError($"Error receiving data: {e.Message}");
        }
    }

    private async Task ReconnectToServer()
    {
        // WebSocketが既に接続されているか確認
        if (ws != null && ws.State == WebSocketState.Open)
        {
            Debug.LogWarning("WebSocket is already connected.");
            return;  // すでに接続されている場合は再接続しない
        }

        // WebSocketが接続中または接続済みの場合は何もしない
        if (ws != null && (ws.State == WebSocketState.Connecting || ws.State == WebSocketState.Open))
        {
            Debug.LogWarning("WebSocket is already connecting or open.");
            return;
        }

        // WebSocketがCloseReceivedの場合、接続を閉じて再接続
        if (ws.State == WebSocketState.CloseReceived)
        {
            Debug.Log("WebSocket received close request. Closing the connection...");
            await ws.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing connection", CancellationToken.None);
            ws.Dispose();
            ws = new ClientWebSocket();  // 新しいWebSocketインスタンスを作成
        }

        // WebSocketが閉じられている場合にのみ新しい接続を試みる
        if (ws.State == WebSocketState.Closed || ws.State == WebSocketState.Aborted)
        {
            ws.Dispose();
            ws = new ClientWebSocket();  // 新しいインスタンスを作成して接続を再試行
            await ConnectToServer();
        }
    }

    // 受信したJSONデータを処理するメソッド
    private void ProcessJsonData(string jsonData)
    {
        try
        {
            // JSONデータをデシリアライズして、MarkerDataのリストに変換
            List<MarkerData> markers = JsonConvert.DeserializeObject<List<MarkerData>>(jsonData);

            if (markers != null && markers.Count > 0)
            {
                // 既存のオブジェクトを削除
                objectPlacer.ClearPlacedObjects();

                // 新しいオブジェクトを配置
                foreach (var marker in markers)
                {
                    if (marker.x >= 0 && marker.y >= 0)
                    {
                        // サーバーから受け取ったX, Y座標 (500 x 500 の範囲) を -10 ~ 10 の範囲に反転してスケーリング
                        float unityX = ((500 - marker.x) / 500.0f) * (10 - (-10)) + (-10);
                        float unityZ = ((500 - marker.y) / 500.0f) * (10 - (-10)) + (-10);

                        // ObjectPlacerに座標とIDを渡してPrefabを配置
                        objectPlacer.PlaceObject(marker.id, new Vector3(unityX, 0, unityZ));
                        objectPlacer.PlaceObject(marker.id, new Vector3(unityX + 20, 0, unityZ));
                        objectPlacer.PlaceObject(marker.id, new Vector3(unityX - 20, 0, unityZ));
                        objectPlacer.PlaceObject(marker.id, new Vector3(unityX, 0, unityZ - 20));
                        objectPlacer.PlaceObject(marker.id, new Vector3(unityX + 20, 0, unityZ - 20));
                        objectPlacer.PlaceObject(marker.id, new Vector3(unityX - 20, 0, unityZ - 20));
                    }
                    else
                    {
                        Debug.LogError($"Marker {marker.id}'s coordinates are out of range or invalid.");
                    }
                }
                objectplacer.Count();
            }
            /*else
            {
                Debug.LogError("Received marker data is null or empty.");
            }*/
        }
        catch (JsonException je)
        {
            Debug.LogError($"JSON deserialization error: {je.Message}");
            Debug.LogError("Received JSON data: " + jsonData);  // 受け取ったJSONデータをログに表示
        }
        catch (Exception e)
        {
            Debug.LogError($"Error processing JSON data: {e.Message}");
        }
    }

    // マーカーのデータ構造 (JSONデータを受け取るためのクラス)
    [Serializable]
    public class MarkerData
    {
        public int id;  // markerID (オブジェクトの識別子)
        public float x; // X座標
        public float y; // Y座標
    }
}




/*10秒ごとに設置するシステム
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using System.Net.WebSockets;
using Newtonsoft.Json;
using System.Collections; // ←追加済み
using System.Collections.Generic; // List<T> を使用するために必要

public class WebSocketClient : MonoBehaviour
{
    private ClientWebSocket ws;
    private Uri serverUri = new Uri("ws://153.121.41.11:8765");

    // ObjectPlacerの参照 (Prefabを配置するためのスクリプト)
    public ObjectPlacer objectPlacer;

    async void Start()
    {
        ws = new ClientWebSocket();
        await ConnectToServer();  // サーバーに接続

        // 10秒ごとにリクエストを送信するコルーチンを開始
        StartCoroutine(RequestDataPeriodically());
    }

    // 一定間隔でリクエストを送信するコルーチン
    private IEnumerator RequestDataPeriodically()
    {
        while (true)
        {
            // 非同期メソッドを呼び出し、Taskの完了を待つ
            var requestTask = RequestJsonData();
            while (!requestTask.IsCompleted) // タスクの完了を待機
            {
                yield return null;
            }

            // 10秒待つ
            yield return new WaitForSeconds(10);
        }
    }

    // サーバーへ接続するメソッド
    private async Task ConnectToServer()
    {
        try
        {
            if (ws != null)
            {
                ws.Dispose();  // 古いWebSocketをクリーンアップ
                ws = new ClientWebSocket();
            }

            await ws.ConnectAsync(serverUri, CancellationToken.None);
            Debug.Log("Connected to server");
        }
        catch (Exception e)
        {
            Debug.LogError($"Connection failed: {e.Message}");
        }
    }

    // サーバーからJSONデータをリクエストして受信するメソッド
    private async Task RequestJsonData()
    {
        try
        {
            // WebSocketがOpen状態であるか確認
            if (ws.State == WebSocketState.Open)
            {
                // リクエストメッセージを送信
                byte[] requestBytes = Encoding.UTF8.GetBytes("Requesting data");
                await ws.SendAsync(new ArraySegment<byte>(requestBytes), WebSocketMessageType.Text, true, CancellationToken.None);

                // サーバーからのレスポンスを受信
                var buffer = new byte[1024];
                var result = await ws.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                string jsonResponse = Encoding.UTF8.GetString(buffer, 0, result.Count);

                // 受信データをログに表示
                Debug.Log("Received JSON data: " + jsonResponse);

                // 受信データを処理
                ProcessJsonData(jsonResponse);
            }
            else
            {
                Debug.LogWarning("WebSocket is not open. Attempting to reconnect...");
                await ReconnectToServer();
            }
        }
        catch (WebSocketException we)
        {
            Debug.LogError($"WebSocket exception: {we.Message}");
            await ReconnectToServer();  // WebSocket例外が発生した場合は再接続を試みる
        }
        catch (Exception e)
        {
            Debug.LogError($"Error receiving data: {e.Message}");
        }
    }

    // 再接続を試みるメソッド
    private async Task ReconnectToServer()
    {
        Debug.LogWarning("Reconnecting to server...");
        await ConnectToServer();
    }

    // 受信したJSONデータを処理するメソッド
    private void ProcessJsonData(string jsonData)
    {
    try
    {
        // JSONデータをデシリアライズして、MarkerDataのリストに変換
        List<MarkerData> markers = JsonConvert.DeserializeObject<List<MarkerData>>(jsonData);

        if (markers != null && markers.Count > 0)
        {
            foreach (var marker in markers)
            {
                // Nullチェックと範囲チェック
                if (marker.x >= 0 && marker.y >= 0)
                {
                    // サーバーから受け取ったX, Y座標 (500 x 500 の範囲) を -10 ~ 10 の範囲にスケーリング
                    float unityX = ((marker.x - 0) / (500 - 0)) * (10 - (-10)) + (-10);
                    float unityZ = ((marker.y - 0) / (500 - 0)) * (10 - (-10)) + (-10);

                    // 座標変換結果をログに表示
                    Debug.Log($"Converted Unity Coordinates for marker {marker.id}: X = {unityX}, Z = {unityZ}");

                    // ObjectPlacerに座標とIDを渡してPrefabを配置
                    objectPlacer.PlaceObject(marker.id, new Vector3(unityX, 0, unityZ));
                }
                else
                {
                    Debug.LogError($"Marker {marker.id}'s coordinates are out of range or invalid.");
                }
            }
        }
        else
        {
            Debug.LogError("Received marker data is null or empty.");
        }
    }
    catch (JsonException je)
    {
        Debug.LogError($"JSON deserialization error: {je.Message}");
        Debug.LogError("Received JSON data: " + jsonData);  // 受け取ったJSONデータをログに表示
    }
    catch (Exception e)
    {
        Debug.LogError($"Error processing JSON data: {e.Message}");
    }
    }

    // マーカーのデータ構造 (JSONデータを受け取るためのクラス)
    [Serializable]
    public class MarkerData
    {
        public int id;  // markerID (オブジェクトの識別子)
        public float x; // X座標
        public float y; // Y座標
    }
}


/*10月4日　深夜
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using System.Net.WebSockets;
using Newtonsoft.Json;
using System.Collections; // ←追加済み

public class WebSocketClient : MonoBehaviour
{
    private ClientWebSocket ws;
    private Uri serverUri = new Uri("ws://153.121.41.11:8765");

    // ObjectPlacerの参照 (Prefabを配置するためのスクリプト)
    public ObjectPlacer objectPlacer;

    async void Start()
    {
        ws = new ClientWebSocket();
        await ConnectToServer();  // サーバーに接続

        // 10秒ごとにリクエストを送信するコルーチンを開始
        StartCoroutine(RequestDataPeriodically());
    }

    // 一定間隔でリクエストを送信するコルーチン
    private IEnumerator RequestDataPeriodically()
    {
        while (true)
        {
            // 非同期メソッドを呼び出し、Taskの完了を待つ
            var requestTask = RequestJsonData();
            while (!requestTask.IsCompleted) // タスクの完了を待機
            {
                yield return null;
            }

            // 10秒待つ
            yield return new WaitForSeconds(10);
        }
    }

    // サーバーへ接続するメソッド
    private async Task ConnectToServer()
    {
        try
        {
            await ws.ConnectAsync(serverUri, CancellationToken.None);
            Debug.Log("Connected to server");
        }
        catch (Exception e)
        {
            Debug.LogError($"Connection failed: {e.Message}");
        }
    }

    // サーバーからJSONデータをリクエストして受信するメソッド
    private async Task RequestJsonData()
    {
        try
        {
            // リクエストメッセージを送信
            byte[] requestBytes = Encoding.UTF8.GetBytes("Requesting data");
            await ws.SendAsync(new ArraySegment<byte>(requestBytes), WebSocketMessageType.Text, true, CancellationToken.None);

            // サーバーからのレスポンスを受信
            var buffer = new byte[1024];
            var result = await ws.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            string jsonResponse = Encoding.UTF8.GetString(buffer, 0, result.Count);

            // 受信データをログに表示
            Debug.Log("Received JSON data: " + jsonResponse);

            // 受信データを処理
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
        try
        {
            // JSONデータをデシリアライズ (MarkerDataというID, X, Yを含むクラスに変換)
            var marker = JsonConvert.DeserializeObject<MarkerData>(jsonData);

            // サーバーから受け取ったX, Y座標 (500 x 500 の範囲) を -10 ~ 10 の範囲にスケーリング
            float unityX = ((marker.x - 0) / (500 - 0)) * (10 - (-10)) + (-10);
            float unityZ = ((marker.y - 0) / (500 - 0)) * (10 - (-10)) + (-10);

            // 座標変換結果をログに表示
            Debug.Log($"Converted Unity Coordinates: X = {unityX}, Z = {unityZ}");

            // ObjectPlacerに座標とIDを渡してPrefabを配置
            objectPlacer.PlaceObject(marker.id, new Vector3(unityX, 0, unityZ));
        }
        catch (Exception e)
        {
            Debug.LogError($"Error processing JSON data: {e.Message}");
        }
    }

    // マーカーのデータ構造 (JSONデータを受け取るためのクラス)
    [Serializable]
    public class MarkerData
    {
        public int id;  // markerID (オブジェクトの識別子)
        public float x; // X座標
        public float y; // Y座標
    }
}


/*
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using System.Net.WebSockets;
using Newtonsoft.Json;


public class WebSocketClient : MonoBehaviour
{
    private int i = 0;

    private ClientWebSocket ws;
    private Uri serverUri = new Uri("ws://153.121.41.11:8765");  // �T�[�o�[�̃A�h���X

    // ObjectPlacer�̎Q�� (Prefab��z�u���邽�߂̃X�N���v�g)
    public ObjectPlacer objectPlacer;

    async void Start()
    {
        ws = new ClientWebSocket();
        await ConnectToServer();  // �T�[�o�[�ɐڑ�
        await RequestJsonData();  // �f�[�^�����N�G�X�g
    }

    void Update()
    {
        i++;
        if (i / 600==1)
        {
            RequestJsonData();  // �f�[�^�����N�G�X�g
            i = 0;
        }
    }
    // �T�[�o�[�֐ڑ����郁�\�b�h
    private async Task ConnectToServer()
    {
        try
        {
            await ws.ConnectAsync(serverUri, CancellationToken.None);
            Debug.Log("Connected to server");
        }
        catch (Exception e)
        {
            Debug.LogError($"Connection failed: {e.Message}");
        }
    }

    // �T�[�o�[����JSON�f�[�^�����N�G�X�g���A��M���郁�\�b�h
    private async Task RequestJsonData()
    {
        try
        {
            // ���N�G�X�g���b�Z�[�W�𑗐M
            byte[] requestBytes = Encoding.UTF8.GetBytes("Requesting data");
            await ws.SendAsync(new ArraySegment<byte>(requestBytes), WebSocketMessageType.Text, true, CancellationToken.None);

            // �T�[�o�[����̃��X�|���X����M
            var buffer = new byte[1024];
            var result = await ws.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            string jsonResponse = Encoding.UTF8.GetString(buffer, 0, result.Count);

            // ��M�����f�[�^�����O�ɏo��
            Debug.Log("Received JSON data: " + jsonResponse);

            // ��M�f�[�^���������ăI�u�W�F�N�g��z�u
            ProcessJsonData(jsonResponse);
        }
        catch (Exception e)
        {
            Debug.LogError($"Error receiving data: {e.Message}");
        }
    }

    // ��M����JSON�f�[�^���������郁�\�b�h
    private void ProcessJsonData(string jsonData)
    {
        try
        {
            // JSON�f�[�^����� (MarkerData��ID, X, Y�����N���X)
            var marker = JsonConvert.DeserializeObject<MarkerData>(jsonData);

            // �T�[�o�[����󂯎����X, Y���W (500 x 500 �͈̔�) �� -10 ~ 10 �͈̔͂ɃX�P�[���ϊ�
            float unityX = ((marker.x - 0) / (500 - 0)) * (10 - (-10)) + (-10);
            float unityZ = ((marker.y - 0) / (500 - 0)) * (10 - (-10)) + (-10);

            // ���O�ɕϊ���̍��W��\��
            Debug.Log($"Converted Unity Coordinates: X = {unityX}, Z = {unityZ}");

            // ObjectPlacer�ɍ��W��ID��n���āAPrefab��z�u
            objectPlacer.PlaceObject(marker.id, new Vector3(unityX, 0, unityZ));  // ���W��Unity���W�ɕϊ����Ĕz�u
        }
        catch (Exception e)
        {
            Debug.LogError($"Error processing JSON data: {e.Message}");
        }
    }

    // �}�[�J�[�̃f�[�^�\�� (JSON�f�[�^���󂯎�邽�߂̃N���X)
    [Serializable]
    public class MarkerData
    {
        public int id;  // markerID (�I�u�W�F�N�g�̎�ނ�����)
        public float x; // X���W
        public float y; // Y���W
    }
}*/