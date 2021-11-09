using Scripts.ScriptableObjects;
using Scripts.Systems;
using UnityEngine;

namespace Scripts.UI.Buttons
{
	public class RemoveRadiusButton : MonoBehaviour
	{
		private RadiusDataTypeHolder radiusDataTypeHolder;
		private RadiusModifierDataHolder radiusModifierDataHolder;
		
		private ClickUIButton clickUIButton;

		private void Awake()
		{
			radiusDataTypeHolder = Resources.Load<RadiusDataTypeHolder>("RadiusDataTypeHolder");
			radiusModifierDataHolder = Resources.Load<RadiusModifierDataHolder>("RadiusModifierDataHolder");
			
			clickUIButton = GetComponent<ClickUIButton>();
			clickUIButton.OnClickDone += RemoveRadiusButtonClicked;
		}

		private void OnDestroy() => clickUIButton.OnClickDone -= RemoveRadiusButtonClicked;

		private void RemoveRadiusButtonClicked()
		{
			radiusDataTypeHolder.RemoveRadius(radiusModifierDataHolder.RadiusIndex);

			GameStateManager.CurrentState = GameState.Menu;
		}
	}
}