using System;
using System.Collections.Generic;


namespace Geostorm.Core
{
    public abstract class GameEvent { }

    public class BulletDestroyedEvent : GameEvent
    {
        public Bullet Bullet { get; set; }

        public BulletDestroyedEvent(in Bullet bullet) { Bullet = bullet; }
    }

     public class EnemyKilledEvent : GameEvent
    {
        public Enemy  Enemy  { get; set; }
        public Bullet Bullet { get; set; }

        public EnemyKilledEvent(in Enemy enemy, in Bullet bullet) { Enemy = enemy; Bullet = bullet; }
    }

    public class GruntSpawnedEvent : GameEvent
    {
        public Grunt Grunt { get; set; }

        public GruntSpawnedEvent(in Grunt grunt) { Grunt = grunt; }
    }

    public class WandererSpawnedEvent : GameEvent
    {
        public Wanderer Wanderer { get; set; }

        public WandererSpawnedEvent(in Wanderer grunt) { Wanderer = grunt; }
    }

    public class RocketSpawnedEvent : GameEvent
    {
        public Rocket Rocket { get; set; }

        public RocketSpawnedEvent(in Rocket rocket) { Rocket = rocket; }
    }

    public class PlayerKilledEvent : GameEvent
    {
        public Player Player { get; set; }
        public Enemy  Enemy  { get; set; }

        public PlayerKilledEvent(in Player player, in Enemy enemy) { Player = player; Enemy  = enemy; }
    }

    public class PlayerShootEvent : GameEvent
    {
        public Bullet Bullet { get; set; }

        public PlayerShootEvent(in Bullet bullet) { Bullet = bullet; }
    }

    public class PlayerBounced : GameEvent { }

    public class PlayerDashed  : GameEvent { }

    public class PlayerLevelUp : GameEvent { }

    public class ParticleDestroyedEvent : GameEvent
    {
        public Particle Particle { get; set; }

        public ParticleDestroyedEvent(in Particle particle)
        {
            Particle = particle;
        }
    }
}
