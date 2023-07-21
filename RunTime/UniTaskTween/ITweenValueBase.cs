namespace ButterflyDreamUtility.UniTaskTween
{
    /// <summary>
    /// トゥイーターの基底インターフェイス
    /// <remarks>
    /// トゥイーンを構造体にするため、インターフェイスを使用
    /// </remarks>
    /// </summary>
    public interface ITweenValueBase
    {
        /// <summary>
        /// 秒単位でトゥイーンの持続時間
        /// </summary>
        float duration { get; }
        
        /// <summary>
        /// Time.timeScaleを無視するかどうか
        /// </summary>
        bool isIgnoreTimeScale { get; }
        
        /// <summary>
        /// コールバックを呼び出す処理
        /// </summary>
        /// <param name="floatPercentage"></param>
        void TweenValue(float floatPercentage);

        /// <summary>
        /// コールバックが登録されているかどうか
        /// </summary>
        /// <returns></returns>
        bool IsValidTarget();
    }
}
