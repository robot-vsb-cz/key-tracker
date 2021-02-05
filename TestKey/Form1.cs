using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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

        public Form1()
        {
            InitializeComponent();

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


        bool mouseclick = false;
        private void Test_onMouseInput(List<ActiveInput> inputs)
        {
            mouseclick = !mouseclick;
            PrintMouse(Color.White, "");
            PrintMouse(Color.White, "Right");
            PrintMouse(Color.White, "Middle");
            for (int i = 0; i < inputs.Count; i++)
                PrintMouse(Color.Green, inputs[i].Name);

        }

        public void PrintMouse(Color color, string btn)
        {
            Pen p = new Pen(Color.Black, -1);
            int h = pictureBox1.Height / 3;
            int button = 0;
            int offset = (int)(pictureBox1.Width / 3);
            if (btn == "Right")
            {
                button = 2;
            }
            if (btn == "Middle")
            {
                button = 1;
            }

            int wpul = (int)(pictureBox1.Width / 2);

            SolidBrush blueBrush = new SolidBrush(color);
            using(Graphics g = pictureBox1.CreateGraphics())
            {

                g.FillRectangle(blueBrush, offset * button, 0, offset, h);

                for (int i = 0; i < 3; i++)
                {
                    g.DrawRectangle(p, offset * i, 0, offset, h);
                }
                g.DrawRectangle(p, 0, 0, pictureBox1.Width - 1, pictureBox1.Height - 1);
            }
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
                        text = "L CTRL";
                        break;
                    case "LMenu":
                        text = "L ALT";
                        break;
                    case "LShiftKey":
                        text = "L SHIFT";
                        break;
                    case "RControlKey":
                        text = "R CTRL";
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
            int width = this.Width / 4;
            for (int i = 0; i < 3; i++)
            {
                buttons[i].Width = width;
                buttons[i].Location = new Point(width * i);
                buttons[i].Font = new Font(buttons[i].Font.FontFamily, this.Height / 8, FontStyle.Bold);

            }
            pictureBox1.Width = width - 20;
            pictureBox1.Location = new Point(width * 3);
            pictureBox1.Image = null;
            pictureBox1.Invalidate();
        }
    }
}
