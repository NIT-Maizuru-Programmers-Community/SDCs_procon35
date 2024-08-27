/*
using UnityEngine;
using System.Collections.Generic;

public class ObjectPlacer : MonoBehaviour
{
    public GameObject housePrefab; // 家のPrefab
    public GameObject buildingPrefab; // ビルのPrefab
    public GameObject factoryPrefab; // 工場のPrefab

    // マーカーIDとPrefabの対応を設定
    private Dictionary<int, GameObject> markerToPrefab;

    void Start()
    {
        // マーカーIDに対して対応するPrefabを設定
        markerToPrefab = new Dictionary<int, GameObject>
        {
            { 0, housePrefab },     // マーカーID 0 -> 家
            { 1, buildingPrefab },  // マーカーID 1 -> ビル
            { 2, factoryPrefab }    // マーカーID 2 -> 工場
        };
    }

    // 外部から呼び出すメソッドでオブジェクトを配置
    public void PlaceObject(int markerID, Vector3 markerPosition)
    {
        if (markerToPrefab.ContainsKey(markerID))
        {
            // プレハブの取得
            GameObject prefabToPlace = markerToPrefab[markerID];

            // Y軸を0に固定してオブジェクトを配置
            Vector3 placementPosition = new Vector3(markerPosition.x, 0, markerPosition.z);
            Instantiate(prefabToPlace, placementPosition, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning("Invalid marker ID: " + markerID);
        }
    }
}
*/

using UnityEngine;
using System.Collections.Generic;

public class ObjectPlacer : MonoBehaviour
{
    public GameObject housePrefab; // 家のPrefab
    public GameObject buildingPrefab; // ビルのPrefab
    public GameObject factoryPrefab; // 工場のPrefab

    // マーカーIDとPrefabの対応を設定
    private Dictionary<int, GameObject> markerToPrefab;

    void Start()
    {
        // マーカーIDに対して対応するPrefabを設定
        markerToPrefab = new Dictionary<int, GameObject>
        {
            { 0, housePrefab },     // マーカーID 0 -> 家
            { 1, buildingPrefab },  // マーカーID 1 -> ビル
            { 2, factoryPrefab }    // マーカーID 2 -> 工場
        };
    }

    void Update()
    {
        // テスト用のコード
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            PlaceObject(0, new Vector3(0, 0, 0)); // マーカーID 0の家を(0, 0, 0)に配置
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            PlaceObject(0, new Vector3(50, 0, 20)); // マーカーID 0の家を(0, 0, 0)に配置
        }

        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            PlaceObject(0, new Vector3(0, 0, 0)); // マーカーID 0の家を(0, 0, 0)に配置
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            PlaceObject(0, new Vector3(100, 0, 0)); // マーカーID 1のビルを(2, 0, 0)に配置
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            PlaceObject(2, new Vector3(4, 0, 100)); // マーカーID 2の工場を(4, 0, 0)に配置
        }
    }

    public void PlaceObject(int markerID, Vector3 markerPosition)
    {
        if (markerToPrefab.ContainsKey(markerID))
        {
            // プレハブの取得
            GameObject prefabToPlace = markerToPrefab[markerID];

            // Y軸を0に固定してオブジェクトを配置
            Vector3 placementPosition = new Vector3(markerPosition.x, 0, markerPosition.z);
            Instantiate(prefabToPlace, placementPosition, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning("Invalid marker ID: " + markerID);
        }
    }
}
