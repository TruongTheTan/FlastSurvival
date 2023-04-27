using System.Collections;

namespace Assets.Scripts.DesignPatterns.StrategyPattern
{
	public interface ICloseCombatBehavior
	{
		public IEnumerator CloseCombat();
	}
}
