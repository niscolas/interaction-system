using System.Collections.Generic;

namespace niscolas.Interaction
{
	public interface IInteractable
	{
		List<IInteractionAgent> InteractionAgents { get; }
		bool IsInteractionEnabled { get; set; }

		void ReceiveInteractionPossibility(IInteractionAgent interactionAgent);
		void EndInteractionPossibility(IInteractionAgent interactionAgent);
		void ReceiveInteraction(IInteractionAgent interactionAgent);
	}
}