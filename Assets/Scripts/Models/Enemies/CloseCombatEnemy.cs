using Assets.Scripts.DesignPatterns.StrategyPattern;
using UnityEngine;

namespace Assets.Scripts.Models.Enemies
{
	public class CloseCombatEnemy : Enemy
	{

		public ICloseCombatBehavior CloseCombatBehavior { get; set; }




		private void OnCollisionEnter2D(Collision2D collision)
		{
			if (collision.gameObject.Equals(_playerReference))
			{
				MeleeCombat.IsCollidingPlayer = true;

				if (CloseCombatBehavior != null)
					StartCoroutine(CloseCombatBehavior.CloseCombat());
			}
		}



		private void OnCollisionExit2D(Collision2D collision)
		{
			if (collision.gameObject.Equals(_playerReference))
				MeleeCombat.IsCollidingPlayer = false;
		}
	}
}
