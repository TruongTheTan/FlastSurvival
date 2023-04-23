using System.Collections;
using UnityEngine;

namespace Assets.Scripts.DesignPatterns.StrategyPattern
{
	public class MeleeCombat : ICloseCombatBehavior
	{
		public static bool IsCollidingPlayer { get; set; }


		private int _enemyDamage;

		public MeleeCombat(int _enemyDamage)
		{
			this._enemyDamage = _enemyDamage;
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
