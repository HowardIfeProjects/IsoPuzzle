using UnityEngine;
using com.ootii.Helpers;
using com.ootii.Input;
using com.ootii.Timing;

namespace com.ootii.Actors
{
    /// <summary>
    /// The ActorDriverForAnimator is an ActorDriver that hooks into an
    /// animator controller. This allows us to use the root motion and drive
    /// the character based on that.
    /// </summary>
    [AddComponentMenu("ootii/Actor Drivers/Animator Driver")]
    public class AnimatorDriver : ActorDriver
    {
        /// <summary>
        /// Animator that the driver is tied to
        /// </summary>
        protected Animator mAnimator = null;

        /// <summary>
        /// Tracks the position based root motion so we can apply it later
        /// </summary>
        protected Vector3 mRootMotionMovement = Vector3.zero;
        public virtual Vector3 RootMotionMovement
        {
            get { return mRootMotionMovement; }
            set { mRootMotionMovement = value; }
        }

        /// <summary>
        /// Tracks the rotation based root motion so we can apply it later
        /// </summary>
        protected Quaternion mRootMotionRotation = Quaternion.identity;
        public virtual Quaternion RootMotionRotation
        {
            get { return mRootMotionRotation; }
            set { mRootMotionRotation = value; }
        }

        /// <summary>
        /// Once the objects are instanciated, awake is called before start. Use it
        /// to setup references to other objects
        /// </summary>
        protected override void Awake()
        {
            mAnimator = gameObject.GetComponent<Animator>();
            base.Awake();
        }

        /// <summary>
        /// Called every frame so the driver can process input and
        /// update the actor controller.
        /// </summary>
        protected override void Update()
        {
            if (!_IsEnabled) { return; }
            if (mAnimator == null) { return; }
            if (mActorController == null) { return; }
            if (mInputSource == null || !mInputSource.IsEnabled) { return; }

            float lDeltaTime = TimeManager.SmoothedDeltaTime;

            // Rotate based on the mouse
            Quaternion lUserRotation = Quaternion.identity;
            if (mInputSource.IsViewingActivated)
            {
                float lYaw = mInputSource.ViewX;
                lUserRotation = Quaternion.Euler(0f, lYaw * mDegreesPer60FPSTick, 0f);
            }

            Quaternion lRotation = mRootMotionRotation * lUserRotation;
            mActorController.Rotate(lRotation);

            // Move based on WASD
            Vector3 lInput = new Vector3(mInputSource.MovementX, 0f, mInputSource.MovementY);

            Vector3 lMovement = lInput * MovementSpeed * lDeltaTime;
            if (mRootMotionMovement.sqrMagnitude > 0f) { lMovement = mRootMotionMovement; }

            mActorController.RelativeMove(lMovement);

            // Tell the animator what to do next
            SetAnimatorProperties(lInput, lMovement, lRotation);
        }

        /// <summary>
        /// Provides a place to set the properties of the animator
        /// </summary>
        /// <param name="rInput">Vector3 representing the input</param>
        /// <param name="rMovement">Vector3 representing the amount of movement taking place (in local space)</param>
        /// <param name="rRotation">Quaternion representing the amount of rotation taking place</param>
        protected virtual void SetAnimatorProperties(Vector3 rInput, Vector3 rMovement, Quaternion rRotation)
        {
        }

        /// <summary>
        /// Called to apply root motion manually. The existance of this
        /// stops the application of any existing root motion since we're
        /// essencially overriding the function. 
        /// 
        /// This function is called right before Update()
        /// </summary>
        protected virtual void OnAnimatorMove()
        {
            if (Time.deltaTime == 0f) { return; }

            // Clear any root motion values
            if (mAnimator == null)
            {
                mRootMotionMovement = Vector3.zero;
                mRootMotionRotation = Quaternion.identity;
            }
            // Store the root motion as a velocity per second. We also
            // want to keep it relative to the avatar's forward vector (for now).
            // Use Time.deltaTime to create an accurate velocity (as opposed to Time.fixedDeltaTime).
            else
            {
                // Convert the movement to relative the current rotation
                mRootMotionMovement = Quaternion.Inverse(transform.rotation) * (mAnimator.deltaPosition);

                // Store the rotation as a velocity per second.
                mRootMotionRotation = mAnimator.deltaRotation;
            }
        }
    }
}
