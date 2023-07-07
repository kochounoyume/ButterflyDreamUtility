using System.Diagnostics;
using UnityEngine;

namespace ButterflyDreamUtility.Debug
{
    /// <summary>
    /// Unityエディタ実行時のみ有効なログ出力の拡張ラッパークラス
    /// <remarks>
    /// UNITY_EDITOR 及び DEVELOPMENT_BUILD が無効な場合、ログ出力の呼び出し自体が削除される.
    /// </remarks>
    /// </summary>
    public static class DebugExtension
    {
        /// <summary>
        /// Unityエディタ実行時のみ有効なログ出力関数
        /// </summary>
        /// <param name="message">ログとして出力するオブジェクト</param>
        /// <param name="color">ログとして出力した際のメッセージの色</param>
        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        public static void ColorLog(object message, Color color)
        {
            UnityEngine.Debug.Log(string.Format("<color=#{0}>{1}</color>", ColorUtility.ToHtmlStringRGB(color), message));
        }
		
        /// <summary>
        /// Unityエディタ実行時のみ有効なログ出力関数
        /// </summary>
        /// <param name='message'>ログとして出力するオブジェクト</param>
        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        public static void RedLog(object message)
        {
            UnityEngine.Debug.Log(string.Format("<color=red>{0}</color>", message));
        }
		
        /// <summary>
        /// Unityエディタ実行時のみ有効なログ出力関数
        /// </summary>
        /// <param name='message'>ログとして出力するオブジェクト</param>
        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        public static void BlueLog(object message)
        {
            UnityEngine.Debug.Log(string.Format("<color=blue>{0}</color>", message));
        }
        
        /// <summary>
        /// Unityエディタ実行時のみ有効なログ出力関数
        /// </summary>
        /// <param name='message'>ログとして出力するオブジェクト</param>
        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        public static void GreenLog(object message)
        {
            UnityEngine.Debug.Log(string.Format("<color=green>{0}</color>", message));
        }
    }
}