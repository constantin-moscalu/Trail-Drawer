using System.Collections;
using UnityEngine;

namespace Scripts.Controllers
{
	public abstract class ActionControllerBase : MonoBehaviour
	{
		[SerializeField] private float widthMultiply;
		[SerializeField] protected float actionRefreshDelay;

		private bool canDoAction;
		private WaitForSeconds waitTime;
		
		private void Awake()
		{
			canDoAction = false;
			waitTime = new WaitForSeconds(actionRefreshDelay);
			
			InitData();
		}

		private void InitData()
		{
			OnInitData();
		}

		protected abstract void OnInitData();

		private void OnDestroy()
		{
			StopCoroutine(nameof(ActionCoroutine));
			
			OnDestroyed();
		}

		public void StartAction()
		{
			if (canDoAction)
				return;

			canDoAction = true;
			
			StartCoroutine(nameof(ActionCoroutine));
			
			OnStartAction();
		}

		public void StopAction()
		{
			canDoAction = false;
			
			OnStopAction();
		}

		public abstract void Reset();

		public void SetWidth(float maxRadius)
		{
			float width = maxRadius * widthMultiply;
			
			OnSetWidth(width);
		}
		
		protected abstract void OnSetWidth(float width);
		protected abstract void OnStartAction();
		protected abstract void OnDoAction();
		protected abstract void OnStopAction();
		protected abstract void OnDestroyed();

		private IEnumerator ActionCoroutine()
		{
			while (canDoAction)
			{
				OnDoAction();

				yield return waitTime;
			}
		}
	}
}