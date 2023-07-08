using System;
using UnityEngine;
using UnityEngine.Events;

namespace ButterflyDreamUtility.AsyncTween
{
    /// <summary>
    /// Color型のトゥイーン構造体
    /// </summary>
    public struct ColorTween : ITweenValue<Color>
    {
        public enum ColorTweenMode
        {
            All,
            RGB,
            Alpha
        }
        
        /// <inheritdoc />
        public event UnityAction<Color> onTweenChanged;

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
                    newColor.r = startValue.r;
                    newColor.g = startValue.g;
                    newColor.b = startValue.b;
                    newColor.a = startValue.a;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            onTweenChanged.Invoke(newColor);
        }

        /// <inheritdoc />
        public bool IsValidTarget() => onTweenChanged != null;
    }
}