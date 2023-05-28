using UnityEngine;
using System;

namespace CC2DModule.Modules
{
    public class GravityModule : CC2DModule
    {
        public override bool IsActive => true;
        public override bool AllowHorizontalMove => true;

        protected override bool AllowActivateModule => true;

        [SerializeField] private float gravitySpeed = -2;
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private float groundCheckDistance = .05f;
        private float lastVelocityY;
        public event Action OnFall;
        public event Action OnGround;
        private bool isGrounded;
        private Vector2 direction => gravitySpeed < 0 ? Vector2.down : Vector2.up;

        protected override void ApplyActivate() 
        {
            cc.VelocityY = 0;
        }

        public override void Process()
        {
            base.Process();
            GravityProcess();
        }

        protected override void ApplyDeactivate() { }

        private void GravityProcess()
        {
            if (!IsGrounded)
            {
                cc.VelocityY += gravitySpeed * Time.deltaTime;

                // Fall Event 
                if (cc.VelocityY < 0 && lastVelocityY >= 0)
                    OnFall?.Invoke();
                lastVelocityY = cc.VelocityY;

                isGrounded = false;
            }
            else if (!isGrounded && cc.VelocityY != 0)
            {
                isGrounded = true;
                cc.VelocityY = 0;
                OnGround?.Invoke();
            }
        }

        public bool IsFalling() => !Physics2D.Raycast(cc.Tr.position, direction, groundCheckDistance * 3, groundLayer);

        public RaycastHit2D IsGrounded => Physics2D.Raycast(cc.Tr.position, direction, groundCheckDistance, groundLayer);

        private void IsGroundedGizmos()
        {
            RaycastHit2D hitInfo = IsGrounded;
            Gizmos.color = hitInfo ? Color.red : Color.yellow;
            Gizmos.DrawLine(cc.Tr.position, hitInfo? hitInfo.point : cc.Tr.position + (Vector3)(direction * groundCheckDistance));
        }

        private void OnDrawGizmos()
        {
            if (!Application.isPlaying) return;

            IsGroundedGizmos();
        }

        public void SwapGravity()
        {
            //cc.VelocityY = 0;
            gravitySpeed = -gravitySpeed;
        }
    }
}