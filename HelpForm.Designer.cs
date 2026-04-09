namespace TimerResolutionApp
{
    partial class HelpForm
    {
        private System.ComponentModel.IContainer components = null;
        private TextBox txtHelp;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            txtHelp = new TextBox();
            SuspendLayout();
            txtHelp.Dock = DockStyle.Fill;
            txtHelp.Multiline = true;
            txtHelp.ReadOnly = true;
            txtHelp.ScrollBars = ScrollBars.Vertical;
            txtHelp.Font = new Font("Segoe UI Variable Text", 10F);
            txtHelp.BorderStyle = BorderStyle.None;
            txtHelp.Text =
@"¿Qué es la resolución del temporizador?

Windows usa un temporizador del sistema para planificar tareas, Sleep(), y eventos de tiempo. Por defecto la resolución suele ser ~15,6 ms (64 Hz). Eso significa que Sleep(1) puede durar hasta ~15 ms en el peor caso.

Al establecer la resolución al MÍNIMO (0,5 ms):
• Sleep(1) y temporizadores pueden despertar en ~1 ms.
• Menor latencia en juegos y audio.
• Mediciones de tiempo más precisas.

Al usar POR DEFECTO:
• Menor consumo (menos interrupciones del temporizador).
• Recomendado en portátil con batería.

Maximum = 0,5 ms (mínimo posible).
Default = restaura el valor normal de Windows (~15,6 ms).
Cerrar siempre restaura antes de salir.

Bandeja del sistema:
• El icono muestra en el tooltip si estás en alta precisión o en modo habitual y la resolución actual aproximada.
• Clic derecho → «Copiar información de diagnóstico» genera un texto con versión de la app, build de Windows y valores del temporizador (para soporte o incidencias).

La última resolución elegida en el menú desplegable (0,5 ms, 1 ms, etc.) se recuerda al abrir la aplicación de nuevo.

Opciones del pie de ventana:
• «Confirmar al cerrar si hay alta precisión»: al usar la X de la ventana con el temporizador aún en ~0,5 ms, se pregunta antes de salir (siempre se restaura el valor habitual al cerrar).

Los atajos también se muestran en una línea bajo la lista de botones.";
            Padding = new Padding(14, 12, 14, 12);
            ClientSize = new Size(560, 380);
            Controls.Add(txtHelp);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Ayuda - Timer Resolution";
            ResumeLayout(false);
        }
    }
}
