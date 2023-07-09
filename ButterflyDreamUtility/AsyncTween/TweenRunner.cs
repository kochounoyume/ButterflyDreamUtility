using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace ButterflyDreamUtility.AsyncTween
{
    /// <summary>
    /// 与えられたトゥイーンを実行するトゥイーンランナー
    /// </summary>
    /// <typeparam name="T">トゥイーン構造体</typeparam>
    public sealed class TweenRunner<T> where T : ITweenValueBase
    {
        /// <summary>
        /// トゥイーンのキャンセルトークンソース
        /// </summary>
        private CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        
        /// <summary>
        /// 対象のコンポーネントの破棄時のキャンセルトークン
        /// </summary>
        private readonly CancellationToken destroyToken = CancellationToken.None;

        /// <summary>
        /// TokenSourceが停止したときに呼び出されるイベント
        /// </summary>
        public event UnityAction onTweenStoped = null;

        /// <summary>
        /// コンストラクタ（主にUnityのコンポーネントクラス用）
        /// </summary>
        /// <param name="cancelTokenContainer">該当のコンポーネントのインスタンス</param>
        public TweenRunner(Component cancelTokenContainer)
        {
            this.destroyToken = cancelTokenContainer.GetCancellationTokenOnDestroy();
            this.cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(destroyToken);
        }

        /// <summary>
        /// コンストラクタ（Unityのコンポーネント以外のクラスでも使用できる）
        /// </summary>
        /// <param name="cancellationTokenSource">トゥイーンのキャンセルトークンソース</param>
        public TweenRunner(CancellationTokenSource cancellationTokenSource = default)
        {
            this.cancellationTokenSource = cancellationTokenSource;
        }

        /// <summary>
        /// 非同期でトゥイーンを実行する
        /// </summary>
        /// <param name="tweenInfo">トゥイーン構造体</param>
        /// <param name="cancellationToken">キャンセルトークン</param>
        private async UniTask AsyncDoTween(T tweenInfo, CancellationToken cancellationToken)
        {
            if (!tweenInfo.IsValidTarget()) return;
            
            float elapsedTime = 0.0f;
            while (elapsedTime < tweenInfo.duration)
            {
                elapsedTime += tweenInfo.isIgnoreTimeScale ? Time.unscaledDeltaTime : Time.deltaTime;
                tweenInfo.TweenValue(Mathf.Clamp01(elapsedTime / tweenInfo.duration));
                await UniTask.DelayFrame(1, PlayerLoopTiming.Update, cancellationToken);
            }
            tweenInfo.TweenValue(1.0f);
            // キャンセルトークンソースをリセット
            StopTween();
        }

        /// <summary>
        /// トゥイーン開始処理
        /// </summary>
        /// <param name="info">トゥイーン構造体</param>
        public void StartTween(T info)
        {
            // 投げっぱなしでトゥイーン開始
            AsyncDoTween(info, cancellationTokenSource.Token).Forget();
        }
        
        /// <summary>
        /// トゥイーン開始処理
        /// </summary>
        /// <param name="info">トゥイーン構造体</param>
        public async UniTask StartTweenAsync(T info)
        {
            await AsyncDoTween(info, cancellationTokenSource.Token);
        }

        /// <summary>
        /// トゥイーン停止処理
        /// </summary>
        public void StopTween()
        {
            if (cancellationTokenSource is {IsCancellationRequested: false})
            {
                cancellationTokenSource.Cancel();
            }
            cancellationTokenSource.Dispose();
            cancellationTokenSource = null;
            onTweenStoped?.Invoke();
        }
    }
}