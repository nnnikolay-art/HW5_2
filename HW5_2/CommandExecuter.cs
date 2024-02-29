using System.Collections.Concurrent;

namespace HW5_2
{
    public class CommandExecuter
    {
        private BlockingCollection<ICommand> commandQueue = new BlockingCollection<ICommand>();
        private volatile int currentQueueQty = 0;
        private volatile bool isProcessing = true;

        public void AddCommand(ICommand command)
        {
            commandQueue.Add(command);
            Interlocked.Increment(ref currentQueueQty);
        }

        public int CountCommandsInQueue()
        {
            return currentQueueQty;
        }

        public void StartExecution(int threadsRunners = 3)
        {
            for (int i = 0; i< threadsRunners; i++) 
            {
                Thread.Sleep(1);
                Task.Run(() => ExecCommands());
            }
        }

        public void ExecCommands()
        {
            ICommand command;
            while (isProcessing)
            {
                if (commandQueue.Count > 0)
                {
                    try
                    {
                        command = commandQueue.Take();
                        command.Execute();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    Interlocked.Decrement(ref currentQueueQty);
                }
            }
        }

        public void SetSoftStop()
        {
            while (currentQueueQty > 0)
            {
                Thread.Sleep(50);
            }
            isProcessing = false;
            Thread.Sleep(100);
        }

        public void SetHardStop()
        {
            isProcessing = false;
        }

        public void Resume()
        {
            isProcessing = true;
        }
    }
}
