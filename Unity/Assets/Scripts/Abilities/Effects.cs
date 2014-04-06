using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Abilities.Effects
{
	public sealed class AttackPacket
	{
		float _initialPhysical;
		float _initialEnergy;

		//List<IEffect> _layeredEffects

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

	public interface IEffect
	{
		void OnApply(Player.PlayerData data);
		void OnDetach(Player.PlayerData data);
	}
	public interface IDefensiveBuff : IEffect
	{
		void ApplyBuff(AttackPacket attackPacket);
	}
	public interface IOffensiveBuff : IEffect
	{
		void ApplyBuff(AttackPacket attackPacket);
	}
	public interface IUpdateable : IEffect
	{
		void UpdateEffect(Player.PlayerData data);
	}
	public interface IRemoveable : IEffect
	{
		bool ToRemove(Player.PlayerData data);
	}
	public interface IToggle : IUpdateable
	{
		void ToggleEffect();
	}

	public class PhysicalDamage : IEffect
	{
		readonly float _damage;

		public PhysicalDamage(float damage)
		{
			_damage = damage;
			Debug.Log(_damage);
		}

		public AttackPacket GetAttackPacket(Player.PlayerData data)
		{
			return new AttackPacket(_damage);
		}

		public void OnApply(Player.PlayerData data)
		{
			data.Player.AttackPlayer(GetAttackPacket(data));
			throw new NotImplementedException();
		}

		public void OnDetach(Player.PlayerData data)
		{
			throw new NotImplementedException();
		}
	}
	public class EnergyDamage : IEffect
	{
		readonly float _damage;

		public EnergyDamage(float damage)
		{
			_damage = damage;
			Debug.Log(_damage);
		}

		public AttackPacket GetAttackPacket(Player.PlayerData data)
		{
			return new AttackPacket(energyDamage: _damage);
		}

		public void OnApply(Player.PlayerData data)
		{
			throw new NotImplementedException();
		}

		public void OnDetach(Player.PlayerData data)
		{
			throw new NotImplementedException();
		}
	}
	public class EnergyDamagePerSecond : IUpdateable, IRemoveable, IEffect
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

		public void UpdateEffect(Player.PlayerData data)
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

		public bool ToRemove(Player.PlayerData data)
		{
			return _remove;
		}

		public AttackPacket GetAttackPacket(Player.PlayerData data)
		{
			return new AttackPacket(energyDamage: _damageThisFrame);
		}

		public void OnApply(Player.PlayerData data)
		{
			throw new NotImplementedException();
		}

		public void OnDetach(Player.PlayerData data)
		{
			throw new NotImplementedException();
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

		public void UpdateEffect(Player.PlayerData data)
		{
			_timer += Time.deltaTime;
			if (_timer >= _duration)
			{
				_remove = true;
			}

			throw new NotImplementedException("MoveSpeedScale.UpdateEffect not fully implemented.");
		}
		public bool ToRemove(Player.PlayerData data)
		{
			return _remove;
		}

		public void OnApply(Player.PlayerData data)
		{
			throw new NotImplementedException();
		}

		public void OnDetach(Player.PlayerData data)
		{
			throw new NotImplementedException();
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

		public void OnApply(Player.PlayerData data)
		{
			throw new NotImplementedException();
		}

		public void OnDetach(Player.PlayerData data)
		{
			throw new NotImplementedException();
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

		public void OnApply(Player.PlayerData data)
		{
			throw new NotImplementedException();
		}

		public void OnDetach(Player.PlayerData data)
		{
			throw new NotImplementedException();
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

		public void OnApply(Player.PlayerData data)
		{
			throw new NotImplementedException();
		}

		public void OnDetach(Player.PlayerData data)
		{
			throw new NotImplementedException();
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
		public void UpdateEffect(Player.PlayerData data)
		{
			throw new NotImplementedException("HealthRegeneration.UpdateEffect has not been implemented.");
		}

		public void OnApply(Player.PlayerData data)
		{
			throw new NotImplementedException();
		}

		public void OnDetach(Player.PlayerData data)
		{
			throw new NotImplementedException();
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

		public void UpdateEffect(Player.PlayerData data)
		{
			_timer += Time.deltaTime;
			if (_timer >= _duration)
			{
				_remove = true;
			}

			throw new NotImplementedException();
		}

		public bool ToRemove(Player.PlayerData data)
		{
			return _remove;
		}

		public void OnApply(Player.PlayerData data)
		{
			throw new NotImplementedException();
		}

		public void OnDetach(Player.PlayerData data)
		{
			throw new NotImplementedException();
		}
	}
	public class Growth : IEffect
	{
		private readonly float _scaleMultiplier;

		public Growth(float scaleMultiply)
		{
			_scaleMultiplier = scaleMultiply;
			Debug.Log(_scaleMultiplier);
		}

		public void ApplyEffect(Player.PlayerData data)
		{
			//Apply player resize here.
			throw new NotImplementedException("Gigantism.ApplyEffect has not been implemented.");
		}

		public void OnApply(Player.PlayerData data)
		{
			throw new NotImplementedException();
		}

		public void OnDetach(Player.PlayerData data)
		{
			throw new NotImplementedException();
		}
	}
	public class Silence : IUpdateable, IRemoveable
	{
		private readonly float _duration;
		private float _timer = 0.0f;
		private bool _remove = false;

		public Silence(float silenceDuration)
		{
			_duration = silenceDuration;
		}

		public void ApplyEffect(Player.PlayerData data)
		{
			//Apply silence effect to this player.
			throw new NotImplementedException("Silence.ApplyEffect has not been implemented.");
		}
		public void UpdateEffect(Player.PlayerData data)
		{
			_timer += Time.deltaTime;
			if (_timer >= _duration)
			{
				//Reverse the effects of Silence here
				_remove = true;
			}
			throw new NotImplementedException("Silence.UpdateEffect is not fully implemented");
		}
		public bool ToRemove(Player.PlayerData data)
		{
			return _remove;
		}

		public void OnApply(Player.PlayerData data)
		{
			throw new NotImplementedException();
		}

		public void OnDetach(Player.PlayerData data)
		{
			throw new NotImplementedException();
		}
	}
}
