using Scripts.Systems;
using UnityEngine;

namespace Scripts.UI.Buttons
{
	public class BackToMenuButton : MonoBehaviour
	{
		private ClickUIButton clickUIButton;

		private void Awake()
		{
			clickUIButton = GetComponent<ClickUIButton>();
			clickUIButton.OnClickDone += BackToMenuButtonClicked;
		}

		private void OnDestroy()
		{
			clickUIButton.OnClickDone -= BackToMenuButtonClicked;
		}

		private void BackToMenuButtonClicked()
		{
			GameStateManager.CurrentState = GameState.Menu;
		}
	}
}