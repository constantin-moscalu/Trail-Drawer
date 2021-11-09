using Scripts.ScriptableObjects;
using UnityEngine;

namespace Scripts.UI.Buttons
{
	public class AddRadiusButton : MonoBehaviour
	{
		private RadiusDataTypeHolder radiusDataTypeHolder;
		private ClickUIButton clickUIButton;
		
		private void Awake()
		{
			radiusDataTypeHolder = Resources.Load<RadiusDataTypeHolder>("RadiusDataTypeHolder");
			
			clickUIButton = GetComponent<ClickUIButton>();
			clickUIButton.OnClickDone += AddRadiusButtonClicked;
		}

		private void OnDestroy() => clickUIButton.OnClickDone -= AddRadiusButtonClicked;

		private void AddRadiusButtonClicked() => radiusDataTypeHolder.AddRadius();
	}
}