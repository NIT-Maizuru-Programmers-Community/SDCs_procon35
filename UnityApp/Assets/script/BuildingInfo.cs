using UnityEngine;

public class BuildingInfo : MonoBehaviour
{
    public int population; // このオブジェクトの人口
    public int electricityUsage; // このオブジェクトの電気使用量
    public int co2;//このオブジェクトの二酸化炭素の排出量
    public int qol;//このオブジェクトが町に与える幸福度

    public int pollution;//このオブジェクトが川に与える汚染度



    private void Start()
    {
        // オブジェクトが生成されたとき、CityManagerに通知して人口と電気使用量を追加
        if (CityManager.Instance != null)
        {
            CityManager.Instance.AddBuilding(this);
        }
    }

    private void OnDestroy()
    {
        // オブジェクトが破壊されたとき、CityManagerに通知して人口と電気使用量を減少
        if (CityManager.Instance != null)
        {
            CityManager.Instance.UpdateCityData(-population, -electricityUsage,-co2,-qol, -pollution);
        }
    }
}
