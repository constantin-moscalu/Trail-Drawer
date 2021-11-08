using System.Collections.Generic;
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
		
		private RadiusDataTypeHolder radiusDataTypeHolder; 
		private List<RadiusController> radiusControllers;
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
			radiusDataTypeHolder = Resources.Load<RadiusDataTypeHolder>("RadiusDataTypeHolder");
			
			cameraController.SetCameraSize(radiusDataTypeHolder.GetMaxRadius());

			radiusDataTypeHolder.onRadiusUpdated += OnRadiusUpdated;
			radiusDataTypeHolder.onRotationSpeedUpdated += OnRotationSpeedUpdated;

			GameStateManager.onGameStateChange += OnGameStateChanged;

			InitRadius();
		}
		
		private void OnDestroy()
		{
			GameStateManager.onGameStateChange -= OnGameStateChanged;
		}

		private void InitRadius()
		{
			radiusControllers = new List<RadiusController>();

			for (int i = 0; i < radiusDataTypeHolder.RadiusDataTypes.Count; i++)
			{
				var radiusData = radiusDataTypeHolder.RadiusDataTypes[i];

				var newRadius = Instantiate(radiusControllerPrefab, radiusContainerTransform);
				newRadius.Initialize(radiusData.radius, radiusData.rotationSpeed);
				newRadius.SetWidth(radiusDataTypeHolder.GetMaxRadius());

				radiusControllers.Add(newRadius);

				if (i > 0)
				{
					radiusControllers[0].AddRadius(newRadius);
				}
			}

			trailRadiusController = Instantiate(trailControllerPrefab);
			radiusControllers[0].AddTrail(trailRadiusController);
		}

		private void OnRadiusUpdated(int index) => radiusControllers[index].SetRadius(radiusDataTypeHolder.RadiusDataTypes[index].radius);
		private void OnRotationSpeedUpdated(int index) => radiusControllers[index].SetRotationSpeed(radiusDataTypeHolder.RadiusDataTypes[index].rotationSpeed);
		
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