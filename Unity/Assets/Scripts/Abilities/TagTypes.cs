using System;
using UnityEngine;

namespace Assets.Scripts.Abilities
{
	public interface ICastable
	{
		void Cast();
	}

	namespace Effects
	{
		public interface IBuff
		{
			void ApplyBuff(/*PlayerStats data*/);
		}

		public interface IDefensive : IBuff
		{
			
		}
		public interface IPassive
		{

		}
		public interface IOffensive : IBuff
		{
			
		}
		public interface IPersistent
		{
			void UpdateEffect(/*PlayerStats data*/);
		}
		public interface IInstant
		{
			void ApplyEffect(/*PlayerStats data*/);
		}
		public interface IRemoveable
		{
			bool ToRemove(/*PlayerStats data*/);
		}
		public interface IToggle : IPersistent
		{
			void ToggleEffect();
		}

		public class ColdSnap : IPassive, IPersistent, IRemoveable
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
				if(_timer >= _duration)
				{
					_remove = true;
				}

				throw new NotImplementedException("ColdSnap.UpdateEffect not fully implemented.");
			}
			bool IRemoveable.ToRemove()
			{
				return _remove;
			}
		}
		public class SuperStrength : IPassive, IOffensive
		{
			private readonly float _baseDamageMultiply;

			public SuperStrength(float damageMultiplier)
			{
				_baseDamageMultiply = damageMultiplier;
				Debug.Log(_baseDamageMultiply);
			}

			void IBuff.ApplyBuff(/*PlayerStats data*/)
			{
				throw new NotImplementedException("SuperStrength.Applybuff has not been implemented.");
			}
		}
		public class KineticAbsorption : IPassive, IDefensive
		{
			private readonly float _baseDamageMultiply;

			public KineticAbsorption(float damageMultiply)
			{
				_baseDamageMultiply = damageMultiply;
				Debug.Log(_baseDamageMultiply);
			}

			void IBuff.ApplyBuff(/*PlayerStats data*/)
			{
				throw new NotImplementedException("KineticAbsorption.Applybuff has not been implemented.");
			}
		}
		public class EnergyAbsorption : IPassive, IDefensive
		{
			private readonly float _energyDamageMultiply;

			public EnergyAbsorption(float damageMultiply)
			{
				_energyDamageMultiply = damageMultiply;
				Debug.Log(_energyDamageMultiply);
			}

			void IBuff.ApplyBuff(/*PlayerStats data*/)
			{
				throw new NotImplementedException("EnergyAbsorption.Applybuff has not been implemented.");
			}
		}
		public class ActiveRegeneration : IPassive, IPersistent
		{
			private readonly float _hitPointsPerSecond;
			public ActiveRegeneration(float regenRate)
			{
				_hitPointsPerSecond = regenRate;
				Debug.Log(_hitPointsPerSecond);
			}
			void IPersistent.UpdateEffect()
			{
				throw new NotImplementedException();
			}
		}
	}
}
