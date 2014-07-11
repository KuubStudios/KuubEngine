using System;

namespace KuubEngine.Scene.Components {
    public class HealthChangedEventArgs : EventArgs {
        public readonly int Changed;
        public readonly GameObject Entity;

        public bool HasEntity {
            get { return Entity != null; }
        }

        public HealthChangedEventArgs(int changed, GameObject entity) {
            Changed = changed;
            Entity = entity;
        }
    }

    public class KilledEventArgs : EventArgs {
        public readonly GameObject Attacker;

        public bool HasAttacker {
            get { return Attacker != null; }
        }

        public KilledEventArgs(GameObject attacker = null) {
            Attacker = attacker;
        }
    }

    public class Health : Component {
        private int maxHealth;
        private int value;

        public int Value {
            get { return value; }
            private set {
                this.value = value;
                if(this.value > MaxHealth) this.value = MaxHealth;
            }
        }

        public int MaxHealth {
            get { return maxHealth; }
            set {
                maxHealth = value;
                if(Value > maxHealth) Value = value;
            }
        }

        public bool IsAlive { get; private set; }

        public Health(GameObject ent) : base(ent) {
            MaxHealth = 1;
            Value = MaxHealth;

            IsAlive = true;
        }

        public event EventHandler<HealthChangedEventArgs> Healed;
        public event EventHandler<HealthChangedEventArgs> Damaged;
        public event EventHandler<KilledEventArgs> Killed;

        public void Revive(GameObject healer = null) {
            if(IsAlive) return;

            Value = MaxHealth;
            IsAlive = true;

            if(Healed != null) Healed(this, new HealthChangedEventArgs(MaxHealth, healer));
        }

        public void Set(int amount, GameObject entity = null) {
            int diff = amount - Value;

            if(diff > 0) Heal(diff, entity);
            else Damage(-diff, entity);
        }

        public void Heal(int amount, GameObject healer = null) {
            if(amount == 0) return;

            if(amount < 0) {
                Damage(-amount, healer);
                return;
            }

            Value += amount;
            if(Healed != null) Healed(this, new HealthChangedEventArgs(amount, healer));
        }

        public void Damage(int amount, GameObject attacker = null) {
            if(amount == 0) return;

            if(amount < 0) {
                Heal(-amount, attacker);
                return;
            }

            Value -= amount;
            if(Damaged != null) Damaged(this, new HealthChangedEventArgs(amount, attacker));

            if(Value <= 0) {
                Value = 0;
                IsAlive = false;

                if(Killed != null) Killed(this, new KilledEventArgs(attacker));
            }
        }
    }
}