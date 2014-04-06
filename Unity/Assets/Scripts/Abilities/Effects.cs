﻿using System;
using UnityEngine;

namespace Assets.Scripts.Abilities.Effects
{
	public sealed class AttackPacket
	{
		float _initialPhysical;
		float _initialEnergy;

		public float InitialPhysicalDamage
		{
			get { return _initialPhysical; }
		}
		public float InitialEnergyDamage
		{
			get { return _initialEnergy; }
		}

		public float PhysicalDamage
		{
			get;
			set;
		}
		public float EnergyDamage
		{
			get;
			set;
		}
		public IEffect[] Effects
		{
			get;
			set;
		}

		//Attacking player script

		public AttackPacket(float physicalDamage = 0.0f, float energyDamage = 0.0f, params IEffect[] effects)
		{
			_initialPhysical = physicalDamage;
			_initialEnergy = energyDamage;

			PhysicalDamage = _initialPhysical;
			EnergyDamage = _initialEnergy;
			Effects = effects;
		}
	}

	public interface IEffect { }
	public interface IDefensiveBuff : IEffect
	{
		void ApplyBuff(AttackPacket attackPacket);
	}
	public interface IOffensiveBuff : IEffect
	{
		void ApplyBuff(AttackPacket attackPacket);
	}
	public interface IDamaging : IEffect
	{
		AttackPacket GetAttackPacket(/*PlayerStats data*/);
	}
	public interface IUpdateable : IEffect
	{
		void UpdateEffect(/*PlayerStats data*/);
	}
	public interface IInstant : IEffect
	{
		void ApplyEffect(/*PlayerStats data*/);
	}
	public interface IRemoveable : IEffect
	{
		bool ToRemove(/*PlayerStats data*/);
	}
	public interface IToggle : IUpdateable
	{
		void ToggleEffect();
	}

	public class PhysicalDamage : IInstant, IDamaging
	{
		readonly float _damage;

		public PhysicalDamage(float damage)
		{
			_damage = damage;
			Debug.Log(_damage);
		}

		public void ApplyEffect(/*PlayerStats data*/)
		{
			//Apply a physical damage attack packet to the provided player.
			throw new NotImplementedException();
		}

		public AttackPacket GetAttackPacket(/*PlayerStats data*/)
		{
			return new AttackPacket(_damage);
		}
	}
	public class EnergyDamage : IInstant, IDamaging
	{
		readonly float _damage;

		public EnergyDamage(float damage)
		{
			_damage = damage;
			Debug.Log(_damage);
		}

		public void ApplyEffect(/*PlayerStats data*/)
		{
			//Apply a physical damage attack packet to the provided player.
			throw new NotImplementedException();
		}

		public AttackPacket GetAttackPacket(/*PlayerStats data*/)
		{
			return new AttackPacket(energyDamage: _damage);
		}
	}
	public class EnergyDamagePerSecond : IUpdateable, IRemoveable, IDamaging
	{
		readonly float _damagePerSecond;
		readonly float _duration;
		float _timer = 0.0f;
		protected bool _remove = false;
		float _damageThisFrame;

		public EnergyDamagePerSecond(float burnDamagePerSecond, float burnDuration)
		{
			_damagePerSecond = burnDamagePerSecond;
			_duration = burnDuration;
			Debug.Log(_damagePerSecond);
		}

		public void UpdateEffect(/*PlayerStats data*/)
		{
			float dt = Time.deltaTime;
			_timer += dt;
			if (_timer >= _duration)
			{
				_remove = true;
			}
			_damageThisFrame = _damagePerSecond * dt;

			//Retrieve attack packet for this frame and apply it to the provided player.
			throw new NotImplementedException();
		}

		public bool ToRemove()
		{
			return _remove;
		}

		public AttackPacket GetAttackPacket(/*PlayerStats data*/)
		{
			return new AttackPacket(energyDamage: _damageThisFrame);
		}
	}
	public class ToggleEnergyDPS : EnergyDamagePerSecond, IToggle
	{
		public ToggleEnergyDPS(float burnDamagePerSecond, float burnDuration)
			: base(burnDamagePerSecond, burnDuration)
		{}

		public void ToggleEffect()
		{
			_remove = !_remove;
		}
	}

	public class MoveSpeedScale : IUpdateable, IRemoveable
	{
		private bool _remove = false;
		private readonly float _duration;
		private readonly float _moveMultiply;
		private float _timer = 0.0f;

		public MoveSpeedScale(float freezeDuration, float movementMultiplier)
		{
			_duration = freezeDuration;
			_moveMultiply = movementMultiplier;
			Debug.Log(_moveMultiply);
		}

		public void UpdateEffect(/*PlayerStats data*/)
		{
			_timer += Time.deltaTime;
			if (_timer >= _duration)
			{
				_remove = true;
			}

			throw new NotImplementedException("MoveSpeedScale.UpdateEffect not fully implemented.");
		}
		public bool ToRemove(/*PlayerStats data*/)
		{
			return _remove;
		}
	}
	public class PhysicalAttackBuff : IOffensiveBuff
	{
		private readonly float _baseDamageMultiply;

		public PhysicalAttackBuff(float damageMultiplier)
		{
			_baseDamageMultiply = damageMultiplier;
			Debug.Log(_baseDamageMultiply);
		}

		public void ApplyBuff(AttackPacket attackPacket)
		{
			throw new NotImplementedException("PhysicalAttackBuff.Applybuff has not been implemented.");
		}
	}
	public class PhysicalDefenseBuff : IDefensiveBuff
	{
		private readonly float _baseDamageMultiply;

		public PhysicalDefenseBuff(float damageMultiply)
		{
			_baseDamageMultiply = damageMultiply;
			Debug.Log(_baseDamageMultiply);
		}

		public void ApplyBuff(AttackPacket attackPacket)
		{
			throw new NotImplementedException("PhysicalDefenseBuff.Applybuff has not been implemented.");
		}
	}
	public class EnergyDefenseBuff : IDefensiveBuff
	{
		private readonly float _energyDamageMultiply;

		public EnergyDefenseBuff(float damageMultiply)
		{
			_energyDamageMultiply = damageMultiply;
			Debug.Log(_energyDamageMultiply);
		}

		public void ApplyBuff(AttackPacket attackPacket)
		{
			throw new NotImplementedException("EnergyDefenseBuff.Applybuff has not been implemented.");
		}
	}
	public class HealthRegeneration : IUpdateable
	{
		private readonly float _hitPointsPerSecond;

		public HealthRegeneration(float regenRate)
		{
			_hitPointsPerSecond = regenRate;
			Debug.Log(_hitPointsPerSecond);
		}
		public void UpdateEffect(/*PlayerStats data*/)
		{
			throw new NotImplementedException("HealthRegeneration.UpdateEffect has not been implemented.");
		}
	}
	public class AttackSpeedScale : IUpdateable, IRemoveable
	{
		private readonly float _attackSpeedMultiply;
		private readonly float _duration;
		private float _timer = 0.0f;
		private bool _remove = false;

		public AttackSpeedScale(float attackSpeedMul,
			float effectDuration)
		{
			_attackSpeedMultiply = attackSpeedMul;
			_duration = effectDuration;

			Debug.Log(_attackSpeedMultiply);
		}

		public void UpdateEffect(/*PlayerStats data*/)
		{
			_timer += Time.deltaTime;
			if (_timer >= _duration)
			{
				_remove = true;
			}

			throw new NotImplementedException();
		}

		public bool ToRemove(/*PlayerStats data*/)
		{
			return _remove;
		}
	}
	public class Growth : IInstant
	{
		private readonly float _scaleMultiplier;

		public Growth(float scaleMultiply)
		{
			_scaleMultiplier = scaleMultiply;
			Debug.Log(_scaleMultiplier);
		}

		public void ApplyEffect(/*PlayerStats data*/)
		{
			//Apply player resize here.
			throw new NotImplementedException("Gigantism.ApplyEffect has not been implemented.");
		}
	}
	public class Silence : IUpdateable, IRemoveable, IInstant
	{
		private readonly float _duration;
		private float _timer = 0.0f;
		private bool _remove = false;

		public Silence(float silenceDuration)
		{
			_duration = silenceDuration;
		}

		public void ApplyEffect(/*PlayerStats data*/)
		{
			//Apply silence effect to this player.
			throw new NotImplementedException("Silence.ApplyEffect has not been implemented.");
		}
		public void UpdateEffect(/*PlayerStats data*/)
		{
			_timer += Time.deltaTime;
			if (_timer >= _duration)
			{
				//Reverse the effects of Silence here
				_remove = true;
			}
			throw new NotImplementedException("Silence.UpdateEffect is not fully implemented");
		}
		public bool ToRemove(/*PlayerStats data*/)
		{
			return _remove;
		}
	}
}
