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

namespace AssemblyTranslator
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        Translator translator;

        private void Init()
        {
            translator = new Translator();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Init();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string[] inputData = InputTextBox.Text.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
            string data = string.Empty;
            foreach (string input in inputData)
            {
                data += input + "\n";
            }

            MessageBox.Show("Input data:\n" + data);
            bool succesed = translator.TranslateCode(inputData, out string[] outputData, out List<string> errorList, out string[] labelsList);
            if (succesed) MessageBox.Show("Translation Succeded. :)");
            else MessageBox.Show("Translation Failed. :(");

            if (outputData == null)
            {
                MessageBox.Show("No output data.");
                return;
            }

            errorListView.Items.Clear();
            foreach (string error in errorList)
            {
                errorListView.Items.Add(error);
            }

            OutputTextBox.Clear();
            data = string.Empty;
            foreach (string output in outputData)
            {
                OutputTextBox.AppendText(output + "\n");
                data += output + "\n";
            }

            labelsListView.Items.Clear();
            foreach (string label in labelsList)
            {
                labelsListView.Items.Add(label);
            }

            MessageBox.Show("Output data:\n" + data);

        }
    }
}
