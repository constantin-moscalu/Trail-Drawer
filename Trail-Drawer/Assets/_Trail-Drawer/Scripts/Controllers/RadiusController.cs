using System.Collections;
using NaughtyAttributes;
using Unity.Mathematics;
using UnityEngine;

namespace Scripts.Controllers
{
	public class RadiusController : MonoBehaviour
	{
		[SerializeField] private float radius;
		[SerializeField] private float rotationSpeed;
		[SerializeField] private float widthMultiply;

		[HorizontalLine]
		[SerializeField] private bool drawGizmos;
		[SerializeField] private Color color;

		private LineRenderer lineRenderer;
		private RadiusController next;
		private TrailController trail;
		
		private bool canRotate;
		private bool haveChild;

		private void Awake()
		{
			InitData();
		}

		private void InitData()
		{
			lineRenderer = GetComponent<LineRenderer>();
			lineRenderer.positionCount = 2;

			canRotate = false;
			haveChild = false;
		}

		private void OnDestroy()
		{
			StopCoroutine(nameof(Rotate));
		}

		public void Initialize(float newRadius, float newSpeed)
		{
			radius = newRadius;
			rotationSpeed = newSpeed;
			
			DrawRadius();
		}

		public void SetWidth(float maxRadius)
		{
			float newWidth = maxRadius * widthMultiply;

			lineRenderer.startWidth = newWidth;
			lineRenderer.endWidth = newWidth;
		}

		public void AddRadius(RadiusController radiusController)
		{
			if (haveChild)
			{
				next.AddRadius(radiusController);
				return;
			}

			next = radiusController;
			next.transform.SetParent(transform);
			next.transform.localPosition = transform.up * radius;
			haveChild = true;
		}

		public void AddTrail(TrailController trailController)
		{
			if (haveChild)
			{
				next.AddTrail(trailController);
				return;
			}

			trail = trailController;
			trail.transform.SetParent(transform);
			trail.transform.localPosition = transform.up * radius;
		}

		public void StartRotation()
		{
			if (canRotate)
				return;

			canRotate = true;

			StartCoroutine(nameof(Rotate));

			if (haveChild)
			{
				next.StartRotation();
			}
		}

		public void StopRotation()
		{
			canRotate = false;

			if (haveChild)
			{
				next.StopRotation();
			}
		}

		public void Reset()
		{
			transform.localRotation = quaternion.identity;
			DrawRadius();

			if (haveChild)
			{
				next.Reset();
			}
		}

		private IEnumerator Rotate()
		{
			while (canRotate)
			{
				transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);

				DrawRadius();

				yield return new WaitForSeconds(Time.deltaTime);
			}
		}

		private void DrawRadius()
		{
			lineRenderer.SetPosition(0, transform.position);
			lineRenderer.SetPosition(1, transform.position + transform.up * radius);
		}

		private void OnDrawGizmos()
		{
			if (!drawGizmos)
				return;

			Gizmos.color = color;
			Gizmos.DrawLine(transform.position, transform.position + transform.up * radius);
		}
	}
}