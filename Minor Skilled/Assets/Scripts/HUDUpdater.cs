using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDUpdater : MonoBehaviour
{
    [SerializeField] private GameObject _canvas;
    [SerializeField] private Slider _healthBar;
    [SerializeField] private TextMeshProUGUI _nameText;
    private Vector3 _playerPosition;

    public void Update()
    {
        _canvas.transform.LookAt(GameManager.Instance.GetPlayer().position);
    }

    public void UpdateHealth(int newHealth)
    {
        _healthBar.value = newHealth;
    }

    public void SetMaxHealth(int maxHealth)
    {
        _healthBar.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, maxHealth);
    }

    public void SetName(string name)
    {
        _nameText.SetText(name);
    }
}