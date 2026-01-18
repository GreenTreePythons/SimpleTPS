using _Datas;
using UnityEngine;

namespace _Scripts.Player.Controller
{
    public enum AnimationParameterType { MoveSpeed, ShootSpeed }
     
    [RequireComponent(typeof(Animator))]
    public class PlayerAnimationController : MonoBehaviour
    {
        [SerializeField] PlayerAnimations m_AnimationData;
        
        private Animator m_Animator;
        private int m_HorizontalHash;
        private int m_VerticalHash;
        private int m_SpeedHash;

        private void Awake()
        {
            m_Animator = GetComponent<Animator>();
        }

        private void Start()
        {
            m_HorizontalHash = Animator.StringToHash("Horizontal");
            m_VerticalHash = Animator.StringToHash("Vertical");
            m_SpeedHash = Animator.StringToHash("Speed");
        }
        
        public void SetLocomotion(Vector2 move, float speed, float dampTime, float dt)
        {
            m_Animator.SetFloat(m_HorizontalHash, move.x, dampTime, dt);
            m_Animator.SetFloat(m_VerticalHash,   move.y, dampTime, dt);
            m_Animator.SetFloat(m_SpeedHash,      speed, dampTime, dt);
        }

        public void PlayAnimation(PlayerStateType state)
        {
            if (m_Animator == null || m_AnimationData == null) return;

            var clip = m_AnimationData.GetClip(state);
            if (clip == null)
            {
                Debug.LogWarning($"[PlayerAnimationController] Clip not found: {state}");
                return;
            }

            m_Animator.CrossFade(clip.StateName, clip.TransitionDuration);
        }

        public void SetLayerWeight(int layerIndex, float weight)
        {
            if (layerIndex < 0 || layerIndex >= m_Animator.layerCount)
            {
                Debug.LogWarning($"[PlayerAnimationController] {layerIndex} is not a valid layer index");
                return;
            }

            m_Animator.SetLayerWeight(layerIndex, weight);
        }

        public float GetAnimationPlayTime(PlayerStateType state, float animationSpeed = 1f)
        {
            if (m_AnimationData == null)
            {
                Debug.LogWarning($"[PlayerAnimationController] AnimationData is null");
                return 0f;
            }

            var clip = m_AnimationData.GetClip(state);
            if (clip == null || clip.Clip == null)
            {
                Debug.LogWarning($"[PlayerAnimationController] animation clip is null");
                return 0f;
            }

            return clip.Clip.length / Mathf.Max(0.0001f, animationSpeed);
        }
    }
}