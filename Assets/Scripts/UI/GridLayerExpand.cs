using System.Collections.Generic;

namespace UnityEngine.UI
{
    [AddComponentMenu("Layout/MGrid", 155)]
    public class GridLayerExpand : MonoBehaviour
    {
        protected DrivenRectTransformTracker m_Tracker;

        private List<RectTransform> _Children = new List<RectTransform>();
        protected  List<RectTransform> Children
        {
            get
            {
                return _Children;
            }
        }
        [SerializeField]
        protected Vector2 m_CellSize = new Vector2(100, 100);
        public Vector2 cellSize
        {
            get
            {
                return m_CellSize;
            }
            set
            {
                m_CellSize = value;
            }
        }

        [SerializeField]
        protected Vector2 m_IgnorerSize = new Vector2(100, 100);
        public Vector2 ignorerSize
        {
            get
            {
                return m_IgnorerSize;
            }
            set
            {
                m_IgnorerSize = value;
            }
        }

        //间距
        [SerializeField]
        protected Vector2 m_Spacing = Vector2.zero;
        public Vector2 spacing
        {
            get
            {
                return this.m_Spacing;
            }
            set
            {
                m_Spacing = value;
            }
        }
        [SerializeField]
        protected TextAnchor m_ChildAlignment = TextAnchor.UpperLeft;

        public TextAnchor childAlignment
        {
            get
            {
                return m_ChildAlignment;
            }
            set
            {
                m_ChildAlignment = value;
            }
        }

        private RectTransform rectTransform;

        [SerializeField]
        private int m_ConstraintCount = 0;

        [ContextMenu("Execute")]
        public void Execute()
        {
            rectTransform = GetComponent<RectTransform>();
            Children.Clear();
            for (int i = 0; i < rectTransform.childCount; i++)
            {
                RectTransform ct = transform.GetChild(i) as RectTransform;
                Children.Add(ct);
            }
            m_Tracker.Clear();
            SetLayoutHorizontal();
            SetLayoutVertical();
        }

        private void SetLayoutHorizontal()
        {
            SetCellsAlongAxis(0);
        }

        private void SetLayoutVertical()
        {
            SetCellsAlongAxis(1);
        }

        private void SetCellsAlongAxis(int axis)
        {
            if (axis == 0)
            {
                for (int i = 0; i < Children.Count; i++)
                {
                    RectTransform rectTransform = Children[i];
                    m_Tracker.Add(this, rectTransform,
                    DrivenTransformProperties.AnchoredPositionX | DrivenTransformProperties.AnchoredPositionY 
                    | DrivenTransformProperties.AnchorMinX | DrivenTransformProperties.AnchorMinY 
                    | DrivenTransformProperties.AnchorMaxX | DrivenTransformProperties.AnchorMaxY 
                    | DrivenTransformProperties.SizeDeltaX | DrivenTransformProperties.SizeDeltaY);
                    rectTransform.anchorMin = Vector2.up;
                    rectTransform.anchorMax = Vector2.up;
                    rectTransform.sizeDelta = rectTransform.GetComponent<ILayoutIgnorer>() == null ? cellSize : ignorerSize;
                }
            }
            else
            {
                int num;
                int num2;
                num = m_ConstraintCount;
                num2 = Mathf.CeilToInt(Children.Count / (float)num - 0.001f);

                int num5;
                int num6;
                int num7;
                num5 = num2;
                num7 = Mathf.Clamp(num2, 1, Children.Count);
                num6 = Mathf.Clamp(num, 1, Mathf.CeilToInt(Children.Count / (float)num5));
                Vector2 vector = new Vector2(num6 * cellSize.x + (num6 - 1) * spacing.x, num7 * cellSize.y + (num7 - 1) * spacing.y);
                Vector2 vector2 = new Vector2(GetStartOffset(0, vector.x), GetStartOffset(1, vector.y));
                int ignorerTimes = 0;
                int mo = 0;
                for (int j = 0; j < Children.Count; j++)
                {
                    int num8;
                    int num9;

                    //第几列
                    num8 = (j + mo)% num;
                    //第几行
                    num9 = (j + mo) / num;
                    bool isIgnorer = Children[j].GetComponent<ILayoutIgnorer>() != null;
                    if (isIgnorer)
                    {
                        ignorerTimes++;
                        SetChildAlongAxis(Children[j], 0, 0, ignorerSize[0]);
                        SetChildAlongAxis(Children[j], 1, vector2.y + (cellSize[1] + spacing[1]) * (num9 + num8), ignorerSize[1]);
                        if (num8 != 1)
                            mo++;
                    }
                    else
                    {
                        SetChildAlongAxis(Children[j], 0, vector2.x + (cellSize[0] + spacing[0]) * num8, cellSize[0]);
                        SetChildAlongAxis(Children[j], 1, vector2.y + (cellSize[1] + spacing[1]) * num9 + ignorerSize.y * ignorerTimes - mo * cellSize[1], cellSize[1]);
                    }                 
                }
            }
        }
        protected float GetStartOffset(int axis, float requiredSpaceWithoutPadding)
        {
            float num = requiredSpaceWithoutPadding ;
            float num2 = rectTransform.rect.size[axis];
            float num3 = num2 - num;
            float alignmentOnAxis = this.GetAlignmentOnAxis(axis);
            return num3 * alignmentOnAxis;
        }
        protected float GetAlignmentOnAxis(int axis)
        {
            float result;
            if (axis == 0)
            {
                result = ((int)childAlignment % (int)TextAnchor.MiddleLeft) * 0.5f;
            }
            else
            {
                result = ((int)childAlignment / (int)TextAnchor.MiddleLeft) * 0.5f;
            }
            return result;
        }

        protected void SetChildAlongAxis(RectTransform rect, int axis, float pos, float size)
        {
            if (!(rect == null))
            {
                this.m_Tracker.Add(this, rect, DrivenTransformProperties.Anchors | ((axis != 0) ? (DrivenTransformProperties.AnchoredPositionY | DrivenTransformProperties.SizeDeltaY) : (DrivenTransformProperties.AnchoredPositionX | DrivenTransformProperties.SizeDeltaX)));
                rect.SetInsetAndSizeFromParentEdge((axis != 0) ? RectTransform.Edge.Top : RectTransform.Edge.Left, pos, size);
            }
        }

		void OnDisable()
        {
            this.m_Tracker.Clear();
            LayoutRebuilder.MarkLayoutForRebuild(rectTransform);
        }
    }
}
