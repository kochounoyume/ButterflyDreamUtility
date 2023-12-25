using UnityEngine;

namespace ButterflyDreamUtility.UI
{
    using Constants;
    using Extensions;
    using Debug = UnityEngine.Debug;

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
                HasCheckAxis(scrollAxis);
                base.scrollAxis = value;
            }
        }

        /// <summary>
        /// スクロール位置の割合
        /// </summary>
        public float scrollRatio
        {
            get
            {
                try
                {
                    HasCheckAxis(scrollAxis);
                }
                catch (System.Exception e)
                {
                    Debug.LogException(e);
                    return default;
                }

                switch (scrollAxis)
                {
                    case ScrollAxis.Horizontal:
                        float minX = -(virtualSizeDelta.x - realRect.width) / 2;
                        float maxX = (virtualSizeDelta.x - realRect.width) / 2;
                        return Mathf.InverseLerp(minX, maxX, viewport.anchoredPosition.x) * virtualSizeDelta.y;
                    case ScrollAxis.Vertical:
                        float minY = -(virtualSizeDelta.y - realRect.height) / 2;
                        float maxY = (virtualSizeDelta.y - realRect.height) / 2;
                        return Mathf.InverseLerp(minY, maxY, viewport.anchoredPosition.y) * virtualSizeDelta.x;
                    default:
                        throw new System.NotImplementedException();
                }
            }
            set
            {
                HasCheckAxis(scrollAxis);
                float normalizeValue = Mathf.Clamp01(value);

                switch (scrollAxis)
                {
                    case ScrollAxis.Horizontal:
                        float minX = -(virtualSizeDelta.x - realRect.width) / 2;
                        float maxX = (virtualSizeDelta.x - realRect.width) / 2;
                        viewport.anchoredPosition = new Vector2(Mathf.Lerp(minX, maxX, normalizeValue), 0);
                        break;
                    case ScrollAxis.Vertical:
                        float minY = -(virtualSizeDelta.y - realRect.height) / 2;
                        float maxY = (virtualSizeDelta.y - realRect.height) / 2;
                        viewport.anchoredPosition = new Vector2(0, Mathf.Lerp(minY, maxY, normalizeValue));
                        break;
                    default:
                        throw new System.NotImplementedException();
                }
            }
        }

        /// <summary>
        /// スクロール方向が単一方向でないことを通知する
        /// </summary>
        private void HasCheckAxis(ScrollAxis axis)
        {
            if (axis is ScrollAxis.Horizontal or ScrollAxis.Vertical) return;
            const string message = "SingleAxisScrollViewは両方向スクロール、スクロールしない設定はできません。";
            throw new System.ArgumentException(message);
        }

        /// <summary>
        /// スクロールする仮想領域のサイズの設定
        /// </summary>
        /// <param name="virtualSizeX">任意のxサイズ</param>
        /// <param name="virtualSizeY">任意のyサイズ</param>
        /// <param name="ratio">スクロール位置の割合</param>
        public void SetVirtualSizeDelta(in float virtualSizeX, in float virtualSizeY, in float ratio)
        {
            SetVirtualSizeDelta(new Vector2(virtualSizeX, virtualSizeY), ratio);
        }

        /// <summary>
        /// スクロールする仮想領域のサイズの設定
        /// </summary>
        /// <param name="virtualSize">任意のサイズ</param>
        /// <param name="ratio">スクロール位置の割合</param>
        public void SetVirtualSizeDelta(Vector2 virtualSize, in float ratio)
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
                scrollRatio = ratio;
            }
            else if (Mathf.Approximately(virtualSize.y, realRect.height))
            {
                scrollAxis = ScrollAxis.Horizontal;
                scrollRatio = ratio;
            }
            else
            {
                scrollAxis = ScrollAxis.Both;
            }
            tracker.Clear();
            viewport.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, virtualSize.x);
            viewport.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, virtualSize.y);
            tracker.Add(this, viewport, ConstantDrivenTransformProperties.ExceptXYPos);
        }

        /// <summary>
        /// スクロール割合そのままで、スクロールする仮想領域のサイズを再設定する
        /// </summary>
        /// <param name="virtualSizeX">任意のxサイズ</param>
        /// <param name="virtualSizeY">任意のyサイズ</param>
        public void SetVirtualSizeInSameRatio(in float virtualSizeX, in float virtualSizeY)
        {
            SetVirtualSizeDelta(new Vector2(virtualSizeX, virtualSizeY), scrollRatio);
        }

        /// <summary>
        /// スクロール割合そのままで、スクロールする仮想領域のサイズを再設定する
        /// </summary>
        /// <param name="virtualSize">任意のサイズ</param>
        public void SetVirtualSizeInSameRatio(Vector2 virtualSize)
        {
            SetVirtualSizeDelta(virtualSize, scrollRatio);
        }
    }
}