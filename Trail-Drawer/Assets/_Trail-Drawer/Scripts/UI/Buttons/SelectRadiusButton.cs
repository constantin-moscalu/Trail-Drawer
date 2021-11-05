using Scripts.Systems;
using TMPro;
using UnityEngine;

namespace Scripts.UI.Buttons
{
	public class SelectRadiusButton : MonoBehaviour
	{
		[SerializeField] private TextMeshProUGUI name;

		private int index;
		
		private ClickUIButton clickUIButton;

		private void Awake()
		{
			clickUIButton = GetComponent<ClickUIButton>();
			clickUIButton.OnClickDone += SelectRadiusButtonClicked;
		}

		private void OnDestroy()
		{
			clickUIButton.OnClickDone -= SelectRadiusButtonClicked;
		}

		public void UpdateData(int newIndex)
		{
			index = newIndex;
			name.text = "Radius " + (newIndex + 1);
		}

		private void SelectRadiusButtonClicked()
		{
			GameStateManager.CurrentState = GameState.Settings;
		}
	}
}