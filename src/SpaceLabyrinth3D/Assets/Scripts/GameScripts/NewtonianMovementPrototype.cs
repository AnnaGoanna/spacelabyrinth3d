using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace GameScripts
{
    public class NewtonianMovementPrototype : MonoBehaviour
    {
        private Vector3 _force;
        private Vector3 _torque;
        private Rigidbody _rigidBody;

        public ThrustersSoundEffects SoundEffect;
        public Text HudXValues;
        public Text HudZValues;
        public Text HudYValues;

        private void Start()
        {
            _rigidBody = GetComponent<Rigidbody>();
        }

        // called every fixed framerate frame
        private void FixedUpdate()
        {
            if (Input.GetButton("Stop"))
            {
                if (SoundEffect != null) SoundEffect.PlaySound(ThrustersSoundEffects.Sound.FullStop, false);
                StopMovement();
            }
            else
            {
                AddMovement();
                if (SoundEffect != null)
                {
                    if (!Mathf.Approximately(_force.x, 0)) SoundEffect.PlaySound(ThrustersSoundEffects.Sound.MoveX);
                    else SoundEffect.StopSound(ThrustersSoundEffects.Sound.MoveX);
                    if (!Mathf.Approximately(_force.y, 0)) SoundEffect.PlaySound(ThrustersSoundEffects.Sound.MoveY);
                    else SoundEffect.StopSound(ThrustersSoundEffects.Sound.MoveY);
                    if (!Mathf.Approximately(_force.z, 0)) SoundEffect.PlaySound(ThrustersSoundEffects.Sound.MoveZ);
                    else SoundEffect.StopSound(ThrustersSoundEffects.Sound.MoveZ);

                    if (!Mathf.Approximately(_torque.x, 0)) SoundEffect.PlaySound(ThrustersSoundEffects.Sound.RotateX);
                    else SoundEffect.StopSound(ThrustersSoundEffects.Sound.RotateX);
                    if (!Mathf.Approximately(_torque.y, 0)) SoundEffect.PlaySound(ThrustersSoundEffects.Sound.RotateY);
                    else SoundEffect.StopSound(ThrustersSoundEffects.Sound.RotateY);
                    if (!Mathf.Approximately(_torque.z, 0)) SoundEffect.PlaySound(ThrustersSoundEffects.Sound.RotateZ);
                    else SoundEffect.StopSound(ThrustersSoundEffects.Sound.RotateZ);
                }
            }

            UpdateHud();
        }

        private void UpdateHud()
        {
            var textFields = new[]
            {
                HudXValues,
                HudYValues,
                HudZValues
            };

            var values = new[]
            {
                _rigidBody.velocity,
                transform.InverseTransformDirection(_rigidBody.velocity),
                transform.InverseTransformDirection(_rigidBody.angularVelocity),
                _force,
                _torque
            };

            var stringBuilders = new[]
            {
                new StringBuilder(),
                new StringBuilder(),
                new StringBuilder()
            };


            const string format = "0.00";
            foreach (var value in values)
            {
                stringBuilders[0].AppendLine(value.x.ToString(format));
                stringBuilders[1].AppendLine(value.y.ToString(format));
                stringBuilders[2].AppendLine(value.z.ToString(format));
            }

            for (int i = 0; i < textFields.Length; i++)
            {
                textFields[i].text = stringBuilders[i].ToString();
            }
        }

        private void AddMovement()
        {
            var leftRight = Input.GetAxis("Horizontal");
            var forwardBackward = Input.GetAxis("Vertical");
            var upDown = Input.GetAxis("UpDown");

            var yaw = Input.GetAxis("Yaw");
            var pitch = Input.GetAxis("Pitch");
            var roll = Input.GetAxis("Roll");

            _force = new Vector3(leftRight * 75, upDown * 75, forwardBackward * 75);
            _torque = new Vector3(pitch * 2, yaw * 2, roll * 2); // x-pitch, y-yaw, z-roll !!

            _rigidBody.AddRelativeForce(_force);
            _rigidBody.AddRelativeTorque(_torque);
        }

        public void StopMovement()
        {
            _rigidBody.velocity = Vector3.zero;
            _rigidBody.angularVelocity = Vector3.zero;
        }
    }
}