using System;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Abilities.Effects;

namespace Assets.Scripts.Abilities
{
	public interface IAbility { }
	public interface IApplyImmediate
	{
		void ApplyAbility(Player.PlayerData data);
	}
	public interface ICastable : IAbility
	{
		void Cast(Player.PlayerData data);
	}
	public interface IPersistent
	{
		void UpdateAbility(Player.PlayerData data);
		bool IsActive
		{
			get;
		}
	}
	public interface ICooldown : ICastable
	{
		bool IsCastable();
		void UpdateCooldown();

		float CooldownTime { get; }
	}

	public class FireBall : ICooldown, ICastable
	{
		[RequireComponent(typeof(Rigidbody), typeof(Collider))]
		public sealed class FireBallProjectile : MonoBehaviour
		{
			public Vector3 InitialVelocity
			{
				get;
				set;
			}
			public GameObject CastingPlayer
			{
				get;
				set;
			}

			void Start()
			{
				GetComponent<Rigidbody>().velocity = InitialVelocity;
			}

			void OnCollisionEnter(Collision other)
			{
				GameObject otherObject = other.gameObject;
				if (otherObject.CompareTag(""/*Player tag here*/) && otherObject != CastingPlayer)
				{
					//Apply fireball effects to opposing player
				}
				throw new NotImplementedException();
			}
		}

		readonly float _cooldown;
		float _timer = 0.0f;
		readonly GameObject _fireBall;

		public float CooldownTime
		{
			get { return _cooldown - _timer; }
		}

		public FireBall(float cooldown, GameObject fireBall)
		{
			_cooldown = cooldown;
			_fireBall = fireBall;
			_timer = _cooldown;
			Debug.Log(_fireBall);
		}

		public bool IsCastable()
		{
			return _timer >= _cooldown;
		}

		public void UpdateCooldown()
		{
			_timer += Time.deltaTime;
		}

		public void Cast(Player.PlayerData data)
		{
			//Get enemy positions from PlayerStats and do range checking.
			//Call some apply function on all players within range and give them
			//the ability's effects.
			_timer = 0.0f;
			throw new NotImplementedException();
		}
	}
	public class Laser : ICooldown, ICastable
	{
		readonly float _cooldown;
		float _timer = 0.0f;
		readonly float _range;

		public float CooldownTime
		{
			get { return _cooldown - _timer; }
		}

		public Laser(float cooldown, float range)
		{
			_cooldown = cooldown;
			_range = range;
			Debug.Log(_range);
		}

		public bool IsCastable()
		{
			return _timer >= _cooldown;
		}

		public void UpdateCooldown()
		{
			_timer += Time.deltaTime;
		}

		public void Cast(Player.PlayerData data)
		{
			//Get enemy positions from PlayerStats and do range checking.
			//Call some apply function on all players within range and give them
			//the ability's effects.
			_timer = 0.0f;
			throw new NotImplementedException();
		}
	}
	public class ColdSnap : ICooldown, ICastable
	{
		const float _cooldown = 10.0f;
		float _timer = 0.0f;
		const float _range = 5.0f;

		public bool IsCastable()
		{
			return _timer >= _cooldown;
		}
		public void UpdateCooldown()
		{
			_timer += Time.deltaTime;
		}

		public void Cast(Player.PlayerData data)
		{
			//Get enemy positions from PlayerStats and do range checking.
			//Call some apply function on all players within range and give them
			//the ability's effects.
			_timer = 0.0f;
			throw new NotImplementedException();
		}

		public float CooldownTime
		{
			get { return _cooldown - _timer; }
		}
	}
    public class Replusion : ICooldown, ICastable
    {
		readonly float _cooldown;
		float _timer = 0.0f;
		readonly float _range;

        public Replusion(float cooldown, float range)
        {
            _cooldown = cooldown;
            _range = range;
			Debug.Log(_range);
        }

		public bool IsCastable()
        {
            return _timer >= _cooldown;
        }

		public void UpdateCooldown()
        {
            _timer += Time.deltaTime;
        }

		public void Cast(Player.PlayerData data)
        {
            //Get enemy positions from PlayerStats and do range checking.
            //Call some apply function on all players within range and give them
            //the ability's effects.
            _timer = 0.0f;
            throw new NotImplementedException();
        }

		public float CooldownTime
		{
			get { return _cooldown - _timer; }
		}
	}
    public class IncrediLeap : ICooldown, ICastable
    {
        readonly float _cooldown;
        float _timer = 0.0f;
        readonly float _range;

        public IncrediLeap(float cooldown, float range)
        {
            _cooldown = cooldown;
            _range = range;
			Debug.Log(_range);
        }

		public bool IsCastable()
        {
            return _timer >= _cooldown;
        }

		public void UpdateCooldown()
        {
            _timer += Time.deltaTime;
        }

		public void Cast(Player.PlayerData data)
        {
            //Get enemy positions from PlayerStats and do range checking.
            //Call some apply function on all players within range and give them
            //the ability's effects.
            _timer = 0.0f;
            throw new NotImplementedException();
        }

		public float CooldownTime
		{
			get { return _cooldown - _timer; }
		}
	}
    public class Terraform : ICooldown, ICastable
    {
        readonly float _cooldown;
        float _timer = 0.0f;
        readonly float _range;

        public Terraform(float cooldown, float range)
        {
            _cooldown = cooldown;
            _range = range;
			Debug.Log(_range);
        }

		public bool IsCastable()
        {
            return _timer >= _cooldown;
        }

		public void UpdateCooldown()
        {
            _timer += Time.deltaTime;
        }

		public void Cast(Player.PlayerData data)
        {
            //Get enemy positions from PlayerStats and do range checking.
            //Call some apply function on all players within range and give them
            //the ability's effects.
            _timer = 0.0f;
            throw new NotImplementedException();
        }

		public float CooldownTime
		{
			get { return _cooldown - _timer; }
		}
	}
	public class SelfImmolation : ICastable, IPersistent
    {
        readonly float _range;
		bool _active = false;

		const float DPS = 2;
		ToggleEnergyDPS _selfDamage;
		//Dictionary<Player, ToggleEnergyDPS>

        public SelfImmolation(float range)
        {
			_selfDamage = new ToggleEnergyDPS(DPS, float.PositiveInfinity);
            _range = range;
			Debug.Log(_range);
        }

		public void Cast(Player.PlayerData data)
        {
			if(_active)
			{
				//Loop through all damage effects still active and remove them.
				_selfDamage.ToggleEffect();
			}
			else
			{
				_selfDamage.ToggleEffect();
				//Add damage effect to self here.
			}

			_active = !_active;
            throw new NotImplementedException();
        }

		public void UpdateAbility(Player.PlayerData data)
		{
			//Loop through all other players, apply a damage effect to all
			//players within range.
			throw new NotImplementedException();
		}

		public bool IsActive
		{
			get { return _active; }
		}
	}

	public class SuperStrength : IAbility, IApplyImmediate
	{
		const float _damageIncrease = 2.0f;

		public void ApplyAbility(Player.PlayerData data)
		{
			//Apply damage buff to player.
			throw new NotImplementedException();
		}
	}
	public class KineticAbsorption : IAbility, IApplyImmediate
	{
		const float _damageMultiplier = 0.75f;
		public void ApplyAbility(Player.PlayerData data)
		{
			//Apply the defensive buff to this player.
			throw new NotImplementedException();
		}
	}
	public class EnergyAbsorption : IAbility, IApplyImmediate
	{
		const float _damageMultiplier = 0.75f;
		public void ApplyAbility(Player.PlayerData data)
		{
			//Apply the defensive buff to this player.
			throw new NotImplementedException();
		}
	}
	public class ActiveRegeneration : IAbility, IApplyImmediate
	{
		const float _healthRegenPerSecond = 0.2f;

		public void ApplyAbility(Player.PlayerData data)
		{
			//Apply regeneration effect to this player.
			throw new NotImplementedException();
		}
	}
	public class Gigantism : IAbility, IApplyImmediate
	{
		const float _damageIncrease = 1.15f;
		const float _damageMultiplier = 0.9f;
		const float _sizeMultiplier = 2.0f;

		void IApplyImmediate.ApplyAbility(Player.PlayerData data)
		{
			//Apply size, physical damage and defensive effects to this player.
			throw new NotImplementedException();
		}
	}
}
