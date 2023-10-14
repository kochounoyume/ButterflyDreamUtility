using UnityEngine;
#if UNITY_EDITOR
using Screen = UnityEngine.Device.Screen;
#else
using Screen = UnityEngine.Screen;
#endif

namespace ButterflyDreamUtility.UI
{
    using Attributes;

    /// <summary>
    /// セーフエリア調整クラス
    /// <remarks>
    /// DeviceSimulatorにも対応済
    /// </remarks>
    /// </summary>
    [RequireComponent(typeof(RectTransform))]
    internal sealed class SafeAreaAdjuster : MonoBehaviour
    {
        private void Start() => Padding();

        [Button("セーフエリア調整")]
        private void Padding()
        {
            RectTransform rectTransform = GetComponent<RectTransform>();
            Rect safeArea = Screen.safeArea;
            Vector2 screenSize = new Vector2(Screen.width, Screen.height);

            rectTransform.anchorMin = safeArea.min / screenSize;
            rectTransform.anchorMax = safeArea.max / screenSize;
        }
    }
}