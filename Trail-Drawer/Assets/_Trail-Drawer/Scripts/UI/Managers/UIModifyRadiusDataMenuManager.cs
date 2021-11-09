using Scripts.ScriptableObjects;
using Scripts.Systems;
using Scripts.UI.Controllers;
using TMPro;
using UnityEngine;

namespace Scripts.UI.Managers
{
	public class UIModifyRadiusDataMenuManager : MonoBehaviour
	{
		[SerializeField] private TextMeshProUGUI radiusNameText;
		[SerializeField] private UIModifyValueController radiusModifyController;
		[SerializeField] private UIModifyValueController speedModifyController;

		private RadiusModifierDataHolder radiusModifierDataHolder;
		private RadiusDataTypeHolder radiusDataTypeHolder;

		private void Awake()
		{
			InitData();
		}

		private void InitData()
		{
			radiusDataTypeHolder = Resources.Load<RadiusDataTypeHolder>("RadiusDataTypeHolder");
			radiusModifierDataHolder = Resources.Load<RadiusModifierDataHolder>("RadiusModifierDataHolder");

			radiusModifyController.onValueModified += OnRadiusValueModified;
			speedModifyController.onValueModified += OnSpeedValueModified;
			
			GameStateManager.onGameStateChange += OnGameStateChanged;
		}

		private void OnDestroy()
		{
			radiusModifyController.onValueModified -= OnRadiusValueModified;
			speedModifyController.onValueModified -= OnSpeedValueModified;
			
			GameStateManager.onGameStateChange -= OnGameStateChanged;
		}

		private void OnRadiusValueModified(float value) => radiusDataTypeHolder.UpdateRadius(radiusModifierDataHolder.RadiusIndex, value);
		private void OnSpeedValueModified(float value) => radiusDataTypeHolder.UpdateRotationSpeed(radiusModifierDataHolder.RadiusIndex, value);

		private void OnGameStateChanged(GameState gameState)
		{
			print("Update panel: " + GameStateManager.CurrentState);
			if (gameState != GameState.Settings)
				return;
			print("Update panel: " + gameState);
			radiusNameText.text = radiusModifierDataHolder.RadiusName;

			int index = radiusModifierDataHolder.RadiusIndex;
			var radiusData = radiusDataTypeHolder.RadiusDataTypes[index];

			radiusModifyController.Initialize(radiusData.radius);
			speedModifyController.Initialize(radiusData.rotationSpeed);
		}
		
		
	}
}