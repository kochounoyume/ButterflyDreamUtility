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
    public sealed class TweenRunner<T> where T : struct, ITweenValueBase
    {
        /// <summary>
        /// トゥイーンのキャンセルトークンソース
        /// </summary>
        private CancellationTokenSource cancellationTokenSource = null;
        
        /// <summary>
        /// 対象のコンポーネントの破棄時のキャンセルトークン
        /// </summary>
        private readonly CancellationToken destroyToken = CancellationToken.None;

        /// <summary>
        /// TokenSourceが破棄されたときに呼び出されるイベント
        /// </summary>
        public event UnityAction onTokenSourceDisposed = null;
        
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
        /// コンストラクタ（主にUnityのコンポーネントクラス用）
        /// </summary>
        /// <param name="cancelTokenContainer">該当のコンポーネントのインスタンス</param>
        /// <param name="cancellationTokenSource">外部からトゥイーンのキャンセルを行うためのトークンソース</param>
        public TweenRunner(Component cancelTokenContainer, out CancellationTokenSource cancellationTokenSource)
        {
            this.destroyToken = cancelTokenContainer.GetCancellationTokenOnDestroy();
            this.cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(destroyToken);
            cancellationTokenSource = this.cancellationTokenSource;
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
        /// トゥイーンを実行する
        /// </summary>
        /// <param name="tweenInfo">トゥイーン構造体</param>
        /// <param name="cancellationToken">キャンセルトークン</param>
        private static async UniTask Entry(T tweenInfo, CancellationToken cancellationToken)
        {
            if (!tweenInfo.IsValidTarget()) return;

            float elapsedTime = 0.0f;
            while (elapsedTime < tweenInfo.duration)
            {
                elapsedTime += tweenInfo.isIgnoreTimeScale ? Time.unscaledDeltaTime : Time.deltaTime;
                tweenInfo.TweenValue(Mathf.Clamp01(elapsedTime / tweenInfo.duration));
                await UniTask.Yield(cancellationToken);
            }
            tweenInfo.TweenValue(1.0f);
        }

        /// <summary>
        /// トゥイーン開始処理
        /// </summary>
        /// <param name="info">トゥイーン構造体</param>
        public void StartTween(T info)
        {
            // 投げっぱなしでトゥイーン開始
            Entry(info, cancellationTokenSource.Token).Forget();
        }

        /// <summary>
        /// トゥイーン停止処理
        /// </summary>
        public void StopTween()
        {
            if (cancellationTokenSource is {IsCancellationRequested: false})
            {
                cancellationTokenSource.Cancel();
                cancellationTokenSource.Dispose();
                cancellationTokenSource = null;
                onTokenSourceDisposed?.Invoke();
            }
        }
    }
}