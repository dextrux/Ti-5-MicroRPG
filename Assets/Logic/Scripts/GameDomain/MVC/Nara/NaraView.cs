using UnityEngine;
using System.Collections;

namespace Logic.Scripts.GameDomain.MVC.Nara
{
    public class NaraView : MonoBehaviour
    {
        public Transform CastPoint;
        public LineRenderer CastLineRenderer;
        public GameObject TargetPrefab;
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private Collider _collider;
        [SerializeField] private Animator _animator;

        public Rigidbody GetRigidbody()
        {
            return _rigidbody;
        }

        public Camera GetCamera()
        {
            return Camera.main;
        }

        public void SetMoving(bool isMoving)
        {
            if (_animator != null)
            {
                _animator.SetBool("Moving", isMoving);
            }
        }

        public void PlayDeath()
        {
            if (_animator != null)
            {
                _animator.SetTrigger("Dead");
            }
        }

        public void SetAttackType(int type)
        {
            if (_animator != null)
            {
                _animator.SetInteger("AKY_AttackType", type);
            }
        }

        public void ResetAttackType()
        {
            if (_animator != null)
            {
                _animator.SetInteger("AKY_AttackType", 0);
            }
        }

        public void TriggerExecute()
        {
            if (_animator != null)
            {
                _animator.SetTrigger("Execute");
				StartCoroutine(ResetTriggerNextFrame("Execute"));
            }
        }

        public void ResetExecuteTrigger()
        {
            if (_animator != null)
            {
                _animator.ResetTrigger("Execute");
            }
        }

		public void TriggerCancel()
		{
			if (_animator != null)
			{
				_animator.SetTrigger("Cancel");
				StartCoroutine(ResetTriggerNextFrame("Cancel"));
			}
		}

        public LineRenderer GetPointLineRenderer()
        {
            return CastLineRenderer;
        }

		private IEnumerator ResetTriggerNextFrame(string triggerName)
		{
			// Reset on the next frame so the Animator can consume the trigger this frame.
			yield return null;
			if (_animator != null)
			{
				_animator.ResetTrigger(triggerName);
			}
		}
    }
}
