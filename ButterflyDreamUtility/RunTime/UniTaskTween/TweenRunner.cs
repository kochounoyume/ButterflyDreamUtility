using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace ButterflyDreamUtility.UniTaskTween
{
    /// <summary>
    /// 与えられたトゥイーンを実行するトゥイーンランナー
    /// </summary>
    /// <typeparam name="T">トゥイーン構造体</typeparam>
    internal sealed class TweenRunner<T> : IDisposable where T : ITweenValueBase
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
        /// TokenSourceが無事終了したときに呼び出されるイベント
        /// </summary>
        public event Action<int> onTweenFinished = null;

        /// <summary>
        /// コンポーネント拡張での使用時に終了イベント起動で必要なインスタンスid
        /// </summary>
        private readonly int instanceId = default;

        /// <summary>
        /// コンストラクタ（主にUnityのコンポーネントクラス用）
        /// </summary>
        /// <param name="cancelTokenContainer">該当のコンポーネントのインスタンス</param>
        /// <param name="instanceId">コンポーネント拡張での使用時に終了イベント起動で必要なインスタンスid</param>
        public TweenRunner(Component cancelTokenContainer, int instanceId = default)
        {
            this.destroyToken = cancelTokenContainer.GetCancellationTokenOnDestroy();
            this.instanceId = instanceId;
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
                // UniTask.Yieldを使うと、Time.unscaledDeltaTime使用時の挙動がおかしくなるのでUniTask.DelayFrameを使用
                await UniTask.DelayFrame(1, PlayerLoopTiming.Update, cancellationToken);
            }
            tweenInfo.TweenValue(1.0f);
            // トゥイーン終了時のイベントを呼び出す
            onTweenFinished?.Invoke(instanceId);
        }

        /// <summary>
        /// トゥイーン開始処理
        /// </summary>
        /// <param name="info">トゥイーン構造体</param>
        public void StartTween(T info)
        {
            cancellationTokenSource ??=
                destroyToken != CancellationToken.None
                    ? CancellationTokenSource.CreateLinkedTokenSource(new CancellationToken[]{destroyToken})
                    : new CancellationTokenSource();
            // 投げっぱなしでトゥイーン開始
            AsyncDoTween(info, cancellationTokenSource.Token).Forget();
        }

        /// <summary>
        /// トゥイーン開始処理を返す
        /// </summary>
        /// <param name="info">トゥイーン構造体</param>
        /// <param name="setToken">外部からセットしたキャンセルトークン</param>
        /// <returns>トゥイーン開始処理</returns>
        public UniTask StartTweenAsync(T info, CancellationToken setToken = default)
        {
            cancellationTokenSource ??=
                destroyToken != CancellationToken.None
                    ? CancellationTokenSource.CreateLinkedTokenSource(new CancellationToken[]{destroyToken, setToken})
                    : CancellationTokenSource.CreateLinkedTokenSource(new CancellationToken[]{setToken});

            var cancellationToken = cancellationTokenSource.Token;
            // キャンセルした際の処理登録
            cancellationToken.Register(() =>
            {
                // 使いまわすためいろいろリセットするだけにする
                cancellationTokenSource.Dispose();
                cancellationTokenSource = null;
                // 破壊処理がなされていてのキャンセルの場合、終了処理をかまさないといけない（でないとDisposeするタイミングを失う）
                if (destroyToken != CancellationToken.None && destroyToken.IsCancellationRequested)
                {
                    onTweenFinished?.Invoke(instanceId);
                }
                else
                {
                    onTweenFinished = null;
                }
            });

            return AsyncDoTween(info, cancellationToken);
        }

        /// <summary>
        /// トゥイーン停止処理
        /// <remarks>
        /// ランナークラスを使いまわす設計になっている
        /// </remarks>
        /// </summary>
        public void StopTween()
        {
            cancellationTokenSource.Cancel();
            // 使いまわすためのリセット処理
            cancellationTokenSource.Dispose();
            cancellationTokenSource = null;
            onTweenFinished = null;
        }

        // Disposeが呼ばれるのは、トゥイーン完全終了時と、次のトゥイーンを上書きせず即中断するときのみのはず
        public void Dispose()
        {
            cancellationTokenSource?.Dispose();
            cancellationTokenSource = null;
            onTweenFinished = null;
        }
    }
}