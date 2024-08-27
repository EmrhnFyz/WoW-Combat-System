using UnityEngine;
using UnityEngine.AI;

namespace Common
{
    public class AgentMoveTo : MonoBehaviour
    {
        [SerializeField] private Transform goal;

        private NavMeshAgent agent;


        private void Start()
        {
            agent = GetComponent<NavMeshAgent>();

            agent.destination = goal?.position ?? agent.transform.position;
        }

        private void Update()
        {
            if (agent.enabled && agent.isOnNavMesh && goal != null)
            {
                agent.destination = goal.position;
            }
        }
    }
}