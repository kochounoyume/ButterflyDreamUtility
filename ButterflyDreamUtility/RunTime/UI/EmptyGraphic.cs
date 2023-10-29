using UnityEngine;
using UnityEngine.UI;

namespace ButterflyDreamUtility.UI
{
    using Extensions;

    /// <summary>
    /// 空処理のグラフィック
    /// <remarks>
    /// 想定用途：マスク時のベース, 画面全体を透明なuGUIで覆いたいとき、Buttonの判定領域を透明にしたいとき
    /// </remarks>
    /// </summary>
    [RequireComponent(typeof(CanvasRenderer))]
    public sealed class EmptyGraphic : Graphic
    {
        /// <summary>
        /// 子オブジェクト以下のGraphic（自分は含まない）
        /// </summary>
        private Graphic[] childGraphics = null;

        /// <inheritdoc/>
        public override Color color
        {
            get => base.color;
            set
            {
                // 子オブジェクトの色は変更する
                foreach (var childGraphic in childGraphics)
                {
                    childGraphic.color = value;
                }
            }
        }

        /// <inheritdoc/>
        protected override void Start()
        {
            base.Start();
            childGraphics = this.GetComponentsInChildren<Graphic>(true, false);
        }

        /// <inheritdoc/>
        public override void SetMaterialDirty()
        {
            // 空処理を上書き
        }

        /// <inheritdoc/>
        public override void SetVerticesDirty()
        {
            // 空処理を上書き
        }

        /// <inheritdoc/>
        protected override void OnPopulateMesh(VertexHelper vh)
        {
            vh.Clear();
        }
    }
}