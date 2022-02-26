using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WorksetChanger
{
    /// <summary>
    /// Interaction logic for worksetUI.xaml
    /// </summary>
    public partial class worksetUI : Window
    {
        private WorksetSahin Model { get; set; }
        public worksetUI(WorksetSahin worksetSahin)
        {
            Model = worksetSahin;
            InitializeComponent();
            onLoad();

        }
        public worksetUI()
        {
          
            InitializeComponent();
            onLoad();

        }

        private void onLoad()
        {
            var categories = Enum.GetNames(typeof(BuiltInCategory)).ToList();
            categories.ForEach(x =>
            {
                cmb1.Items.Add(x);
            });
            Model.worksets.ForEach(x =>
            {
                cmb2.Items.Add(x.Name);
            });
            if (cmb1.Items.Count > 0)
            {
                cmb1.SelectedIndex = 0;
            }
            if (cmb2.Items.Count > 0)
            {
                cmb2.SelectedIndex = 0;
            }


        }

        private void btn1_Click(object sender, RoutedEventArgs e)
        {
            var s = Enum.TryParse<BuiltInCategory>( cmb1.SelectedItem.ToString(),out var outCategory);
            Model.builtInCategory = outCategory;
            Model.ws = Model.worksets.FirstOrDefault(x => x.Name == cmb2.SelectedItem.ToString());
            Close();
        }
    }
}
