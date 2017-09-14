using System.Reflection;
using System.Windows.Forms;

namespace BSForms.Extensions
{
    public static class Extensions
    {
        public static void DoubleBuffered(this DataGridView dgv, bool setting)
        {
            // Taxes: Remote Desktop Connection and painting
            if (SystemInformation.TerminalServerSession)
                return;

            if (dgv == null) return;

            var pi = dgv.GetType().GetProperty("DoubleBuffered",
                                               BindingFlags.Instance | BindingFlags.NonPublic);

            if (pi != null) pi.SetValue(dgv, setting, null);
        }
    }
}
