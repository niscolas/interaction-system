using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityAtoms.BaseAtoms;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;

namespace niscolas.Interaction
{
	public class Interactable : MonoBehaviour, IInteractable
	{
		[SerializeField]
		private BoolReference _isInteractionEnabled = new BoolReference(true);

		[SerializeField]
		private BoolReference _shouldManageCollider = new BoolReference(true);

		[Required]
		[SerializeField]
		private Collider _collider;

		[Header("Events")]
		[SerializeField]
		private UnityEvent<IInteractionAgent> _onInteractionPossibilityReceived;

		[SerializeField]
		private UnityEvent<IInteractionAgent> _onInteractionPossibilityEnded;

		[SerializeField]
		private UnityEvent<IInteractionAgent> _onInteractionReceived;
		
		[SerializeField]
		private UnityEvent<GameObject> _onInteractionPossibilityReceivedGameObject;

		[SerializeField]
		private UnityEvent<GameObject> _onInteractionPossibilityEndedGameObject;

		[SerializeField]
		private UnityEvent<GameObject> _onInteractionReceivedGameObject;

		public UnityEvent<IInteractionAgent> InteractionPossibilityReceived => _onInteractionPossibilityReceived;

		public UnityEvent<IInteractionAgent> InteractionPossibilityEnded => _onInteractionPossibilityEnded;

		public UnityEvent<IInteractionAgent> InteractionReceived => _onInteractionReceived;

		public UnityEvent<GameObject> InteractionPossibilityReceivedGameObject => _onInteractionPossibilityReceivedGameObject;

		public UnityEvent<GameObject> InteractionPossibilityEndedGameObject => _onInteractionPossibilityEndedGameObject;

		public UnityEvent<GameObject> InteractionReceivedGameObject => _onInteractionReceivedGameObject;

		public Collider Collider
		{
			get => _collider;
			set => _collider = value;
		}

		public List<IInteractionAgent> InteractionAgents { get; } = new List<IInteractionAgent>();

		public bool IsInteractionEnabled
		{
			get => _isInteractionEnabled;
			set => _isInteractionEnabled.Value = value;
		}

		private InteractableController _controller;

		private void Awake()
		{
			_controller = new InteractableController(this);
		}

		private void OnEnable()
		{
			if (!_shouldManageCollider.Value || !_collider)
			{
				return;
			}

			_collider.enabled = true;
		}

		private void Start()
		{
			Assert.IsNotNull(_collider);
			SetupCollider();
		}

		private void OnDisable()
		{
			if (_shouldManageCollider.Value)
			{
				_collider.enabled = false;
			}

			EndAllInteractionPossibilities();
		}

		private void SetupCollider()
		{
			if (!_shouldManageCollider.Value)
			{
				return;
			}

			_collider.isTrigger = true;
			_collider.enabled = true;
		}

		public void ReceiveInteractionPossibility(IInteractionAgent interactionAgent)
		{
			if (!_controller.TryReceiveInteractionPossibility(interactionAgent))
			{
				return;
			}

			NotifyInteractionPossibilityReceived(interactionAgent);
		}

		public void EndInteractionPossibility(IInteractionAgent interactionAgent)
		{
			_controller.EndInteractionPossibility(interactionAgent);
			NotifyInteractionPossibilityEnd(interactionAgent);
		}

		public void ReceiveInteraction(IInteractionAgent interactionAgent)
		{
			if (!_controller.TryReceiveInteraction())
			{
				return;
			}

			NotifyInteractionReceived(interactionAgent);
		}

		public void EndAllInteractionPossibilities()
		{
			InteractionAgents.ForEach(NotifyInteractionPossibilityEnd);
			_controller.EndAllInteractionPossibilities();
		}
		
		private void NotifyInteractionPossibilityReceived(IInteractionAgent interactionAgent)
		{
			_onInteractionPossibilityReceived?.Invoke(interactionAgent);
			_onInteractionPossibilityReceivedGameObject?.Invoke(interactionAgent.GameObject);
		}

		private void NotifyInteractionReceived(IInteractionAgent interactionAgent)
		{
			_onInteractionReceived?.Invoke(interactionAgent);
			_onInteractionReceivedGameObject?.Invoke(interactionAgent.GameObject);
		}
		
		private void NotifyInteractionPossibilityEnd(IInteractionAgent interactionAgent)
		{
			_onInteractionPossibilityEnded?.Invoke(interactionAgent);
			_onInteractionPossibilityEndedGameObject?.Invoke(interactionAgent.GameObject);
		}
	}
}