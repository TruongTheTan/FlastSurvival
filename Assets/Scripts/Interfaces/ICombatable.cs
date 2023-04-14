using System.Collections;

public interface ICombatable
{
	public void Shoot();
	public IEnumerator Melee();
	public void ReceiveDamaged(int damage);
}
