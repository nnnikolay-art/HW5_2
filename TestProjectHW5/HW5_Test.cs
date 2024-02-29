using HW5_2;

namespace TestProjectHW5
{
    public class HW5_Test
    {
        [Fact]
        public void CheckStopAfterSoftStop()
        {
            CommandExecuter commandExecuter = new CommandExecuter();
            ICommand command = new Command();

            // Добавляем 1000 команд в очередь
            for (int i = 0; i < 1000; i++) 
            { 
                commandExecuter.AddCommand(command);
            }
            // Запускаем обработку сообщений в 3 потока
            commandExecuter.StartExecution();

            // Запускаем команду мягкой остановки
            commandExecuter.SetSoftStop();

            // Ждем опустошения очереди
            while (commandExecuter.CountCommandsInQueue() > 0)
            {
                Thread.Sleep(500);
            } 
            // Добавляем одну команду после опустошения очереди и команды Soft-stop
            commandExecuter.AddCommand(command);
            // Ждем 500 мс для убеждения, что очередь не обрабатыватся
            Thread.Sleep(500);
            // Проверяем, что команда так и висит в очереди
            Assert.Equal(1, commandExecuter.CountCommandsInQueue() );
        }

        [Fact]
        public void CheckInstantStopAfterHardStop()
        {
            CommandExecuter commandExecuter = new CommandExecuter();
            ICommand command = new Command();

            // Добавляем 1000000 команд в очередь
            for (int i = 0; i < 1000000; i++)
            {
                commandExecuter.AddCommand(command);
            }
            // Запускаем обработку сообщений в 3 потока
            commandExecuter.StartExecution();

            // Запускаем команду Hard остановки
            commandExecuter.SetHardStop();

            // Ждем 500 мс для остановки выполнения
            Thread.Sleep(500);
            // Узнаем количество после вызова HardStop
            var countAfterHardStop = commandExecuter.CountCommandsInQueue();
            // Ждем ещё 500 мс после измерения количества
            Thread.Sleep(500);
            // Проверяем, что количество не изменилось по истечению 500 мс
            Assert.Equal(countAfterHardStop, commandExecuter.CountCommandsInQueue());
        }
    }
}