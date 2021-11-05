using System;
using Scripts.Systems;
using Scripts.UI.Controllers;
using UnityEngine;

namespace Scripts.UI
{
	public class UIScreen : ScreenElementsAnimationController
	{
		[SerializeField] protected GameState state;
		public GameState State => state;
		
		public Action<bool> OnScreenStateChanged { get; set; }

		protected override void OnElementShow()
		{
			OnScreenStateChanged?.Invoke(true);
		}

		protected override void OnElementHide()
		{
			OnScreenStateChanged?.Invoke(false);
		}
	}
}