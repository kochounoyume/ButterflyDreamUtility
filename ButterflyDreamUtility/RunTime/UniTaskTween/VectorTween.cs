using System;
using UnityEngine;
using UnityEngine.Events;

namespace ButterflyDreamUtility.UniTaskTween
{
    /// <summary>
    /// Vector3型のトゥイーン種類
    /// </summary>
    internal enum VectorTweenMode
    {
        XYZ,
        XY,
        XZ,
        YZ,
        X,
        Y,
        Z
    }
    
    /// <summary>
    /// Vector3型のトゥイーン構造体
    /// </summary>
    internal struct VectorTween : IEquatable<VectorTween>, ITweenValue<Vector3>
    {
        /// <inheritdoc />
        public event UnityAction<Vector3> onTweenChanged;

        /// <inheritdoc />
        public Vector3 startValue { get; }
        
        /// <inheritdoc />
        public Vector3 targetValue { get; }

        /// <inheritdoc />
        public float duration { get; }
        
        /// <inheritdoc />
        public bool isIgnoreTimeScale { get; }
        
        /// <summary>
        /// トゥイーンのモード
        /// </summary>
        private VectorTweenMode tweenMode { get; }
        
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="tweenMode">トゥイーンのモード</param>
        /// <param name="startValue">開始値</param>
        /// <param name="targetValue">終了値</param>
        /// <param name="duration">トゥイーンの持続時間</param>
        /// <param name="isIgnoreTimeScale">Time.timeScaleを無視するかどうか</param>
        public VectorTween(VectorTweenMode tweenMode, Vector3 startValue, Vector3 targetValue, float duration, bool isIgnoreTimeScale)
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
            
            var newVector = Vector3.Lerp(startValue, targetValue, floatPercentage);

            switch (tweenMode)
            {
                case VectorTweenMode.XYZ:
                    break;
                case VectorTweenMode.XY:
                    newVector.z = startValue.z;
                    break;
                case VectorTweenMode.XZ:
                    newVector.y = startValue.y;
                    break;
                case VectorTweenMode.YZ:
                    newVector.x = startValue.x;
                    break;
                case VectorTweenMode.X:
                    newVector.y = startValue.y;
                    newVector.z = startValue.z;
                    break;
                case VectorTweenMode.Y:
                    newVector.x = startValue.x;
                    newVector.z = startValue.z;
                    break;
                case VectorTweenMode.Z:
                    newVector.x = startValue.x;
                    newVector.y = startValue.y;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            onTweenChanged.Invoke(newVector);
        }

        /// <inheritdoc />
        public bool IsValidTarget() => onTweenChanged != null;

        public bool Equals(VectorTween other) 
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