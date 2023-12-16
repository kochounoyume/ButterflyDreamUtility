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

        /// <summary>
        /// XY座標を除いた全DrivenTransformProperties
        /// </summary>
        public static DrivenTransformProperties ExceptXYPos
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)] get
                => DrivenTransformProperties.AnchoredPositionZ
                   | DrivenTransformProperties.SizeDelta
                   | DrivenTransformProperties.Anchors
                   | DrivenTransformProperties.Pivot
                   | DrivenTransformProperties.Rotation
                   | DrivenTransformProperties.Scale;
        }

        /// <summary>
        /// X座標を除いた全DrivenTransformProperties
        /// </summary>
        public static DrivenTransformProperties ExceptXPos
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)] get
                => DrivenTransformProperties.AnchoredPositionY
                   | DrivenTransformProperties.AnchoredPositionZ
                   | DrivenTransformProperties.SizeDelta
                   | DrivenTransformProperties.Anchors
                   | DrivenTransformProperties.Pivot
                   | DrivenTransformProperties.Rotation
                   | DrivenTransformProperties.Scale;
        }

        /// <summary>
        /// Y座標を除いた全DrivenTransformProperties
        /// </summary>
        public static DrivenTransformProperties ExceptYPos
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)] get
                => DrivenTransformProperties.AnchoredPositionX
                   | DrivenTransformProperties.AnchoredPositionZ
                   | DrivenTransformProperties.SizeDelta
                   | DrivenTransformProperties.Anchors
                   | DrivenTransformProperties.Pivot
                   | DrivenTransformProperties.Rotation
                   | DrivenTransformProperties.Scale;
        }
    }
}
