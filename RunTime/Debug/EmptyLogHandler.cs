using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace ButterflyDreamUtility.Debug
{
    /// <summary>
    /// 中身の処理が空のログハンドラー
    /// <remarks>
    /// エディタ実行時と開発ビルド時以外の環境下で使用し、ログ出力を無効化する
    /// </remarks>
    /// </summary>
    internal sealed class EmptyLogHandler : ILogHandler
    {
        public void LogFormat(LogType logType, Object context, string format, params object[] args) { }

        public void LogException(Exception exception, Object context) { }
    }
}