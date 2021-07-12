using System.Collections.Generic;
using UnityExtensions;

namespace niscolas.Interaction
{
	public class InteractableController
	{
		private readonly IInteractable _humbleInteractable;

		public InteractableController(IInteractable humbleInteractable)
		{
			_humbleInteractable = humbleInteractable;
		}

		private List<IInteractionAgent> InteractionAgents => _humbleInteractable.InteractionAgents;
		private bool IsInteractionEnabled => _humbleInteractable.IsInteractionEnabled;

		public bool TryReceiveInteractionPossibility(IInteractionAgent interactionAgent)
		{
			if (!IsInteractionEnabled)
			{
				return false;
			}

			InteractionAgents.Add(interactionAgent);
			return true;
		}

		public void EndAllInteractionPossibilities()
		{
			InteractionAgents.ToArray().ForEach(EndInteractionPossibility);
		}

		public void EndInteractionPossibility(IInteractionAgent interactionAgent)
		{
			if (!InteractionAgents.Contains(interactionAgent))
			{
				return;
			}

			InteractionAgents.Remove(interactionAgent);
		}

		public bool TryReceiveInteraction()
		{
			return IsInteractionEnabled;
		}
	}
}