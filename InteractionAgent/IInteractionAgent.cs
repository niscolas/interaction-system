using UnityEngine;

namespace niscolas.Interaction
{
	public interface IInteractionAgent
	{
		GameObject GameObject { get; }

		IInteractable CurrentInteractable { get; set; }

		void HandleNewInteractionPossibility(IInteractable interactable);
		void HandleInteractionPossibilityEnd(IInteractable interactable);
		void ClearCurrentInteractionPossibility();
	}
}