using System;
using NaughtyAttributes;
using Scripts.UI.Buttons;
using TMPro;
using UnityEngine;

namespace Scripts.UI.Controllers
{
	public class UIModifyValueController : MonoBehaviour
	{
		[SerializeField] private float addValueMultiplier;
		
		[HorizontalLine]
		[SerializeField] private TextMeshProUGUI valueText;
		[SerializeField] private AddValueControlButton addValueControlButton;
		[SerializeField] private AddValueControlButton subtractValueControlButton;

		public event Action<float> onValueModified; 
		
		private float value;

		private void Awake()
		{
			InitData();
		}

		private void InitData()
		{
			addValueControlButton.onAddValueButtonClicked += ModifyValue;
			subtractValueControlButton.onAddValueButtonClicked += ModifyValue;
		}

		private void OnDestroy()
		{
			addValueControlButton.onAddValueButtonClicked -= ModifyValue;
			subtractValueControlButton.onAddValueButtonClicked -= ModifyValue;
		}

		public void Initialize(float value)
		{
			this.value = value;
			
			UpdateValueText();
		}

		private void ModifyValue(float addValue)
		{
			value += addValueMultiplier * addValue;

			onValueModified?.Invoke(value);
			
			UpdateValueText();
		}

		private void UpdateValueText() => valueText.text = value.ToString();
	}
}