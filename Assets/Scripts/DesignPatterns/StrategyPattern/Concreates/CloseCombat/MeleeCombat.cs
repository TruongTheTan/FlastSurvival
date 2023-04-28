using System.Collections;
using UnityEngine;

namespace Assets.Scripts.DesignPatterns.StrategyPattern.Concreates
{
	public class MeleeCombat : ICloseCombatBehavior
	{
		private const float _attackPeriod = 1f;
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
				yield return new WaitForSeconds(_attackPeriod);
			}
		}
	}
}
