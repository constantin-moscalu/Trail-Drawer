using System.Collections.Generic;
using Scripts.Models;
using UnityEngine;

namespace Scripts.ScriptableObjects
{
	[CreateAssetMenu(fileName = "RadiusDataTypeDataHolder", menuName = "Game Data/RadiusDataTypeDataHolder", order = 0)]
	public class RadiusDataTypeDataHolder : ScriptableObject
	{
		[SerializeField] private List<RadiusDataType> radiusDataTypes;

		public List<RadiusDataType> RadiusDataTypes => radiusDataTypes;

		public float GetMaxRadius()
		{
			float maxRadius = 0;

			foreach (var radiusDataType in RadiusDataTypes)
			{
				maxRadius += Mathf.Abs(radiusDataType.radius);
			}

			return maxRadius;
		}
	}
}