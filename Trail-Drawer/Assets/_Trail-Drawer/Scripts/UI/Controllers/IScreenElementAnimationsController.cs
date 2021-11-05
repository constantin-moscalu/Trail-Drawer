using DG.Tweening;

namespace Scripts.UI.Controllers
{
	public interface IScreenElementAnimationsController
	{
		public void ForceClose();
		public void ShowElement(TweenCallback callback);
		public void HideElement(TweenCallback callback);
	}
}