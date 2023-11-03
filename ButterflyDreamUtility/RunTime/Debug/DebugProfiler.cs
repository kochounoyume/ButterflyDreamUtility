#if ENABLE_ZSTRING
using Cysharp.Text;
#else
using System.Text;
#endif
using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Profiling;

namespace ButterflyDreamUtility.Debug
{
    /// <summary>
    /// FPSなどのプロファイル情報を画面に表示するためのクラス
    /// <remarks>
    /// <see cref="Time"/> の Time.realtimeSinceStartup の公式リファレンスサンプルコードを参照
    /// </remarks>
    /// </summary>
    internal sealed class DebugProfiler : MonoBehaviour
    {
        [SerializeField, Tooltip("表示する総メモリ使用量の単位")]
        private MemoryUnit memoryUnit = MemoryUnit.GB;

        /// <summary>
        /// プロファイル情報を表示する際の表示領域(セーフエリアを考慮)
        /// </summary>
        private Rect debugField = default;

        /// <summary>
        /// 次に表示するプロファイル情報テキスト
        /// </summary>
        private string nextDebugText = string.Empty;

        /// <summary>
        /// プロファイル情報を更新する間隔
        /// </summary>
        private const float UPDATE_INTERVAL = 0.5f;

        /// <summary>
        /// プロファイル情報を表示する際の文字の大きさ
        /// </summary>
        private const int FONT_SIZE = 30;

#if !ENABLE_ZSTRING
        /// <summary>
        /// 使いまわせるStringBuilder
        /// </summary>
        readonly StringBuilder sb = new StringBuilder();
#endif
        /// <summary>
        /// 総メモリ使用量表示の単位指定列挙体
        /// </summary>
        private enum MemoryUnit
        {
            B = 0,
            KB = 1,
            MB = 2,
            GB = 3
        }

        private async UniTaskVoid Start()
        {
            // セーフエリアを考慮した表示領域を取得
            Vector2 safeAreaStartPos = new Vector2(Screen.width, Screen.height) - Screen.safeArea.size - Screen.safeArea.position;
            debugField = new Rect(safeAreaStartPos, new Vector2(500, 80));

            double lastInterval = Time.realtimeSinceStartup;
            int frames = 0;

            while (!destroyCancellationToken.IsCancellationRequested)
            {
                frames++;
                float timeNow = Time.realtimeSinceStartup;
                if (timeNow > lastInterval + UPDATE_INTERVAL)
                {
                    float fps = (float)(frames / (timeNow - lastInterval));
                    frames = 0;
                    lastInterval = timeNow;
                    // 確保している総メモリ
                    float totalMemory = Profiler.GetTotalReservedMemoryLong() / Mathf.Pow(1024f, (int) memoryUnit);

#if ENABLE_ZSTRING
                    using (var sb = ZString.CreateStringBuilder(true))
                    {
#else
                    sb.Clear();
#endif
                        sb.Append("CPU: ");
                        sb.Append(fps.ToString("F0"));
                        sb.Append("fps (");
                        sb.Append(timeNow.ToString("F1"));
                        sb.AppendLine("ms)");
                        sb.Append("Memory: ");
                        sb.Append(totalMemory.ToString("F"));
                        sb.Append(memoryUnit switch
                        {
                            MemoryUnit.B => "B",
                            MemoryUnit.KB => "KB",
                            MemoryUnit.MB => "MB",
                            MemoryUnit.GB => "GB",
                            _ => "GB" // ここには来ない
                        });
                        nextDebugText = sb.ToString();
#if ENABLE_ZSTRING
                    }
#endif
                }

                await UniTask.Yield(PlayerLoopTiming.Update, destroyCancellationToken);
            }
        }

        private void OnGUI()
        {
            GUIStyle styleBox = new GUIStyle(GUI.skin.box)
            {
                fontSize = FONT_SIZE,
                normal =
                {
                    textColor = Color.white
                },
                alignment = TextAnchor.UpperLeft
            };
            GUI.Box(debugField, nextDebugText, styleBox);
        }

#if !ENABLE_ZSTRING
        private void OnDestroy()
        {
            sb.Clear();
        }
#endif
    }
}
