using System;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Threading.Tasks;

class MonitorPreviewForm : Form
{
    private System.Windows.Forms.Timer timer;
    private Bitmap screenBitmap;
    private Point mouseDownLocation;
    private int width = 400;
    private int monitorIndexTop = 1;
    private int monitorIndexBottom = 2;

    public MonitorPreviewForm()
    {
        this.FormBorderStyle = FormBorderStyle.None;
        this.TopMost = true;
        this.Size = new Size(width, width);
        this.StartPosition = FormStartPosition.Manual;
        this.Location = new Point(10, 10);
        this.DoubleBuffered = true;

        this.MouseDown += MonitorPreviewForm_MouseDown;
        this.MouseMove += MonitorPreviewForm_MouseMove;
        this.MouseWheel += MonitorPreviewForm_MouseWheel;

        timer = new System.Windows.Forms.Timer();
        timer.Interval = 200; // Refresh every 500ms
        timer.Tick += (s, e) => RefreshScreen();
        timer.Start();
    }

    private void MonitorPreviewForm_MouseDown(object sender, MouseEventArgs e)
    {
        if (e.Button == MouseButtons.Left && Control.ModifierKeys == Keys.Control)
        {
            mouseDownLocation = e.Location;
        }
        else if (e.Button == MouseButtons.Right && Control.ModifierKeys == Keys.Control)
        {
            Screen[] screens = Screen.AllScreens;
            if (e.Y < this.Height / 2)
            {
                monitorIndexTop = (monitorIndexTop + 1) % screens.Length;
            }
            else
            {
                monitorIndexBottom = (monitorIndexBottom + 1) % screens.Length;
            }
        }
    }

    private void MonitorPreviewForm_MouseMove(object sender, MouseEventArgs e)
    {
        if (e.Button == MouseButtons.Left && Control.ModifierKeys == Keys.Control)
        {
            this.Left += e.X - mouseDownLocation.X;
            this.Top += e.Y - mouseDownLocation.Y;
        }
    }

    private void MonitorPreviewForm_MouseWheel(object sender, MouseEventArgs e)
    {
        if (Control.ModifierKeys == Keys.Control)
        {
            width = Math.Max(100, width + (e.Delta > 0 ? 50 : -50));
            RefreshScreen();
        }
    }

    private void RefreshScreen()
    {
        try
        {
            Screen[] screens = Screen.AllScreens;
            if (screens.Length < 2) return; // Ensure at least 2 monitors exist

            using (Graphics g = CreateGraphics())
            {
                // Capture selected monitor for top
                Rectangle rectTop = screens[monitorIndexTop].Bounds;
                int heightTop = (int)((double)rectTop.Height / rectTop.Width * width);
                using (Bitmap bmpTop = new Bitmap(rectTop.Width, rectTop.Height))
                using (Graphics gTop = Graphics.FromImage(bmpTop))
                {
                    gTop.CopyFromScreen(rectTop.X, rectTop.Y, 0, 0, rectTop.Size);
                    g.DrawImage(bmpTop, 0, 0, width, heightTop);
                }

                // Capture selected monitor for bottom
                Rectangle rectBottom = screens[monitorIndexBottom].Bounds;
                int heightBottom = (int)((double)rectBottom.Height / rectBottom.Width * width);
                using (Bitmap bmpBottom = new Bitmap(rectBottom.Width, rectBottom.Height))
                using (Graphics gBottom = Graphics.FromImage(bmpBottom))
                {
                    gBottom.CopyFromScreen(rectBottom.X, rectBottom.Y, 0, 0, rectBottom.Size);
                    g.DrawImage(bmpBottom, 0, heightTop, width, heightBottom);
                }

                this.Size = new Size(width, heightTop + heightBottom);

                // Draw cursor cross if inside displayed monitors
                Point cursorPosition = Cursor.Position;
                Rectangle currentRect;
                int offsetY;
                if (rectTop.Contains(cursorPosition))
                {
                    currentRect = rectTop;
                    offsetY = 0;
                }
                else if (rectBottom.Contains(cursorPosition))
                {
                    currentRect = rectBottom;
                    offsetY = heightTop;
                }
                else
                {
                    return;
                }

                int crossSize = 20;
                Point relativeCursorPos = new Point(
                    (cursorPosition.X - currentRect.X) * width / currentRect.Width,
                    (cursorPosition.Y - currentRect.Y) * (currentRect == rectTop ? heightTop : heightBottom) / currentRect.Height + offsetY
                );
                using (Pen pen = new Pen(Color.Red, 3))
                {
                    g.DrawLine(pen, relativeCursorPos.X - crossSize, relativeCursorPos.Y, relativeCursorPos.X + crossSize, relativeCursorPos.Y);
                    g.DrawLine(pen, relativeCursorPos.X, relativeCursorPos.Y - crossSize, relativeCursorPos.X, relativeCursorPos.Y + crossSize);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error capturing screen: " + ex.Message);
        }
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        if (screenBitmap != null)
            e.Graphics.DrawImage(screenBitmap, 0, 0);
    }
}
