namespace niscolas.Interaction
{
	public class InteractionAgentController
	{
		private readonly IInteractionAgent _humbleInteractionAgent;

		public InteractionAgentController(IInteractionAgent humbleInteractionAgent)
		{
			_humbleInteractionAgent = humbleInteractionAgent;
		}

		private IInteractable CurrentInteractable
		{
			get => _humbleInteractionAgent.CurrentInteractable;
			set => _humbleInteractionAgent.CurrentInteractable = value;
		}

		public void HandleNewInteractionPossibility(IInteractable interactable)
		{
			if (CurrentInteractable != null)
			{
				HandleCurrentInteractionPossibilityEnd();
			}

			CurrentInteractable = interactable;
			CurrentInteractable.ReceiveInteractionPossibility(_humbleInteractionAgent);
		}

		public void HandleCurrentInteractionPossibilityEnd()
		{
			HandleInteractionPossibilityEnd(CurrentInteractable);
			CurrentInteractable = null;
		}

		public void HandleInteractionPossibilityEnd(IInteractable interactable)
		{
			interactable?.EndInteractionPossibility(_humbleInteractionAgent);

			if (interactable == CurrentInteractable)
			{
				_humbleInteractionAgent.ClearCurrentInteractionPossibility();
			}
		}
	}
}