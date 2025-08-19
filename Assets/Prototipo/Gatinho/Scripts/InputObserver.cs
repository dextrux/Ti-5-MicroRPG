using System;
using UnityEngine;

namespace Proto_Samuel
{
    public class InputObserver : MonoBehaviour
    {
        public static InputObserver Instance;

        #region // Actions

        public Action OnSpaceDown;

        #endregion

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(this);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
                OnSpaceDown?.Invoke();
        }
    }
}
