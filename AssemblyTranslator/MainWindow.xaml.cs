using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
        StringBuilder hexStringBuidler;

        private void Init()
        {
            translator = new Translator();
            hexStringBuidler = new StringBuilder(43);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Init();
        }

        private void Translate(object sender, RoutedEventArgs e)
        {
            string[] inputData = InputTextBox.Text.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);

            bool succesed = translator.TranslateCode(inputData, out UInt16[] opcodeData, out List<string> errorList, out string[] labelsList);
            if (succesed) MessageBox.Show("Translation Succeded. :)");
            else MessageBox.Show("Translation Failed. :(");

            errorListView.Items.Clear();
            foreach (string error in errorList)
            {
                errorListView.Items.Add(error);
            }

            OutputTextBox.Clear();
            foreach (UInt16 opcode in opcodeData)
            {
                OutputTextBox.AppendText(opcode.ToString("X4") + "\n");
            }

            labelsListView.Items.Clear();
            foreach (string label in labelsList)
            {
                labelsListView.Items.Add(label);
            }

        }

        private void ImportASM(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() != true) return;
            if (!openFileDialog.CheckFileExists)
            {
                MessageBox.Show($"File doesn't exist");
                return;
            }

            using (StreamReader sr = new StreamReader(openFileDialog.FileName))
            {
                InputTextBox.Text = sr.ReadToEnd();
            }
        }

        private void ExportBin(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFieDialog = new SaveFileDialog
            {
                FileName = "SourceCode.bin"
            };
            if (saveFieDialog.ShowDialog() != true) return;
            if (!saveFieDialog.CheckPathExists)
            {
                MessageBox.Show($"Path doesn't exist");
                return;
            }

            string[] inputData = InputTextBox.Text.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);

            bool succesed = translator.TranslateCode(inputData, out UInt16[] opcodeData, out List<string> errorList);
            if (succesed) MessageBox.Show("Translation Succeded. :)");
            else MessageBox.Show("Translation Failed. :(");

            errorListView.Items.Clear();
            foreach (string error in errorList)
            {
                errorListView.Items.Add(error);
            }

            if (!succesed) return;
            using (FileStream fs = new FileStream(saveFieDialog.FileName, FileMode.Create, FileAccess.Write))
            {
                using (BinaryWriter bw = new BinaryWriter(fs))
                {
                    foreach (UInt16 opcode in opcodeData)
                    {
                        bw.Write(opcode);
                    }
                }
            }
        }

        private void ExportHex(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFieDialog = new SaveFileDialog
            {
                FileName = "SourceCode.hex"
            };
            if (saveFieDialog.ShowDialog() == true)
            {
                if (!saveFieDialog.CheckPathExists)
                {
                    MessageBox.Show($"Path doesn't exist");
                }

                string[] inputData = InputTextBox.Text.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);

                bool succesed = translator.TranslateCode(inputData, out UInt16[] opcodeData, out List<string> errorList);
                if (succesed) MessageBox.Show("Translation Succeded. :)");
                else MessageBox.Show("Translation Failed. :(");

                errorListView.Items.Clear();
                foreach (string error in errorList)
                {
                    errorListView.Items.Add(error);
                }

                if (!succesed) return;
                using (StreamWriter sw = new StreamWriter(saveFieDialog.FileName, false))
                {
                    UInt16[] opcodeBuffer = new UInt16[8];
                    Address curAddress = new Address(0);
                    for(int i = 0; i < opcodeData.Length; i += 8)
                    {
                        int amount = Math.Min(opcodeData.Length - i, 8);
                        if(amount != 8) opcodeBuffer = new UInt16[amount];
                        Array.Copy(opcodeData, i, opcodeBuffer, 0, amount);
                        sw.WriteLine(GetHexLine(opcodeBuffer, curAddress));
                        curAddress += (UInt32)(amount * 2);
                    }
                    sw.WriteLine(":00000001FF"); // end of file
                }
            }
        }

        private char[] hexOpcodeBuffer = new char[4];

        private string GetHexLine(UInt16[] opcodes, Address address)
        {
            hexStringBuidler.Clear(); // clear old data
            hexStringBuidler.Append(':'); // starting value
            hexStringBuidler.Append((opcodes.Length * 2).ToString("X2")); // byte count
            byte checksum = (byte)(opcodes.Length * 2);

            hexStringBuidler.Append(address.value.ToString("X4"));
            checksum += (byte)(address.value & 0xFF);
            checksum += (byte)((address.value >> 8) & 0xFF);

            hexStringBuidler.Append("00"); // type

            foreach (UInt16 opcode in opcodes)
            {
                hexOpcodeBuffer = opcode.ToString("X4").ToArray();
                hexStringBuidler.Append(hexOpcodeBuffer, 2, 2);
                hexStringBuidler.Append(hexOpcodeBuffer, 0, 2);
                checksum += (byte)(opcode & 0xFF);
                checksum += (byte)((opcode >> 8) & 0xFF);
            }

            checksum = (byte)~checksum;
            checksum++;
            hexStringBuidler.Append(checksum.ToString("X2")); // checksum

            return hexStringBuidler.ToString();
        }
    }
}