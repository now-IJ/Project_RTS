using UnityEngine;

namespace RS
{
    public class Ragdoll : MonoBehaviour
    {
        [SerializeField] private Transform ragdollRootBone;

        public void Setup(GameObject originalRootBone)
        {
            MatchAllChildTransforms(originalRootBone.transform, ragdollRootBone);
            ApplyExplosionToRagdoll(ragdollRootBone, 300f, transform.position,10f);
        }

        private void MatchAllChildTransforms(Transform root, Transform clone)
        {
            foreach (Transform child in root)
            {
                Transform cloneChild = clone.Find(child.name);
                if (cloneChild != null)
                {
                    cloneChild.position = child.position;
                    cloneChild.rotation = child.rotation;
                    
                    MatchAllChildTransforms(child, cloneChild);
                }
            }
        }

        private void ApplyExplosionToRagdoll(Transform root, float explosionForce, Vector3 explosionPosition, float explosionRadius)
        {
            foreach (Transform child in root)
            {
                if (child.TryGetComponent<Rigidbody>(out Rigidbody childRigidbody))
                {
                    childRigidbody.AddExplosionForce(explosionForce, explosionPosition, explosionRadius);
                }
            }
        }
    }
}