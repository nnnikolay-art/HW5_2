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

            // ��������� 1000 ������ � �������
            for (int i = 0; i < 1000; i++) 
            { 
                commandExecuter.AddCommand(command);
            }
            // ��������� ��������� ��������� � 3 ������
            commandExecuter.StartExecution();

            // ��������� ������� ������ ���������
            commandExecuter.SetSoftStop();

            // ���� ����������� �������
            while (commandExecuter.CountCommandsInQueue() > 0)
            {
                Thread.Sleep(500);
            } 
            // ��������� ���� ������� ����� ����������� ������� � ������� Soft-stop
            commandExecuter.AddCommand(command);
            // ���� 500 �� ��� ���������, ��� ������� �� �������������
            Thread.Sleep(500);
            // ���������, ��� ������� ��� � ����� � �������
            Assert.Equal(1, commandExecuter.CountCommandsInQueue() );
        }

        [Fact]
        public void CheckInstantStopAfterHardStop()
        {
            CommandExecuter commandExecuter = new CommandExecuter();
            ICommand command = new Command();

            // ��������� 1000000 ������ � �������
            for (int i = 0; i < 1000000; i++)
            {
                commandExecuter.AddCommand(command);
            }
            // ��������� ��������� ��������� � 3 ������
            commandExecuter.StartExecution();

            // ��������� ������� Hard ���������
            commandExecuter.SetHardStop();

            // ���� 500 �� ��� ��������� ����������
            Thread.Sleep(500);
            // ������ ���������� ����� ������ HardStop
            var countAfterHardStop = commandExecuter.CountCommandsInQueue();
            // ���� ��� 500 �� ����� ��������� ����������
            Thread.Sleep(500);
            // ���������, ��� ���������� �� ���������� �� ��������� 500 ��
            Assert.Equal(countAfterHardStop, commandExecuter.CountCommandsInQueue());
        }
    }
}