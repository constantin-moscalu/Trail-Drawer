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

			radiusDataTypeHolder.onRadiusDataTypesModified += FullyUpdateRadiusList;
			radiusDataTypeHolder.onRadiusUpdated += UpdateRadius;
			radiusDataTypeHolder.onRotationSpeedUpdated += UpdateRotationSpeed;

			GameStateManager.onGameStateChange += OnGameStateChanged;

			InitRadius();
		}

		private void OnDestroy()
		{
			radiusDataTypeHolder.onRadiusDataTypesModified -= FullyUpdateRadiusList;
			radiusDataTypeHolder.onRadiusUpdated -= UpdateRadius;
			radiusDataTypeHolder.onRotationSpeedUpdated -= UpdateRotationSpeed;

			GameStateManager.onGameStateChange -= OnGameStateChanged;
		}

		private void InitRadius()
		{
			trailController = Instantiate(trailControllerPrefab);

			FullyUpdateRadiusList();
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

		private void FullyUpdateRadiusList()
		{
			UpdateRadiusList();
			UpdateRadiusListValues();
			UpdateHierarchy();
		}

		private void UpdateRadiusList()
		{
			if (radiusControllers.Count == radiusDataTypeHolder.RadiusDataTypes.Count)
				return;

			if (radiusControllers.Count < radiusDataTypeHolder.RadiusDataTypes.Count)
			{
				int elementsToCreateAmount = radiusDataTypeHolder.RadiusDataTypes.Count - radiusControllers.Count;

				for (int i = 0; i < elementsToCreateAmount; i++)
				{
					var newElement = Instantiate(radiusControllerPrefab, radiusContainerTransform);

					radiusControllers.Add(newElement);
				}
			}
			else
			{
				int elementsToRemoveAmount = radiusControllers.Count - radiusDataTypeHolder.RadiusDataTypes.Count;
				int index = radiusDataTypeHolder.RadiusDataTypes.Count;

				for (int i = index; i < index + elementsToRemoveAmount; i++)
				{
					Destroy(radiusControllers[i].gameObject);
				}

				radiusControllers.RemoveRange(radiusDataTypeHolder.RadiusDataTypes.Count, elementsToRemoveAmount);
			}
		}

		private void UpdateHierarchy()
		{
			for (int i = 1; i < radiusControllers.Count; i++)
			{
				SetHierarchy(radiusControllers[i - 1], radiusControllers[i].transform);
			}

			SetHierarchy(radiusControllers[radiusControllers.Count - 1], trailController.transform);
		}

		private void UpdateRadiusListValues()
		{
			for (int i = 0; i < radiusControllers.Count; i++)
			{
				radiusControllers[i].Initialize(radiusDataTypeHolder.RadiusDataTypes[i].radius, radiusDataTypeHolder.RadiusDataTypes[i].rotationSpeed);
			}
		}

		private void SetHierarchy(RadiusController radiusController, Transform child)
		{
			child.SetParent(radiusController.transform);
			child.localPosition = radiusController.LocalRadiusEndPosition;
		}

		private void UpdateRadius(int index)
		{
			radiusControllers[index].UpdateRadius(radiusDataTypeHolder.RadiusDataTypes[index].radius);
			cameraController.SetCameraSize(radiusDataTypeHolder.GetMaxRadius());

			for (int i = index + 1; i < radiusControllers.Count; i++)
			{
				radiusControllers[i].transform.localPosition = radiusControllers[i - 1].LocalRadiusEndPosition;
				radiusControllers[i].DrawRadius();
			}
		}

		private void UpdateRotationSpeed(int index) => radiusControllers[index].UpdateRotationSpeed(radiusDataTypeHolder.RadiusDataTypes[index].rotationSpeed);
	}
}