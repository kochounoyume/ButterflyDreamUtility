using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ButterflyDreamUtility.UI
{
    using Constants;

    [RequireComponent(typeof(RectTransform))]
    public class SimpleScrollView : UIBehaviour, IBeginDragHandler, IDragHandler
    {
        /// <summary>
        /// スクロール方向
        /// </summary>
        public enum ScrollDirection
        {
            /// <summary> 横スクロール </summary>
            Horizontal,
            /// <summary> 縦スクロール </summary>
            Vertical,
            /// <summary> 両方向スクロール </summary>
            Both
        }

        [SerializeField, Tooltip("このScrollViewのRectTransform")]
        private RectTransform m_rectTransform = null;

        /// <summary>
        /// このScrollViewのRectTransform
        /// </summary>
        public RectTransform rectTransform
        {
            get
            {
                m_rectTransform ??= transform as RectTransform;
                return m_rectTransform;
            }
        }

        /// <summary>
        /// このScrollViewのRectTransformのRect
        /// </summary>
        private Rect rect => rectTransform.rect;

        [SerializeField, Tooltip("スクロール内容の表示範囲を表すRectTransform")]
        private RectTransform m_viewport = null;

        /// <summary>
        /// スクロール内容の表示範囲を表すRectTransform
        /// </summary>
        public RectTransform viewport => m_viewport;

        [SerializeField, Tooltip("スクロールする方向")]
        private ScrollDirection m_scrollDirection = ScrollDirection.Horizontal;

        /// <summary>
        /// スクロールする方向
        /// </summary>
        public ScrollDirection scrollDirection => m_scrollDirection;

        /// <summary>
        /// スクロール内容の表示範囲のサイズ
        /// </summary>
        public float size
        {
            get
            {
                Rect viewRect = viewport.rect;
                return scrollDirection switch
                {
                    ScrollDirection.Horizontal => viewRect.width,
                    ScrollDirection.Vertical => viewRect.height,
                    ScrollDirection.Both => default,
                    _ => default
                };
            }
            set
            {
                tracker.Clear();
                switch (scrollDirection)
                {
                    case ScrollDirection.Horizontal:
                        viewport.sizeDelta = new Vector2(value, rect.height);
                        tracker.Add(this, viewport, ConstantDrivenTransformProperties.ExceptXPos);
                        break;
                    case ScrollDirection.Vertical:
                        viewport.sizeDelta = new Vector2(rect.width, value);
                        tracker.Add(this, viewport, ConstantDrivenTransformProperties.ExceptYPos);
                        break;
                    case ScrollDirection.Both:
                        const string message = "ScrollDirectionがBothの場合はsizeDeltaを使用してください";
                        throw new InvalidOperationException(message);
                    default:
                        throw new ArgumentOutOfRangeException(nameof(scrollDirection));
                }
            }
        }

        private float m_minPos = default;

        /// <summary>
        /// スクロール可能な最小位置
        /// </summary>
        public float minPos
        {
            get
            {
                if (Mathf.Approximately(m_minPos, default))
                {
                    m_minPos = scrollDirection switch
                    {
                        ScrollDirection.Horizontal => - size / 2 + rect.width / 2,
                        ScrollDirection.Vertical => - size / 2 + rect.height / 2,
                        ScrollDirection.Both => default,
                        _ => default
                    };
                }
                return m_minPos;
            }
        }

        private float m_maxPos = default;

        /// <summary>
        /// スクロール可能な最大位置
        /// </summary>
        public float maxPos
        {
            get
            {
                if (Mathf.Approximately(m_maxPos, default))
                {
                    m_maxPos = scrollDirection switch
                    {
                        ScrollDirection.Horizontal => size / 2 - rect.width / 2,
                        ScrollDirection.Vertical => size / 2 - rect.height / 2,
                        ScrollDirection.Both => default,
                        _ => default
                    };
                }
                return m_maxPos;
            }
        }

        /// <summary>
        /// スクロール内容の表示範囲のサイズ
        /// </summary>
        public Vector2 sizeDelta
        {
            get => viewport.sizeDelta;
            set
            {
                if (scrollDirection != ScrollDirection.Both)
                {
                    const string message = "ScrollDirectionがBoth以外の場合はfloat型のsizeを使用してください";
                    throw new InvalidOperationException(message);
                }
                tracker.Clear();
                viewport.sizeDelta = value;
                tracker.Add(this, viewport, ConstantDrivenTransformProperties.ExceptXYPos);
            }
        }

        /// <summary>
        /// 縦・横スクロールの際のスクロール時のイベント
        /// <remarks>
        /// 値はViewportの該当座標の値
        /// </remarks>
        /// </summary>
        public Action<float> onValueChanged = null;

        /// <summary>
        /// ドラッグを開始した時のタッチ位置
        /// </summary>
        private Vector2 pointerStartLocalPoint = Vector2.zero;

        /// <summary>
        /// ドラッグを開始した時のviewportの位置
        /// </summary>
        private Vector2 viewPortStartPos = Vector2.zero;

        /// <summary>
        /// RectTransformのトラッカー
        /// </summary>
        private DrivenRectTransformTracker tracker = new DrivenRectTransformTracker();

        // ドラッグが開始される前にBaseInputModuleによって呼び出される
        void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
        {
            // 左クリック以外は無視
            if (eventData.button != PointerEventData.InputButton.Left) return;
            if (!IsActive()) return;

            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                rectTransform, eventData.position, eventData.pressEventCamera, out pointerStartLocalPoint);
            viewPortStartPos = viewport.anchoredPosition;
        }

        // ドラッグが発生しているとき、カーソルが移動するたびに呼び出される
        void IDragHandler.OnDrag(PointerEventData eventData)
        {
            // 左クリック以外は無視
            if (eventData.button != PointerEventData.InputButton.Left) return;
            if (!IsActive()) return;

            if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    rectTransform, eventData.position, eventData.pressEventCamera, out Vector2 localPoint))
            {
                return;
            }

            Vector2 pointerDelta = localPoint - pointerStartLocalPoint;
            // MEMO:Vector系の等価評価は浮動小数点の誤差が考慮されている
            if (pointerDelta == Vector2.zero) return;
            // 移動量が多く、このまま移動してしまうとダメな場合は閾値まで移動する
            if(!CanMovableRange(rect, viewport.rect, ref pointerDelta))
            {
                if (scrollDirection != ScrollDirection.Horizontal && scrollDirection != ScrollDirection.Vertical) return;
                viewport.anchoredPosition = GetAnchoredPos(ref pointerDelta, true);
                return;
            }
            viewport.anchoredPosition = GetAnchoredPos(ref pointerDelta);
        }

        /// <summary>
        /// 移動後、指定したRectが比較対象のRectと重なるかどうか
        /// </summary>
        /// <param name="criteria">基準になるRect</param>
        /// <param name="other">比較対象のRect</param>
        /// <param name="distance">移動量</param>
        /// <param name="allowInverse">Rect の width と height に負の数が認められるかどうか</param>
        /// <returns></returns>
        private bool CanMovableRange(Rect criteria, Rect other, ref Vector2 distance, in bool allowInverse = false)
        {
            criteria.center = rectTransform.position;
            other.center = (Vector2)viewport.position + distance;
            return criteria.Overlaps(other, allowInverse);
        }

        /// <summary>
        /// スクロール後のviewportの実際の位置を取得する
        /// </summary>
        /// <param name="pointerDelta">移動量</param>
        /// <param name="isLimit">スクロール可能な範囲を超えているかどうか</param>
        /// <returns></returns>
        private Vector2 GetAnchoredPos(ref Vector2 pointerDelta, in bool isLimit = false)
        {
            if (isLimit)
            {
                switch (scrollDirection)
                {
                    case ScrollDirection.Horizontal:
                        float paramX = pointerDelta.x < 0 ? minPos : maxPos;
                        onValueChanged?.Invoke(paramX);
                        return new Vector2(paramX, viewPortStartPos.y);
                    case ScrollDirection.Vertical:
                        float paramY = pointerDelta.y < 0 ? minPos : maxPos;
                        onValueChanged?.Invoke(paramY);
                        return new Vector2(viewPortStartPos.x, paramY);
                    case ScrollDirection.Both:
                        return viewport.anchoredPosition;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(scrollDirection));
                }
            }
            switch (scrollDirection)
            {
                case ScrollDirection.Horizontal:
                    float paramX = viewPortStartPos.x + pointerDelta.x;
                    onValueChanged?.Invoke(paramX);
                    return new Vector2(paramX, viewPortStartPos.y);
                case ScrollDirection.Vertical:
                    float paramY = viewPortStartPos.y + pointerDelta.y;
                    onValueChanged?.Invoke(paramY);
                    return new Vector2(viewPortStartPos.x, paramY);
                case ScrollDirection.Both:
                    return viewPortStartPos + pointerDelta;
                default:
                    throw new ArgumentOutOfRangeException(nameof(scrollDirection));
            }
        }
    }
}