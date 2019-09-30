using Queue;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Que
{
    class QueueParticipant
    {
        private int id;
        private System.Threading.Semaphore semaphore;
        private int moneyToSpend;
        private Barrier barrier;
        private Shop que;
        public QueueParticipant(int id, System.Threading.Semaphore semaphore, int moneyToSpend, Shop que, System.Threading.Barrier barrier)
        {
            this.id = id;
            this.que = que;
            this.semaphore = semaphore;
            this.moneyToSpend = moneyToSpend;
            this.barrier = barrier;
        }
        public void start()
        {
            semaphore.WaitOne();

            int cashRegisterId = que.reserveCashRegister(id);

            System.Threading.Thread.Sleep(new Random().Next(100, 4000));

            que.account.deposit(moneyToSpend);

            que.releaseCashRegister(id, cashRegisterId);

            semaphore.Release();

            barrier.SignalAndWait();
        }
    }
}
