using UnityEngine;
using UnityEngine.UI;

public abstract class BaseHUDManager : MonoBehaviour
{
    [SerializeField] protected Slider healthIndicator;
    [SerializeField] protected Slider armorIndicator;

    public abstract void UpdateName(string name);

    public virtual void SetMaxHealth(int maxHealth)
    {
        healthIndicator.maxValue = maxHealth;
        healthIndicator.value = maxHealth;
    }

    public virtual void SetMaxArmor(int maxArmor)
    {
        armorIndicator.maxValue = maxArmor;
        armorIndicator.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, maxArmor);
        armorIndicator.value = maxArmor;
    }

    public void UpdateHealth(int health)
    {
        healthIndicator.value = health;
    }

    public void UpdateArmor(int armor)
    {
        armorIndicator.value = armor;
    }

    public virtual void UpdateSpeed(float speed)
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
}