using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace ButterflyDreamUtility.Extensions
{
    using UniTaskTween;

    public static class AudioExtension
    {
        private static Dictionary<int, TweenRunner<FloatTween>> volumeTweenRunnerTable = null;

        /// <summary>
        /// AudioSourceの音量をフェードする
        /// <example>
        /// <code>
        /// <![CDATA[
        /// using System.Collections;
        /// using UnityEngine;
        /// using ButterflyDreamUtility.Extension;
        ///
        /// [RequireComponent(typeof(AudioSource))]
        /// public class Test : MonoBehaviour
        /// {
        ///     private IEnumerator Start()
        ///     {
        ///         AudioSource audioSource = GetComponent<AudioSource>();
        ///         audioSource.volume = 0;
        ///         audioSource.Play();
        ///
        ///         // 音量をフェードインする
        ///         audioSource.FadeTween(1, 1.0f);
        ///
        ///         yield return new WaitForSeconds(0.5f);
        ///
        ///         // 音量をフェードアウトする（前のTweenは自動的に停止）
        ///         audioSource.FadeTween(0, 1.0f);
        ///
        ///         yield return new WaitForSeconds(0.5f);
        ///
        ///         // フェードアウトをキャンセルする
        ///         audioSource.FadeStopTween();
        ///     }
        /// }
        /// ]]>
        /// </code>
        /// </example>
        /// </summary>
        /// <param name="target">AudioSource</param>
        /// <param name="endValue">フェード後の音量</param>
        /// <param name="duration">フェード時間</param>
        /// <param name="isIgnoreTimeScale">Time.timeScaleを無視するかどうか</param>
        /// <param name="callback">フェード完了後のコールバック</param>
        public static void FadeTween(this AudioSource target, float endValue, float duration, bool isIgnoreTimeScale = false, Action<AudioSource> callback = null)
        {
            TweenDataSet<FloatTween> dataSet = SetFadeTween(target, endValue, duration, isIgnoreTimeScale, callback);
            if(dataSet.Equals(default)) return;
            volumeTweenRunnerTable[dataSet.instanceId].StartTween(dataSet.tweenValue);
        }

        /// <summary>
        /// AudioSourceの音量を非同期でフェードするUniTaskを返す
        /// </summary>
        /// <param name="target">AudioSource</param>
        /// <param name="endValue">フェード後の音量</param>
        /// <param name="duration">フェード時間</param>
        /// <param name="isIgnoreTimeScale">Time.timeScaleを無視するかどうか</param>
        /// <param name="token">キャンセルトークン（敢えて指定しなくても、AudioSource破壊時にキャンセルはされる）</param>
        /// <returns>AudioSourceの音量をフェードする非同期処理</returns>
        public static UniTask FadeTweenAsync(this AudioSource target, float endValue, float duration, bool isIgnoreTimeScale = false, CancellationToken token = default)
        {
            TweenDataSet<FloatTween> dataSet = SetFadeTween(target, endValue, duration, isIgnoreTimeScale);
            return dataSet.Equals(default) ? default : volumeTweenRunnerTable[dataSet.instanceId].StartTweenAsync(dataSet.tweenValue, token);
        }

        /// <summary>
        /// フェードを設定する
        /// </summary>
        /// <param name="target">AudioSource</param>
        /// <param name="endValue">フェード後の音量</param>
        /// <param name="duration">フェード時間</param>
        /// <param name="isIgnoreTimeScale">Time.timeScaleを無視するかどうか</param>
        /// <param name="callback">フェード完了後のコールバック</param>
        /// <returns>フェードデータセット</returns>
        private static TweenDataSet<FloatTween> SetFadeTween(AudioSource target, float endValue, float duration, bool isIgnoreTimeScale = false, Action<AudioSource> callback = null)
        {
            if (target == null) return default;

            int id = target.GetInstanceID();
            bool isBeforeTableContain = volumeTweenRunnerTable != null && volumeTweenRunnerTable.ContainsKey(id);

            // 前回のフェードをキャンセルする
            if (isBeforeTableContain)
            {
                // キャンセルトークンやイベントをリセットしてランナークラスを使いまわす
                volumeTweenRunnerTable[id].StopTween();
            }

            float currentValue = target.volume;

            if (Mathf.Approximately(currentValue, endValue))
            {
                target.volume = endValue;
                return default;
            }

            var floatTween = new FloatTween(currentValue, endValue, duration, isIgnoreTimeScale);
            floatTween.onTweenChanged += value => target.volume = value;

            // TweenRunnerがなければ登録する
            if (!isBeforeTableContain)
            {
                volumeTweenRunnerTable ??= new Dictionary<int, TweenRunner<FloatTween>>(1);
                volumeTweenRunnerTable.Add(id, new TweenRunner<FloatTween>(target , id));
            }

            // コールバックがあれば登録する
            if (callback != null)
            {
                volumeTweenRunnerTable[id].onTweenFinished += _ => callback.Invoke(target);
            }
            volumeTweenRunnerTable[id].onTweenFinished += instanceID =>
            {
                volumeTweenRunnerTable[instanceID].Dispose();
                volumeTweenRunnerTable.Remove(instanceID);
            };

            return new TweenDataSet<FloatTween>(floatTween, id);
        }

        /// <summary>
        /// AudioSourceの音量のフェードを停止する
        /// </summary>
        /// <param name="target">AudioSource</param>
        public static void FadeStopTween(this AudioSource target)
        {
            if (target == null) return;

            int id = target.GetInstanceID();

            // 前回のフェードをキャンセルする
            if (volumeTweenRunnerTable != null && volumeTweenRunnerTable.ContainsKey(id))
            {
                volumeTweenRunnerTable[id].StopTween();
                volumeTweenRunnerTable[id].Dispose();
                volumeTweenRunnerTable.Remove(id);
            }
        }
    }
}
