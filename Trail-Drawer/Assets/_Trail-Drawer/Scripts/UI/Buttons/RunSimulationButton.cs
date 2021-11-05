using Scripts.Systems;
using UnityEngine;

namespace Scripts.UI.Buttons
{
	public class RunSimulationButton : MonoBehaviour
	{
		private ClickUIButton clickUIButton;

		private void Awake()
		{
			clickUIButton = GetComponent<ClickUIButton>();
			clickUIButton.OnClickDone += RunSimulationButtonClicked;
		}

		private void OnDestroy()
		{
			clickUIButton.OnClickDone -= RunSimulationButtonClicked;
		}

		private void RunSimulationButtonClicked()
		{
			GameStateManager.CurrentState = GameState.Simulation;
		}
	}
}