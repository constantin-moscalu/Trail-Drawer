using System.Collections.Generic;
using Scripts.ScriptableObjects;
using Scripts.Systems;
using Scripts.UI.Buttons;
using UnityEngine;

namespace Scripts.UI.Managers
{
	public class UIRadiusListManager : MonoBehaviour
	{
		[SerializeField] private SelectRadiusButton selectRadiusButtonPrefab;
		[SerializeField] private RectTransform buttonsHolder;

		private RadiusDataTypeHolder radiusDataTypeHolder;
		private RadiusModifierDataHolder radiusModifierDataHolder;
		private List<SelectRadiusButton> selectRadiusButtons;

		private void Awake()
		{
			InitData();
		}

		private void InitData()
		{
			radiusDataTypeHolder = Resources.Load<RadiusDataTypeHolder>("RadiusDataTypeHolder");
			radiusModifierDataHolder = Resources.Load<RadiusModifierDataHolder>("RadiusModifierDataHolder");

			selectRadiusButtons = new List<SelectRadiusButton>();

			GameStateManager.onGameStateChange += TryUpdateButtonsList;

			UpdateButtonsList();
		}

		private void OnDestroy()
		{
			GameStateManager.onGameStateChange -= TryUpdateButtonsList;
		}

		private void TryUpdateButtonsList(GameState gameState)
		{
			if (gameState != GameState.Menu)
				return;

			UpdateButtonsList();
		}

		private void UpdateButtonsList()
		{
			UpdateButtonsListByCount();
			UpdateButtonsListByName();
		}

		private void UpdateButtonsListByCount()
		{
			if (selectRadiusButtons.Count == radiusDataTypeHolder.RadiusDataTypes.Count)
				return;

			if (selectRadiusButtons.Count < radiusDataTypeHolder.RadiusDataTypes.Count)
			{
				int buttonsToCreateAmount = radiusDataTypeHolder.RadiusDataTypes.Count - selectRadiusButtons.Count;

				for (int i = 0; i < buttonsToCreateAmount; i++)
				{
					var newButton = Instantiate(selectRadiusButtonPrefab, buttonsHolder);
					newButton.Initialize(radiusModifierDataHolder);
					
					selectRadiusButtons.Add(newButton);
				}
			}
			else
			{
				int buttonsToRemoveAmount = selectRadiusButtons.Count - radiusDataTypeHolder.RadiusDataTypes.Count;
				int index = radiusDataTypeHolder.RadiusDataTypes.Count;

				for (int i = index; i < index + buttonsToRemoveAmount; i++)
				{
					Destroy(selectRadiusButtons[i].gameObject);
				}

				selectRadiusButtons.RemoveRange(radiusDataTypeHolder.RadiusDataTypes.Count, buttonsToRemoveAmount);
			}
		}

		private void UpdateButtonsListByName()
		{
			for (int i = 0; i < selectRadiusButtons.Count; i++)
			{
				selectRadiusButtons[i].SetIndex(i);
			}
		}
	}
}