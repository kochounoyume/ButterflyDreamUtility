using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace ButterflyDreamUtility.Attribute
{
    /// <summary>
    /// 指定したメソッドをUnityのInspector上に表示したボタンでテスト実行できるようになる属性
    /// </summary>
    [Conditional("UNITY_EDITOR"), AttributeUsage(AttributeTargets.Method, Inherited = true)]
    public sealed class ButtonAttribute : System.Attribute
    {
        /// <summary>
        /// 引数
        /// </summary>
        public readonly object[] parameters;

        /// <summary>
        /// ボタンの名前
        /// </summary>
        public readonly string buttonName;

        /// <summary>
        /// 指定したメソッドをUnityのInspector上に表示したボタンでテスト実行できるようになる属性
        /// </summary>
        /// <param name="buttonName">ボタンの名前</param>
        /// <param name="parameters">引数</param>
        public ButtonAttribute(string buttonName, params object[] parameters)
        {
            this.buttonName = buttonName;
            this.parameters = parameters;
        }

        /// <summary>
        /// 指定したメソッドをUnityのInspector上に表示したボタンでテスト実行できるようになる属性
        /// </summary>
        /// <param name="param1">1つ目の引数</param>
        /// <param name="buttonName">メソッドの呼び出し元のメソッド名またはプロパティ名を自動で付与</param>
        public ButtonAttribute(object param1 = null, [CallerMemberName] string buttonName = "")
        {
            this.parameters = param1 == null ? null : new object[] { param1 };
            this.buttonName = buttonName;
        }

        /// <summary>
        /// 指定したメソッドをUnityのInspector上に表示したボタンでテスト実行できるようになる属性
        /// </summary>
        /// <param name="param1">1つ目の引数</param>
        /// <param name="param2">2つ目の引数</param>
        /// <param name="buttonName">メソッドの呼び出し元のメソッド名またはプロパティ名を自動で付与</param>
        public ButtonAttribute(object param1, object param2, [CallerMemberName] string buttonName = "")
        {
            this.parameters = new object[] { param1, param2 };
            this.buttonName = buttonName;
        }

        /// <summary>
        /// 指定したメソッドをUnityのInspector上に表示したボタンでテスト実行できるようになる属性
        /// </summary>
        /// <param name="param1">1つ目の引数</param>
        /// <param name="param2">2つ目の引数</param>
        /// <param name="param3">3つ目の引数</param>
        /// <param name="buttonName">メソッドの呼び出し元のメソッド名またはプロパティ名を自動で付与</param>
        public ButtonAttribute(object param1, object param2, object param3, [CallerMemberName] string buttonName = "")
        {
            this.parameters = new object[] { param1, param2, param3 };
            this.buttonName = buttonName;
        }

        /// <summary>
        /// 指定したメソッドをUnityのInspector上に表示したボタンでテスト実行できるようになる属性
        /// </summary>
        /// <param name="param1">1つ目の引数</param>
        /// <param name="param2">2つ目の引数</param>
        /// <param name="param3">3つ目の引数</param>
        /// <param name="param4">4つ目の引数</param>
        /// <param name="buttonName">メソッドの呼び出し元のメソッド名またはプロパティ名を自動で付与</param>
        public ButtonAttribute(object param1, object param2, object param3, object param4, [CallerMemberName] string buttonName = "")
        {
            this.parameters = new object[] { param1, param2, param3, param4 };
            this.buttonName = buttonName;
        }

        /// <summary>
        /// 指定したメソッドをUnityのInspector上に表示したボタンでテスト実行できるようになる属性
        /// </summary>
        /// <param name="param1">1つ目の引数</param>
        /// <param name="param2">2つ目の引数</param>
        /// <param name="param3">3つ目の引数</param>
        /// <param name="param4">4つ目の引数</param>
        /// <param name="param5">5つ目の引数</param>
        /// <param name="buttonName">メソッドの呼び出し元のメソッド名またはプロパティ名を自動で付与</param>
        public ButtonAttribute(object param1, object param2, object param3, object param4, object param5, [CallerMemberName] string buttonName = "")
        {
            this.parameters = new object[] { param1, param2, param3, param4, param5 };
            this.buttonName = buttonName;
        }
    }
}