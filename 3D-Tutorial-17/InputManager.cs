using Microsoft.Xna.Framework.Input;

namespace Tutorial_17
{
    internal class InputManager
    {
        KeyboardState previousState;
        KeyboardState currentState;
        GamePadState previousGamePadState;
        GamePadState currentGamePadState;
        MouseState previousMouseState;
        MouseState currentMouseState;

        bool isPreviousStateNull = true;
        bool isPreviousGamePadStateNull = true;
        bool isPreviousMouseStateNull = true;

        public void Update(KeyboardState state, GamePadState gamePadState)
        {
            currentState = state;
            currentGamePadState = gamePadState;
        }

        public void EndUpdate()
        {
            previousState = currentState;
            previousGamePadState = currentGamePadState;
            isPreviousStateNull = false;
            isPreviousMouseStateNull = false;
            isPreviousGamePadStateNull = false;
        }

        private bool IsKeyPressed(Keys key)
        {
            return currentState.IsKeyDown(key) 
                && previousState.IsKeyUp(key) 
                && !isPreviousStateNull;
        }

        private bool IsKeyDown(Keys key)
        {
            return currentState.IsKeyDown(key);
        }

        private bool IsButtonPressed(Buttons button)
        {
            return currentGamePadState.IsButtonDown(button) 
                && previousGamePadState.IsButtonUp(button) 
                && !isPreviousGamePadStateNull;
        }

        private bool IsButtonDown(Buttons button)
        {
            return currentGamePadState.IsButtonDown(button);
        }

        private bool IsMouseLeftButtonPressed()
        {
            return currentMouseState.LeftButton == ButtonState.Pressed 
                && previousMouseState.LeftButton == ButtonState.Released 
                && !isPreviousMouseStateNull;
        }

        private bool IsMouseLeftButtonDown()
        {
            return currentMouseState.LeftButton == ButtonState.Pressed;
        }

        public bool IsValidationActionPressed()
        {
            return IsKeyPressed(Keys.Enter) || IsKeyPressed(Keys.Space) 
                || IsButtonPressed(Buttons.A) || IsButtonPressed(Buttons.RightShoulder) 
                || IsButtonPressed(Buttons.RightTrigger);
        }

        public bool IsShootActionPressed()
        {
            return IsMouseLeftButtonPressed() || IsButtonPressed(Buttons.RightTrigger)
                || IsButtonPressed(Buttons.RightShoulder) || IsButtonPressed(Buttons.A)
                || IsKeyPressed(Keys.Space) || IsKeyPressed(Keys.Enter);
        }

        public bool IsUpAction() 
        {
            return IsKeyDown(Keys.W) || IsKeyDown(Keys.Up) || IsKeyDown(Keys.Z)
                || IsButtonDown(Buttons.DPadUp) || IsButtonDown(Buttons.LeftThumbstickUp);
        }

        public bool IsDownAction() 
        {
            return IsKeyDown(Keys.S) || IsKeyDown(Keys.Down) 
                || IsButtonDown(Buttons.DPadDown) || IsButtonDown(Buttons.LeftThumbstickDown);
        }

        public bool IsLeftAction() 
        {
            return IsKeyDown(Keys.A) || IsKeyDown(Keys.Left) || IsKeyDown(Keys.Q)
                || IsButtonDown(Buttons.DPadLeft) || IsButtonDown(Buttons.LeftThumbstickLeft);
        }

        public bool IsRightAction() 
        {
            return IsKeyDown(Keys.D) || IsKeyDown(Keys.Right) 
                || IsButtonDown(Buttons.DPadRight) || IsButtonDown(Buttons.LeftThumbstickRight);
        }
    }
}
