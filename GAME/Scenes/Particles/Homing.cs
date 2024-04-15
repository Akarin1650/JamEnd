using Com.IsartDigital.Jam.Managers;
using Com.IsartDigital.Jam.MyGameObjects.MyEntities;
using Godot;
using System;

// Author : Baptiste VU LIEM
namespace Com.IsartDigital.Jam
{

    public class Homing : Sprite
    {
        public Vector2 targetPosition;
        public float speed = 1000f;
        private float direction;
        public float moveWeight = .02f;
        private float destroyDistance = 30f;
        private float lifeTime = 0.8f;
        private float timer;

        //Trail
        private Line2D trail;
        private int trailPointmb = 50;
        private Color trailColor = Colors.White;
        private float trailWidth = 10f;
        [Export] private Curve trailWidthCurve;

        //Hit Animation
        [Export] private PackedScene hitAnimation;

        public override void _Ready()
        {
            direction = (float)GD.RandRange(0f, Mathf.Pi * 2f);
            trail = new Line2D();
            trail.DefaultColor = trailColor;
            trail.Width = trailWidth;
            trail.WidthCurve = trailWidthCurve;
            trail.ZIndex = 1;
            GetParent().AddChild(trail);
        }

        public override void _Process(float pDelta)
        {
            direction = Mathf.LerpAngle(direction, GlobalPosition.DirectionTo(targetPosition).Angle(), moveWeight);
            GlobalPosition += Mathf.Polar2Cartesian(speed * pDelta, direction);

            //Trail
            trail.AddPoint(GlobalPosition);
            if (trail.GetPointCount() >= trailPointmb) trail.RemovePoint(0);

            timer += pDelta;
            if (timer >= lifeTime || GlobalPosition.DistanceTo(targetPosition) <= destroyDistance)
            {
                trail.QueueFree();
                QueueFree();
                Particles2D lHitAnimation = (Particles2D)hitAnimation.Instance();
                lHitAnimation.GlobalPosition = GlobalPosition;
                lHitAnimation.Emitting = true;
                GetParent().AddChild(lHitAnimation);

                HitEnemyAnimation();
            }
        }

        private void HitEnemyAnimation()
        {
            //Hit Flash + Scale
            /*Sprite lEnemy = EntityManager.GetInstance().currentEnemySprite;

            if (IsInstanceValid(lEnemy))
            {
                Tween lTween = new Tween();
                lTween.InterpolateProperty(lEnemy, "scale", Vector2.One * 2f, Vector2.One, .5f);
                GetParent().AddChild(lTween);
                lTween.Start();
            }*/
        }
    }
}