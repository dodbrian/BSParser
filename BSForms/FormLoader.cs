using System.Xml;
using WeifenLuo.WinFormsUI.Docking;

namespace BSForms
{
    /// <summary>
    /// Determines the type of the file being opened
    /// and returns the appropriate Form object
    /// </summary>
    public static class FormLoader
    {
        public static DockContent FindForm(string fileName)
        {
            // Check whether the file is:
            // * Rules specification files
            var rdr = XmlReader.Create(fileName);

            try
            {
                while (rdr.Read())
                {
                    if (rdr.NodeType != XmlNodeType.Comment) continue;

                    if (!rdr.Value.Contains("BSParser Rules")) continue;

                    var rules = new RulesForm();
                    rules.LoadFile(fileName);
                    return rules;
                }
            }
            catch
            {
            }
            finally
            {
                rdr.Close();
            }


            // * Statement file
            // (no checkings so far)
            var form = new StatementForm();
            form.LoadFile(fileName);            
            
            return form;
        }
    }
}
