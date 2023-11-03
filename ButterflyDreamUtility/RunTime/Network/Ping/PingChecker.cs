using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using UnityEngine;

namespace ButterflyDreamUtility.Network.Ping
{
    using Ping = UnityEngine.Ping;

    /// <summary>
    /// Pingテストを行うクラス
    /// </summary>
    public sealed class PingChecker : IPingChecker
    {
        /// <summary>
        /// 最大のPing時間定数
        /// </summary>
        private const int MAX_TIME = 500;

        /// <summary>
        /// インターバル時間定数
        /// </summary>
        private readonly TimeSpan interval = TimeSpan.FromSeconds(0.5);

        /// <summary>
        /// 特定のPingのIPアドレス
        /// </summary>
        private readonly string address;

        /// <summary>
        ///コンストラクタ
        /// <remarks>
        /// デフォルトではGoogle Public DNSのIPアドレスが設定されている
        /// </remarks>
        /// </summary>
        /// <param name="address">特定のPingのIPアドレス</param>
        public PingChecker(string address = "8.8.8.8")
        {
            this.address = address;
        }

        /// <inheritdoc/>
        public async UniTask<int> CheckPingAsync(CancellationToken cancellationToken)
        {
            Ping ping = new Ping(address);

#if UNITY_EDITOR
            if (UnityEngine.Device.Application.internetReachability == NetworkReachability.NotReachable)
#else
            if(Application.internetReachability == NetworkReachability.NotReachable)
#endif
            {
                // インターネットに接続されていない
                throw new NetworkException();
            }

            try
            {
                await UniTask.WaitUntil(() => ping.isDone, cancellationToken: cancellationToken)
                    .TimeoutWithoutException(interval);
            }
            catch (TimeoutException e)
            {
                // pingを終了させる（このメソッドはスレッドセーフ）
                UniTask.RunOnThreadPool(ping.DestroyPing, cancellationToken: cancellationToken).Forget();
                UnityEngine.Debug.LogException(e);
                return MAX_TIME;
            }
            return ping.time < 0 ? MAX_TIME : ping.time;
        }

        /// <inheritdoc/>
        public IUniTaskAsyncEnumerable<int> RepeatPingAsyncEnumerable()
        {
            return UniTaskAsyncEnumerable.Create<int>(async (writer, token) =>
            {
                while (!token.IsCancellationRequested)
                {
                    try
                    {
                        await writer.YieldAsync(await CheckPingAsync(token));
                        // Ping再検証前にインターバルを挟む
                        await UniTask.Delay(interval, cancellationToken: token);
                    }
                    catch (NetworkException)
                    {
                        // そのままエラーをスローするとPull機構が停止してしまうので、適当な値を返す
                        await writer.YieldAsync(IPingChecker.ERROR_TINE);
                    }
                }
            });
        }
    }
}