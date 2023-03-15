using UnityEngine;
using UnityEngine.UI;

public abstract class BaseHUDManager : MonoBehaviour
{
    [SerializeField] protected Slider _healthIndicator;
    [SerializeField] protected Slider _armorIndicator;

    public abstract void UpdateName(string name);

    public virtual void SetMaxHealth(int maxHealth)
    {
        _healthIndicator.maxValue = maxHealth;
        _healthIndicator.value = maxHealth;
    }

    public virtual void SetMaxArmor(int maxArmor)
    {
        _armorIndicator.maxValue = maxArmor;
        _armorIndicator.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, maxArmor);
        _armorIndicator.value = maxArmor;
    }

    public void UpdateHealth(int health)
    {
        _healthIndicator.value = health;
    }

    public void UpdateArmor(int armor)
    {
        _armorIndicator.value = armor;
    }

    public virtual void UpdateSpeed(float speed)
    {
    }

    public virtual void UpdateGearData(int gear, int rpm, int torque)
    {
    }

    public virtual void UpdateCalculationText(string text)
    {
    }
    
    public virtual void UpdateAmmoCountUI(int ammoCount)
    {
    }

    public virtual void UpdateShellTypeUI(string shellType)
    {
    }

    public virtual void UpdateDistanceUI(float targetDistance)
    {
    }

    public virtual void UpdateWheelRPMCalculation(string text)
    {
    }
}