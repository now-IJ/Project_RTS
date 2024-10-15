using System;
using UnityEngine;

namespace RS
{
    public class UnitRagdollSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject ragdollPrefab;
        [SerializeField] private GameObject rootBone;
        private UnitHealthSystem healthSystem;

        private void Awake()
        {
            healthSystem = GetComponent<UnitHealthSystem>();
        }

        private void Start()
        {
            healthSystem.ON_UNIT_DEATH += HealthSystem_OnUnitDeath;
        }

        private void HealthSystem_OnUnitDeath(object sender, EventArgs e)
        {
            GameObject ragdollObject = Instantiate(ragdollPrefab, transform.position, transform.rotation);
            Ragdoll ragdoll = ragdollObject.GetComponent<Ragdoll>();
            ragdoll.Setup(rootBone);
        }
    }
}