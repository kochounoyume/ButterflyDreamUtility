using System.Runtime.CompilerServices;
using UnityEngine;

namespace ButterflyDreamUtility.Constants
{
    /// <summary>
    /// Color32型の定数定義クラス
    /// </summary>
    public sealed class ConstantColor32
    {
        /// <summary>
        /// <para>赤単色. RGBAは(255, 0, 0, 255).</para>
        /// </summary>
        public static Color32 red
        {
            [MethodImpl((MethodImplOptions) 256)] get => new Color32(255, 0, 0, 255);
        }

        /// <summary>
        /// <para>緑単色. RGBAは(0, 255, 0, 255).</para>
        /// </summary>
        public static Color32 green
        {
            [MethodImpl((MethodImplOptions) 256)] get => new Color32(0, 255, 0, 255);
        }

        /// <summary>
        /// <para>青単色. RGBAは(0, 0, 255, 255).</para>
        /// </summary>
        public static Color32 blue
        {
            [MethodImpl((MethodImplOptions) 256)] get => new Color32(0, 0, 255, 255);
        }

        /// <summary>
        /// <para>白単色. RGBAは(255, 255, 255, 255).</para>
        /// </summary>
        public static Color32 white
        {
            [MethodImpl((MethodImplOptions) 256)] get => new Color32(255, 255, 255, 255);
        }

        /// <summary>
        /// <para>黒単色. RGBAは(0, 0, 0, 255).</para>
        /// </summary>
        public static Color32 black
        {
            [MethodImpl((MethodImplOptions) 256)] get => new Color32(0, 0, 0, 255);
        }

        /// <summary>
        /// <para>黄色. RGBAは(255, 235, 4, 255). Unity独自の設定値.</para>
        /// <seealso cref="UnityEngine.Color.yellow"/>
        /// </summary>
        public static Color32 yellow
        {
            [MethodImpl((MethodImplOptions) 256)] get => new Color32(255, 235, 4, 255);
        }

        /// <summary>
        /// <para>シアン. RGBAは(0, 255, 255, 255). </para>
        /// </summary>
        public static Color32 cyan
        {
            [MethodImpl((MethodImplOptions) 256)] get => new Color32(0, 255, 255, 255);
        }

        /// <summary>
        /// <para>マゼンタ. RGBAは(255, 0, 255, 255). </para>
        /// </summary>
        public static Color32 magenta
        {
            [MethodImpl((MethodImplOptions) 256)] get => new Color32(255, 0, 255, 255);
        }

        /// <summary>
        /// <para>灰色. RGBAは(128, 128, 128, 255). </para>
        /// </summary>
        public static Color32 gray
        {
            [MethodImpl((MethodImplOptions) 256)] get => new Color32(128, 128, 128, 255);
        }

        /// <summary>
        /// <para>grayの英語上の近似表記. RGBAは同じ値だが(128, 128, 128, 255).</para>
        /// </summary>
        public static Color32 grey
        {
            [MethodImpl((MethodImplOptions) 256)] get => new Color32(128, 128, 128, 255);
        }

        /// <summary>
        /// <para>完全に透明. RGBAは(0, 0, 0, 0).</para>
        /// </summary>
        public static Color32 clear
        {
            [MethodImpl((MethodImplOptions) 256)] get => new Color32(0, 0, 0, 0);
        } 
    }
}
