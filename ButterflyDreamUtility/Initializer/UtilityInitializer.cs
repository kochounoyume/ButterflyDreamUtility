using UnityEngine;

namespace ButterflyDreamUtility.Initializer
{
    using Debug;
    
    /// <summary>
    /// Utilityの初期化処理クラス
    /// </summary>
    internal sealed class UtilityInitializer
    {
        /// <summary>
        /// Utilityの初期化処理
        /// </summary>
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Initialize()
        {
#if !UNITY_EDITOR && !DEVELOPMENT_BUILD
            // エディタ実行時と開発ビルド時以外はログ出力を無効化する
            UnityEngine.Debug.unityLogger.logHandler = new EmptyLogHandler();
#endif
        }
    }
}
