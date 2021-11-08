using Scripts.ScriptableObjects;
using Scripts.Systems;
using TMPro;
using UnityEngine;

namespace Scripts.UI.Buttons
{
	public class SelectRadiusButton : MonoBehaviour
	{
		[SerializeField] private TextMeshProUGUI nameText;
		
		private int index;

		private RadiusModifierDataHolder radiusModifierDataHolder;
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

		public void Initialize(RadiusModifierDataHolder radiusModifierDataHolder) => this.radiusModifierDataHolder = radiusModifierDataHolder;

		public void SetIndex(int newIndex)
		{
			index = newIndex;
			nameText.text = "Radius " + (index + 1);
		}

		private void SelectRadiusButtonClicked()
		{
			radiusModifierDataHolder.SetData(nameText.text, index);
			
			GameStateManager.CurrentState = GameState.Settings;
		}
	}
}