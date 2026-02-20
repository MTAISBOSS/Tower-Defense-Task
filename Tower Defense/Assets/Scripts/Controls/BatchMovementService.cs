using System.Collections.Generic;
using Service_Locator;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using Update_Service;

namespace Controls
{
    public class BatchMovementService : MonoBehaviour, IService, IUpdate
    {
        [SerializeField] private Transform[] waypoints;
        [SerializeField] private float waypointReachedDistance = 1f;

        private readonly List<Movement> _agents = new List<Movement>();
        private NativeArray<float3> _agentPositions;
        private NativeArray<float3> _agentRotations;
        private NativeArray<float> _agentSpeeds;
        private NativeArray<int> _agentIndices;
        private NativeArray<float3> _waypointPositions;

        private void OnEnable()
        {
            ServiceLocator.Instance.Register(this);
            ServiceLocator.Instance.Get<UpdateManager>().Register(this);
            _agentPositions = new NativeArray<float3>(0, Allocator.Persistent);
            _agentRotations = new NativeArray<float3>(0, Allocator.Persistent);
            _agentSpeeds = new NativeArray<float>(0, Allocator.Persistent);
            _agentIndices = new NativeArray<int>(0, Allocator.Persistent);
        }

        private void OnDisable()
        {
            ServiceLocator.Instance.Unregister(this);
            ServiceLocator.Instance.Get<UpdateManager>().UnRegister(this);
            if (_agentPositions.IsCreated) _agentPositions.Dispose();
            if (_agentRotations.IsCreated) _agentRotations.Dispose();
            if (_agentSpeeds.IsCreated) _agentSpeeds.Dispose();
            if (_agentIndices.IsCreated) _agentIndices.Dispose();
        }

        public void Update()
        {
            if (_agents.Count == 0 || waypoints.Length == 0) return;

            if (_waypointPositions.IsCreated)
            {
                _waypointPositions.Dispose();
            }

            _waypointPositions = new NativeArray<float3>(waypoints.Length, Allocator.TempJob);
            for (int i = 0; i < waypoints.Length; i++)
            {
                _waypointPositions[i] = waypoints[i].position;
            }

            for (int i = 0; i < _agents.Count; i++)
            {
                _agentSpeeds[i] = _agents[i].GetSpeed();
            }

            MoveJob job = new MoveJob
            {
                deltaTime = Time.deltaTime,
                positions = _agentPositions,
                rotations = _agentRotations,
                speeds = _agentSpeeds,
                indices = _agentIndices,
                waypoints = _waypointPositions,
                reachDistance = waypointReachedDistance
            };

            JobHandle jobHandle = job.Schedule(_agents.Count, 64);
            jobHandle.Complete();

            for (int i = 0; i < _agents.Count; i++)
            {
                _agents[i].Move(_agentPositions[i], _agentRotations[i]);
            }

            _waypointPositions.Dispose();
        }

        public void AddAgent(Movement agent)
        {
            if (_agents.Contains(agent)) return;
            int newIndex = _agents.Count;
            _agents.Add(agent);

            var newPositions = new NativeArray<float3>(newIndex + 1, Allocator.Persistent);
            var newRotations = new NativeArray<float3>(newIndex + 1, Allocator.Persistent);
            var newSpeeds = new NativeArray<float>(newIndex + 1, Allocator.Persistent);
            var newIndices = new NativeArray<int>(newIndex + 1, Allocator.Persistent);

            if (_agentPositions.IsCreated)
            {
                NativeArray<float3>.Copy(_agentPositions, newPositions, _agentPositions.Length);
                NativeArray<float3>.Copy(_agentRotations, newRotations, _agentRotations.Length);
                NativeArray<float>.Copy(_agentSpeeds, newSpeeds, _agentSpeeds.Length);
                NativeArray<int>.Copy(_agentIndices, newIndices, _agentIndices.Length);
                _agentPositions.Dispose();
                _agentRotations.Dispose();
                _agentSpeeds.Dispose();
                _agentIndices.Dispose();
            }

            newPositions[newIndex] = agent.transform.position;
            newRotations[newIndex] = agent.transform.forward;
            newSpeeds[newIndex] = agent.GetSpeed();
            newIndices[newIndex] = 0;

            _agentPositions = newPositions;
            _agentRotations = newRotations;
            _agentSpeeds = newSpeeds;
            _agentIndices = newIndices;
        }

        public void RemoveAgent(Movement agent)
        {
            int index = _agents.IndexOf(agent);
            if (index < 0) return;
            _agents.RemoveAt(index);

            var newPositions = new NativeArray<float3>(_agents.Count, Allocator.Persistent);
            var newRotations = new NativeArray<float3>(_agents.Count, Allocator.Persistent);
            var newSpeeds = new NativeArray<float>(_agents.Count, Allocator.Persistent);
            var newIndices = new NativeArray<int>(_agents.Count, Allocator.Persistent);

            int dst = 0;
            for (int src = 0; src < _agentPositions.Length; src++)
            {
                if (src == index) continue;
                newPositions[dst] = _agentPositions[src];
                newRotations[dst] = _agentRotations[src];
                newSpeeds[dst] = _agentSpeeds[src];
                newIndices[dst] = _agentIndices[src];
                dst++;
            }

            _agentPositions.Dispose();
            _agentRotations.Dispose();
            _agentSpeeds.Dispose();
            _agentIndices.Dispose();

            _agentPositions = newPositions;
            _agentRotations = newRotations;
            _agentSpeeds = newSpeeds;
            _agentIndices = newIndices;
        }
    }

    [BurstCompile]
    public struct MoveJob : IJobParallelFor
    {
        public NativeArray<float3> positions;
        public NativeArray<float3> rotations;
        public NativeArray<float> speeds;
        public NativeArray<int> indices;
        [ReadOnly] public NativeArray<float3> waypoints;
        [ReadOnly] public float deltaTime;
        [ReadOnly] public float reachDistance;

        public void Execute(int i)
        {
            int wpIndex = indices[i];
            if (wpIndex >= waypoints.Length) return;

            float3 currentPos = positions[i];
            float3 targetPos = waypoints[wpIndex];
            float3 toTarget = targetPos - currentPos;
            float dist = math.length(toTarget);

            if (dist < reachDistance)
            {
                wpIndex = (wpIndex + 1) % waypoints.Length;
                indices[i] = wpIndex;
                targetPos = waypoints[wpIndex];
                toTarget = targetPos - currentPos;
            }

            float3 moveDir = math.normalizesafe(toTarget);
            float3 newPos = currentPos + moveDir * speeds[i] * deltaTime;
            positions[i] = newPos;
            rotations[i] = moveDir;
        }
    }
}