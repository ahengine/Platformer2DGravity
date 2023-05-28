using UnityEngine;
using CC2DModule.Modules;

namespace CC2DModule
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Collider2D))]
    public class CC2D : MonoBehaviour
    {
        [SerializeField] private float horizontalSpeed = 1;

        [SerializeField] private Vector2 velocity = Vector2.zero;
        public Vector2 Velocity => velocity;
        public float VelocityX { set => velocity.x = value; get => velocity.x; }
        public float VelocityY { set => velocity.y = value; get => velocity.y; }

        public Transform Tr { private set; get; }
        private Rigidbody2D rb;
        private Collider2D col;
        internal int FaceDirection { private set; get; } = 1;
        public bool AllowAction { private set; get; } = true;

        [field: SerializeField] public Modules.CC2DModule[] Modules { private set; get; }

        public T GetModule<T>() where T : Modules.CC2DModule
        {
            for (int i = 0; i < Modules.Length; i++)
                if (Modules[i] is T)
                    return Modules[i] as T;

            return null;
        }

        private void Awake()
        {
            Tr = transform;
            col = GetComponent<Collider2D>();
            rb = GetComponent<Rigidbody2D>();

            for (int i = 0; i < Modules.Length; i++)
                Modules[i].SetOwner(this);
        }

        private void FixedUpdate()
        {
            for (int i = 0; i < Modules.Length; i++)
                if (Modules[i].IsActive)
                    Modules[i].Process();

            rb.velocity = velocity;
        }

        public void SetHorizontal(float value)
        {
            for (int i = 0; i < Modules.Length; i++)
                if (Modules[i].IsActive && !Modules[i].AllowHorizontalMove)
                    return;

            if (!AllowAction) return;

            VelocityX = value * horizontalSpeed;
            if (value != 0)
                FaceDirection = value > 0 ? 1 : -1;
        }

        public void SetAllowAction(bool value)
        {
            AllowAction = value;
            if (!AllowAction)
                VelocityX = 0;
        }

        public void SetState(bool value)
        {
            col.enabled = value;
            rb.simulated = value;
            if (!value) rb.velocity = Vector2.zero;
            AllowAction = value;
        }

        public void SetKinematic() => rb.bodyType = RigidbodyType2D.Kinematic;
        public void SetDynamic() => rb.bodyType = RigidbodyType2D.Dynamic;
        public void SetTrigger(bool isTrigger) => col.isTrigger = isTrigger;
    }
}