using System.Threading;

namespace TimerResolutionApp
{
    internal static class Program
    {
        private const string SingleInstanceMutexName = @"Local\TimerResolutionApp_KalurStudio";

        [STAThread]
        static void Main()
        {
            using var singleton = new Mutex(initiallyOwned: true, name: SingleInstanceMutexName, createdNew: out bool createdNew);
            if (!createdNew)
            {
                MessageBox.Show(
                    "Timer Resolution ya está en ejecución. Revisa la bandeja del sistema o la barra de tareas.",
                    "Timer Resolution",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }

            ApplicationConfiguration.Initialize();
            Application.Run(new Form1());
        }
    }
}
