using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

namespace Scripts.Controllers
{
	public class TrailController : MonoBehaviour
	{
		[SerializeField] private float widthMultiply;
		
		[HorizontalLine]
		[SerializeField] private float eraseDuration;
		[SerializeField] private Ease eraseEase;
		
		private TrailRenderer trailRenderer;

		private Tween eraseTween;
		private bool canDraw;

		private void Awake()
		{
			InitData();
		}

		private void InitData()
		{
			trailRenderer = GetComponent<TrailRenderer>();
			trailRenderer.time = 0;
		}

		private void OnDestroy()
		{
			eraseTween.Kill();
		}

		private void Update()
		{
			if (!canDraw)
				return;

			trailRenderer.time += Time.deltaTime;
		}

		public void SetWidth(float maxRadius)
		{
			float newWidth = maxRadius * widthMultiply;

			trailRenderer.startWidth = newWidth;
			trailRenderer.endWidth = newWidth;
		}

		public void StartDraw()
		{
			if (canDraw)
				return;
			
			canDraw = true;

			trailRenderer.time = .1f;
			trailRenderer.emitting = true;
		}

		public void StopDraw()
		{
			canDraw = false;
			trailRenderer.emitting = false;
			
			eraseTween.Kill();
			eraseTween = DOVirtual.Float(trailRenderer.time, 0f, eraseDuration, value => trailRenderer.time = value).SetEase(eraseEase);
		}
	}
}