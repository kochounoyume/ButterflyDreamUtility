namespace ButterflyDreamUtility.AsyncTween
{
    /// <summary>
    /// トゥイーンの情報と実行クラスをセットにした構造体
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TTween"></typeparam>
    public struct TweenDataSet<T> where T : ITweenValueBase
    {
        /// <summary>
        /// トゥイーン構造体
        /// </summary>
        public T tweenValue { get; }
        
        /// <summary>
        /// トゥイーンランナー
        /// </summary>
        public TweenRunner<T> tweenRunner { get; }
        
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="tweenValue">トゥイーン構造体</param>
        /// <param name="tweenRunner">トゥイーンランナー</param>
        public TweenDataSet(T tweenValue, TweenRunner<T> tweenRunner)
        {
            this.tweenValue = tweenValue;
            this.tweenRunner = tweenRunner;
        }
    }
}