using Assets.Scripts.Abilities;
using Assets.Scripts.Abilities.Effects;
using System.Collections.Generic;
using UnityEngine;

public partial class Player
{
	public sealed class PlayerData
	{
		Player _thisPlayer;
		//Provide access to movement speed, aim direction, etc.

		public Player Player
		{
			get { return _thisPlayer; }
		}

		public PlayerData(Player playerScript)
		{
			_thisPlayer = playerScript;
		}
	}

	const int _totalAbilities = 4;
	IAbility[] _playerAbilities = new IAbility[_totalAbilities];
	ICooldown[] _cooldownAbilities = new ICooldown[_totalAbilities];
	IPersistent[] _persistentAbilities = new IPersistent[_totalAbilities];
	bool[] _isCastable = new bool[_totalAbilities];

	List<IDefensiveBuff> _defensiveEffects = new List<IDefensiveBuff>();
	List<IOffensiveBuff> _offensiveEffects = new List<IOffensiveBuff>();
	List<IUpdateable> _updateableEffects = new List<IUpdateable>();
	List<IRemoveable> _removableEffects = new List<IRemoveable>();
	List<AttackPacket> _attacksOnSelf = new List<AttackPacket>();

	PlayerData _mydata;

	void InitAbilitiesData()
	{
		_mydata = new PlayerData(this);
	}

	public void AssignAbilities(params IAbility[] abilities)
	{
		if(abilities.Length < _totalAbilities)
		{
			throw new System.InvalidProgramException(
				"Number of abilities provided is less than expected. "+
			"Expected "+_totalAbilities.ToString()+" , found "+abilities.Length.ToString());
		}

		for(int i = 0; i < _totalAbilities; ++i)
		{
			_playerAbilities[i] = abilities[i];

			_isCastable[i] = (_playerAbilities[i] is ICastable);
		
			IApplyImmediate isImmediate = _playerAbilities[i] as IApplyImmediate;
			if(isImmediate != null)
			{
				isImmediate.ApplyAbility(_mydata);
			}

			_cooldownAbilities[i] = _playerAbilities[i] as ICooldown;
			_persistentAbilities[i] = _playerAbilities[i] as IPersistent;
		}
	}

	public void ApplyEffects(params IEffect[] effects)
	{
		IDefensiveBuff defensiveEffect;
		IOffensiveBuff offensiveEffect;
		IUpdateable updateableEffect;
		IRemoveable removeableEffect;

		foreach(IEffect effect in effects)
		{
			defensiveEffect = effect as IDefensiveBuff;
			if(defensiveEffect != null)
			{
				_defensiveEffects.Add(defensiveEffect);
			}
			offensiveEffect = effect as IOffensiveBuff;
			if(offensiveEffect != null)
			{
				_offensiveEffects.Add(offensiveEffect);
			}
			updateableEffect = effect as IUpdateable;
			if(updateableEffect != null)
			{
				_updateableEffects.Add(updateableEffect);
			}
			removeableEffect = effect as IRemoveable;
			if(removeableEffect != null)
			{
				_removableEffects.Add(removeableEffect);
			}

			effect.OnApply(_mydata);
		}
	}
	public void UpdateEffects()
	{
		//Update abilities
		foreach(ICooldown i in _cooldownAbilities)
		{
			if(i != null)
			{
				i.UpdateCooldown();
			}
		}
		foreach(IPersistent i in _persistentAbilities)
		{
			if(i != null)
			{
				i.UpdateAbility(_mydata);
			}
		}

		//Update effects
		foreach(IUpdateable i in _updateableEffects)
		{
			i.UpdateEffect(_mydata);
		}
		List<IRemoveable> removeList = new List<IRemoveable>();
		foreach(IRemoveable i in _removableEffects)
		{
			if(i.ToRemove(_mydata))
			{
				removeList.Add(i);
			}
		}
		foreach(IRemoveable i in removeList)
		{
			RemoveEffect(i);
		}
		removeList.Clear();

		//Update attacks
		foreach(AttackPacket attack in _attacksOnSelf)
		{
			foreach(IDefensiveBuff defence in _defensiveEffects)
			{
				defence.ApplyBuff(attack);
			}
			
			TakeDamage(attack);
		}
	}

	private void TakeDamage(AttackPacket attack)
	{
		//Do something to health here.
		//Called after effects of the attack have been dampened by defenses.
		ApplyEffects(attack.Effects);
	}

	private void RemoveEffect(IRemoveable effect)
	{
		effect.OnDetach(_mydata);

		_removableEffects.Remove(effect);
		_defensiveEffects.Remove(effect as IDefensiveBuff);
		_offensiveEffects.Remove(effect as IOffensiveBuff);
		_updateableEffects.Remove(effect as IUpdateable);
	}

	private void HandleAbilityInput(AbilitySlot slot)
	{
		ICastable cast = null;
		switch(slot)
		{
			default:
				{
					throw new System.ArgumentException("Invalid value of slot: "+slot.ToString());
				}
			case AbilitySlot.Slot1:
				if(_isCastable[0])
				{
					cast = (ICastable)_playerAbilities[0];
				}
				break;
			case AbilitySlot.Slot2:
				if (_isCastable[1])
				{
					cast = (ICastable)_playerAbilities[1];
				}
				break;
			case AbilitySlot.Slot3:
				if (_isCastable[2])
				{
					cast = (ICastable)_playerAbilities[2];
				}
				break;
			case AbilitySlot.Slot4:
				if (_isCastable[3])
				{
					cast = (ICastable)_playerAbilities[3];
				}
				break;
		}

		ICooldown cooler = (ICooldown)cast;
		if(cast != null)
		{
			if(cooler != null)
			{
				if(!cooler.IsCastable())
				{
					return;
				}
			}
			cast.Cast(_mydata);
		}
	}
	public void AttackPlayer(AttackPacket attack)
	{
		_attacksOnSelf.Add(attack);
	}
}
