using System.Text;
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
        private MemoryUnit memoryUnit = MemoryUnit.GigaByte;
        
        /// <summary>
        /// プロファイル情報を表示する際の表示領域(セーフエリアを考慮)
        /// </summary>
        private Rect debugField = default;
        
        /// <summary>
        /// 表示するプロファイル情報を連結するためのStringBuilder
        /// </summary>
        private readonly StringBuilder sb = new StringBuilder();

        /// <summary>
        /// プロファイル情報を更新する間隔
        /// </summary>
        private const float updateInterval = 0.5f;

        /// <summary>
        /// プロファイル情報を表示する際の文字の大きさ
        /// </summary>
        private const int fontSize = 30;
        
        /// <summary>
        /// 総メモリ使用量表示の単位指定列挙体
        /// </summary>
        private enum MemoryUnit
        {
            Byte = 0,
            KiloByte = 1,
            MegaByte = 2,
            GigaByte = 3
        }

        private async UniTaskVoid Start()
        {
            // セーフエリアを考慮した表示領域を取得
            Vector2 safeAreaStartPos = new Vector2(Screen.width, Screen.height) - Screen.safeArea.size - Screen.safeArea.position;
            debugField = new Rect(safeAreaStartPos, new Vector2(500, 80));
            
            double lastInterval = Time.realtimeSinceStartup;
            int frames = 0;
            var ctSToken = this.GetCancellationTokenOnDestroy();

            while (true)
            {
                frames++;
                float timeNow = Time.realtimeSinceStartup;
                if (timeNow > lastInterval + updateInterval)
                {
                    float fps = (float)(frames / (timeNow - lastInterval));
                    frames = 0;
                    lastInterval = timeNow;
                    // 確保している総メモリ
                    float totalMemory = Profiler.GetTotalReservedMemoryLong() / Mathf.Pow(1024f, (int) memoryUnit);

                    sb.Clear();
                    sb
                        .Append("CPU: ")
                        .Append(fps.ToString("F0"))
                        .Append("fps (")
                        .Append(timeNow.ToString("F1"))
                        .AppendLine("ms)")
                        .Append("Memory: ")
                        .Append(totalMemory.ToString("F"))
                        .Append(memoryUnit switch
                        {
                            MemoryUnit.Byte => "B",
                            MemoryUnit.KiloByte => "KB",
                            MemoryUnit.MegaByte => "MB",
                            MemoryUnit.GigaByte => "GB",
                            _ => "GB" // ここには来ない
                        });
                }

                await UniTask.Yield(PlayerLoopTiming.Update, ctSToken);
            }
        }

        private void OnGUI()
        {
            GUIStyle styleBox = new GUIStyle(GUI.skin.box)
            {
                fontSize = fontSize,
                normal =
                {
                    textColor = Color.white
                },
                alignment = TextAnchor.UpperLeft
            };
            GUI.Box(debugField, sb.ToString(), styleBox);
        }
    }
}
