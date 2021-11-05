using NaughtyAttributes;
using Scripts.Systems;
using UnityEngine;

namespace Scripts.Managers
{
	public class GameStateTester : MonoBehaviour
	{
		[SerializeField] private GameState gameState;

		[Button("Set Game State")]
		private void SetGameState()
		{
			GameStateManager.CurrentState = gameState;
		}
	}
}