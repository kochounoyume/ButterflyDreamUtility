using UnityEngine;

namespace ButterflyDreamUtility.uGUI
{
    /// <summary>
    /// セーフエリア調整クラス
    /// <remarks>
    /// DeviceSimulatorにも対応済
    /// </remarks>
    /// </summary>
    [RequireComponent(typeof(RectTransform))]
    [ExecuteAlways]
    public sealed class SafeAreaAdjuster : MonoBehaviour
    {
#if UNITY_EDITOR
        private bool isAdjusted = false;
#endif
        private void Start() => Padding();

        private void Padding()
        {
#if UNITY_EDITOR
            if(Application.isPlaying && isAdjusted) return;
            isAdjusted = true;
#endif
            RectTransform rectTransform = GetComponent<RectTransform>();
            Rect safeArea = Screen.safeArea;
            Vector2 screenSize = new Vector2(Screen.width, Screen.height);
        
            rectTransform.anchorMin = safeArea.min / screenSize;
            rectTransform.anchorMax = safeArea.max / screenSize;
        }
    
#if UNITY_EDITOR
        private void Update() => Padding();
#endif
    }
}