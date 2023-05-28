using System;
using UnityEngine;

namespace CC2DModule.Modules
{
    public abstract class CC2DModule : MonoBehaviour
    {
        protected CC2D cc;
        public virtual bool AllowHorizontalMove => false;

        public virtual bool IsActive { protected set; get; }
        protected float startTime;

        [SerializeField] protected CC2DModule[] activateDependencies;

        public event Action OnComplete;

        public bool AllowActivate()
        {
            if (!AllowActivateModule) return false;

            for (int i = 0; i < activateDependencies.Length; i++)
                if (activateDependencies[i].IsActive) return false;

            return true;
        }
        protected virtual bool AllowActivateModule { set; get; }

        public virtual void SetOwner(CC2D owner)
            => cc = owner;

        public void DoActivate()
        {
            if (!AllowActivate()) return;

            IsActive = true;
            startTime = Time.time;

            ApplyActivate();
        }

        protected abstract void ApplyActivate();

        public virtual void Process()
        {

        }

        public void DoDeactivate()
        {
            if (!IsActive) return;

            OnComplete.Invoke();
            IsActive = false;
            ApplyDeactivate();
        }

        protected abstract void ApplyDeactivate();

    }
}
