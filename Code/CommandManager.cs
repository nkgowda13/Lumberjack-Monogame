using Lumberjack;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Lumberjack
{
    class CommandManager
    {
        private Dictionary<Keys, GameAction> m_KeyBindings = new Dictionary<Keys, GameAction>();

        private InputListener m_Input;

        public CommandManager()
        {
            m_Input = new InputListener();

            // Register events with the input listener 
            m_Input.OnKeyDown += this.OnKeyDown;
        }
        public void Update()
        {
            // Update polling input listener, everything else is handled by events 
            m_Input.Update();
        }

        public void OnKeyDown(object sender, KeyboardEventArgs e)
        {
            GameAction action = m_KeyBindings[e.Key];

            if (action != null)
            {
                action(eButtonState.DOWN, new Vector2(1.0f));
            }
        }

        public void AddKeyboardBinding(Keys key, GameAction action)
        {
            // Add key to listen for when polling 
            m_Input.AddKey(key);

            // Add the binding to the command map 
            m_KeyBindings.Add(key, action);
        }

        public void RemoveKeyboardBinding(Keys key)
        {
            m_Input.KeyList.Remove(key);

            m_KeyBindings.Remove(key);
        }
    }
}
