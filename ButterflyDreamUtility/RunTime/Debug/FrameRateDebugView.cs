using System;
using System.Text;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace ButterflyDreamUtility.Debug
{
    /// <summary>
    /// ゲーム画面左上にフレームレート等を表示するビュークラス
    /// </summary>
    public sealed class FrameRateDebugView : MonoBehaviour
    {
#if UNITY_EDITOR
        private void Reset()
        {
            UnityEditor.PlayerSettings.enableFrameTimingStats = true;
        }
#endif

#if UNITY_EDITOR || DEVELOPMENT_BUILD

        private StringBuilder sb = new StringBuilder();

        private async UniTask Start()
        {
            FrameTiming[] frameTimings = new FrameTiming[1];

            while (true)
            {
                var getTimingCount = FrameTimingManager.GetLatestTimings(1, frameTimings);
                if (getTimingCount != 0)
                {
                    var cpuDeltaTime = frameTimings[0].cpuFrameTime;
                    var gpuDeltaTime = frameTimings[0].gpuFrameTime;
                    sb.Clear();
                    sb
                        .Append("CPU: ")
                        .Append(cpuDeltaTime.ToString("F1"))
                        .Append("ms (")
                        .Append((1000 / cpuDeltaTime).ToString("F0"))
                        .Append("fps )")
                        .Append(Environment.NewLine)
                        .Append("GPU: ")
                        .Append(gpuDeltaTime.ToString("F1"))
                        .Append("ms (")
                        .Append((1000 / gpuDeltaTime).ToString("F0"))
                        .Append("fps )");
                }

                await UniTask.DelayFrame(50, cancellationToken: this.GetCancellationTokenOnDestroy());
            }
        }

        private void OnGUI()
        {
            GUIStyle styleBox = GUI.skin.GetStyle("box");
            styleBox.fontSize = 40;
            styleBox.alignment = TextAnchor.UpperLeft;
            GUI.Box(new Rect(30, 30, 450, 100), sb.ToString(), styleBox);
        }
#endif
    }
}
