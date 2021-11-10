using DG.Tweening;
using NaughtyAttributes;
using Scripts.Systems;
using UnityEngine;

namespace Scripts.Controllers
{
	public class TrailController : ActionControllerBase
	{
		[HorizontalLine]
		[SerializeField] private float eraseDuration;

		[SerializeField] private Ease eraseEase;

		private TrailRenderer trailRenderer;
		private Tween eraseTween;
		private float startTime;
		private Vector3 startPosition;

		protected override void OnInitData()
		{
			trailRenderer = GetComponent<TrailRenderer>();
			trailRenderer.time = .1f;
		}

		public override void Reset()
		{
		}

		protected override void OnSetWidth(float width)
		{
			trailRenderer.startWidth = width;
			trailRenderer.endWidth = width;
		}

		protected override void OnStartAction()
		{
			startTime = Time.time;
			startPosition = transform.position;
			trailRenderer.emitting = true;
		}

		protected override void OnDoAction()
		{
			trailRenderer.time += actionRefreshDelay * 1.2f;

			// if ((Time.time - startTime > 1) && Vector3.Distance(startPosition, transform.position) < 0.1)
			// {
			// 	GameStateManager.CurrentState = GameState.StopSimulation;
			// }
		}

		protected override void OnStopAction() => StopDraw();

		protected override void OnDestroyed() => eraseTween.Kill();

		private void StopDraw()
		{
			trailRenderer.emitting = false;
			trailRenderer.time = Time.time - startTime;

			eraseTween.Kill();
			eraseTween = DOVirtual.Float(trailRenderer.time, 0f, eraseDuration, value => trailRenderer.time = value)
				.SetEase(eraseEase).OnComplete(() => GameStateManager.CurrentState = GameState.Menu);
		}
	}
}