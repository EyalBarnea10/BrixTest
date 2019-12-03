using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace BrixTest
{
    class Program
    {
        private static  List<object> lsobj = new List<object>();
        private static Timer aTimer;
        private static Queue myQueue = new Queue();
        static void Main(string[] args)
        {
            //1:The exercise simulates a supermarket queue - there is an interval that adds to a client's 
            //queue. And in the background, 5 TASKS runners and when any  TASK finishes he adds   another 
            //customer and takes the customer off the queue.
            //Every customer will wait between 1-5 secend beacouse all cashir runs as async
            //The line will stop only if exception has thrown - Error Occured 
            aTimer = new System.Timers.Timer();
            aTimer.Interval = 5000;
            aTimer.Elapsed += OnTimedEvent;
            aTimer.AutoReset = true;
            aTimer.Enabled = true;
            bool isRunLine = true;
            try
            {
                while (isRunLine)
                {
                    List<Task> tasks = new List<Task>();
                    tasks.Add(Task.Run(() => { GoToCashir(); }));
                    tasks.Add(Task.Run(() => { GoToCashir(); }));
                    tasks.Add(Task.Run(() => { GoToCashir(); }));
                    tasks.Add(Task.Run(() => { GoToCashir(); }));
                    tasks.Add(Task.Run(() => { GoToCashir(); }));
                    Task.WhenAny(Task.Run(() => { tasks.Add(Task.Run(() => {
                        if (myQueue.Count>0) // check if there are customers in the queue
                        {
                            myQueue.Dequeue();// remove customer from the line
                        } ; 
                        GoToCashir();
                       }));}));
                }
            }
            catch (Exception ex)
            {
                //if line cought error - stop he line 
                isRunLine = false;
            }
             
            //2:When we update the list of letters - we sort them first and update the list 
            //and then add them to DIctionary which gives us a faster search - 
            //with direct access plus sorting gives us the option to use COTAINSKEY which is a quick search method
            //The complexity is O(1) Because there is no difference in the amount of data

            Dictionary<String, String> ldapDocument = new Dictionary<String, String>();
            //load your list here
            ldapDocument.Add(SortString("BDSCA"), "BDSCA");
            ldapDocument.Add(SortString("FCDPO"), "FCDPO");
            ldapDocument.Add(SortString("ABCCD"), "ABCCD");
            ldapDocument.Add(SortString("BDSAB"), "BDSAB");
            Console.WriteLine("Insert String");
            string dataInsert = Console.ReadLine();
            if (ldapDocument.ContainsKey(SortString(dataInsert)))
            {
                Console.WriteLine(" the string " + dataInsert + " was found ");
            }
            else
            {
                Console.WriteLine(" the string " + dataInsert + " was Not Found ");
            }
        }

        /// <summary>
        /// The cashir work with the customer 
        /// </summary>
        private static void GoToCashir()
        {
            Task.Delay(5000);
        }

        /// <summary>
        /// Timer interval
        /// Fiers every 1 secend to add another customer 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void OnTimedEvent(object sender, ElapsedEventArgs e)
        {
            //check if the queue is not full - if not add new 
            if (myQueue.Count<5)
            {
                object obj = new object();
                obj = lsobj.First();// get the first customer to annd to the line 
                lsobj.RemoveAt(0);//remove the first element
                myQueue.Enqueue(obj); // add to the begginig of the line - the first customer
            }
            else
            {
                lsobj.Add(new object());
            }
        }


        /// <summary>
        /// Sort the string 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        static string SortString(string input)
        {
            char[] characters = input.ToArray();
            Array.Sort(characters);
            return new string(characters);
        }
    }
}
