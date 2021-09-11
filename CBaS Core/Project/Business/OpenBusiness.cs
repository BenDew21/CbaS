using System.IO;
using System.Windows;
using CBaSCore.Project.Storage;

namespace CBaSCore.Project.Business
{
    public class OpenBusiness
    {
        private readonly OpenStorage storage = new();

        public void OpenFile(string fileName, string safeFileName)
        {
            DatabaseHandler.GetInstance().SetConnectionString(@"Data Source=" + fileName);
            var items = storage.GetProjectStructure();

            ProjectViewHandler.GetInstance().SetProjectDirectory(Path.GetDirectoryName(fileName));
            // Get rid of the file extension off the file
            ProjectViewHandler.GetInstance().SetProjectName(safeFileName.Split('.')[0]);
            ProjectViewHandler.GetInstance().GenerateView(items);

            if (Application.Current.MainWindow != null)
                Application.Current.MainWindow.Title = "Combinatorics Calculator - " + safeFileName;
        }
    }
}