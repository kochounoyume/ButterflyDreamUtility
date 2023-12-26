using UnityEngine;

namespace ButterflyDreamUtility.UI
{
    using Constants;
    using Extensions;

    /// <summary>
    /// 単一方向のみスクロールできるスクロールビュークラス
    /// </summary>
    public class SingleAxisScrollView : SimpleScrollView
    {
        /// <inheritdoc/>
        public override ScrollAxis scrollAxis
        {
            get => base.scrollAxis;
            protected set
            {
                if (value is ScrollAxis.Horizontal or ScrollAxis.Vertical)
                {
                    base.scrollAxis = value;
                    return;
                }
                const string message = "SingleAxisScrollViewは両方向スクロール、スクロールしない設定はできません。";
                UnityEngine.Debug.LogWarning(message);
            }
        }

        /// <summary>
        /// スクロールする仮想領域のサイズの設定
        /// <remarks>
        /// anchoredLengthで指定した長さ分、初期アンカーから距離を開ける
        /// </remarks>
        /// </summary>
        /// <param name="virtualSizeX">任意のxサイズ</param>
        /// <param name="virtualSizeY">任意のyサイズ</param>
        /// <param name="anchoredLength">初期アンカーからの長さ</param>
        public void SetVirtualSizeDelta(in float virtualSizeX, in float virtualSizeY, in float anchoredLength)
        {
            SetVirtualSizeDelta(new Vector2(virtualSizeX, virtualSizeY), anchoredLength);
        }

        /// <summary>
        /// スクロールする仮想領域のサイズの設定
        /// <remarks>
        /// anchoredLengthで指定した長さ分、初期アンカーから距離を開ける
        /// </remarks>
        /// </summary>
        /// <param name="virtualSize">任意のサイズ</param>
        /// <param name="anchoredLength">初期アンカーからの長さ</param>
        public void SetVirtualSizeDelta(Vector2 virtualSize, in float anchoredLength)
        {
            virtualSizeDelta = virtualSize;
            if (virtualSize.IsLessThanOrEqual(realSizeDelta))
            {
                scrollAxis = ScrollAxis.None;
                viewport.anchoredPosition = Vector2.zero;
            }
            else if(Mathf.Approximately(virtualSize.x, realRect.width))
            {
                scrollAxis = ScrollAxis.Vertical;
                viewport.anchoredPosition = new Vector2(0, (- virtualSize.y + realRect.height) / 2 + anchoredLength);
            }
            else if (Mathf.Approximately(virtualSize.y, realRect.height))
            {
                scrollAxis = ScrollAxis.Horizontal;
                viewport.anchoredPosition = new Vector2((- virtualSize.x + realRect.width) / 2 + anchoredLength, 0);
            }
            tracker.Clear();
            viewport.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, virtualSize.x);
            viewport.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, virtualSize.y);
            tracker.Add(this, viewport, ConstantDrivenTransformProperties.ExceptXYPos);
        }
    }
}