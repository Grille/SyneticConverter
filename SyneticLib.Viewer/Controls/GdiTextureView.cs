using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using OpenTK.Mathematics;

using SyneticLib.WinForms.Drawing;
using SyneticLib.WinForms.IO;

namespace SyneticLib.WinForms.Controls;

public class GdiTextureView : Control
{
    private Bitmap? _bitmap;

    private readonly StringBuilder _sb;

    private TextureFormat _format;

    private int _levelCount;

    public void SubmitTexture(Texture texture)
    {
        if (_bitmap != null) _bitmap.Dispose();
        _bitmap = BitmapConverter.ConvertToBitmap(texture);
        _format = texture.Format;
        _levelCount = texture.Levels.Length;
    }

    public GdiCamera Camera { get; set; }

    public GdiTextureView()
    {
        _sb = new StringBuilder();
        Camera = new GdiCamera();
        DoubleBuffered = true;
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        Camera.ScreenSize = new Vector2(Size.Width, Size.Height);
        var g = e.Graphics;

        g.PixelOffsetMode = PixelOffsetMode.Half;
        g.InterpolationMode = InterpolationMode.NearestNeighbor;

        var zero = Camera.WorldToScreenSpace(Vector2.Zero);

        var zrect = new RectangleF(zero.X - 5, zero.Y - 5, 10, 10);

        if (_bitmap != null)
        {
            float width = _bitmap.Width * Camera.Scale;
            float height = _bitmap.Height * Camera.Scale;
            var irect = new RectangleF(zero.X - width * 0.5f, zero.Y - height * 0.5f, width, height);
            g.DrawRectangle(new Pen(ForeColor), irect.X, irect.Y, irect.Width, irect.Height);
            g.DrawImage(_bitmap, irect);
        }

        _sb.Clear();
        if (_bitmap != null)
        {
            _sb.AppendLine($"Size: {_bitmap.Size}");
            _sb.AppendLine($"Format: {_format}");
            _sb.AppendLine($"Levels: {_levelCount}");
        }
        _sb.AppendLine($"Position {Camera.Position}");
        _sb.AppendLine($"Scale {Camera.Scale}");
        g.DrawString(_sb.ToString(), Font, new SolidBrush(ForeColor), new PointF(0, 0));

        base.OnPaint(e);
    }

    protected override void OnMouseMove(MouseEventArgs e)
    {
        bool move = e.Button == MouseButtons.Left;
        Camera.MouseMoveEvent(e, move);
        base.OnMouseMove(e);
        if (move)
        {
            Invalidate();
        }
    }

    protected override void OnMouseWheel(MouseEventArgs e)
    {
        Camera.MouseScrollEvent(e, 0.5f);
        base.OnMouseWheel(e);
        Invalidate();
    }
}
