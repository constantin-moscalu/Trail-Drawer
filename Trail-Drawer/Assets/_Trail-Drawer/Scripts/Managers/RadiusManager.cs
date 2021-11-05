using NaughtyAttributes;
using Scripts.Controllers;
using Scripts.ScriptableObjects;
using Scripts.Systems;
using UnityEngine;

namespace Scripts.Managers
{
	public class RadiusManager : MonoBehaviour
	{
		[SerializeField] private CameraController cameraController;

		[HorizontalLine]
		[SerializeField] private RadiusController radiusControllerPrefab;
		[SerializeField] private TrailController trailControllerPrefab;
		
		[HorizontalLine]
		[SerializeField] private Transform radiusContainerTransform;

		[HorizontalLine]
		[SerializeField, Range(1f, 10f)] private float timeScale = 1f;
		
		private RadiusDataTypeDataHolder radiusSetupData; 
		private RadiusController[] radiusControllers;
		private TrailController trailRadiusController;

		private void OnValidate()
		{
			Time.timeScale = timeScale;
		}

		private void Awake()
		{
			InitData();
		}

		private void InitData()
		{
			radiusSetupData = Resources.Load<RadiusDataTypeDataHolder>("RadiusDataTypeDataHolder");
			
			cameraController.SetCameraSize(radiusSetupData.GetMaxRadius());

			GameStateManager.onGameStateChange += OnGameStateChanged;

			InitRadius();
		}

		private void OnDestroy()
		{
			GameStateManager.onGameStateChange -= OnGameStateChanged;
		}

		private void InitRadius()
		{
			radiusControllers = new RadiusController[radiusSetupData.RadiusDataTypes.Count];

			for (int i = 0; i < radiusControllers.Length; i++)
			{
				var radiusData = radiusSetupData.RadiusDataTypes[i];

				var newRadius = Instantiate(radiusControllerPrefab, radiusContainerTransform);
				newRadius.Initialize(radiusData.radius, radiusData.rotationSpeed);
				newRadius.SetWidth(radiusSetupData.GetMaxRadius());
				
				radiusControllers[i] = newRadius;

				if (i > 0)
				{
					radiusControllers[0].AddRadius(newRadius);
				}
			}

			trailRadiusController = Instantiate(trailControllerPrefab);
			radiusControllers[0].AddTrail(trailRadiusController);
		}
		
		private void OnGameStateChanged(GameState gameState)
		{
			switch (gameState)
			{
				case GameState.Simulation:
					RunSimulation();
					break;
				case GameState.StopSimulation:
					StopSimulation();
					break;
				case GameState.Menu:
					ResetSimulation();
					break;
			}
		}
		
		[Button("Run Simulation")]
		private void RunSimulation()
		{
			radiusControllers[0].StartRotation();
			trailRadiusController.StartDraw();
		}
		
		[Button("Stop Simulation")]
		private void StopSimulation()
		{
			Time.timeScale = 1f;
			
			radiusControllers[0].StopRotation();
			trailRadiusController.StopDraw();
		}
		
		[Button("Reset Simulation")]
		private void ResetSimulation() => radiusControllers[0].Reset();
	}
}