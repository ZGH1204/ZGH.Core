using System.Collections;
using UnityEngine;

namespace ZGH.Core
{
    [DisallowMultipleComponent]
    public class DelayDestroy : MonoBehaviour
    {
        public float delay = 3f;

        private IEnumerator Start()
        {
            yield return null;
            Destroy(transform.gameObject, delay);
        }
    }
}