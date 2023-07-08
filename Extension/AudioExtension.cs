using System.Threading;
using UnityEngine;

namespace ButterflyDream.Utility.Extension
{
    using AsyncTween;
    
    public static class AudioExtension
    {
        /// <summary>
        /// AudioSourceの音量をフェードする
        /// <example>
        /// <code>
        /// <![CDATA[
        /// using UnityEngine;
        ///
        /// [RequireComponent(typeof(AudioSource))]
        /// public class Test : MonoBehaviour
        /// {
        ///     private void Start()
        ///     {
        ///         AudioSource audioSource = GetComponent<AudioSource>();
        ///         audioSource.volume = 0;
        ///         audioSource.Play();
        ///         
        ///         // 音量をフェードインする
        ///         var cancellationTokenSource = audioSource.FadeTween(0, 1.0f);
        ///         // フェードをキャンセルする
        ///         cancellationTokenSource.Cancel();
        ///         // 音量をフェードアウトする
        ///         audioSource.FadeTween(0, 0f);
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
        
            var currentValue = target.volume;

            if (Mathf.Approximately(currentValue, endValue))
            {
                target.volume = endValue;
                return null;
            }

            var colorTween = new FloatTween(currentValue, endValue, duration, true);
            colorTween.AddOnChangedCallback(_ => target.volume = _);
            TweenRunner<FloatTween> floatTweenRunner = new TweenRunner<FloatTween>(target, out CancellationTokenSource cancellationTokenSource);
            floatTweenRunner.StartTween(colorTween);
            return cancellationTokenSource;
        }
    }
}
