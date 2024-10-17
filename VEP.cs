using System;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;


namespace Ericka
{
    public partial class VEP : Form
    {
        private bool dragging = false;
        private Point dragCursorPoint;
        private Point dragFormPoint;
        private PictureBox pictureBox;
        private NotifyIcon notifyIcon;

        public VEP()
        {
            InitializeComponent();
            this.BackColor = Color.Black;
            this.TransparencyKey = Color.Black;
            this.FormBorderStyle = FormBorderStyle.None;
            this.TopMost = true;

            pictureBox = new PictureBox
            {
                SizeMode = PictureBoxSizeMode.StretchImage,
                Dock = DockStyle.Fill
            };
            this.Controls.Add(pictureBox);

            pictureBox.MouseDown += PictureBox_MouseDown;
            pictureBox.MouseMove += PictureBox_MouseMove;
            pictureBox.MouseUp += PictureBox_MouseUp;

            this.Load += Form1_Load;
            this.Resize += Form_Resize;

            CrearNotifyIcon();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        //ingresar el nombre del gif previamente importado en la carpeta Resources desde las propiedades
            pictureBox.Image = RMP.Properties.Resources.cristals;
            Debug.WriteLine("Imagen cargada en Form1_Load");
        }

        private void PictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                dragging = true;
                dragCursorPoint = Cursor.Position;
                dragFormPoint = this.Location;
                Debug.WriteLine($"MouseDown - Cursor: {dragCursorPoint}, Form: {dragFormPoint}");
            }
        }

        private void PictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (dragging)
            {
                Point dif = Point.Subtract(Cursor.Position, new Size(dragCursorPoint));
                this.Location = Point.Add(dragFormPoint, new Size(dif));
                Debug.WriteLine($"MouseMove - Nueva ubicación: {this.Location}");
            }
        }

        private void PictureBox_MouseUp(object sender, MouseEventArgs e)
        {
            dragging = false;
            Debug.WriteLine("MouseUp - Arrastre finalizado");
        }

        private void CrearNotifyIcon()
        {
            notifyIcon = new NotifyIcon
            {
                Icon = RMP.Properties.Resources.emblem,
                Visible = true,
                Text = "Widget"
            };

            ContextMenuStrip menu = new ContextMenuStrip();
            ToolStripMenuItem salirItem = new ToolStripMenuItem("Salir");
            salirItem.Click += SalirItem_Click;
            menu.Items.Add(salirItem);

            notifyIcon.ContextMenuStrip = menu;


            notifyIcon.MouseDoubleClick += NotifyIcon_MouseDoubleClick;
        }


        private void SalirItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }


        private void NotifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
        }

        private void Form_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.Hide();
                notifyIcon.Visible = true;
            }
        }
    }
}
