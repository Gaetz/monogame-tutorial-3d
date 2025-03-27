using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Tutorial_13
{
    public enum EntityPhase
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
        private const float COLLIDER_SCALE = 4.0f;
        private BoundingBox boundingBox;

        private Vector3 targetPosition = Vector3.Zero;
        private Vector3 velocity = Vector3.Zero;
        private float speed = 300.0f;
        private EntityPhase phase = EntityPhase.Enter;
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
            scale = new Vector3(10f, 10f, 10f);
            ChangePhase(EntityPhase.Enter);
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
                case EntityPhase.Enter:
                    UpdateEnterPhase(dt);
                    break;
                case EntityPhase.Main:
                    UpdateMainPhase(dt);
                    break;
                case EntityPhase.Exit:
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
                ChangePhase(EntityPhase.Main);
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
                ChangePhase(EntityPhase.Exit);
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

        private void ChangePhase(EntityPhase newPhase)
        {
            phase = newPhase;
            switch (phase)
            {
                case EntityPhase.Enter:
                    phase = EntityPhase.Enter;
                    position = GetPositionFromScreenSide(screenSideEnter);
                    break;

                case EntityPhase.Main:
                    phase = EntityPhase.Main;
                    mainPhaseCounter = 0.0f;
                    velocity = Vector3.Zero;
                    shootState = ShootState.Waiting;
                    break;

                case EntityPhase.Exit:
                    phase = EntityPhase.Exit;
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

        public override void Draw(Matrix view, Matrix projection)
        {
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.DiffuseColor = currentFlashColor.ToVector3();
                }
            }
            base.Draw(view, projection);
        }
    }
}
