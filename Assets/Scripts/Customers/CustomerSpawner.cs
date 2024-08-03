using System.Collections;
using System.Collections.Generic;
using Internal.Dependencies.Core;
using UI.Core;
using UnityEngine;

namespace Customers
{
    public class CustomerSpawner
    {
        private DependencyRecipe<IEnvironmentSetup> _setup = DependencyInjector.GetRecipe<IEnvironmentSetup>();
        private Customer _customerPrefab;
        private ITimePanel _timePanel;
        private SpawnData _data;

        private CustomersQueue _customersQueue;
        private Transform _spawnPoint;
        private float _startTime;
        
        public bool IsDone { get; private set; }

        public CustomerSpawner(ITimePanel timePanel, Customer customerPrefab, SpawnData data)
        {
            _customerPrefab = customerPrefab;
            _timePanel = timePanel;
            _data = data;
        }

        public IEnumerator CSpawnCustomers()
        {
            MarkDone(false);
            GetValues();
            PresentIncomingSpawns();

            for (int i = 0; i < _data.SpawnCount; i++)
            {
                SpawnCustomer();
                yield return WaitForNextSpawn();
            }

            yield return WaitForCustomersToBeGone();
            PresentSpawnsFinish();
            MarkDone(true);
        }

        public void Deinit()
        {
            PresentSpawnsFinish();
            MarkDone(true);
            ResetGameplayElements();
        }

        private void GetValues()
        {
            _customersQueue = new CustomersQueue();
            _spawnPoint = _setup.Value.SpawnPoint;
            _startTime = Time.time;
        }

        private void PresentIncomingSpawns()
        {
            float stepNormalizedDuration = _data.SpawnInterval / _data.Duration;
            List<float> customerSpawnNormalizedTimes = new List<float>();

            for (int i = 1; i < _data.SpawnCount; i++)
                customerSpawnNormalizedTimes.Add(stepNormalizedDuration * i);
            
            _timePanel.Present(customerSpawnNormalizedTimes.ToArray());
        }

        private void SpawnCustomer()
        {
            Customer customer = Object.Instantiate(_customerPrefab, _spawnPoint.position, _spawnPoint.rotation);
            customer.Init(_customersQueue);
            customer.Init(_setup.Value);
        }

        private IEnumerator WaitForNextSpawn()
        {
            float nextSpawnTime = _data.SpawnInterval + Time.time;
            
            while (Time.time < nextSpawnTime)
            {
                _timePanel.Present((Time.time - _startTime) / _data.Duration);
                yield return null;
            }
        }

        private IEnumerator WaitForCustomersToBeGone() => new WaitWhile(() => _customersQueue.Customers.Count > 0);

        private void ResetGameplayElements()
        {
            foreach (Table table in Object.FindObjectsOfType<Table>())
                table.IsTaken = false;
            
            foreach (Customer customer in Object.FindObjectsOfType<Customer>())
                Object.Destroy(customer.gameObject);
            
            _customersQueue.Customers.Clear();
        }

        private void PresentSpawnsFinish() => _timePanel.Present();

        private void MarkDone(bool state) => IsDone = state;
    }
}