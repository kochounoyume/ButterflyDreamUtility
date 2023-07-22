using System;
using UnityEngine;
using UnityEngine.Events;

namespace ButterflyDreamUtility.UniTaskTween
{
    /// <summary>
    /// Float型のトゥイーン構造体
    /// </summary>
    internal struct FloatTween : IEquatable<FloatTween>, ITweenValue<float>
    {
        /// <inheritdoc />
        public event UnityAction<float> onTweenChanged;

        /// <inheritdoc />
        public float startValue { get; }
        
        /// <inheritdoc />
        public float targetValue { get; }
        
        /// <inheritdoc />
        public float duration { get; }
        
        /// <inheritdoc />
        public bool isIgnoreTimeScale { get; }
        
        /// <summary>
        /// Float型のトゥイーン構造体のコンストラクタ
        /// </summary>
        /// <param name="startValue">開始値</param>
        /// <param name="targetValue">終了値</param>
        /// <param name="duration">トゥイーンの持続時間</param>
        /// <param name="isIgnoreTimeScale">Time.timeScaleを無視するかどうか</param>
        public FloatTween(float startValue, float targetValue, float duration, bool isIgnoreTimeScale)
        {
            this.startValue = startValue;
            this.targetValue = targetValue;
            this.duration = duration;
            this.isIgnoreTimeScale = isIgnoreTimeScale;
            onTweenChanged = null;
        }

        /// <inheritdoc />
        public void TweenValue(float floatPercentage)
        {
            onTweenChanged?.Invoke(Mathf.Lerp(startValue, targetValue, floatPercentage));
        }

        /// <inheritdoc />
        public bool IsValidTarget() => onTweenChanged != null;

        public bool Equals(FloatTween other) 
            => Mathf.Approximately(startValue, other.startValue)
               && Mathf.Approximately(targetValue, other.targetValue)
               && Mathf.Approximately(duration, other.duration)
               && isIgnoreTimeScale == other.isIgnoreTimeScale 
               && onTweenChanged == other.onTweenChanged;

        public override bool Equals(object obj) => obj is FloatTween other && Equals(other);

        public override int GetHashCode() => HashCode.Combine(startValue, targetValue, duration, isIgnoreTimeScale, onTweenChanged);
    }
}