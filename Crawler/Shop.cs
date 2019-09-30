using System;
using System.Collections.Generic;
using System.Text;

namespace Que
{
    public enum TimeOfDay { MORNING, AFTERNOON, MIDNIGHT };
    class Shop
    {

        public Que.CashAccount account = new Que.CashAccount();
        public Boolean[] que;
        public TimeOfDay currentTimeOfDay = TimeOfDay.MORNING;

        public Shop(int cashRegisterCount)
        {
            this.que = new Boolean[cashRegisterCount];
            for (int i=0;i<que.Length;i++)
            {
                que[i] = false;
            }
        }

        public int reserveCashRegister(int clientId)
        {

            lock (que)
            {
                for (int i = 0; i < que.Length; i++)
                {
                    if (!que[i])
                    {
                        Console.WriteLine("Klient od ID: " + clientId + ", podszedł do kasy numer: " + i);
                        que[i] = true;
                        return i;
                    }
                }
            }

            return -1;
        }        


        public void releaseCashRegister(int clientId, int cashRegisterId)
        {
            lock (que)
            {
                Console.WriteLine("Klient od ID: " + clientId + ", odszedł od kasy numer: " + cashRegisterId);
                que[cashRegisterId] = false;
            }
        }

    }
}
