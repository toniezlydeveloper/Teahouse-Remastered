using UnityEngine;

namespace Customers
{
    public class Table : MonoBehaviour
    {
        [SerializeField] private TableSet tables;
        [SerializeField] private Transform seat;

        public Vector3 SeatRotation => seat.rotation.eulerAngles;
        public Vector3 SeatPosition => seat.position;

        public bool IsTaken { get; set; }

        private void Start() => tables.Add(this);

        private void OnDestroy() => tables.Remove(this);
    }
}