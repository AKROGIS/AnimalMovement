using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

//Adapted from Eyal Shilony's code on http://social.msdn.microsoft.com/Forums/en/csharpgeneral/thread/79c6a0c7-5d38-4ae0-b2f4-62d1076f161b

namespace AnimalMovement
{
    public sealed class ColoredListBox : ListBox
    {
        private readonly Color _highlight = SystemColors.Highlight;
        private readonly Color _highlightText = SystemColors.HighlightText;
        private readonly IDictionary<int, Color> _colorList;

        public ColoredListBox()
        {
            DrawMode = DrawMode.OwnerDrawFixed;
            _colorList = new Dictionary<int, Color>();
        }

        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            if (Font.Height < 0)
                Font = DefaultFont;

            if (e.Index < 0)
                return;

            if (DataSource == null && Items.Count == 0)
            {
                return;
            }

            var rect = GetItemRectangle(e.Index);

            var background = ItemIsSelected(e) ? _highlight : BackColor;

            using (Brush brush = new SolidBrush(background))
            {
                e.Graphics.FillRectangle(brush, rect);
            }

            var textColor = ItemIsSelected(e) ? _highlightText : GetItemColor(e.Index);

            string text = DataSource == null ? Items[e.Index].ToString() : ItemTextFromDataSource(e.Index);

            TextRenderer.DrawText(e.Graphics, text, Font, rect, textColor, TextFormatFlags.GlyphOverhangPadding);
        }

        private bool ItemIsSelected(DrawItemEventArgs e)
        {
            return (SelectionMode != SelectionMode.None) &&
                   ((e.State & DrawItemState.Selected) == DrawItemState.Selected);
        }

        public void ClearItemColors()
        {
            _colorList.Clear();
        }

        public void SetItemColor(int index, Color color)
        {
            if (_colorList.ContainsKey(index))
                _colorList.Remove(index);
            _colorList.Add(index, color);
        }

        public Color GetItemColor(int index)
        {
            return _colorList.ContainsKey(index) ? _colorList[index] : ForeColor;
        }

        private string ItemTextFromDataSource(int index)
        {
            var list = DataSource as IList;
            var listSource = DataSource as IListSource;
            object item = list != null
                              ? list[index]
                              : (listSource?.GetList()[index]);
            if (item == null)
                return string.Empty;
            object prop = GetPropValue(item, DisplayMember);
            return prop.ToString();
        }

        public static object GetPropValue   (object src, string propName)
        {
            return src.GetType().GetProperty(propName).GetValue(src, null);
        }
    }
}