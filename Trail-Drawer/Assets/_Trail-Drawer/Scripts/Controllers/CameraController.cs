using NaughtyAttributes;
using UnityEngine;

namespace Scripts.Controllers
{
	public class CameraController : MonoBehaviour
	{
		[SerializeField] private float sizeToRadiusRation;
		[SerializeField] private float positionToRadiusRation;

		[HorizontalLine]
		[SerializeField] private Camera actualCamera;

		public void SetCameraSize(float radius)
		{
			if (radius == 0 || sizeToRadiusRation == 0)
				return;

			actualCamera.orthographicSize = radius * 2f * sizeToRadiusRation;
			transform.position = Vector3.down * radius * positionToRadiusRation;
		}
	}
}