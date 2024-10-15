#include <Adafruit_NeoPixel.h>

// LEDストリップの設定
#define PIN 9
#define NUM_LEDS 260
#define NUM_ANIMATIONS 10
Adafruit_NeoPixel strip = Adafruit_NeoPixel(NUM_LEDS, PIN, NEO_GRB + NEO_KHZ800);

int brightness[NUM_LEDS];       // 各LEDの輝度
int fadeAmount[NUM_LEDS];       // フェードアウト速度
int currentLED[NUM_ANIMATIONS]; // 各アニメーションLEDの位置

bool isRed = false; // 起動時は青色から開始

unsigned long previousMillisAnimation = 0;
const long animationInterval = 100; // アニメーション間隔

void setup()
{
    strip.begin();
    strip.show(); // 初期化：すべて消灯

    // 各LEDの初期状態を設定
    for (int i = 0; i < NUM_LEDS; i++)
    {
        brightness[i] = 0;
        fadeAmount[i] = 0;
    }

    // アニメーション用LEDの位置を均等に分散
    for (int i = 0; i < NUM_ANIMATIONS; i++)
    {
        currentLED[i] = i * (NUM_LEDS / NUM_ANIMATIONS);
    }

    Serial.begin(9600); // Jetsonとの通信を開始

    // **起動時は必ず青色で初期化**
    isRed = false;
    applyColorToAllLEDs(); // 初期設定の色（青）を反映
}

void loop()
{
    // Jetsonから色変更のコマンドを受け取る
    if (Serial.available() > 0)
    {
        char command = Serial.read(); // コマンドを1バイト取得
        if (command == '1')
        {
            isRed = true; // 赤色に変更
        }
        else if (command == '0')
        {
            isRed = false; // 青色に変更
        }
        applyColorToAllLEDs(); // 新しい色を即座に反映
    }

    // アニメーションを一定間隔で更新
    unsigned long currentMillis = millis();
    if (currentMillis - previousMillisAnimation >= animationInterval)
    {
        previousMillisAnimation = currentMillis;
        animateMultipleLEDs(); // アニメーションの実行
    }
}

// LED全体に現在の色を反映する関数
void applyColorToAllLEDs()
{
    for (int i = 0; i < NUM_LEDS; i++)
    {
        if (isRed)
        {
            // 赤色 (#964033 → RGB(150, 64, 51))
            strip.setPixelColor(i, strip.Color(brightness[i] * 100 / 255,
                                               brightness[i] * 30 / 255,
                                               brightness[i] * 5 / 255));
        }
        else
        {
            // 青色 (#B2DFFF → RGB(178, 223, 255))
            strip.setPixelColor(i, strip.Color(brightness[i] * 8 / 255,
                                               brightness[i] * 15 / 255,
                                               brightness[i] * 120 / 255));
        }
    }
    strip.show(); // 更新を反映
}

// 複数のLEDアニメーションを実行する関数
void animateMultipleLEDs()
{
    int colorShade;

    // 各アニメーションLEDを更新
    for (int i = 0; i < NUM_ANIMATIONS; i++)
    {
        colorShade = random(150, 255); // ランダムな明るさ
        setLEDBrightness(currentLED[i], colorShade);

        // 次のLED位置に移動（ループする）
        currentLED[i] = (currentLED[i] + 1) % NUM_LEDS;
    }

    // 各LEDの明るさと色を更新
    applyColorToAllLEDs(); // 現在の色に応じてLEDを更新
}

// 指定したLEDとその周囲の明るさを設定する関数
void setLEDBrightness(int currentLED, int colorShade)
{
    for (int offset = -2; offset <= 2; offset++)
    {
        int ledIndex = (currentLED + offset + NUM_LEDS) % NUM_LEDS; // 配列の端をループさせる
        int distance = abs(offset);                                 // 中心からの距離
        int brightnessLevel = colorShade - (distance * 50);         // 距離に応じた減衰
        brightness[ledIndex] = max(0, brightnessLevel);             // 0未満にならないように
        fadeAmount[ledIndex] = random(1, 5);                        // ランダムなフェード速度
    }
}