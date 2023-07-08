﻿using System;
using UnityEngine;
using UnityEngine.Events;

namespace ButterflyDream.Utility.AsyncTween
{
    /// <summary>
    /// Float型のトゥイーン構造体
    /// </summary>
    internal struct FloatTween : ITweenValue<float>
    {
        /// <summary>
        /// トゥイーンする処理
        /// </summary>
        private event UnityAction<float> target;

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
            target = null;
        }

        /// <inheritdoc />
        public void TweenValue(float floatPercentage)
        {
            target?.Invoke(Mathf.Lerp(startValue, targetValue, floatPercentage));
        }

        /// <summary>
        /// トゥイーンしたい処理を登録する
        /// </summary>
        /// <param name="callback">トゥイーンしたい処理</param>
        public void AddOnChangedCallback(UnityAction<float> callback)
        {
            target += callback;
        }

        /// <inheritdoc />
        public bool IsValidTarget() => target != null;
    }
}