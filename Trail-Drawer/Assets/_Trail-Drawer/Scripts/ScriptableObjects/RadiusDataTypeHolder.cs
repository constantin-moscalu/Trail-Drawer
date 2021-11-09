using System;
using System.Collections.Generic;
using Scripts.Models;
using UnityEngine;

namespace Scripts.ScriptableObjects
{
	[CreateAssetMenu(fileName = "RadiusDataTypeHolder", menuName = "GameData/RadiusDataTypeHolder", order = 0)]
	public class RadiusDataTypeHolder : ScriptableObject
	{
		[SerializeField] private List<RadiusDataType> radiusDataTypes;

		public List<RadiusDataType> RadiusDataTypes => radiusDataTypes;

		public event Action onRadiusDataTypesModified; 
		public event Action<int> onRadiusUpdated;
		public event Action<int> onRotationSpeedUpdated;

		public float GetMaxRadius()
		{
			float maxRadius = 0;

			foreach (var radiusDataType in RadiusDataTypes)
			{
				maxRadius += Mathf.Abs(radiusDataType.radius);
			}

			return maxRadius;
		}

		public void UpdateRadius(int index, float value)
		{
			UpdateRadiusData(index, value, radiusDataTypes[index].rotationSpeed);

			onRadiusUpdated?.Invoke(index);
			// onRadiusDataTypesModified?.Invoke();
		}

		public void UpdateRotationSpeed(int index, float value)
		{
			UpdateRadiusData(index, radiusDataTypes[index].radius, value);

			onRotationSpeedUpdated?.Invoke(index);
		}

		public void AddRadius()
		{
			radiusDataTypes.Add(new RadiusDataType {radius = 0, rotationSpeed = 0});
			
			onRadiusDataTypesModified?.Invoke();
		}

		private void UpdateRadiusData(int index, float radius, float rotationSpeed)
		{
			var radiusDataType = radiusDataTypes[index];

			radiusDataType.radius = radius;
			radiusDataType.rotationSpeed = rotationSpeed;

			radiusDataTypes[index] = radiusDataType;
		}
	}
}