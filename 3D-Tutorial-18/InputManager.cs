using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Tutorial_18
{
    internal class InputManager
    {
        KeyboardState previousKeyboardState;
        KeyboardState currentKeyboardState;
        GamePadState previousGamePadState;
        GamePadState currentGamePadState;
        MouseState previousMouseState;
        MouseState currentMouseState;

        bool isPreviousStateNull = true;
        bool isPreviousGamePadStateNull = true;
        bool isPreviousMouseStateNull = true;

        public void Update()
        {
            KeyboardState keyboardState = Keyboard.GetState();
            GamePadState gamePadState = GamePad.GetState(PlayerIndex.One);
            MouseState mouseState = Mouse.GetState();
            currentKeyboardState = keyboardState;
            currentGamePadState = gamePadState;
            currentMouseState = mouseState;
        }

        public void EndUpdate()
        {
            previousKeyboardState = currentKeyboardState;
            previousGamePadState = currentGamePadState;
            previousMouseState = currentMouseState;
            isPreviousStateNull = false;
            isPreviousMouseStateNull = false;
            isPreviousGamePadStateNull = false;
        }

        private bool IsKeyPressed(Keys key)
        {
            return currentKeyboardState.IsKeyDown(key) 
                && previousKeyboardState.IsKeyUp(key) 
                && !isPreviousStateNull;
        }

        private bool IsKeyDown(Keys key)
        {
            return currentKeyboardState.IsKeyDown(key);
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

        public bool IsUpActionDown() 
        {
            return IsKeyDown(Keys.W) || IsKeyDown(Keys.Up) || IsKeyDown(Keys.Z)
                || IsButtonDown(Buttons.DPadUp) || IsButtonDown(Buttons.LeftThumbstickUp);
        }

        public bool IsDownActionDown() 
        {
            return IsKeyDown(Keys.S) || IsKeyDown(Keys.Down) 
                || IsButtonDown(Buttons.DPadDown) || IsButtonDown(Buttons.LeftThumbstickDown);
        }

        public bool IsLeftActionDown() 
        {
            return IsKeyDown(Keys.A) || IsKeyDown(Keys.Left) || IsKeyDown(Keys.Q)
                || IsButtonDown(Buttons.DPadLeft) || IsButtonDown(Buttons.LeftThumbstickLeft);
        }

        public bool IsRightActionDown() 
        {
            return IsKeyDown(Keys.D) || IsKeyDown(Keys.Right) 
                || IsButtonDown(Buttons.DPadRight) || IsButtonDown(Buttons.LeftThumbstickRight);
        }

        public bool IsUpActionPressed()
        {
            return IsKeyPressed(Keys.W) || IsKeyPressed(Keys.Up) || IsKeyPressed(Keys.Z)
                || IsButtonPressed(Buttons.DPadUp) || IsButtonPressed(Buttons.LeftThumbstickUp);
        }

        public bool IsDownActionPressed()
        {
            return IsKeyPressed(Keys.S) || IsKeyPressed(Keys.Down)
                || IsButtonPressed(Buttons.DPadDown) || IsButtonPressed(Buttons.LeftThumbstickDown);
        }

        public bool IsLeftActionPressed()
        {
            return IsKeyPressed(Keys.A) || IsKeyPressed(Keys.Left) || IsKeyPressed(Keys.Q)
                || IsButtonPressed(Buttons.DPadLeft) || IsButtonPressed(Buttons.LeftThumbstickLeft);
        }

        public bool IsRightActionPressed()
        {
            return IsKeyPressed(Keys.D) || IsKeyPressed(Keys.Right)
                || IsButtonPressed(Buttons.DPadRight) || IsButtonPressed(Buttons.LeftThumbstickRight);
        }
    }
}
