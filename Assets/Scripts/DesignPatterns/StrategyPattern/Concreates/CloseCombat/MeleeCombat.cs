﻿using System.Collections;
using UnityEngine;

namespace Assets.Scripts.DesignPatterns.StrategyPattern
{
	public class MeleeCombat : ICloseCombatBehavior
	{
		private readonly int _enemyDamage;
		public static bool IsCollidingPlayer { get; set; }



		public MeleeCombat(int enemyDamage)
		{
			this._enemyDamage = enemyDamage;
		}


		public IEnumerator CloseCombat()
		{
			while (IsCollidingPlayer)
			{
				DataPreserve.player.GetComponent<PlayableCharacterController>().ReceiveDamaged(_enemyDamage);
				yield return new WaitForSeconds(1f);
			}
		}
	}
}
