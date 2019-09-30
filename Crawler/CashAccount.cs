using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Que
{
    class CashAccount
    {
        private float currentBalance = 0.0f;
        private Mutex mutex = new Mutex();

        public void deposit(float value)
        {
            mutex.WaitOne();
            currentBalance += value;
            mutex.ReleaseMutex();
        }

        public float balance() {
            return currentBalance;
        }

        public void withdrawAllMoney()
        {
            currentBalance = 0.0f;
        }
    }
}
