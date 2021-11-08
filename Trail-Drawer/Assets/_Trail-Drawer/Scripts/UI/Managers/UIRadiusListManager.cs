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
		
		private RadiusDataTypeDataHolder radiusDataTypeDataHolder;
		private List<SelectRadiusButton> selectRadiusButtons;

		private void Awake()
		{
			InitData();
		}

		private void InitData()
		{
			radiusDataTypeDataHolder = Resources.Load<RadiusDataTypeDataHolder>("RadiusDataTypeDataHolder");
			selectRadiusButtons=new List<SelectRadiusButton>();

			GameStateManager.onGameStateChange += TryUpdateButtonsList;
			
			UpdateButtonsList();
		}

		private void OnDestroy()
		{
			GameStateManager.onGameStateChange -= TryUpdateButtonsList;
		}

		private void TryUpdateButtonsList(GameState gameState)
		{
			if(gameState!=GameState.Menu)
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
			if (selectRadiusButtons.Count == radiusDataTypeDataHolder.RadiusDataTypes.Count)
				return;

			if (selectRadiusButtons.Count < radiusDataTypeDataHolder.RadiusDataTypes.Count)
			{
				int buttonsToCreateAmount = radiusDataTypeDataHolder.RadiusDataTypes.Count - selectRadiusButtons.Count;

				for (int i = 0; i < buttonsToCreateAmount; i++)
				{
					var newButton = Instantiate(selectRadiusButtonPrefab, buttonsHolder);
					selectRadiusButtons.Add(newButton);
				}
			}
			else
			{
				int buttonsToRemoveAmount = selectRadiusButtons.Count - radiusDataTypeDataHolder.RadiusDataTypes.Count;
				int index = radiusDataTypeDataHolder.RadiusDataTypes.Count;
				
				for (int i = index; i < index+buttonsToRemoveAmount; i++)
				{
					Destroy(selectRadiusButtons[i].gameObject);
				}
				
				selectRadiusButtons.RemoveRange(radiusDataTypeDataHolder.RadiusDataTypes.Count, buttonsToRemoveAmount);
			}
		}

		private void UpdateButtonsListByName()
		{
			for (int i = 0; i < selectRadiusButtons.Count; i++)
			{
				selectRadiusButtons[i].UpdateData(i);
			}
		}
	}
}