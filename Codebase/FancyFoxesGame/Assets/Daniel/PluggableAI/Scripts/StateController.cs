using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StateController : MonoBehaviour {
	public PluggableState currentState;
	private PluggableState nextState = null;

	public MonsterStats stats;
	public Transform eyes;

	[Min(0.01f)]
	public float attackRateMultiplier = 1f;
	[Min(0.01f)]
	public float attackDamageMultiplier = 1f;

	[HideInInspector] public NavMeshAgent navMeshAgent;
	[HideInInspector] public UseElementalAbility useAbility;
	[HideInInspector] public List<Transform> wayPointList; // TODO: Future feature?
	[HideInInspector] public int nextWayPoint;
	[HideInInspector] public Transform target;
	[HideInInspector] public float stateTimeElapsed;
	[HideInInspector] public float stateTimeElapsedDelta;
	[HideInInspector] public new Renderer renderer;
	[HideInInspector] public Animator anim;

	public PluggableState.Phase CurrentPhase { get; set; } = PluggableState.Phase.Enter;

	protected virtual void Awake() {
		stats = Instantiate(stats);
	}


	protected virtual void Start() {
		navMeshAgent = GetComponent<NavMeshAgent>();
		useAbility = GetComponent<UseElementalAbility>();
		renderer = GetComponentInChildren<Renderer>();
		anim = GetComponent<Animator>();
	}

	protected virtual void Update() {
		if (navMeshAgent.enabled) {
			stateTimeElapsed += Time.deltaTime;
			stateTimeElapsedDelta += Time.deltaTime;
			if (null != nextState && PluggableState.Phase.Enter == CurrentPhase) {
				currentState = nextState;
				nextState = null;
				OnExitState();
			}
			currentState.ProcessState(this);
		}
	}

	void OnDrawGizmos() {
		if (currentState != null && eyes != null)
		{
			Gizmos.color = currentState.sceneGizmoColor;
			Gizmos.DrawWireSphere(eyes.position, Mathf.Max(transform.localScale.x, transform.localScale.y, transform.localScale.z));
			Gizmos.DrawRay(eyes.position, transform.forward * stats.lookRange);
		}
	}

	public bool TransitionToState(PluggableState pendingState) {
		if (null != pendingState && default != pendingState && pendingState != currentState) {
			nextState = pendingState;
			return true;
		}
		return false;
	}

	public bool StateElapsed(float duration) {
		return stateTimeElapsed >= duration;
	}

	public bool StateElapsedDelta(float duration) {
		bool result = stateTimeElapsedDelta >= duration;
		if (result) stateTimeElapsedDelta = 0;
		return result;
	}

	private void OnExitState() {
		stateTimeElapsed = 0;
		CurrentPhase = PluggableState.Phase.Enter;
	}
}
