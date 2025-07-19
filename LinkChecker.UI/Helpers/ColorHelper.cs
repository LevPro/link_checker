using System.Windows.Media;

namespace LinkChecker.UI.Helpers
{
    public static class ColorHelper
    {
        public static Brush GetBrushByResponseCode(int? code)
        {
            if (!code.HasValue) return Brushes.Gray;
            if (code.Value >= 200 && code.Value < 300) return Brushes.LightGreen;
            if (code.Value >= 300 && code.Value < 400) return Brushes.LightBlue;
            if (code.Value >= 400 && code.Value < 500) return Brushes.Orange;
            if (code.Value >= 500) return Brushes.Red;
            return Brushes.Gray;
        }
    }
}