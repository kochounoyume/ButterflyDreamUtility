#if ENABLE_ZSTRING
using Cysharp.Text;
#endif
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace ButterflyDreamUtility.Network.Ping
{
    using Constants;

    /// <summary>
    /// Ping値を表示するビュークラス
    /// </summary>
    [RequireComponent(typeof(TextMeshProUGUI))]
    internal sealed class PingTextView : MonoBehaviour
    {
        [SerializeField, Tooltip("Pingを取得するアドレス")]
        private string pingAddress = "8.8.8.8";

        private async UniTaskVoid Start()
        {
            TextMeshProUGUI textMeshProUGUI = GetComponent<TextMeshProUGUI>();
            IPingChecker pingChecker = new PingChecker(pingAddress);

            await foreach (int result in pingChecker.RepeatPingAsyncEnumerable().WithCancellation(destroyCancellationToken))
            {
                if (result == IPingChecker.ERROR_TINE)
                {
                    textMeshProUGUI.text = "Not Networking";
                    textMeshProUGUI.color = ConstantColor32.red;
                    continue;
                }
#if ENABLE_ZSTRING
                textMeshProUGUI.SetTextFormat(IPingChecker.PING_FORMAT, result);
#else
                textMeshProUGUI.SetText(pingChecker.pingFormat, ping.time);
#endif
                textMeshProUGUI.color = result switch
                {
                    // 0ms ~ 30msは黄緑で"非常に快適"
                    <= 30 => ConstantColor32.lightGreen,
                    // 31ms ~ 40msは水色で"快適"
                    <= 40 => ConstantColor32.cyan,
                    // 41ms ~ 60msは青で"普通"
                    <= 60 => ConstantColor32.blue,
                    // 61ms ~ 100msは黄で"やや遅い"
                    <= 100 => ConstantColor32.yellow,
                    // 101ms ~ 200msはオレンジで"遅い"
                    <= 200 => ConstantColor32.orange,
                    // 201ms ~ は紫で"非常に遅い"
                    _ => ConstantColor32.magenta
                };
            }
        }
    }
}