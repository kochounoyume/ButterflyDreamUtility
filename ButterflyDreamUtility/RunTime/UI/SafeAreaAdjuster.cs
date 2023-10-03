using UnityEngine;

namespace ButterflyDreamUtility.UI
{
    /// <summary>
    /// セーフエリア調整クラス
    /// <remarks>
    /// DeviceSimulatorにも対応済
    /// </remarks>
    /// </summary>
    [ExecuteAlways, RequireComponent(typeof(RectTransform))]
    internal sealed class SafeAreaAdjuster : MonoBehaviour
    {
        private void Start() => Padding();

        private void Padding()
        {
            RectTransform rectTransform = GetComponent<RectTransform>();
            Rect safeArea = Screen.safeArea;
            Vector2 screenSize = new Vector2(Screen.width, Screen.height);

            rectTransform.anchorMin = safeArea.min / screenSize;
            rectTransform.anchorMax = safeArea.max / screenSize;
        }

#if UNITY_EDITOR
        private bool isAdjusted = false;

        private void Update()
        {
            if(Application.isPlaying && isAdjusted)
            {
                isAdjusted = true;
                return;
            }
            Padding();
        }
#endif
    }
}