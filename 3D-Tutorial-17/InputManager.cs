using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tutorial_17
{
    internal class InputManager
    {
        KeyboardState previousState;
        KeyboardState currentState;
        bool isPreviousStateNull = true;

        public void Update(KeyboardState state)
        {
            currentState = state;
        }

        public void EndUpdate()
        {
            previousState = currentState;
            isPreviousStateNull = false;
        }

        public bool IsKeyPressed(Keys key)
        {
            return currentState.IsKeyDown(key) 
                && previousState.IsKeyUp(key) 
                && !isPreviousStateNull;
        }

        public bool IsKeyDown(Keys key)
        {
            return currentState.IsKeyDown(key);
        }
    }
}
