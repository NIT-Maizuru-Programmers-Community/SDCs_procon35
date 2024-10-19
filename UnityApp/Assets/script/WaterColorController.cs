using UnityEngine;

public class WaterColorController : MonoBehaviour
{
    public Material waterMaterial;

    // 汚染前と汚染後のカラーコードを設定
    private Color cleanDeepWaterColor = new Color32(0x0E, 0x32, 0x89, 0xFF);      // 汚染前の深い色 (Hex: #0E3289)
    private Color pollutedDeepWaterColor = new Color32(0xEE, 0x9E, 0x35, 0xFF);   // 汚染後の深い色 (Hex: #EE9E35)
    private Color cleanShallowWaterColor = new Color32(0x53, 0xAF, 0xCA, 0xFF);   // 汚染前の浅い色 (Hex: #53AFCA)
    private Color pollutedShallowWaterColor = new Color32(0x24, 0x6F, 0x28, 0xFF); // 汚染後の浅い色 (Hex: #246F28)

    public void SetWaterPollution(float pollutionLevel)
    {
        if (waterMaterial != null)
        {
            // 汚染レベル (0〜100) に応じて DeepWaterColor と ShallowWaterColor を補間
            float normalizedPollution = pollutionLevel / 100.0f;  // 0〜1に変換
            Color deepWaterColor = Color.Lerp(cleanDeepWaterColor, pollutedDeepWaterColor, normalizedPollution);
            Color shallowWaterColor = Color.Lerp(cleanShallowWaterColor, pollutedShallowWaterColor, normalizedPollution);

            // マテリアルに色を設定
            waterMaterial.SetColor("_DeepWaterColor", deepWaterColor);
            waterMaterial.SetColor("_ShallowWaterColor", shallowWaterColor);

            Debug.Log($"DeepWaterColor: {deepWaterColor}, ShallowWaterColor: {shallowWaterColor}, Pollution Level: {pollutionLevel}");
        }
        else
        {
            Debug.LogError("Water material が設定されていません。");
        }
    }
}

