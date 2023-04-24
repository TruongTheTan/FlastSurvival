using Assets.Scripts.DesignPatterns.StrategyPattern;
using UnityEngine;

namespace Assets.Scripts.Models.Enemies
{
	public abstract class RangedCombatEnemy : Enemy
	{

		private float _distanceToPlayer;

		public IRangedCombatBehavior RangedCombatBehavior { get; set; }




		protected void Start()
		{
			if (RangedCombatBehavior != null)
				StartCoroutine(RangedCombatBehavior.RangedCombat(_distanceToPlayer));
		}




		protected override void Update()
		{
			base.Update();
			this._distanceToPlayer = Vector2.Distance(_playerReference.transform.position, gameObject.transform.position);
		}
	}
}
