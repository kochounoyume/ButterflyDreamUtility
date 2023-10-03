using System;
using UnityEngine;

namespace ButterflyDreamUtility.UniTaskTween
{
    /// <summary>
    /// Color型のトゥイーン種類
    /// </summary>
    internal enum ColorTweenMode
    {
        All,
        RGB,
        Alpha
    }

    /// <summary>
    /// Color型のトゥイーン構造体
    /// </summary>
    internal struct ColorTween : IEquatable<ColorTween>, ITweenValue<Color>
    {
        /// <inheritdoc />
        public event Action<Color> onTweenChanged;

        /// <inheritdoc />
        public Color startValue { get; }

        /// <inheritdoc />
        public Color targetValue { get; }

        /// <inheritdoc />
        public float duration { get; }

        /// <inheritdoc />
        public bool isIgnoreTimeScale { get; }

        /// <summary>
        /// トゥイーンのモード
        /// </summary>
        private ColorTweenMode tweenMode { get; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="tweenMode">トゥイーンのモード</param>
        /// <param name="startValue">開始値</param>
        /// <param name="targetValue">終了値</param>
        /// <param name="duration">トゥイーンの持続時間</param>
        /// <param name="isIgnoreTimeScale">Time.timeScaleを無視するかどうか</param>
        public ColorTween(ColorTweenMode tweenMode, Color startValue, Color targetValue, float duration, bool isIgnoreTimeScale)
        {
            this.tweenMode = tweenMode;
            this.startValue = startValue;
            this.targetValue = targetValue;
            this.duration = duration;
            this.isIgnoreTimeScale = isIgnoreTimeScale;
            onTweenChanged = null;
        }

        /// <inheritdoc />
        public void TweenValue(float floatPercentage)
        {
            if (onTweenChanged == null) return;

            var newColor = Color.Lerp(startValue, targetValue, floatPercentage);

            switch (tweenMode)
            {
                case ColorTweenMode.Alpha:
                    newColor.r = startValue.r;
                    newColor.g = startValue.g;
                    newColor.b = startValue.b;
                    break;
                case ColorTweenMode.RGB:
                    newColor.a = startValue.a;
                    break;
                case ColorTweenMode.All:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            onTweenChanged.Invoke(newColor);
        }

        /// <inheritdoc />
        public bool IsValidTarget() => onTweenChanged != null;

        public bool Equals(ColorTween other)
            => startValue == other.startValue
               && targetValue == other.targetValue
               && Mathf.Approximately(duration, other.duration)
               && isIgnoreTimeScale == other.isIgnoreTimeScale
               && tweenMode == other.tweenMode
               && onTweenChanged == other.onTweenChanged;

        public override bool Equals(object obj) => obj is ColorTween other && Equals(other);

        public override int GetHashCode() => HashCode.Combine(startValue, targetValue, duration, isIgnoreTimeScale, (int) tweenMode, onTweenChanged);
    }
}