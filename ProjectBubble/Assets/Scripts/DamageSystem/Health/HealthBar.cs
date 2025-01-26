using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField, Required] private Progressive _health;   // Health object (used to manage health amount)
    [SerializeField] private Gradient _healthgradient;    // Used to determine the Colour of the health bar
    [SerializeField] private Image _healthfillImage;      // Image for the health bar
    [SerializeField] private TextMeshProUGUI _healthTextDisplay;    // Text for the health

    [SerializeField] private Progressive _shield;
    [SerializeField] private Gradient _shieldgradient;
    private Image _shieldfillImage;      
    private TextMeshProUGUI _shieldTextDisplay;    

    public bool HasShield()
    {
        return _shield != null;
    }

    // Set variable functions
    public void SetComponentRefs(Progressive health, Progressive shield = null)
    { 
        _health = health;
        if (shield)
        { _shield = shield; }
    }

    public void SetImage(Image hImage, Image sImage = null)
    { 
        _healthfillImage = hImage; 
        if (sImage)
        { _shieldfillImage = sImage; }
    }

    public void SetTextDisplay(TextMeshProUGUI hText, TextMeshProUGUI sText = null)
    { 
        _healthTextDisplay = hText;
        if (sText)
        { _shieldTextDisplay = sText; }
    }

    public void Start()
    {
        _healthfillImage = EventManager_UI.ReturnHealthBarElement();
        if (_healthfillImage)// && _healthTextDisplay)
        {
            _healthfillImage.color = _healthgradient.Evaluate(_health.Ratio);
            UpdateHealthBar();
        }

        if (_shieldfillImage && _shieldTextDisplay)
        {
            _shieldfillImage.color = _shieldgradient.Evaluate(_shield.Ratio);
            UpdateShieldBar();
        }
        //_fillImage.transform.rotation = Quaternion.LookRotation(transform.position - _cam.transform.position);
    }

    // The two functions below update the health bar when the health object invokes the "OnChange" action variable

    private void OnEnable()
    {
        _health.OnChange += UpdateHealthBar;
        if (_shield) { _shield.OnChange += UpdateShieldBar; }
    }

    private void OnDisable()
    {
        _health.OnChange -= UpdateHealthBar;
        if (_shield) { _shield.OnChange -= UpdateShieldBar; }
    }

    // Changes the fill amount of the bar and the color based on the health objects' ratio
    private void UpdateHealthBar()
    {
        if (_healthfillImage)
        {
            // Disable this when using damage overlay
            _healthfillImage.fillAmount = _health.Ratio;

            _healthfillImage.color = _healthgradient.Evaluate(_health.Ratio);
            EventManager_UI.GetGradientColour(_healthgradient.Evaluate(_health.Ratio));
            EventManager_UI.FaceUIExpression(_health.Ratio);
            //_fillImage.fillAmount =
            //Mathf.MoveTowards(_fillImage.fillAmount,
            //_health.Ratio, reduceSpeed * Time.deltaTime);
        }

        if (_healthTextDisplay)
        {
            float hp = Mathf.Round(_health.Current * 10.0f) * 0.1f;
            _healthTextDisplay.text = hp.ToString();
        }
    }

    private void UpdateShieldBar()
    {
        if (_shieldfillImage)
        {
            // Disable this when using damage overlay
            _shieldfillImage.fillAmount = _shield.Ratio;

            _shieldfillImage.color = _shieldgradient.Evaluate(_shield.Ratio);
        }

        if (_shieldTextDisplay)
        {
            float sb = Mathf.Round(_shield.Current * 10.0f) * 0.1f;
            _shieldTextDisplay.text = sb.ToString();
        }
    }
}
