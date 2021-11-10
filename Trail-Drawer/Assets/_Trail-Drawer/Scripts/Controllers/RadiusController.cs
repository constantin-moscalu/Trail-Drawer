using NaughtyAttributes;
using Unity.Mathematics;
using UnityEngine;

namespace Scripts.Controllers
{
	public class RadiusController : ActionControllerBase
	{
		[HorizontalLine]
		[SerializeField] private float radius;
		[SerializeField] private float rotationSpeed;

		public Vector3 LocalRadiusEndPosition => transform.up * radius;

		private LineRenderer lineRenderer;

		protected override void OnInitData()
		{
			lineRenderer = GetComponent<LineRenderer>();
			lineRenderer.positionCount = 2;
		}

		public void Initialize(float radius, float rotationSpeed)
		{
			UpdateRotationSpeed(rotationSpeed);
			UpdateRadius(radius);
			
			DrawRadius();
		}

		public void UpdateRadius(float value)
		{
			radius = value;

			DrawRadius();
		}

		public void UpdateRotationSpeed(float value) => rotationSpeed = value;

		public override void Reset()
		{
			transform.localRotation = quaternion.identity;

			DrawRadius();
		}

		protected override void OnSetWidth(float width)
		{
			lineRenderer.startWidth = width;
			lineRenderer.endWidth = width;
		}

		protected override void OnStartAction()
		{
		}

		protected override void OnDoAction()
		{
			transform.Rotate(Vector3.forward, rotationSpeed * actionRefreshDelay);

			DrawRadius();
		}

		protected override void OnStopAction()
		{
		}

		protected override void OnDestroyed()
		{
		}

		public void DrawRadius()
		{
			lineRenderer.SetPosition(0, transform.position);
			lineRenderer.SetPosition(1, transform.position + transform.up * radius);
		}
	}
}