using niscolas.UnityUtils;
using UnityAtoms.BaseAtoms;
using UnityEngine;
using UnityEngine.Events;

namespace niscolas.Interaction
{
	public class InteractionAgent : MonoBehaviour, IInteractionAgent
	{
		[SerializeField]
		private GameObjectReference _currentInteractable;

		[Header("Events")]
		[SerializeField]
		private UnityEvent _onInteractionPossibilityStarted;

		[SerializeField]
		private UnityEvent _onInteractionPossibilityEnded;

		[SerializeField]
		private UnityEvent _interactionPerformed;

		public GameObject CurrentInteractableGameObject => _currentInteractable.Value;

		public GameObject GameObject => gameObject;
		public IInteractable CurrentInteractable { get; set; }

		private InteractionAgentController _controller;

		private void Awake()
		{
			_controller = new InteractionAgentController(this);
		}

		public void HandleNewInteractionPossibility(IInteractable interactable)
		{
			_controller.HandleNewInteractionPossibility(interactable);
			_onInteractionPossibilityStarted?.Invoke();
		}

		public void HandleInteractionPossibilityEnd(IInteractable interactable)
		{
			_controller.HandleInteractionPossibilityEnd(interactable);
			_onInteractionPossibilityEnded?.Invoke();
		}

		public void ClearCurrentInteractionPossibility()
		{
			_currentInteractable.Value = null;
		}

		public void HandleNewInteractionPossibility(Component other)
		{
			IInteractable interactable = other.GetComponentFromRoot<IInteractable>();

			if (interactable == null) return;

			HandleNewInteractionPossibility(interactable);
			_currentInteractable.Value = other.Root();
		}

		public void HandleInteractionPossibilityEnd(Component other)
		{
			IInteractable interactable = other.GetComponentFromRoot<IInteractable>();

			if (interactable == null) return;

			HandleInteractionPossibilityEnd(interactable);
		}

		public void InteractWithCurrent()
		{
			Interact(CurrentInteractable);
		}

		public void Interact(IInteractable interactable)
		{
			interactable?.ReceiveInteraction(this);
			_interactionPerformed?.Invoke();
		}
	}
}