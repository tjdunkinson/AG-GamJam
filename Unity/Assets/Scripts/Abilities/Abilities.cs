using System;
using UnityEngine;

namespace Assets.Scripts.Abilities
{
	public interface IAbility { }
	public interface ICastable : IAbility
	{
		void Cast(/*PlayerStats data*/);
	}

	public interface ICooldown : ICastable
	{
		bool IsCastable();
		void UpdateCooldown();
	}

	public class ColdSnapAbility : ICooldown, ICastable
	{
		private readonly float _cooldown;
		private float _timer = 0.0f;
		private readonly float _range;
		private readonly Effects.IEffect[] _effects;

		public ColdSnapAbility(float cooldown, float range, params Effects.IEffect[] effects)
		{
			_cooldown = cooldown;
			_range = range;
			_effects = effects;
			Debug.Log(_range);
			Debug.Log(_effects);
		}

		bool ICooldown.IsCastable()
		{
			return _timer >= _cooldown;
		}
		void ICooldown.UpdateCooldown()
		{
			_timer += Time.deltaTime;
		}

		void ICastable.Cast(/*PlayerStats data*/)
		{
			//Get enemy positions from PlayerStats and do range checking.
			//Call some apply function on all players within range and give them
			//the ability's effects.
			_timer = 0.0f;
			throw new NotImplementedException();
		}
	}

	namespace Effects
	{
		public interface IEffect { }
		public interface IDefensiveBuff : IEffect
		{
			void ApplyBuff(/*PlayerOffensiveData data*/);
		}
		public interface IOffensiveBuff : IEffect
		{
			void ApplyBuff(/*PlayerOffensiveData data*/);
		}
		public interface IPersistent : IEffect
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
		public interface IToggle : IPersistent
		{
			void ToggleEffect();
		}


		public class Burn : IPersistent, IRemoveable
		{
			private readonly float _damagePerSecond;
			private readonly float _duration;
			private float _timer = 0.0f;
			private bool _remove = false;

			public Burn(float burnDamagePerSecond, float burnDuration)
			{
				_damagePerSecond = burnDamagePerSecond;
				_duration = burnDuration;
				Debug.Log(_damagePerSecond);
			}

			void IPersistent.UpdateEffect()
			{
				_timer += Time.deltaTime;
				if (_timer >= _duration)
				{
					_remove = true;
				}

				//Apply damage here.
				throw new NotImplementedException();
			}

			bool IRemoveable.ToRemove()
			{
				return _remove;
			}
		}
		public class ColdSnap : IPersistent, IRemoveable
		{
			private bool _remove = false;
			private readonly float _duration;
			private readonly float _moveMultiply;
			private float _timer = 0.0f;

			public ColdSnap(float freezeDuration, float movementMultiplier)
			{
				_duration = freezeDuration;
				_moveMultiply = movementMultiplier;
				Debug.Log(_moveMultiply);
			}

			void IPersistent.UpdateEffect(/*PlayerStats data*/)
			{
				_timer += Time.deltaTime;
				if (_timer >= _duration)
				{
					_remove = true;
				}

				throw new NotImplementedException("ColdSnap.UpdateEffect not fully implemented.");
			}
			bool IRemoveable.ToRemove(/*PlayerStats data*/)
			{
				return _remove;
			}
		}
		public class SuperStrength : IOffensiveBuff
		{
			private readonly float _baseDamageMultiply;

			public SuperStrength(float damageMultiplier)
			{
				_baseDamageMultiply = damageMultiplier;
				Debug.Log(_baseDamageMultiply);
			}

			void IOffensiveBuff.ApplyBuff(/*PlayerOffensiveData data*/)
			{
				throw new NotImplementedException("SuperStrength.Applybuff has not been implemented.");
			}
		}
		public class KineticAbsorption : IDefensiveBuff
		{
			private readonly float _baseDamageMultiply;

			public KineticAbsorption(float damageMultiply)
			{
				_baseDamageMultiply = damageMultiply;
				Debug.Log(_baseDamageMultiply);
			}

			void IDefensiveBuff.ApplyBuff(/*PlayerOffensiveData data*/)
			{
				throw new NotImplementedException("KineticAbsorption.Applybuff has not been implemented.");
			}
		}
		public class EnergyAbsorption : IDefensiveBuff
		{
			private readonly float _energyDamageMultiply;

			public EnergyAbsorption(float damageMultiply)
			{
				_energyDamageMultiply = damageMultiply;
				Debug.Log(_energyDamageMultiply);
			}

			void IDefensiveBuff.ApplyBuff(/*PlayerOffensiveData data*/)
			{
				throw new NotImplementedException("EnergyAbsorption.Applybuff has not been implemented.");
			}
		}
		public class ActiveRegeneration : IPersistent
		{
			private readonly float _hitPointsPerSecond;

			public ActiveRegeneration(float regenRate)
			{
				_hitPointsPerSecond = regenRate;
				Debug.Log(_hitPointsPerSecond);
			}
			void IPersistent.UpdateEffect(/*PlayerStats data*/)
			{
				throw new NotImplementedException("ActiveRegeneration.UpdateEffect has not been implemented.");
			}
		}
		public class SuperSonicSpeed : IPersistent, IRemoveable
		{
			private readonly float _attackSpeedMultiply;
			private readonly float _movementSpeedMultiply;
			private readonly float _duration;
			private float _timer = 0.0f;
			private bool _remove = false;

			public SuperSonicSpeed(float attackSpeedMul,
				float movementSpeedMul,
				float effectDuration)
			{
				_attackSpeedMultiply = attackSpeedMul;
				_movementSpeedMultiply = movementSpeedMul;
				_duration = effectDuration;

				Debug.Log(_attackSpeedMultiply);
				Debug.Log(_movementSpeedMultiply);
			}

			void IPersistent.UpdateEffect(/*PlayerStats data*/)
			{
				_timer += Time.deltaTime;
				if (_timer >= _duration)
				{
					_remove = true;
				}

				throw new NotImplementedException();
			}

			bool IRemoveable.ToRemove(/*PlayerStats data*/)
			{
				return _remove;
			}
		}
		public class Gigantism : IOffensiveBuff, IDefensiveBuff, IPersistent, IInstant
		{
			private readonly float _baseDamageMultiplier;
			private readonly float _baseDamageDefenceMultiplier;
			private readonly float _movementSpeedMultiplier;
			private readonly float _scaleMultiplier;

			public Gigantism(float baseDamageIncreaseMultiply,
				float baseDamageDecreaseMultiply, float movementSpeedMultiply,
				float scaleMultiply)
			{
				_baseDamageMultiplier = baseDamageIncreaseMultiply;
				_baseDamageDefenceMultiplier = baseDamageDecreaseMultiply;
				_movementSpeedMultiplier = movementSpeedMultiply;
				_scaleMultiplier = scaleMultiply;

				Debug.Log(_baseDamageMultiplier);
				Debug.Log(_baseDamageDefenceMultiplier);
				Debug.Log(_movementSpeedMultiplier);
				Debug.Log(_scaleMultiplier);
			}

			void IOffensiveBuff.ApplyBuff(/*PlayerOffensiveData data*/)
			{
				//Apply damage offensive multiplier here.
				throw new NotImplementedException("Gigantism.IOffensiveBuff.ApplyBuff has not been implemented.");
			}
			void IDefensiveBuff.ApplyBuff(/*PlayerOffensiveData data*/)
			{
				//Apply damage defensive multiplier here.
				throw new NotImplementedException("Gigantism.IDefensiveBuff.ApplyBuff has not been implemented.");
			}

			void IPersistent.UpdateEffect(/*PlayerStats data*/)
			{
				//Apply movement speed multiplier here.
				throw new NotImplementedException("Gigantism.UpdateEffect has not been implemented.");
			}

			void IInstant.ApplyEffect(/*PlayerStats data*/)
			{
				//Apply player resize here.
				throw new NotImplementedException("Gigantism.ApplyEffect has not been implemented.");
			}
		}
	}
}
