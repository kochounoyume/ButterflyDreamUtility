using System;

namespace ButterflyDreamUtility.UniTaskTween
{
    /// <summary>
    /// トゥイーンの情報と対象クラスの情報をセットにした構造体
    /// </summary>
    /// <typeparam name="T">トゥイーン構造体の型</typeparam>
    internal readonly struct TweenDataSet<T> : IEquatable<TweenDataSet<T>> where T : struct, IEquatable<T>, ITweenValueBase
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

        public bool Equals(TweenDataSet<T> other) => tweenValue.Equals(other.tweenValue) && instanceId == other.instanceId;

        public override bool Equals(object obj) => obj is TweenDataSet<T> other && Equals(other);

        public override int GetHashCode() => HashCode.Combine(tweenValue, instanceId);
    }
}