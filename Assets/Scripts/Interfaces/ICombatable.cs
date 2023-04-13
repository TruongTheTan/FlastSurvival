using System.Collections;

public interface ICombatable
{
	public virtual void Shoot() { }
	public abstract IEnumerator Melee();
	public void ReceiveDamaged(int damage);
}
