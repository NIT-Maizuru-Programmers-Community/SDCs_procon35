using System.Collections.Generic;
using UnityEngine;

public class ObjectPlacer : MonoBehaviour
{
    public GameObject HousePrefab;
    public GameObject BuildingPrefab;
    public GameObject FactoryPrefab;
    public GameObject TreePrefab;
    public GameObject ConvenienceStorePrefab;
    public GameObject BridgePrefab;
    public GameObject CarshopPrefab;
    public GameObject DamPrefab;
    public GameObject EcoCarshopPrefab;
    public GameObject EcoConvenienceStorePrefab;
    public GameObject EcoFactoryPrefab;
    public GameObject EcoHousePrefab;
    public GameObject EcoParkPrefab;
    public GameObject HospitalPrefab;
    public GameObject ParkPrefab;
    public GameObject RoadPrefab;
    public GameObject SchoolPrefab;
    public GameObject ThermalPrefab;
    public GameObject WindPowerPrefab;
    public GameObject RestaurantPrefab;
    public GameObject EcoRestaurantPrefab;

    public int count = 0;
    public int time = 0;

    private int _linecount; // 内部で使用する変数
    public delegate void CountIncreasedHandler(int newCount);
    public event CountIncreasedHandler CountIncreased;

    public delegate void CountDecreasedHandler(int newCount);
    public event CountDecreasedHandler CountDecreased;
    // count プロパティの定義
    public int linecount
    {
        get { return _linecount; }
        set
        {
            if (value > _linecount)
            {
                OnCountIncreased();
            }
            else if(value<_linecount){
                OnCountDecreased();
            }
            _linecount = value;
        }
    }

    private void OnCountIncreased()
    {
        CountIncreased?.Invoke(_linecount);
        Debug.Log("Count increased to " + _linecount);
        time += 2;
    }
    private void OnCountDecreased()
    {
        CountDecreased?.Invoke(_linecount);
        Debug.Log("Count Decreased to " + _linecount);
        //time += 1;
    }

    // マーカーIDに対するPrefabのマッピング
    private Dictionary<int, GameObject> markerToPrefab;

    // 現在配置されているオブジェクトを追跡するリスト
    public List<GameObject> placedObjects = new List<GameObject>();

    void Start()
    {
        linecount = 0;

        // マーカーIDとPrefabを対応づける
        markerToPrefab = new Dictionary<int, GameObject>
        {
            { 5, HousePrefab },
            { 6, BuildingPrefab },
            { 7, BridgePrefab },
            { 8, CarshopPrefab },
            { 9, ConvenienceStorePrefab },
            { 10, SchoolPrefab },
            { 11, FactoryPrefab },
            { 12, ThermalPrefab },
            { 13, ParkPrefab },
            { 14, HospitalPrefab },
            { 15, TreePrefab },
            { 16, WindPowerPrefab },
            { 17, DamPrefab },
            { 18, EcoParkPrefab },
            { 19, EcoHousePrefab },
            { 20, EcoCarshopPrefab },
            { 21, EcoConvenienceStorePrefab },
            { 22, EcoFactoryPrefab },
            { 23, RestaurantPrefab},
            { 24, EcoRestaurantPrefab}
        };
    }

    public void ClearPlacedObjects()
    {
        foreach (GameObject obj in placedObjects)
        {
            Destroy(obj);
        }
        placedObjects.Clear();
    }

    public void PlaceObject(int markerID, Vector3 position)
    {
        if (markerToPrefab.ContainsKey(markerID))
        {
            GameObject prefabToPlace = markerToPrefab[markerID];
            Quaternion prefabRotation = prefabToPlace.transform.rotation;
            GameObject placedObject = Instantiate(prefabToPlace, position, prefabRotation);

            placedObjects.Add(placedObject);
            Debug.Log($"Placed {prefabToPlace.name} at {position}");
        }
        else
        {
            Debug.LogWarning($"Invalid marker ID: {markerID}");
        }
    }


    void Update()
    {
        if (count == linecount)
        {

        }
        else if (count > linecount)
        {
            linecount += 1;
        }
        else if (count < linecount)
        {
            //linecount -= 1;
        }

        // Oキーが押されたときに複数の家を生成
        if (Input.GetKeyDown(KeyCode.O))
        {
            GenerateHouses(5); // 例えば5個の家を生成
        }
    }
    public void Count()
    {
        count = placedObjects.Count /6;
    }



    // 家を指定した数だけランダムな位置に生成するメソッド
    private void GenerateHouses(int numberOfHouses)
    {
        for (int i = 0; i < numberOfHouses; i++)
        {
            // ランダムな位置を生成（範囲を調整してください）
            Vector3 randomPosition = new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10));
            PlaceObject(5, randomPosition); // markerID 5はHousePrefabに対応
        }
    }
}


/*
using System.Collections.Generic;
using UnityEngine;

public class ObjectPlacer : MonoBehaviour
{
    // 複数のPrefabをInspectorからアサインする
    public GameObject HousePrefab;
    public GameObject BuildingPrefab;
    public GameObject FactoryPrefab;
    public GameObject TreePrefab;
    public GameObject ConvenienceStorePrefab;
    public GameObject BridgePrefab;
    public GameObject CarshopPrefab;
    public GameObject DamPrefab;
    public GameObject EcoCarshopPrefab;
    public GameObject EcoConvenienceStorePrefab;
    public GameObject EcoFactoryPrefab;
    public GameObject EcoHousePrefab;
    public GameObject EcoParkPrefab;
    public GameObject HospitalPrefab;
    public GameObject ParkPrefab;
    public GameObject RoadPrefab;
    public GameObject SchoolPrefab;
    public GameObject ThermalPrefab;
    public GameObject WindPowerPrefab;

    public int count = 0;

    private int _linecount; // 内部で使用する変数

    // count プロパティの定義
    public int linecount
    {
        get { return _linecount; }
        set
        {
            if (value > _linecount)
            {
                OnCountIncreased();
            }
            _linecount = value;
        }
    }
    
    // マーカーIDに対するPrefabのマッピング
    private Dictionary<int, GameObject> markerToPrefab;

    // 現在配置されているオブジェクトを追跡するリスト
    public List<GameObject> placedObjects = new List<GameObject>();

    void Start()
    {
        // 初期値の設定
        linecount = 0;

        // マーカーIDとPrefabを対応づける
        markerToPrefab = new Dictionary<int, GameObject>
        {
            { 5, HousePrefab },
            { 6, BuildingPrefab },
            { 7, BridgePrefab },
            { 8, CarshopPrefab },
            { 9, ConvenienceStorePrefab },
            { 10, SchoolPrefab },
            { 11, FactoryPrefab },
            { 12, ThermalPrefab },
            { 13, ParkPrefab },
            { 14, HospitalPrefab },
            { 15, TreePrefab },
            { 16, WindPowerPrefab },
            { 17, DamPrefab },
            { 18, EcoParkPrefab },
            { 19, EcoHousePrefab },
            { 20, EcoCarshopPrefab },
            { 21, EcoConvenienceStorePrefab },
            { 22, EcoFactoryPrefab }
        };
    }

    // 新しいオブジェクトを配置する前に、既存のオブジェクトを削除
    public void ClearPlacedObjects()
    {
        foreach (GameObject obj in placedObjects)
        {
            Destroy(obj);
        }
        placedObjects.Clear();
    }

    // 受信したデータに基づいてオブジェクトを配置するメソッド
    public void PlaceObject(int markerID, Vector3 position)
    {
        if (markerToPrefab.ContainsKey(markerID))
        {
            GameObject prefabToPlace = markerToPrefab[markerID];

            // プレハブの回転情報を保持
            Quaternion prefabRotation = prefabToPlace.transform.rotation;

            // プレハブのクローンを指定された位置に、元の回転情報で配置
            GameObject placedObject = Instantiate(prefabToPlace, position, prefabRotation);

            // 生成したオブジェクトを追跡リストに追加
            placedObjects.Add(placedObject);

            Debug.Log($"Placed {prefabToPlace.name} at {position}");

            // オブジェクトの数をログに出力
            Debug.Log($"Placed {prefabToPlace.name} at {position}. Current object count: {placedObjects.Count}");
            count = (placedObjects.Count * 3) + 10;
        }
        else
        {
            Debug.LogWarning($"Invalid marker ID: {markerID}");
        }
    }

    public delegate void CountIncreasedHandler(int newCount);
    public event CountIncreasedHandler CountIncreased;

    void Update()
    {
        if (count > linecount)
        {
            linecount += 3;
        }
    }

    private void OnCountIncreased()
    {
        CountIncreased?.Invoke(_linecount);
        Debug.Log("Count increased to " + _linecount);
    }
}*/


/*
//角度情報も持たせたスクリプト
using System.Collections.Generic;
using UnityEngine;

public class ObjectPlacer : MonoBehaviour
{
    // 複数のPrefabをInspectorからアサインする
    public GameObject HousePrefab;
    public GameObject BuildingPrefab;
    public GameObject FactoryPrefab;
    public GameObject TreePrefab;
    public GameObject ConvenienceStorePrefab;
    public GameObject BridgePrefab;
    public GameObject CarshopPrefab;
    public GameObject DamPrefab;
    public GameObject EcoCarshopPrefab;
    public GameObject EcoConvenienceStorePrefab;
    public GameObject EcoFactoryPrefab;  // ここが重要な追加
    public GameObject EcoHousePrefab;
    public GameObject EcoParkPrefab;
    public GameObject HospitalPrefab;
    public GameObject ParkPrefab;
    public GameObject RoadPrefab;
    public GameObject SchoolPrefab;
    public GameObject ThermalPrefab;
    public GameObject WindPowerPrefab;

    public int count;
    private int _count;

    // マーカーIDに対するPrefabのマッピング
    private Dictionary<int, GameObject> markerToPrefab;
    
    // 現在配置されているオブジェクトを追跡するリスト
    public List<GameObject> placedObjects = new List<GameObject>();

    void Start()
    {
        // マーカーIDとPrefabを対応づける
        markerToPrefab = new Dictionary<int, GameObject>
        {
            { 5, HousePrefab },                    // マーカーID 5 -> 家（ノーマル）
            { 6, BuildingPrefab },                 // マーカーID 6 -> ビル（ノーマル）
            { 7, BridgePrefab },                   // マーカーID 7 -> 橋（ノーマル）
            { 8, CarshopPrefab },                  // マーカーID 8 -> 車屋さん
            { 9, ConvenienceStorePrefab },         // マーカーID 9 -> スーパーコンビニ
            { 10, SchoolPrefab },                  // マーカーID 10 -> 学校
            { 11, FactoryPrefab },                 // マーカーID 11 -> 工場（排水垂れ流し）
            { 12, ThermalPrefab },                 // マーカーID 12 -> 発電所（火力）
            { 13, ParkPrefab },                    // マーカーID 13 -> 公園（ノーマル）
            { 14, HospitalPrefab },                // マーカーID 14 -> 病院
            { 15, TreePrefab },                    // マーカーID 15 -> 木
            { 16, WindPowerPrefab },               // マーカーID 16 -> 風車
            { 17, DamPrefab },                     // マーカーID 17 -> ダム（水力発電）
            { 18, EcoParkPrefab },                 // マーカーID 18 -> 公園（緑化）
            { 19, EcoHousePrefab },                // マーカーID 19 -> 家（エコ）
            { 20, EcoCarshopPrefab },              // マーカーID 20 -> 車屋さん（水素自動車）
            { 21, EcoConvenienceStorePrefab },     // マーカーID 21 -> スーパー（エコ）
            { 22, EcoFactoryPrefab }               // マーカーID 22 -> 工場（排水処理）
        };
    }

    // 新しいオブジェクトを配置する前に、既存のオブジェクトを削除
    public void ClearPlacedObjects()
    {
        foreach (GameObject obj in placedObjects)
        {
            Destroy(obj);
        }
        placedObjects.Clear();
    }

    // 受信したデータに基づいてオブジェクトを配置するメソッド
    public void PlaceObject(int markerID, Vector3 position)
    {
    if (markerToPrefab.ContainsKey(markerID))
    {
        GameObject prefabToPlace = markerToPrefab[markerID];

        // プレハブの回転情報を保持
        Quaternion prefabRotation = prefabToPlace.transform.rotation;

        // プレハブのクローンを指定された位置に、元の回転情報で配置
        GameObject placedObject = Instantiate(prefabToPlace, position, prefabRotation);

        // 生成したオブジェクトを追跡リストに追加
        placedObjects.Add(placedObject);

        Debug.Log($"Placed {prefabToPlace.name} at {position}");

        // オブジェクトの数をログに出力
        Debug.Log($"Placed {prefabToPlace.name} at {position}. Current object count: {placedObjects.Count}");
        count=(placedObjects.Count*3)+10;
    }
    else
    {
        Debug.LogWarning($"Invalid marker ID: {markerID}");
    }

    {
        get { return _count; }
        set
        {
            if (value > _count)
            {
                OnCountIncreased();
            }
            _count = value;
        }
    }

    public delegate void CountIncreasedHandler(int newCount);
    public event CountIncreasedHandler CountIncreased;

    private void OnCountIncreased()
    {
        CountIncreased?.Invoke(_count);
        Debug.Log("Count increased to " + _count);
    }

    void Start()
    {
        count = 0; // 初期値の設定
    }

}*/










/*
using UnityEngine;
using System.Collections.Generic;

public class ObjectPlacer : MonoBehaviour
{
    // プレハブの参照リスト
    public GameObject housePrefab;
    public GameObject buildingPrefab;
    public GameObject factoryPrefab;
    public GameObject treePrefab; // 新しい木のプレハブ
    public GameObject convenienceStorePrefab; // 新しいコンビニのプレハブ

    public GameObject BridgePrefab;
    public GameObject CarshopPrefab;
    public GameObject DamPrefab;
    public GameObject EcoCarshopPrefab;
    public GameObject EcoConvenienceStorePrefab;
    public GameObject EcoFactoryprefab;
    public GameObject EcoHousePrefab;
    public GameObject EcoParkPrefab;
    public GameObject HospitalPrefab;
    public GameObject HousePrefab;
    public GameObject ParkPrefab;
    public GameObject RoadPrefab;
    public GameObject SchoolPreafab;
    public GameObject ThermalPrefab;
    public GameObject TreePrefab;
    public GameObject WindPowerPrefab;

    // マーカーIDとプレハブの対応
    private Dictionary<int, GameObject> markerToPrefab;

    int randomMarkerID = 0;

    void Start()
    {
        // マーカーIDとプレハブの辞書を初期化
        markerToPrefab = new Dictionary<int, GameObject>
        {
            { 0, housePrefab },                    // マーカーID 0 -> 家
            { 1, buildingPrefab },                 // マーカーID 1 -> ビル
            { 2, factoryPrefab },                  // マーカーID 2 -> 工場
            { 3, treePrefab },                     // マーカーID 3 -> 木
            { 4, convenienceStorePrefab },         // マーカーID 4 -> コンビニ
            { 5, BridgePrefab },                   // マーカーID 5 -> 橋
            { 6, CarshopPrefab },                  // マーカーID 6 -> 車の店
            { 7, DamPrefab },                      // マーカーID 7 -> ダム
            { 8, EcoCarshopPrefab },               // マーカーID 8 -> エコ車の店
            { 9, EcoConvenienceStorePrefab },      // マーカーID 9 -> エココンビニ
            { 10, EcoFactoryprefab },              // マーカーID 10 -> エコ工場
            { 11, EcoHousePrefab },                // マーカーID 11 -> エコ家
            { 12, EcoParkPrefab },                 // マーカーID 12 -> エコパーク
            { 13, HospitalPrefab },                // マーカーID 13 -> 病院
            { 14, HousePrefab },                   // マーカーID 14 -> 家 (別のバージョン)
            { 15, ParkPrefab },                    // マーカーID 15 -> 公園
            { 16, RoadPrefab },                    // マーカーID 16 -> 道路
            { 17, SchoolPreafab },                 // マーカーID 17 -> 学校
            { 18, ThermalPrefab },                 // マーカーID 18 -> 火力発電所
            { 19, TreePrefab },                    // マーカーID 19 -> 木 (別のバージョン)
            { 20, WindPowerPrefab }                // マーカーID 20 -> 風力発電
        };
        for(int i = 0; i < 21; i++)
        {
            PlaceObject(i, GetRandomPosition()); // ランダムな位置でオブジェクトを配置
        }
    }

    void Update()
    {
        // キー1を押したときにランダムなオブジェクトを生成
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            randomMarkerID = 0;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            randomMarkerID = 1;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            randomMarkerID = 2;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            randomMarkerID = 3;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            randomMarkerID = 4;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            randomMarkerID = 5;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            randomMarkerID = 6;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            randomMarkerID = 7;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            randomMarkerID = 8;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            randomMarkerID = 9;
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            randomMarkerID = 10;
        }
        else if (Input.GetKeyDown(KeyCode.T))
        {
            randomMarkerID = 11;
        }
        else if (Input.GetKeyDown(KeyCode.Y))
        {
            randomMarkerID = 12;
        }
        else if (Input.GetKeyDown(KeyCode.U))
        {
            randomMarkerID = 13;
        }
        else if (Input.GetKeyDown(KeyCode.I))
        {
            randomMarkerID = 14;
        }
        else if (Input.GetKeyDown(KeyCode.O))
        {
            randomMarkerID = 15;
        }
        else if (Input.GetKeyDown(KeyCode.P))
        {
            randomMarkerID = 16;
        }
        else if (Input.GetKeyDown(KeyCode.F))
        {
            randomMarkerID = 17;
        }
        else if (Input.GetKeyDown(KeyCode.G))
        {
            randomMarkerID = 18;
        }
        else if (Input.GetKeyDown(KeyCode.H))
        {
            randomMarkerID = 19;
        }
        else if (Input.GetKeyDown(KeyCode.J))
        {
            randomMarkerID = 20;
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            PlaceObject(randomMarkerID, GetRandomPosition()); // ランダムな位置でオブジェクトを配置
        }
    }

    public void PlaceObject(int markerID, Vector3 markerPosition)
    {
        if (markerToPrefab.ContainsKey(markerID))
        {
            // 配置するプレハブを取得
            GameObject prefabToPlace = markerToPrefab[markerID];

            // プレハブのデフォルトの回転を取得
            Quaternion originalRotation = prefabToPlace.transform.rotation;

            // 一時オブジェクトを使用して、元のY位置を取得
            float originalY = prefabToPlace.transform.position.y;

            // 新しい配置位置を計算 (XとZは新しい位置、Yはプレハブの元の位置)
            Vector3 placementPosition = new Vector3(markerPosition.x, originalY, markerPosition.z);

            // オブジェクトを指定した位置と回転で生成
            Instantiate(prefabToPlace, placementPosition, originalRotation);
        }
        else
        {
            Debug.LogWarning("無効なマーカーID: " + markerID);
        }
    }

    // ランダムな位置を生成する関数
    private Vector3 GetRandomPosition()
    {
        // XとZのランダムな位置を定義
        float x = Random.Range(-10, 10);
        float z = Random.Range(-10, 10);
        return new Vector3(x, 0, z);
    }
}
*/