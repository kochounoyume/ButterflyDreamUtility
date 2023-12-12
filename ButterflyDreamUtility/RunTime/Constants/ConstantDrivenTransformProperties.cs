using System.Runtime.CompilerServices;
using UnityEngine;

namespace ButterflyDreamUtility.Constants
{
    /// <summary>
    /// DrivenTransformProperties型の定数定義クラス
    /// </summary>
    public static class ConstantDrivenTransformProperties
    {
        /// <summary>
        /// 標準的なDrivenTransformProperties
        /// </summary>
        public static DrivenTransformProperties StandardProperties
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]get
                => DrivenTransformProperties.AnchoredPositionZ
                   | DrivenTransformProperties.Anchors
                   | DrivenTransformProperties.Pivot
                   | DrivenTransformProperties.Rotation
                   | DrivenTransformProperties.Scale;
        }
    }
}
