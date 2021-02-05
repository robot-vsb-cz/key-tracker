using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestKey
{
    public partial class Form1 : Form
    {

        List<Button> buttons = new List<Button>();
        bool mouseMiddle = false;
        bool mouseRight = false;
        bool mouseLeft = false;
        public Form1()
        {
            InitializeComponent();
            pictureMouse.Paint += PictureBox1_Paint;

            buttons.Add(button1);
            buttons.Add(button2);
            buttons.Add(button3);


            this.TopMost = true;

            HookInputs test = new HookInputs();
            test.onChangeInput += Test_onChangeInput;
            test.onMouseInput += Test_onMouseInput;
            test.Subscribe();

            Form1_Resize(null, null);

        }

        private void PictureBox1_Paint(object sender, PaintEventArgs e)
        {
            int radius1 = (int)(0.2f * Math.Min(pictureMouse.Width, pictureMouse.Height)); // poloměr zaoblení nahoře, nedávat větší než 0.33
            int radius2 = (int)(0.4f * Math.Min(pictureMouse.Width, pictureMouse.Height)); // poloměr zaoblení dole
            int buttonHeight = (int)(0.4f * pictureMouse.Height); // výška tlačítek
            int offset = 2;
            Pen penBlack = new Pen(Color.Black, 2);              // 2 je tloušťka čáry
            Brush brushButton = new SolidBrush(Color.RoyalBlue); // barva stiskutého tlačítka

            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;


            // Obrys myši:
            GraphicsPath pathMouse = new GraphicsPath();

            Rectangle arcRect1 = new Rectangle(offset, offset, radius1 * 2, radius1 * 2);
            pathMouse.AddArc(arcRect1, 180, 90); // levý horní

            arcRect1.X = pictureMouse.Width - radius1 * 2 - offset;
            pathMouse.AddArc(arcRect1, 270, 90); // pravý horní

            Rectangle arcRect2 = new Rectangle(pictureMouse.Width - radius2 * 2 - offset, pictureMouse.Height - radius2 * 2 - offset, radius2 * 2, radius2 * 2);
            pathMouse.AddArc(arcRect2, 0, 90); // pravý dolní

            arcRect2.X = offset;
            pathMouse.AddArc(arcRect2, 90, 90); // levý dolní

            pathMouse.CloseFigure();
            g.DrawPath(penBlack, pathMouse);

            // Prostřední tlačítko:
            GraphicsPath pathM = new GraphicsPath();
            pathM.AddLine(new Point((int)(pictureMouse.Width * 0.35), offset), new Point((int)(pictureMouse.Width * 0.65), offset));
            pathM.AddLine(new Point((int)(pictureMouse.Width * 0.65), buttonHeight), new Point((int)(pictureMouse.Width * 0.35), buttonHeight));
            if (mouseMiddle)
                g.FillPath(brushButton, pathM);
            g.DrawPath(penBlack, pathM);

            // Pravé tlačítko:
            GraphicsPath pathR = new GraphicsPath();

            pathR.AddArc(arcRect1, 270, 90);
            pathR.AddLine(new Point(pictureMouse.Width - offset, buttonHeight), new Point((int)(pictureMouse.Width * 0.65), buttonHeight));
            pathR.AddLine(pathR.GetLastPoint(), new Point((int)(pictureMouse.Width * 0.65), offset));
            pathR.CloseFigure();
            if (mouseRight)
                g.FillPath(brushButton, pathR);
            g.DrawPath(penBlack, pathR);

            // Levé tlačítko:
            GraphicsPath pathL = new GraphicsPath();

            arcRect1.X = offset;
            pathL.AddArc(arcRect1, -90, -90);
            pathL.AddLine(new Point(offset, buttonHeight), new Point((int)(pictureMouse.Width * 0.35), buttonHeight));
            pathL.AddLine(pathL.GetLastPoint(), new Point((int)(pictureMouse.Width * 0.35), offset));
            pathL.CloseFigure();
            if (mouseLeft)
                g.FillPath(brushButton, pathL);
            g.DrawPath(penBlack, pathL);



        }

        bool mouseclick = false;
        private void Test_onMouseInput(List<ActiveInput> inputs)
        {
            mouseMiddle = inputs.Exists(x => x.Name == "Middle");
            mouseRight = inputs.Exists(x => x.Name == "Right");
            mouseLeft = inputs.Exists(x => x.Name == "Left");
            pictureMouse.Invalidate();
        }

        private void Test_onChangeInput(List<ActiveInput> inputs)
        {
            foreach (Button btn in buttons)
            {
                btn.Text = "";
                btn.BackColor = Color.White;
            }

            int length = inputs.Count;
            if (length > 3)
                length = 3;

            for (int i = 0; i < length; i++)
            {
                string text = "";
                switch (inputs[i].Name)
                {
                    case "LControlKey":
                        text = "CTRL";
                        break;
                    case "LMenu":
                        text = "L ALT";
                        break;
                    case "LShiftKey":
                        text = "L SHIFT";
                        break;
                    case "RControlKey":
                        text = "CTRL";
                        break;
                    case "RMenu":
                        text = "R ALT";
                        break;
                    case "RShiftKey":
                        text = "R SHIFT";
                        break;
                    case "Return":
                        text = "ENTER";
                        break;
                    case "Capital":
                        text = "Caps Lock";
                        break;
                    default:
                        text = inputs[i].Name;
                        break;
                }

                buttons[i].Text = text;
                buttons[i].BackColor = Color.LightGray;
            }
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            int width = (int)(this.Width * 0.8)/3;
            for (int i = 0; i < 3; i++)
            {
                buttons[i].Width = width;
                buttons[i].Location = new Point(width * i);
                buttons[i].Font = new Font(buttons[i].Font.FontFamily, this.Height / 8, FontStyle.Bold);

            }
            pictureMouse.Width = (int)(this.Width*0.2)-20;
            pictureMouse.Location = new Point(width * 3);
            pictureMouse.Image = null;
            pictureMouse.Invalidate();
        }
    }
}
