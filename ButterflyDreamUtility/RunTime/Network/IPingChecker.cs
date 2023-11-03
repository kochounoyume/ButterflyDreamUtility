using System.Threading;
using Cysharp.Threading.Tasks;

namespace ButterflyDreamUtility.Network
{
    /// <summary>
    /// Pingテストを行うインターフェイス
    /// </summary>
    public interface IPingChecker
    {
        /// <summary>
        /// Pingの単位付き文字列フォーマット定数
        /// </summary>
        const string PING_FORMAT = "{0}ms";

        /// <summary>
        /// 通信エラー時のPing時間定数
        /// </summary>
        const int ERROR_TINE = -1;

        /// <summary>
        /// 非同期でPingを行う
        /// </summary>
        /// <param name="cancellationToken">キャンセルトークン</param>
        /// <returns>Ping結果（単位：ms）</returns>
        /// <exception cref="NetworkException">インターネットに接続できないときに返すエラー</exception>
        UniTask<int> CheckPingAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Pingを非同期で繰り返し行う
        /// </summary>
        /// <returns>Ping結果（単位：ms）</returns>
        IUniTaskAsyncEnumerable<int> RepeatPingAsyncEnumerable();
    }
}