using UnityEngine;

namespace Scripts.ScriptableObjects
{
	[CreateAssetMenu(fileName = "RadiusModifierDataHolder", menuName = "GameData/RadiusModifierDataHolder", order = 1)]
	public class RadiusModifierDataHolder : ScriptableObject
	{
		public string RadiusName => radiusName;
		public int RadiusIndex => radiusIndex;

		private string radiusName;
		private int radiusIndex;

		public void SetData(string name, int index)
		{
			radiusName = name;
			radiusIndex = index;
		}
	}
}