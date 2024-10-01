using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Character;
using DG.Tweening;
using Internal.Dependencies.Core;
using Items.Holders;
using Items.Implementations;
using Pathfinding;
using UnityEngine;

namespace Customers
{
    public interface ICustomer : IDependency
    {
        GameObject GameObject { get; }
        Species Species { get; }
    }
    
    public class CustomersQueue
    {
        public List<Customer> Customers { get; } = new();
    }
    
    public class Customer : ADependencyElement<ICustomer>, ICustomer
    {
        [SerializeField] private TableSet tables;
        [SerializeField] private Species species;

        private CustomersQueue _customersQueue;
        private Transform _queuePoint;
        private Transform _exitPoint;
        private int _queueIndex;
        private Table _table;
        private AIPath _aiPath;

        public GameObject GameObject => _table?.gameObject;
        public Species Species => species;

        private const float SeatRotationDuration = 1f;
        private const float PostOrderInterval = 2.5f;

        private void Start()
        {
            GetReferences();
            StartCoroutine(C_Execute());
        }

        public void Init(IEnvironmentSetup setup)
        {
            _queuePoint = setup.QueuePoint;
            _exitPoint = setup.ExitPoint;
        }

        public void Init(CustomersQueue customersQueue) => _customersQueue = customersQueue;

        private IEnumerator C_Execute()
        {
            yield return WaitInQueue();
            yield return WaitForEmptySeat();
            yield return GoToEmptySeat();
            yield return ExecuteOrder();
            yield return Leave();
        }

        private IEnumerator WaitInQueue()
        {
            SetupQueue();
            yield return MoveTo(_queuePoint.position + _queueIndex * _queuePoint.forward);
            
            while (true)
            {
                if (TryGettingFirstInQueue())
                    break;

                yield return WaitForQueueToMove();
                yield return MoveTo(_queuePoint.position + _queueIndex * _queuePoint.forward);
            }
        }

        private IEnumerator GoToEmptySeat()
        {
            TakeTable();
            yield return MoveTo(_table.SeatPosition);
            yield return RotateToSeat();
        }

        private IEnumerator Leave()
        {
            ReleaseTable();
            yield return MoveTo(_exitPoint.position);
            Destroy(gameObject);
        }

        private IEnumerator ExecuteOrder()
        {
            IItemHolder orderHolder = _table.GetComponentInChildren<IItemHolder>();
            orderHolder.Value = new Order();
            yield return new WaitWhile(() => orderHolder.CastTo<Order>()?.WasCollected != true);
            orderHolder.Value = null;
            yield return new WaitForSeconds(PostOrderInterval);
        }

        private IEnumerator MoveTo(Vector3 position)
        {
            _aiPath.destination = position;

            while (!_aiPath.reachedDestination)
                yield return null;
        }

        private void SetupQueue()
        {
            _customersQueue.Customers.Add(this);
            _queueIndex = _customersQueue.Customers.IndexOf(this);
        }

        private bool TryGettingFirstInQueue()
        {
            _queueIndex = _customersQueue.Customers.IndexOf(this);
            return _queueIndex == 0;
        }

        private IEnumerator WaitForQueueToMove() => new WaitWhile(() => _customersQueue.Customers.IndexOf(this) == _queueIndex);
        
        private IEnumerator WaitForEmptySeat()
        {
            yield return new WaitWhile(() => tables.Set.All(table => table.IsTaken));
            _customersQueue.Customers.RemoveAt(0);
        }

        private YieldInstruction RotateToSeat() => transform.DORotate(_table.SeatRotation, SeatRotationDuration).WaitForCompletion();

        private void TakeTable()
        {
            _table = tables.Set.First(table => !table.IsTaken);
            _table.IsTaken = true;
        }

        private void ReleaseTable() => _table.IsTaken = false;

        private void GetReferences() => _aiPath = GetComponent<AIPath>();
    }
}