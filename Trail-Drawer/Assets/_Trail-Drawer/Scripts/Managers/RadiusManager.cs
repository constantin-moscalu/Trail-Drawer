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
		private TrailController trailController;

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
			radiusControllers = new List<RadiusController>();
			
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
			for (int i = 0; i < radiusDataTypeHolder.RadiusDataTypes.Count; i++)
			{
				var radiusData = radiusDataTypeHolder.RadiusDataTypes[i];

				var newRadius = Instantiate(radiusControllerPrefab, radiusContainerTransform);
				newRadius.Initialize(radiusData.radius, radiusData.rotationSpeed);
				newRadius.SetWidth(radiusDataTypeHolder.GetMaxRadius());

				radiusControllers.Add(newRadius);

				if (i > 0)
				{
					SetHierarchy(radiusControllers[i - 1], newRadius.transform);
				}
			}

			trailController = Instantiate(trailControllerPrefab);
			SetHierarchy(radiusControllers[radiusControllers.Count - 1], trailController.transform);
		}

		private void SetHierarchy(RadiusController radiusController, Transform child)
		{
			child.SetParent(radiusController.transform);
			child.localPosition = radiusController.LocalRadiusEndPosition;
		}

		private void OnRadiusUpdated(int index)
		{
			radiusControllers[index].UpdateRadius(radiusDataTypeHolder.RadiusDataTypes[index].radius);
			cameraController.SetCameraSize(radiusDataTypeHolder.GetMaxRadius());
		}

		private void OnRotationSpeedUpdated(int index) => radiusControllers[index].UpdateRotationSpeed(radiusDataTypeHolder.RadiusDataTypes[index].rotationSpeed);
		
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
			foreach (var radiusController in radiusControllers)
			{
				radiusController.StartAction();
			}
			
			trailController.StartAction();
		}
		
		[Button("Stop Simulation")]
		private void StopSimulation()
		{
			Time.timeScale = 1f;
			
			foreach (var radiusController in radiusControllers)
			{
				radiusController.StopAction();
			}
			
			trailController.StopAction();
			// trailController.StopDraw();
		}
		
		[Button("Reset Simulation")]
		private void ResetSimulation()
		{			
			foreach (var radiusController in radiusControllers)
			{
				radiusController.Reset();
			}
			
			trailController.Reset();
		}
	}
}