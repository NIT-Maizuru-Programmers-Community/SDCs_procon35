using UnityEngine;
using UnityEngine.UI;

public class YearMeter : MonoBehaviour
{
    public Image progressRing;  // 円形ゲージの Image コンポーネント
    private int maxYear = 100;  // 最大の存続年数
    private int currentYear;    // 現在の年数

    private void Start()
    {
         SetYear(maxYear);  // 初期状態で最大の年数に設定
    }

    // 年数に応じてゲージを更新するメソッド
    public void SetYear(int year)
    {
        currentYear = Mathf.Clamp(year, 0, maxYear);  // 年数を0～maxYearの範囲にクランプ
        UpdateProgressRing();  // ゲージの更新
    }

    private void UpdateProgressRing()
    {
        if (progressRing != null)
        {
            float progressPercent = (float)currentYear / maxYear;  // 0～1の範囲に変換
            progressRing.fillAmount = Mathf.Clamp01(progressPercent);
        }
    }
}
