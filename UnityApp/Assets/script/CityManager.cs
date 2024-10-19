
using UnityEngine;
using TMPro;  // TextMeshProを使用するため
using UnityEngine.UI;

public class CityManager : MonoBehaviour
{
    public static CityManager Instance;
    public int Population = 0; // 素の総人口
    public int totalPopulation = 0; // 幸福度含めた全体の総人口

    public int ElectricityUsage = 0; // 素の総電気使用量
    public int totalElectricityUsage = 0; // 全体の総電気使用量

    public int totalco2;//全体の二酸化炭素の排出量

    public int thisqol;//素の幸福度
    public int totalqol = 1;//全体の幸福度
    public int totalyear = 100;//存続年数
    public int totalPollution = 0;

    private float goal;

    public TextMeshProUGUI populationText; // UIで人口を表示するためのTextMeshProテキスト
    public TextMeshProUGUI electricityUsageText; // UIで電気使用量を表示するためのTextMeshProテキスト
    public TextMeshProUGUI co2Text; // UIで二酸化炭素の排出量を表示するためのTextMeshProテキスト
    public TextMeshProUGUI qolText; // UIで幸福度を表示するためのTextMeshProテキスト
    public TextMeshProUGUI yearText; // UIで幸福度を表示するためのTextMeshProテキスト
    public TextMeshProUGUI monthText; // UIで幸福度を表示するためのTextMeshProテキスト
    public YearMeter yearMeter;  // YearMeterの参照を追加←高岡

    public int state = 1;
    private int fazeyear = 0;

    public ObjectPlacer objectplacer;  // Inspectorでアタッチする
    public VideoController videoController;
    public LineRendererProgressor lineRendererProgressor;
    public GameObject resultobject;
    public WaterColorController waterColorController; //川の色のスクリプト

    // 新しいテキスト（Inspectorで設定）
    public TextMeshProUGUI CO22; 
    public TextMeshProUGUI QOL2; 
    public TextMeshProUGUI Electricity2; 
    public TextMeshProUGUI Population2;
    public GameObject Kakushi;
    public GameObject TyukanBackground;
    public TextMeshProUGUI KariText1;
    public TextMeshProUGUI KariText2;
    public GameObject LockImage;

    private const float MaxPollutionLevel = 500.0f;

    private int a = 0;
    private int b = 601;

    private void Awake()
    {
        // シングルトンパターンの実装
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // シーンが変わってもCityManagerを破壊しない
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // 人口と電気使用量を更新するメソッド
    public void UpdateCityData(int populationChange, int electricityChange, int co2, int qol, int pollutionChange)
    {
        Population += populationChange;
        ElectricityUsage += electricityChange;
        totalco2 += co2;
        thisqol += qol;
        if(totalco2<=0){
            totalyear=999;
        }
        else{
        totalyear = (int)(100 * goal / totalco2);
        }
        totalPollution += pollutionChange; //汚染度を加算

        UpdateProgressRing();  // 存続年数に応じてゲージを更新←高岡
    }

    //高岡ここを見て！！
    public void ShowTyukan(string message1, string message2)
    {
        // 画像を表示
        TyukanBackground.gameObject.SetActive(true);

        // テキストの内容を設定して表示
        KariText1.text = message1;
        KariText2.text = message2;
        KariText1.gameObject.SetActive(true);
        KariText2.gameObject.SetActive(true);
    }

    // 全ての要素を非表示にする関数
    public void HideTyukan()
    {
        TyukanBackground.gameObject.SetActive(false);
        KariText1.gameObject.SetActive(false);
        KariText2.gameObject.SetActive(false);
    }

    

    private void UpdateUIText()
    {
        // 総人口を表示するテキストを更新
        if (populationText != null)
        {
            // 0でも表示するように変更
            populationText.text = "人口: " + totalPopulation.ToString() + "人";
        }

        if (state >= 2)
        {
            // 総電気使用量を表示するテキストを更新
            if (electricityUsageText != null)
            {
                // 0でも表示するように変更
                electricityUsageText.text = "電気使用量: " + -1*ElectricityUsage + " kWh";
            }

            if (qolText != null)
            {
                // 0でも表示するように変更
                if (Population == 0)
                {
                    qolText.text = "幸福度: " + (0).ToString() + "%";
                }
                else
                {
                    qolText.text = "幸福度: " + (thisqol * 100 / Population) + "%";
                }
            }
        }

        if (state >= 4)
        {
            if (co2Text != null)
            {
                // 0でも表示するように変更
                co2Text.text = "co2排出量: " + totalco2.ToString() + "kg";
            }

            if(state<8){
            if (yearText != null)
            {
                // 0でも表示するように変更
                yearText.text = "残り" + totalyear.ToString() + "年";
            }
            }
        }
        // ObjectPlacerのオブジェクト数を表示
        if (monthText != null && objectplacer != null)
        {
            monthText.text = 2024 + objectplacer.time + "年" + 10 + "月";
        }

        // リザルトテキストも更新
        UpdateResultTexts();
    }

    // ResultObjectが表示される時のテキストの内容を更新
    private void UpdateResultTexts()
    {
        if (CO22 != null)
            CO22.text = co2Text.text;

        if (QOL2 != null)
            QOL2.text = qolText.text;

        if (Electricity2 != null)
            Electricity2.text = yearText.text;

        if (Population2 != null)
            Population2.text = populationText.text;
    }

    // オブジェクトが生成されたときに人口と電気使用量を増やすメソッド
    public void AddBuilding(BuildingInfo building)
    {
        UpdateCityData(building.population, building.electricityUsage, building.co2, building.qol, building.pollution);
    }
    
    // 存続年数に応じてゲージを更新←高岡
    // YearMeter に年数を渡してゲージを更新
    private void UpdateProgressRing()
    {
        if (yearMeter != null)
        {
            yearMeter.SetYear(totalyear);  // totalyear を YearMeter に渡す
        }
    }

    private void Start()
    {
        HideTyukan();
        if (objectplacer != null)
        {
            objectplacer.CountIncreased += OnCountIncreased;
        }
        // YearMeterを最初に非表示に設定
        if (yearMeter != null)
        {
            yearMeter.gameObject.SetActive(false);
        }
        
        // リザルトオブジェクトと新しいテキストを非表示にする
        if (resultobject != null)
        {
            resultobject.SetActive(false);
            SetResultTextsVisibility(false);
        }

        if (Kakushi != null)
        {
            Kakushi.SetActive(false);
        }

        if (LockImage != null)
        {
            LockImage.SetActive(true);
        }

    }

    // リザルトオブジェクトと4つのテキストの表示/非表示を制御する
    private void SetResultTextsVisibility(bool isVisible)
    {
        if (CO22 != null)
            CO22.gameObject.SetActive(isVisible);

        if (QOL2 != null)
            QOL2.gameObject.SetActive(isVisible);

        if (Electricity2 != null)
            Electricity2.gameObject.SetActive(isVisible);

        if (Population2 != null)
            Population2.gameObject.SetActive(isVisible);
    }

    private void OnCountIncreased(int newCount)
    {
        if (state >= 4)
        {
            Debug.Log("Detected count increase: " + newCount);
            lineRendererProgressor.SetPosition(a, new Vector3(a * 40 + 100,(int)(totalco2*0.1)+10000, 0));
            a += 1;
            // ここに他の処理を追加することができます
        }
    }

    private void OnDestroy()
    {
        if (objectplacer != null)
        {
            objectplacer.CountIncreased -= OnCountIncreased;
        }
    }

    // トータルの汚染度を0〜100に正規化するメソッド←高岡
    private float CalculateTotalPollutionLevel()
    {
        // totalco2 を 0〜100 に正規化
        float normalizedPollution = (totalPollution / MaxPollutionLevel) * 100;
        return Mathf.Clamp(normalizedPollution, 0, 100);  // 0〜100の範囲に制限
    }


    public void Mokuhyo(){
            ShowTyukan("現在の二酸化炭素排出量は"+totalco2+"kg", "10年間で"+goal+"kgまで減らしましょう！");
            b=0;
            if(state>=9){
            if (Kakushi != null)
            {
                Kakushi.SetActive(true);
            }
            }
    }
    void Update()
    {
        //汚染度を渡すこのかたまりを追加←高岡
        if (waterColorController != null)
        {
            // 汚染度を計算して WaterColorController に渡す
            float normalizedPollutionLevel = CalculateTotalPollutionLevel();
            waterColorController.SetWaterPollution(normalizedPollutionLevel);
        }

        if (state == 1 && totalPopulation >= 2500)
        {
            state = 2;
        }
        else if (state == 2)
        {
            if (videoController != null)
            {
                videoController.PlayVideo(0);
                //動画を流す
                state = 3;
            }
        }
        else if (state == 3 && totalPopulation >= 5000)
        {
            if (videoController != null)
            {
                videoController.PlayVideo(1);
                //動画を流す
                state = 4;
            }
        }
        else if (state == 4)
        {
        if (LockImage != null)
        {
            LockImage.SetActive(false);
        }
            objectplacer.time+=50;
            fazeyear = objectplacer.time;
            goal = (totalco2 * 2) / 3;
            totalyear = (int)(100 * goal / totalco2);
            lineRendererProgressor.SetPosition(a, new Vector3(a * 40 + 100, (int)(totalco2*0.1)+10000, 0));
            a += 1;    
            state = 5;
        }
        else if(state==5){
            state=6;
        }
        else if (state == 6 && fazeyear + 10 == objectplacer.time)
        {
            if (videoController != null)
            {
                videoController.PlayVideo(2);
                //動画を流す
                state = 7;
            }
        }
        else if (state == 7)
        {
            goal = totalco2 / 2;
            totalyear = (int)(100 * goal / totalco2);
            state = 8;
        }
        else if (state == 8 && fazeyear + 20 == objectplacer.time)
        {
            if (videoController != null)
            {
                videoController.PlayVideo(3);
                //動画を流す
                state = 9;
            }
        
        }
        else if (state == 9)
        {
            goal = 0;
            totalyear = (int)(100 * goal / totalco2);
            state = 10;
        }
        else if (state == 10 && fazeyear + 30 <= objectplacer.time)
        {
            if (Kakushi != null)
            {
                Kakushi.SetActive(false);
            }
            //リザルト
            state = 11;
        }
        if (state == 11 && resultobject != null)
        {
            goal = 1;
            resultobject.SetActive(true);
            SetResultTextsVisibility(true); 
            UpdateResultTexts();
            if (Input.GetKeyDown(KeyCode.K))
            {
                state = 12;
            }
        }
        else if (state < 11 && resultobject != null)
        {
            resultobject.SetActive(false);
            SetResultTextsVisibility(false);
        }

            if(b<600){
            b+=1;
            }
            else if(b==600){
                HideTyukan();
                b+=1;
            }

        switch (state)
        {
            case 1:
                totalqol = 1;
                totalElectricityUsage = 1;
                break;
            default:
                if (thisqol - Population >= 0)
                {
                    totalqol = 2;
                }
                else
                {
                    totalqol = 1;
                }
                if (ElectricityUsage >= 0)
                {
                    totalElectricityUsage = 2;
                }
                else
                {
                    totalElectricityUsage = 1;
                }
                break;
        }
        totalPopulation = Population * totalqol / totalElectricityUsage;

        // 総人口と電気使用量を表示するUIの更新
        UpdateUIText();
    }
}


/*
using UnityEngine;
using TMPro;  // TextMeshProを使用するため

public class CityManager : MonoBehaviour
{
    public static CityManager Instance;

    private int Population = 0; // 素の総人口
    private int totalPopulation = 0; // 幸福度含めた全体の総人口

    private int ElectricityUsage = 0; // 素の総電気使用量
    private int totalElectricityUsage = 0; // 全体の総電気使用量

    private int totalco2;//全体の二酸化炭素の排出量

    private int thisqol;//素の幸福度
    private int totalqol = 1;//全体の幸福度
    private int totalyear = 100;//存続年数
    private int totalPollution = 0;

    private float goal;

    public TextMeshProUGUI populationText; // UIで人口を表示するためのTextMeshProテキスト
    public TextMeshProUGUI electricityUsageText; // UIで電気使用量を表示するためのTextMeshProテキスト
    public TextMeshProUGUI co2Text; // UIで二酸化炭素の排出量を表示するためのTextMeshProテキスト
    public TextMeshProUGUI qolText; // UIで幸福度を表示するためのTextMeshProテキスト
    public TextMeshProUGUI yearText; // UIで幸福度を表示するためのTextMeshProテキスト
    public TextMeshProUGUI monthText; // UIで幸福度を表示するためのTextMeshProテキスト
    public YearMeter yearMeter;  // YearMeterの参照を追加←高岡

    public int state = 1;
    private int fazeyear = 0;

    public ObjectPlacer objectplacer;  // Inspectorでアタッチする
    public VideoController videoController;
    public LineRendererProgressor lineRendererProgressor;
    public GameObject resultobject;
    public WaterColorController waterColorController; //川の色のスクリプト

    private const float MaxPollutionLevel = 500.0f;

    private int a = 0;

    private void Awake()
    {
        // シングルトンパターンの実装
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // シーンが変わってもCityManagerを破壊しない
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // 人口と電気使用量を更新するメソッド
    public void UpdateCityData(int populationChange, int electricityChange, int co2, int qol, int pollutionChange)
    {
        Population += populationChange;
        ElectricityUsage += electricityChange;
        totalco2 += co2;
        thisqol += qol;
        totalyear = (int)(100 * goal / totalco2);
        totalPollution += pollutionChange; //汚染度を加算

        UpdateProgressRing();  // 存続年数に応じてゲージを更新←高岡
    }

    private void UpdateUIText()
    {
        // 総人口を表示するテキストを更新
        if (populationText != null)
        {
            // 0でも表示するように変更
            populationText.text = "人口: " + totalPopulation.ToString() + "人"+ "/"+state;
        }

        if (state >= 2)
        {
            // 総電気使用量を表示するテキストを更新
            if (electricityUsageText != null)
            {
                // 0でも表示するように変更
                electricityUsageText.text = "電気使用量: " + ElectricityUsage.ToString() + " kWh";
            }

            if (qolText != null)
            {
                // 0でも表示するように変更
                if (Population == 0)
                {
                    qolText.text = "幸福度: " + (0).ToString() + "%";
                }
                else
                {
                    qolText.text = "幸福度: " + (thisqol * 100 / Population).ToString() + "%";
                }
            }
        }

        if (state >= 4)
        {
            if (co2Text != null)
            {
                // 0でも表示するように変更
                co2Text.text = "co2排出量: " + totalco2.ToString() + "kg";
            }

            if (yearText != null)
            {
                // 0でも表示するように変更
                yearText.text = "残り" + totalyear.ToString() + "年";
            }
        }
        // ObjectPlacerのオブジェクト数を表示
        if (monthText != null && objectplacer != null)
        {
            monthText.text = (2024+ (((objectplacer.count - 1) / 12))) +"年"+ (objectplacer.count- (((objectplacer.count - 1) / 12))*12)+ "月";
        }


        Debug.Log("総人口が更新されました: " + Population + ", 総電気使用量: " + totalElectricityUsage + " kWh");
    }

    // オブジェクトが生成されたときに人口と電気使用量を増やすメソッド
    public void AddBuilding(BuildingInfo building)
    {
        UpdateCityData(building.population, building.electricityUsage, building.co2, building.qol, building.pollution);
    }
    
    // 存続年数に応じてゲージを更新←高岡
    // YearMeter に年数を渡してゲージを更新
    private void UpdateProgressRing()
    {
        if (yearMeter != null)
        {
            yearMeter.SetYear(totalyear);  // totalyear を YearMeter に渡す
        }
    }

    private void Start()
    {
        if (objectplacer != null)
        {
            objectplacer.CountIncreased += OnCountIncreased;
        }
        if (resultobject != null)
        {
            resultobject.SetActive(false);
        }
    }

    private void OnCountIncreased(int newCount)
    {
        if (state >= 4)
        {
            Debug.Log("Detected count increase: " + newCount);
            lineRendererProgressor.SetPosition(a, new Vector3(a * 10 + 100, totalco2+10000, 0));
            a += 1;
            // ここに他の処理を追加することができます
        }
    }

    private void OnDestroy()
    {
        if (objectplacer != null)
        {
            objectplacer.CountIncreased -= OnCountIncreased;
        }
    }

    // トータルの汚染度を0〜100に正規化するメソッド←高岡
    private float CalculateTotalPollutionLevel()
    {
        // totalco2 を 0〜100 に正規化
        float normalizedPollution = (totalPollution / MaxPollutionLevel) * 100;
        return Mathf.Clamp(normalizedPollution, 0, 100);  // 0〜100の範囲に制限
    }

    void Update()
    {
        //汚染度を渡すこのかたまりを追加←高岡
        if (waterColorController != null)
        {
            // 汚染度を計算して WaterColorController に渡す
            float normalizedPollutionLevel = CalculateTotalPollutionLevel();
            waterColorController.SetWaterPollution(normalizedPollutionLevel);
        }

        if (state == 1 && totalPopulation > 100)
        {
            state = 2;
        }
        else if (state == 2)
        {
            if (videoController != null)
            {
                videoController.PlayVideo(0);
                //動画を流す
                state = 3;
            }
        }
        else if (state == 3 && totalPopulation > 1000)
        {
            if (videoController != null)
            {
                videoController.PlayVideo(1);
                //動画を流す
                state = 4;
            }
        }
        else if (state == 4)
        {
            fazeyear = (objectplacer.count - 10) / 3;
            goal = (totalco2 * 2) / 3;
            totalyear = (int)(100 * goal / totalco2);
            lineRendererProgressor.SetPosition(a, new Vector3(a * 10 + 100, totalco2+10000, 0));
            a += 1;
            state = 5;
        }
        else if (state == 5 && fazeyear + 10 == (objectplacer.count - 10) / 3)
        {
            if (videoController != null)
            {
                videoController.PlayVideo(2);
                //動画を流す
                state = 6;
            }
        }
        else if (state == 6)
        {
            goal = totalco2 / 2;
            totalyear = (int)(100 * goal / totalco2);
            state = 7;
        }
        else if (state == 7 && fazeyear + 20 == (objectplacer.count - 10) / 3)
        {
            if (videoController != null)
            {
                videoController.PlayVideo(3);
                //動画を流す
                state = 8;
            }
        }
        else if (state == 8)
        {
            goal = 1;
            totalyear = (int)(100 * goal / totalco2);
            state = 9;
        }
        else if (state == 9 && fazeyear + 30 == (objectplacer.count - 10) / 3)
        {
            //リザルト
            state = 10;
        }
        else if (state == 10 && resultobject != null)
        {
            resultobject.SetActive(true);
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            lineRendererProgressor.SetPosition(a, new Vector3(a * 10 + 100, Random.Range(10000, 10200), 0));
            a += 1;
        }


        switch (state)
        {
            case 1:
                totalqol = 1;
                totalElectricityUsage = 1;
                break;
            default:
                if (thisqol - Population >= 0)
                {
                    totalqol = 2;
                }
                else
                {
                    totalqol = 1;
                }
                if (ElectricityUsage >= 0)
                {
                    totalElectricityUsage = 2;
                }
                else
                {
                    totalElectricityUsage = 1;
                }
                break;
        }
        totalPopulation = Population * totalqol / totalElectricityUsage;

        // 総人口と電気使用量を表示するUIの更新
        UpdateUIText();
    }
}*/





/*
using UnityEngine;
using TMPro;  // TextMeshProを使用するため

public class CityManager : MonoBehaviour
{
    public static CityManager Instance;

    private int Population = 0; // 素の総人口
    private int totalPopulation = 0; // 幸福度含めた全体の総人口

    private int ElectricityUsage = 0; // 素の総電気使用量
    private int totalElectricityUsage = 0; // 全体の総電気使用量

    private int totalco2;//全体の二酸化炭素の排出量

    private int thisqol;//素の幸福度
    private int totalqol = 1;//全体の幸福度
    private int totalyear = 100;//存続年数
    public YearMeter yearMeter;  // YearMeterの参照を追加←高岡

    public VideoController videoController;

    private float goal;

    public TextMeshProUGUI populationText; // UIで人口を表示するためのTextMeshProテキスト
    public TextMeshProUGUI electricityUsageText; // UIで電気使用量を表示するためのTextMeshProテキスト
    public TextMeshProUGUI co2Text; // UIで二酸化炭素の排出量を表示するためのTextMeshProテキスト
    public TextMeshProUGUI qolText; // UIで幸福度を表示するためのTextMeshProテキスト
    public TextMeshProUGUI yearText; // UIで幸福度を表示するためのTextMeshProテキスト
    public TextMeshProUGUI monthText; // UIで幸福度を表示するためのTextMeshProテキスト

    public int state = 1;
    private int fazeyear = 0;

    public ObjectPlacer objectplacer;  // Inspectorでアタッチする

    private void Awake()
    {
        // シングルトンパターンの実装
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // シーンが変わってもCityManagerを破壊しない
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // 人口と電気使用量を更新するメソッド
    public void UpdateCityData(int populationChange, int electricityChange, int co2, int qol)
    {
        Population += populationChange;
        ElectricityUsage += electricityChange;
        totalco2 += co2;
        thisqol += qol;
        totalyear = (int)(100 * goal / totalco2);

        UpdateProgressRing();  // 存続年数に応じてゲージを更新←高岡

    }

    private void UpdateUIText()
    {
        // 総人口を表示するテキストを更新
        if (populationText != null)
        {
            // 0でも表示するように変更
            populationText.text = "人口: " + totalPopulation.ToString() + "人"/*+ (objectplacer.count - 10) / 3 + "/"+fazeyear + "/"+state;
        }

        if (state >= 2)
        {
            // 総電気使用量を表示するテキストを更新
            if (electricityUsageText != null)
            {
                // 0でも表示するように変更
                electricityUsageText.text = "電気使用量: " + ElectricityUsage.ToString() + " kWh";
            }

            if (qolText != null)
            {
                // 0でも表示するように変更
                if (Population == 0)
                {
                    qolText.text = "幸福度: " + (0).ToString() + "%";
                }
                else
                {
                    qolText.text = "幸福度: " + (thisqol * 100 / Population).ToString() + "%";
                }
            }
        }

        if (state >= 4)
        {
            if (co2Text != null)
            {
                // 0でも表示するように変更
                co2Text.text = "co2排出量: " + totalco2.ToString() + "kg";
            }

            if (yearText != null)
            {
                // 0でも表示するように変更
                yearText.text = "残り" + totalyear.ToString() + "年";
            }
        }
        // ObjectPlacerのオブジェクト数を表示
        if (monthText != null && objectplacer != null)
        {
            monthText.text = (2024+ (((objectplacer.count - 1) / 12))) +"年"+ (objectplacer.count- (((objectplacer.count - 1) / 12))*12)+ "月";
        }


        Debug.Log("総人口が更新されました: " + Population + ", 総電気使用量: " + totalElectricityUsage + " kWh");
    }

    // オブジェクトが生成されたときに人口と電気使用量を増やすメソッド
    public void AddBuilding(BuildingInfo building)
    {
        UpdateCityData(building.population, building.electricityUsage, building.co2, building.qol);
    }
    private void UpdateProgressRing()
    {
        if (yearMeter != null)
        {
            yearMeter.SetYear(totalyear);  // totalyear を YearMeter に渡す
        }
    }
    void Update()
    {

        if (state == 1 && totalPopulation > 100)
        {
            state = 2;
        }
        else if (state == 2 && totalPopulation > 1000)
        {
            state = 3;
        }
        else if (state == 3)
        {
            fazeyear = (objectplacer.count-10)/3;
            goal = (totalco2 * 2) / 3;
            totalyear = (int)(100 * goal / totalco2);
            state = 4;
        }
        else if (state == 4 && fazeyear + 10 == (objectplacer.count - 10) / 3)
        {
            goal = totalco2 / 2;
            totalyear = (int)(100 * goal / totalco2);
            state = 5;
        }
        else if (state == 5 && fazeyear + 20 == (objectplacer.count - 10) / 3)
        {
            goal = 1;
            totalyear = (int)(100 * goal / totalco2);
            state = 6;
        }
        else if (state == 6 && fazeyear + 30 == (objectplacer.count - 10) / 3)
        {
            state = 7;
        }


        switch (state)
        {
            case 1:
                totalqol = 1;
                totalElectricityUsage = 1;
                break;
            default:
                if (thisqol - Population >= 0)
                {
                    totalqol = 2;
                }
                else
                {
                    totalqol = 1;
                }
                if (ElectricityUsage >= 0)
                {
                    totalElectricityUsage = 2;
                }
                else
                {
                    totalElectricityUsage = 1;
                }
                break;
        }
        totalPopulation = Population * totalqol / totalElectricityUsage;

        // 総人口と電気使用量を表示するUIの更新
        UpdateUIText();
    }
}*/


/*
using UnityEngine;
using TMPro;  // TextMeshProを使用するため

public class CityManager : MonoBehaviour
{
    public static CityManager Instance;

    private int Population = 0; // 素の総人口
    private int totalPopulation = 0; // 幸福度含めた全体の総人口

    private int ElectricityUsage = 0; // 素の総電気使用量
    private int totalElectricityUsage = 0; // 全体の総電気使用量

    private int totalco2;//全体の二酸化炭素の排出量

    private int thisqol;//素の幸福度
    private int totalqol = 1;//全体の幸福度
    private int totalyear = 100;//存続年数

    private float goal;

    //private bool isPaused = false; //動作を停止するためのフラグ←高岡

    public TextMeshProUGUI populationText; // UIで人口を表示するためのTextMeshProテキスト
    public TextMeshProUGUI electricityUsageText; // UIで電気使用量を表示するためのTextMeshProテキスト
    public TextMeshProUGUI co2Text; // UIで二酸化炭素の排出量を表示するためのTextMeshProテキスト
    public TextMeshProUGUI qolText; // UIで幸福度を表示するためのTextMeshProテキスト
    public TextMeshProUGUI yearText; // UIで幸福度を表示するためのTextMeshProテキスト
    public TextMeshProUGUI monthText; // UIで幸福度を表示するためのTextMeshProテキスト

    public int state = 1;

    public ObjectPlacer objectplacer;  // Inspectorでアタッチする

    public YearMeter yearMeter;  // YearMeterの参照を追加←高岡

    private void Awake()
    {
        // シングルトンパターンの実装
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // シーンが変わってもCityManagerを破壊しない
        }
        else
        {
            Destroy(gameObject);
        }
    }


    //動画再生中はこのメソッドを使って　CityManagerを一時停止←高岡
    //public void Pause()
    //{
     //   isPaused = true;
    //}

    // 動画再生が終わったら CityManager を再開←高岡
    //public void Resume()
   // {
   //     isPaused = false;
   // }





    // 人口と電気使用量を更新するメソッド
    public void UpdateCityData(int populationChange, int electricityChange, int co2, int qol)
    {
        Population += populationChange;
        ElectricityUsage += electricityChange;
        totalco2 += co2;
        thisqol += qol;
        totalyear = (int)(100 * goal / totalco2);

         UpdateProgressRing();  // 存続年数に応じてゲージを更新←高岡
    }

    private void UpdateUIText()
    {
        // 総人口を表示するテキストを更新
        if (populationText != null)
        {
            // 0でも表示するように変更
            populationText.text = "総人口: " + totalPopulation.ToString() + "人";
        }

        if (state >= 2)
        {
            // 総電気使用量を表示するテキストを更新
            if (electricityUsageText != null)
            {
                // 0でも表示するように変更
                electricityUsageText.text = "総電気使用量: " + ElectricityUsage.ToString() + " kWh";
            }

            if (qolText != null)
            {
                // 0でも表示するように変更
                qolText.text = "幸福度: " + (thisqol * 100 / Population).ToString() + "%";
            }
        }

        if (state >= 4)
        {
            if (co2Text != null)
            {
                // 0でも表示するように変更
                co2Text.text = "総二酸化炭素排出量: " + totalco2.ToString() + "kg";
            }

            if (yearText != null)
            {
                // 0でも表示するように変更
                yearText.text = "残り" + totalyear.ToString() + "年";
            }
        }
        // ObjectPlacerのオブジェクト数を表示
        if (monthText != null && objectplacer != null)
        {
            monthText.text = "Object数: " + objectplacer.count.ToString() + "個";
        }


        //Debug.Log("総人口が更新されました: " + Population + ", 総電気使用量: " + totalElectricityUsage + " kWh");
    }

    // 存続年数に応じてゲージを更新←高岡
    // YearMeter に年数を渡してゲージを更新
    private void UpdateProgressRing()
    {
        if (yearMeter != null)
        {
            yearMeter.SetYear(totalyear);  // totalyear を YearMeter に渡す
        }
    }


    // オブジェクトが生成されたときに人口と電気使用量を増やすメソッド
    public void AddBuilding(BuildingInfo building)
    {
        UpdateCityData(building.population, building.electricityUsage, building.co2, building.qol);
    }

    void Update()
    {
        //if (isPaused) return;  // ポーズ中は何もしない←高岡

        if (state == 1 && totalPopulation > 100)
        {
            state = 2;
        }
        else if (state == 2 && totalPopulation > 1000)
        {
            state = 3;
        }
        else if (state == 3)
        {
            goal = (totalco2 * 2) / 3;
            totalyear = (int)(100 * goal / totalco2);
            state = 4;
        }
        else if (state == 4&& Input.GetKeyDown(KeyCode.M))
        {
            goal = totalco2 / 2;
            totalyear = (int)(100 * goal / totalco2);
            state = 5;
        }
        else if (state == 5 && Input.GetKeyDown(KeyCode.N))
        {
            goal = 1;
            totalyear = (int)(100 * goal / totalco2);
            state = 6;
        }


        switch (state)
        {
            case 1:
                totalqol = 1;
                totalElectricityUsage = 1;
                break;
            default:
                if (thisqol - Population >= 0)
                {
                    totalqol = 2;
                }
                else
                {
                    totalqol = 1;
                }
                if (ElectricityUsage >= 0)
                {
                    totalElectricityUsage = 2;
                }
                else
                {
                    totalElectricityUsage = 1;
                }
                break;
        }
        totalPopulation = Population * totalqol / totalElectricityUsage;

        // 総人口と電気使用量を表示するUIの更新
        UpdateUIText();

        
    }
}一旦*/

/*10月7日16時高岡くんのを入れる前
using UnityEngine;
using TMPro;  // TextMeshProを使用するため

public class CityManager : MonoBehaviour
{
    public static CityManager Instance;

    private int totalPopulation = 0; // 全体の総人口
    private float totalElectricityUsage = 0f; // 全体の総電気使用量
    private int totalco2;//全体の二酸化炭素の排出量
    private int totalqol;//全体の幸福度
    private int totalyear = 100;//存続年数
    private int A=1;
    private int B=1;

    public TextMeshProUGUI populationText; // UIで人口を表示するためのTextMeshProテキスト
    public TextMeshProUGUI electricityUsageText; // UIで電気使用量を表示するためのTextMeshProテキスト
    public TextMeshProUGUI co2Text; // UIで二酸化炭素の排出量を表示するためのTextMeshProテキスト
    public TextMeshProUGUI qolText; // UIで幸福度を表示するためのTextMeshProテキスト
    public TextMeshProUGUI yearText; // UIで幸福度を表示するためのTextMeshProテキスト

    private void Awake()
    {
        // シングルトンパターンの実装
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // シーンが変わってもCityManagerを破壊しない
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // 人口と電気使用量を更新するメソッド
    public void UpdateCityData(int populationChange, float electricityChange, int co2, int qol)
    {
        totalPopulation += populationChange*totalqol/100;
        totalElectricityUsage += electricityChange;
        totalco2 += co2;
        totalqol += 10*qol;
        totalyear = 100 + B - A * totalco2 / 10;

        // 総人口と電気使用量を表示するUIの更新
        UpdateUIText();
    }

    private void UpdateUIText()
    {
        // 総人口を表示するテキストを更新
        if (populationText != null)
        {
            // 0でも表示するように変更
            populationText.text = "総人口: " + totalPopulation.ToString() + "人";
        }

        // 総電気使用量を表示するテキストを更新
        if (electricityUsageText != null)
        {
            // 0でも表示するように変更
            electricityUsageText.text = "総電気使用量: " + totalElectricityUsage.ToString("F2") + " kWh";
        }

        if (co2Text != null)
        {
            // 0でも表示するように変更
           co2Text.text = "総二酸化炭素排出量: " + totalco2.ToString() + "kg";
        }

        if (qolText != null)
        {
        if (totalPopulation > 0)
        {
            qolText.text = "幸福度: " + (totalqol * 100 / totalPopulation).ToString("F2") + "%";
        }
        else
        {
            qolText.text = "0%";
        }
        }

        if (yearText != null)
        {
            // 0でも表示するように変更
            yearText.text = totalyear.ToString() + "年";
        
        }

        Debug.Log("総人口が更新されました: " + totalPopulation + ", 総電気使用量: " + totalElectricityUsage + " kWh");
    }

    // オブジェクトが生成されたときに人口と電気使用量を増やすメソッド
    public void AddBuilding(BuildingInfo building)
    {
        UpdateCityData(building.population, building.electricityUsage, building.co2, building.qol);
    }
}
*/