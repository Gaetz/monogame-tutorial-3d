using Microsoft.Xna.Framework;
using System;

namespace Tutorial_11
{
    public enum Phase
    {
        Enter,
        Main,
        Exit
    }

    public enum ScreenSide
    {
        Top,
        Bottom,
        Left,
        Right,
        Horizon,
        Back
    }

    public enum ShootState 
    {
        Waiting,
        Shooting,
        Cooldown,
        OutsideMainPhase
    }

    internal class Enemy : Entity
    {
        private const float COLLIDER_SCALE = 30.0f;
        private BoundingBox boundingBox;

        private Vector3 targetPosition = Vector3.Zero;
        private Vector3 velocity = Vector3.Zero;
        private float speed = 300.0f;
        private Phase phase = Phase.Enter;
        private ScreenSide screenSideEnter = ScreenSide.Back;
        private ScreenSide screenSideExit = ScreenSide.Horizon;
        private float mainPhaseDuration = -1.0f;
        private float mainPhaseCounter = 5.0f;
        private int hp = 5;
        private bool isDead = false;

        private Game1 game;
        private ShootState shootState = ShootState.OutsideMainPhase;
        private const float SHOOTING_TIME = 2.0f;
        private const float SHOOTING_INTERVAL = 0.5f;
        private const float SHOOTING_COOLDOWN = 3.0f;
        private int PROJECTILE_NUMBER = 3;
        private float shootingTimer = 0.0f;
        private int projectileCount = 0;

        public BoundingBox BoundingBox
        {
            get { return boundingBox; }
        }

        public bool IsDead
        {
            get { return isDead || hp <= 0; }
        }

        public Enemy(Vector3 position, Game1 game, ScreenSide enterSide, ScreenSide exitSide, float mainPhaseDuration) : base()
        {
            this.game = game;
            this.mainPhaseDuration = mainPhaseDuration;
            targetPosition = position;
            screenSideEnter = enterSide;
            screenSideExit = exitSide;
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
            switch (phase)
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
            switch (shootState)
            {
                case ShootState.Waiting:
                    shootingTimer += (float)dt;
                    if (shootingTimer > SHOOTING_TIME)
                    {
                        shootState = ShootState.Shooting;
                        shootingTimer = 0.0f;
                        projectileCount = 0;
                    }
                    break;
                case ShootState.Shooting:
                    shootingTimer += (float)dt;
                    if (shootingTimer > SHOOTING_INTERVAL)
                    {
                        Vector3 direction = game.Player.Position - position;
                        direction.Normalize();
                        Quaternion directionRotation = Projectile.CreateQuaternionFromDirection(direction);
                        game.AddProjectile(position, directionRotation, 500.0f, false);
                        projectileCount++;
                        shootingTimer = 0.0f;
                    }
                    if (projectileCount >= PROJECTILE_NUMBER)
                    {
                        shootingTimer = SHOOTING_INTERVAL * PROJECTILE_NUMBER;
                        shootState = ShootState.Cooldown;
                    }
                    break;
                case ShootState.Cooldown:
                    shootingTimer += (float)dt;
                    if (shootingTimer > SHOOTING_COOLDOWN)
                    {
                        shootingTimer = 0.0f;
                        shootState = ShootState.Shooting;
                        projectileCount = 0;
                    }
                    break;
                case ShootState.OutsideMainPhase:

                    break;
            }

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
                    shootState = ShootState.Waiting;
                    break;

                case Phase.Exit:
                    phase = Phase.Exit;
                    targetPosition = GetPositionFromScreenSide(screenSideExit);
                    shootState = ShootState.OutsideMainPhase;
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
