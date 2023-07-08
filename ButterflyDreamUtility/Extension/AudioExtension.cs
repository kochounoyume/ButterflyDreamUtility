using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace ButterflyDreamUtility.Extension
{
    using AsyncTween;

    public static class AudioExtension
    {
        private static Dictionary<int, CancellationTokenSource> beforeVolumeActionCancelTable = new Dictionary<int, CancellationTokenSource>(0);

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
        ///         yield return null;
        ///
        ///         // 音量をフェードインする
        ///         var cancellationTokenSource1 = audioSource.FadeTween(1, 1.0f);
        ///
        ///         yield return new WaitForSeconds(0.5f);
        ///
        ///         // フェードインをキャンセルする
        ///         cancellationTokenSource1.Cancel();
        ///         // 音量をフェードアウトする
        ///         var cancellationTokenSource2 = audioSource.FadeTween(0, 1.0f);
        ///
        ///         yield return new WaitForSeconds(0.5f);
        ///
        ///         // フェードアウトをキャンセルする
        ///         cancellationTokenSource2.Cancel();
        ///     }
        /// }
        /// ]]>
        /// </code>
        /// </example>
        /// </summary>
        /// <param name="target">AudioSource</param>
        /// <param name="endValue">フェード後の音量</param>
        /// <param name="duration">フェード時間</param>
        /// <returns>フェードを停止するためのキャンセルトークン</returns>
        public static CancellationTokenSource FadeTween(this AudioSource target, float endValue, float duration)
        {
            if (target == null) return null;
            
            int id = target.GetInstanceID();
            bool isBeforeTableContain = beforeVolumeActionCancelTable.ContainsKey(id);

            // 前回のフェードをキャンセルする
            if (isBeforeTableContain)
            {
                var beforeTokenSource = beforeVolumeActionCancelTable[id];
                beforeTokenSource.Cancel();
                beforeTokenSource.Dispose();
                beforeVolumeActionCancelTable[id] = null;
            }

            float currentValue = target.volume;

            if (Mathf.Approximately(currentValue, endValue))
            {
                target.volume = endValue;
                return null;
            }

            var colorTween = new FloatTween(currentValue, endValue, duration, true);
            colorTween.onTweenChanged += _ => target.volume = _;
            TweenRunner<FloatTween> floatTweenRunner = new TweenRunner<FloatTween>(target, out CancellationTokenSource cancellationTokenSource);
            floatTweenRunner.StartTween(colorTween);
            
            // 今回のフェードを登録する
            if (isBeforeTableContain)
            {
                beforeVolumeActionCancelTable[id] = cancellationTokenSource;
            }
            else
            {
                beforeVolumeActionCancelTable.EnsureCapacity(beforeVolumeActionCancelTable.Count + 1);
                beforeVolumeActionCancelTable.Add(id, cancellationTokenSource);
            }
            // フェードが終了したらテーブルから削除する処理を登録しておく
            floatTweenRunner.onTokenSourceDisposed += () => beforeVolumeActionCancelTable.Remove(id);

            return cancellationTokenSource;
        }
    }
}
