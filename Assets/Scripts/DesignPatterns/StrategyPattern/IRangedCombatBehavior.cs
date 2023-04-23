using System.Collections;

namespace Assets.Scripts.DesignPatterns.StrategyPattern
{
	public interface IRangedCombatBehavior
	{

		public IEnumerator RangedCombat(float distanceFromPlayer);
	}
}
