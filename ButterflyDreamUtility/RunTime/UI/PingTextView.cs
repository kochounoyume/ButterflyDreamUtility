#if !UNITY_WEBGL
using System;
#if ENABLE_ZSTRING
using Cysharp.Text;
#endif
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace ButterflyDreamUtility.UI
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

        private TextMeshProUGUI textMeshProUGUI = null;

        private const string PING_FORMAT = "{0}ms";

        private readonly TimeSpan pingTimeout = TimeSpan.FromSeconds(0.5);

        private async UniTaskVoid Start()
        {
            textMeshProUGUI = GetComponent<TextMeshProUGUI>();
            while (true)
            {
                NetworkReachability internetReachability = default;
#if UNITY_EDITOR
                internetReachability = UnityEngine.Device.Application.internetReachability;
#else
                internetReachability = Application.internetReachability;
#endif
                if(internetReachability != NetworkReachability.NotReachable)
                {
                    try
                    {
                        Ping ping = new Ping(pingAddress);
                        await UniTask.WaitUntil(() => ping.isDone, cancellationToken: destroyCancellationToken)
                            .TimeoutWithoutException(pingTimeout);
#if ENABLE_ZSTRING
                        textMeshProUGUI.SetTextFormat(PING_FORMAT, ping.time);
#else
                        textMeshProUGUI.SetText(PING_FORMAT, ping.time);
#endif
                        textMeshProUGUI.color = ping.time switch
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
                    catch (TimeoutException)
                    {
                        // タイムアウトしてたら、非常にひどいラグを予想される数値でも表示しておく
#if ENABLE_ZSTRING
                        textMeshProUGUI.SetTextFormat(PING_FORMAT, 500);
#else
                        textMeshProUGUI.SetText(PING_FORMAT, 500);
#endif
                        textMeshProUGUI.color = ConstantColor32.magenta;
                    }
                }
                else
                {
                    textMeshProUGUI.text = "Not Networking";
                    textMeshProUGUI.color = ConstantColor32.red;
                }
                await UniTask.Delay(pingTimeout, cancellationToken: destroyCancellationToken);
            }
        }
    }
}
#endif