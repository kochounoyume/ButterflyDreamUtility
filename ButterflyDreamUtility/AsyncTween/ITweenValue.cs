using UnityEngine.Events;

namespace ButterflyDreamUtility.AsyncTween
{
    /// <summary>
    /// トゥイーターの基底インターフェイス
    /// <remarks>
    /// トゥイーンを構造体にするため、インターフェイスを使用
    /// </remarks>
    /// </summary>
    /// <typeparam name="T">トゥイーンする値の型</typeparam>
    public interface ITweenValue<T> : ITweenValueBase where T : struct
    {
        /// <summary>
        /// トゥイーンする処理
        /// </summary>
        event UnityAction<T> onTweenChanged;
        
        /// <summary>
        /// トゥイーンの開始値
        /// </summary>
        T startValue { get; }
        
        /// <summary>
        /// トゥイーンの終了値
        /// </summary>
        T targetValue { get; }
    }
}