using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Models
{
	public abstract class AbstractCharacter : MonoBehaviour, ICombatable
	{
		protected int _health;
		protected float _speedAmount;
		protected ExpBarController _expBarController;

		public virtual void Shoot() { }
		public abstract IEnumerator Melee();
		public abstract void ReceiveDamaged(int damage);
	}
}
