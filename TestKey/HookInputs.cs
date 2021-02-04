using Gma.System.MouseKeyHook;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestKey
{
    class HookInputs
    {

        List<ActiveInput> activeInputs = new List<ActiveInput>();
        List<ActiveInput> mouseInputs = new List<ActiveInput>();

        public delegate void dInputs(List<ActiveInput> inputs);
        public event dInputs onChangeInput;
        public event dInputs onMouseInput;

        private IKeyboardMouseEvents _globalHook;

        public void Subscribe()
        {
            if (_globalHook == null)
            {

                _globalHook = Hook.GlobalEvents();
                _globalHook.KeyDown += _globalHook_KeyDown;
                _globalHook.KeyUp += _globalHook_KeyUp;
                _globalHook.MouseDown += _globalHook_MouseDown;
                _globalHook.MouseUp += _globalHook_MouseUp;
            }
        }

        private void _globalHook_MouseUp(object sender, MouseEventArgs e)
        {
            int index = -1;
            for(int i = 0; i < mouseInputs.Count; i++)
            {
                if (e.Button.ToString() ==mouseInputs[i].Name)
                {
                    index = i;
                    break;
                }
            }
            if (index >= 0)
            {
                mouseInputs.Remove(mouseInputs[index]);
            }
            
            if (onMouseInput != null)
            {
                onMouseInput(mouseInputs);
            }
        }

        private void _globalHook_MouseDown(object sender, MouseEventArgs e)
        {
            mouseInputs.Add(new ActiveInput(e.Button.ToString()));
            if (onMouseInput != null)
            {
                onMouseInput(mouseInputs);
            }
        }

        private void _globalHook_KeyUp(object sender, KeyEventArgs e)
        {

            int newInput = -1;
            for (int i = 0; i < activeInputs.Count; i++)
            {
                if (activeInputs[i].Name == e.KeyCode.ToString())
                {
                    newInput = i;
                    break;
                }
            }
            if (newInput != -1)
            {
                activeInputs.Remove(activeInputs[newInput]);
                if (onChangeInput != null)
                {
                    onChangeInput(activeInputs);
                }
            }
        }

        private void _globalHook_KeyDown(object sender, KeyEventArgs e)
        {
            bool newInput = true;
            foreach (ActiveInput item in activeInputs)
            {
                if (item.Name == e.KeyCode.ToString())
                {
                    newInput = false;
                    break;
                }
            }
            if (newInput)
            {

                activeInputs.Add(new ActiveInput(e.KeyCode.ToString()));
                if (onChangeInput != null)
                {
                    onChangeInput(activeInputs);
                }

            }

        }

    }
}
