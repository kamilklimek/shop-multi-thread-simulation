using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Queue
{
    class Program
    {
        static int cashRegisterAmount = 5;
        static Barrier barrier;

        static int amountMorningClients = 25;
        static int amountAfternoonClient = 31;
        static Boolean timeOfDayChanged = false;


        static Semaphore semaphore = new Semaphore(0, 100);
        static Que.Shop shop = new Que.Shop(cashRegisterAmount);


        static List<Thread> prepareClients(int startIndex, Semaphore semaphore, Que.Shop shop, Barrier barrier, int clientAmount)
        {
            List<Thread> threads = new List<Thread>();
            for (int i = startIndex; i < clientAmount + startIndex; i++)
            {
                int money = new Random().Next(5, 125);
                Que.QueueParticipant participant = new Que.QueueParticipant(i, semaphore, money, shop, barrier);
                threads.Add(new Thread(new ThreadStart(participant.start)));
            }

            return threads;
        }

        static void timeOfDayChangingWatcher()
        {
            while (true)
            {
               if (timeOfDayChanged)
                {
                    barrier.RemoveParticipants(amountMorningClients);
                    barrier.AddParticipants(amountAfternoonClient);

                    List<Thread> threads = prepareClients(amountMorningClients + 10, semaphore, shop, barrier, amountAfternoonClient);
                    threads.ForEach(que => que.Start());
                    threads.ForEach(que => que.Join());

                    return;
                }

                Thread.Sleep(25);
            }
        }
        
        static void Main(string[] args)
        {
            barrier = new Barrier(amountMorningClients, (bar) => {
                if (shop.currentTimeOfDay == Que.TimeOfDay.MORNING)
                {
                    Console.WriteLine("=======================================");
                    Console.WriteLine("Rano zarobiono: " + shop.account.balance());
                    shop.account.withdrawAllMoney();
                    timeOfDayChanged = true;
                    shop.currentTimeOfDay = Que.TimeOfDay.AFTERNOON;
                } else if (shop.currentTimeOfDay == Que.TimeOfDay.AFTERNOON)
                {
                    Console.WriteLine("=======================================");
                    Console.WriteLine("Po południu zarobiono: " + shop.account.balance());

                    shop.currentTimeOfDay = Que.TimeOfDay.MIDNIGHT;
                    Console.WriteLine("Sklep zamknięto");
                }

            });

            new Thread(new ThreadStart(timeOfDayChangingWatcher)).Start();
            List<Thread> threads = prepareClients(0, semaphore, shop, barrier, amountMorningClients);

            threads.ForEach(que => que.Start());
            Console.WriteLine("Otworzono sklep");
            semaphore.Release(cashRegisterAmount);
            threads.ForEach(que => que.Join());

        }
    }
}
