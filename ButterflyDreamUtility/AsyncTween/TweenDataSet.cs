namespace ButterflyDreamUtility.AsyncTween
{
    /// <summary>
    /// トゥイーンの情報と対象クラスの情報をセットにした構造体
    /// </summary>
    /// <typeparam name="T">トゥイーン構造体の型</typeparam>
    public struct TweenDataSet<T> where T : struct, ITweenValueBase
    {
        /// <summary>
        /// トゥイーン構造体
        /// </summary>
        public T tweenValue { get; }
        
        /// <summary>
        /// 対象のインスタンスのid
        /// </summary>
        public int instanceId { get; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="tweenValue">トゥイーン構造体</param>
        /// <param name="instanceId">対象のインスタンスのid</param>
        public TweenDataSet(T tweenValue, int instanceId = default)
        {
            this.tweenValue = tweenValue;
            this.instanceId = instanceId;
        }
    }
}