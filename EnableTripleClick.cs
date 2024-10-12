using System;
using System.Windows.Forms;

public static class RichTextBoxExtensions
{
    public static void EnableTripleClick(this RichTextBox richTextBox, Action<string> onTripleClick)
    {
        int clickCount = 0;
        DateTime lastClick = DateTime.MinValue;

        richTextBox.MouseDown += (sender, e) =>
        {
            if (e.Button == MouseButtons.Left)
            {
                TimeSpan span = DateTime.Now - lastClick;
                if (span.TotalMilliseconds < SystemInformation.DoubleClickTime)
                {
                    clickCount++;
                }
                else
                {
                    clickCount = 1;
                }

                lastClick = DateTime.Now;

                if (clickCount == 3)
                {
                    // deselect all text
                    richTextBox.SelectionLength = 0;
                    onTripleClick?.Invoke(richTextBox.Text);
                }
            }
        };
    }
}
