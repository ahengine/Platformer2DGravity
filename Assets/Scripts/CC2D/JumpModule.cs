using UnityEngine;
using System;

namespace CC2DModule.Modules
{
    public class JumpModule : CC2DModule
    {
        public override bool IsActive => true;
        public override bool AllowHorizontalMove => true;

        protected override bool AllowActivateModule =>
            cc.AllowAction && jumpCounter < jumpCount;

        [SerializeField] private float jumpForce = 4;
        [SerializeField] private int jumpCount = 2;
        private int jumpCounter;

        [SerializeField] private float gravitySpeed = 5;
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private float groundCheckDistance = .05f;
        private float lastVelocityY;
        public event Action OnFall;
        public event Action OnGround;

        protected override void ApplyActivate() 
        {
            jumpCounter++;
            cc.VelocityY = jumpForce;
        }

        public override void Process()
        {
            base.Process();

            GravityProcess();
        }

        protected override void ApplyDeactivate() { }

        private void GravityProcess()
        {
            if (!IsGrounded())
            {
                cc.VelocityY -= gravitySpeed * Time.deltaTime;

                // Fall Event 
                if (cc.VelocityY < 0 && lastVelocityY >= 0)
                    OnFall?.Invoke();
                lastVelocityY = cc.VelocityY;
            }
            else if (cc.VelocityY < 0)
            {
                cc.VelocityY = 0;
                jumpCounter = 0;
                OnGround?.Invoke();
            }
        }

        public bool IsFalling() => !Physics2D.Raycast(cc.Tr.position, Vector2.down, groundCheckDistance * 3, groundLayer);

        public RaycastHit2D IsGrounded() => Physics2D.Raycast(cc.Tr.position, Vector2.down, groundCheckDistance, groundLayer);

        private void IsGroundedGizmos()
        {
            RaycastHit2D hitInfo = IsGrounded();
            Gizmos.color = hitInfo ? Color.red : Color.yellow;
            Gizmos.DrawLine(cc.Tr.position, hitInfo? hitInfo.point : cc.Tr.position - new Vector3(0, groundCheckDistance));
        }

        private void OnDrawGizmos()
        {
            if (!Application.isPlaying) return;

            IsGroundedGizmos();
        }
    }
}