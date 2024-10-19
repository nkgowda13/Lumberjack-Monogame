using System;
using System.Collections.Generic;
using Lumberjack;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Lumberjack
{
    class InputListener
    {
        // Current and previous keyboard states 
        private KeyboardState PrevKeyboardState { get; set; }
        private KeyboardState CurrentKeyboardState { get; set; }

        //Keyboard event handlers 
        //Key is down 
        public event EventHandler<KeyboardEventArgs> OnKeyDown = delegate { };

        // List of keys to check for 
        public HashSet<Keys> KeyList;

        public InputListener()
        {
            CurrentKeyboardState = Keyboard.GetState();
            PrevKeyboardState = CurrentKeyboardState;
            KeyList = new HashSet<Keys>();
        }

        public void AddKey(Keys key)
        {
            KeyList.Add(key);
        }

        public void Update()
        {
            PrevKeyboardState = CurrentKeyboardState;
            CurrentKeyboardState = Keyboard.GetState();

            FireKeyboardEvents();
        }

        private void FireKeyboardEvents()
        {
            // Check through each key in the key list 
            foreach (Keys key in KeyList)
            {
                // Is the key pressed once? 
                if (CurrentKeyboardState.IsKeyDown(key) && !PrevKeyboardState.IsKeyDown(key))
                {
                    // Fire the OnKeyDown event 
                    if (OnKeyDown != null)
                        OnKeyDown(this, new KeyboardEventArgs(key, CurrentKeyboardState, PrevKeyboardState));
                }
            }
        }
    }
}
