using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace ButterflyDreamUtility.Extension
{
    using AsyncTween;

    public static class AudioExtension
    {
        private static Dictionary<int, TweenRunner<FloatTween>> beforeVolumeTweenRunnerTable = new Dictionary<int, TweenRunner<FloatTween>>(0);

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
        /// <returns>フェードを停止するためのキャンセルトークン</returns>
        public static void FadeTween(this AudioSource target, float endValue, float duration, bool isIgnoreTimeScale = false)
        {
            TweenDataSet<FloatTween> dataSet = SetFadeTween(target, endValue, duration, isIgnoreTimeScale);
            dataSet.tweenRunner.StartTween(dataSet.tweenValue);
        }

        /// <summary>
        /// AudioSourceの音量を非同期でフェードする
        /// </summary>
        /// <param name="target">AudioSource</param>
        /// <param name="endValue">フェード後の音量</param>
        /// <param name="duration">フェード時間</param>
        /// <param name="isIgnoreTimeScale">Time.timeScaleを無視するかどうか</param>
        public static async UniTask FadeTweenAsync(this AudioSource target, float endValue, float duration, bool isIgnoreTimeScale = false)
        {
            TweenDataSet<FloatTween> dataSet = SetFadeTween(target, endValue, duration, isIgnoreTimeScale);
            await dataSet.tweenRunner.StartTweenAsync(dataSet.tweenValue);
        }

        /// <summary>
        /// フェードを設定する
        /// </summary>
        /// <param name="target">AudioSource</param>
        /// <param name="endValue">フェード後の音量</param>
        /// <param name="duration">フェード時間</param>
        /// <param name="isIgnoreTimeScale">Time.timeScaleを無視するかどうか</param>
        /// <returns>フェードデータセット</returns>
        private static TweenDataSet<FloatTween> SetFadeTween(this AudioSource target, float endValue, float duration, bool isIgnoreTimeScale = false)
        {
            if (target == null) return default;
            
            int id = target.GetInstanceID();
            bool isBeforeTableContain = beforeVolumeTweenRunnerTable.ContainsKey(id);

            // 前回のフェードをキャンセルする
            if (isBeforeTableContain)
            {
                beforeVolumeTweenRunnerTable[id].StopTween();
                beforeVolumeTweenRunnerTable[id] = null;
            }

            float currentValue = target.volume;

            if (Mathf.Approximately(currentValue, endValue))
            {
                target.volume = endValue;
                return default;
            }

            var floatTween = new FloatTween(currentValue, endValue, duration, isIgnoreTimeScale);
            floatTween.onTweenChanged += _ => target.volume = _;
            TweenRunner<FloatTween> floatTweenRunner = new TweenRunner<FloatTween>(target);

            // 今回のフェードを登録する
            if (isBeforeTableContain)
            {
                beforeVolumeTweenRunnerTable[id] = floatTweenRunner;
            }
            else
            {
                beforeVolumeTweenRunnerTable.Add(id, floatTweenRunner);
            }
            
            // フェードが終了したらテーブルから削除する処理を登録しておく
            floatTweenRunner.onTweenStoped += () => beforeVolumeTweenRunnerTable.Remove(id);

            return new TweenDataSet<FloatTween>(floatTween, floatTweenRunner);
        }
        
        /// <summary>
        /// AudioSourceの音量をフェードを停止する
        /// </summary>
        /// <param name="target">AudioSource</param>
        public static void FadeStopTween(this AudioSource target)
        {
            if (target == null) return;
            
            int id = target.GetInstanceID();

            // 前回のフェードをキャンセルする
            if (beforeVolumeTweenRunnerTable.ContainsKey(id))
            {
                beforeVolumeTweenRunnerTable[id].StopTween();
                beforeVolumeTweenRunnerTable[id] = null;
            }
        }
    }
}
