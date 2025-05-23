﻿using Microsoft.Xna.Framework;
using System;

namespace Tutorial_07
{
    enum Phase
    {
        Enter,
        Main,
        Exit
    }

    enum ScreenSide
    {
        Top,
        Bottom,
        Left,
        Right,
        Horizon,
        Back
    }

    internal class Enemy : Entity
    {
        private const float COLLIDER_SCALE = 30.0f;
        private BoundingBox boundingBox;

        private Vector3 targetPosition = Vector3.Zero;
        private Vector3 velocity = Vector3.Zero;
        private float speed = 300.0f;
        private Phase phase = Phase.Enter;
        private ScreenSide screenSideEnter = ScreenSide.Left;
        private ScreenSide screenSideExit = ScreenSide.Right;
        private float mainPhaseDuration = -1.0f;
        private float mainPhaseCounter = 5.0f;
        private int hp = 5;
        bool isDead = false;

        public BoundingBox BoundingBox
        {
            get { return boundingBox; }
        }

        public bool IsDead
        {
            get { return isDead || hp <= 0; }
        }

        public Enemy(Vector3 position) : base()
        {
            this.targetPosition = position;
            mainPhaseDuration = 5.0f;
            scale = new Vector3(2f, 2f, 2f);
            ChangePhase(Phase.Enter);
        }

        private BoundingBox CreateBoundingBox()
        {
            Vector3 min = new Vector3(-0.5f * COLLIDER_SCALE, -0.5f * COLLIDER_SCALE, -0.5f * COLLIDER_SCALE);
            Vector3 max = new Vector3(0.5f * COLLIDER_SCALE, 0.5f * COLLIDER_SCALE, 0.5f * COLLIDER_SCALE);
            min = Vector3.Transform(min, world);
            max = Vector3.Transform(max, world);
            return new BoundingBox(min, max);
        }

        public override void Update(double dt)
        {
            switch(phase)
            {
                case Phase.Enter:
                    UpdateEnterPhase(dt);
                    break;
                case Phase.Main:
                    UpdateMainPhase(dt);
                    break;
                case Phase.Exit:
                    UpdateExitPhase(dt);
                    break;
            }

            if (hp <= 0)
            {
                isDead = true;
            }

            position += velocity * (float)dt;
            base.Update(dt);
            boundingBox = CreateBoundingBox();
        }

        public void SetRandomPosition()
        {
            Random random = new Random();
            position = new Vector3(random.Next(-100, 100), random.Next(-100, 100), -500);
        }

        private void UpdateEnterPhase(double dt)
        {
            MoveToTargetPosition(dt);
            if (Vector3.Distance(position, targetPosition) < 5.0f)
            {
                ChangePhase(Phase.Main);
            }
        }

        private void UpdateMainPhase(double dt)
        {
            mainPhaseCounter += (float)dt;
            if (mainPhaseDuration == -1f) return;
            if (mainPhaseCounter > mainPhaseDuration)
            {
                ChangePhase(Phase.Exit);
            }
        }

        private void UpdateExitPhase(double dt)
        {
            MoveToTargetPosition(dt);
            if (Vector3.Distance(position, targetPosition) < 5.0f)
            {
                isDead = true;
            }
        }

        private void ChangePhase(Phase newPhase)
        {
            phase = newPhase;
            switch (phase)
            {
                case Phase.Enter:
                    phase = Phase.Enter;
                    position = GetPositionFromScreenSide(screenSideEnter);
                    break;
                
                case Phase.Main:
                    phase = Phase.Main;
                    mainPhaseCounter = 0.0f;
                    velocity = Vector3.Zero;
                    break;
                
                case Phase.Exit:
                    phase = Phase.Exit;
                    targetPosition = GetPositionFromScreenSide(screenSideExit);
                    break;
            }
        }

        private Vector3 GetPositionFromScreenSide(ScreenSide side)
        {
            Vector3 position = Vector3.Zero;
            float startZ = -5000;
            switch (side)
            {
                case ScreenSide.Left:
                    position = targetPosition - new Vector3(100 * MathF.Abs(targetPosition.Z) / 100f, 0, 0);
                    break;
                case ScreenSide.Right:
                    position = targetPosition + new Vector3(100 * MathF.Abs(targetPosition.Z) / 100f, 0, 0);
                    break;
                case ScreenSide.Top:
                    position = targetPosition + new Vector3(0, 100 * MathF.Abs(targetPosition.Z) / 100f, 0);
                    break;
                case ScreenSide.Bottom:
                    position = targetPosition - new Vector3(0, 100 * MathF.Abs(targetPosition.Z) / 100f, 0);
                    break;
                case ScreenSide.Horizon:
                    startZ = MathF.Min(targetPosition.Z, -10000 + targetPosition.Z);
                    position = new Vector3(targetPosition.X, targetPosition.Y, startZ);
                    break;
                case ScreenSide.Back:
                    startZ = MathF.Max(1000, 1000 + targetPosition.Z);
                    position = new Vector3(targetPosition.X, targetPosition.Y, startZ);
                    break;
            }
            return position;
        }

        private void MoveToTargetPosition(double dt)
        {
            Vector3 direction = targetPosition - position;
            direction.Normalize();
            velocity = direction * speed;
        }

        public void RemoveHp()
        {
            hp--;
        }
    }
}
