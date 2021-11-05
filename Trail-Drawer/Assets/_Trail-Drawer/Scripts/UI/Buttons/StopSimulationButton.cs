using DG.Tweening;
using Scripts.Systems;
using UnityEngine;

namespace Scripts.UI.Buttons
{
	public class StopSimulationButton : MonoBehaviour
	{
		private ClickUIButton clickUIButton;

		private void Awake()
		{
			clickUIButton = GetComponent<ClickUIButton>();
			clickUIButton.OnClickDone += StopSimulationButtonClicked;
		}

		private void OnDestroy()
		{
			clickUIButton.OnClickDone -= StopSimulationButtonClicked;
		}

		private void StopSimulationButtonClicked()
		{
			GameStateManager.CurrentState = GameState.StopSimulation;

			DOVirtual.DelayedCall(3f, () => GameStateManager.CurrentState = GameState.Menu);
		}
	}
}