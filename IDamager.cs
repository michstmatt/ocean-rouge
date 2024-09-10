using System.Diagnostics.Contracts;

public interface IDamager
{
    public float GetDamageAmount();
    public void OnDamaged();
}