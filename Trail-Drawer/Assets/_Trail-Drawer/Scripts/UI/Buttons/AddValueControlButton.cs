using System;
using UnityEngine;

namespace Scripts.UI.Buttons
{
	public class AddValueControlButton : MonoBehaviour
	{
		[SerializeField] private float addValue;

		public event Action<float> onAddValueButtonClicked;

		private ClickUIButton clickUIButton;

		private void Awake()
		{
			clickUIButton = GetComponent<ClickUIButton>();
			clickUIButton.OnClickDone += AddValueControlButtonClicked;
		}

		private void AddValueControlButtonClicked() => onAddValueButtonClicked?.Invoke(addValue);
	}
}