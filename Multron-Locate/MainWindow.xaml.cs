using System.IO;
using System.Text;
using System.Threading.Channels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Multron_Locate
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ButtonSearch.IsEnabled = true;
            ButtonStop.IsEnabled = false;
            textBox1.KeyDown += textBox1_KeyDown;
            textBox2.KeyDown += textBox2_KeyDown;
        }
        byte cancel = 0;
        private void CopyToClipboard_Click(object sender, RoutedEventArgs e)
        {
            if (listBox1.SelectedItem != null)
            {
                Clipboard.SetText(listBox1.SelectedItem.ToString());
                MessageBox.Show("Item copied to clipboard: " + listBox1.SelectedItem.ToString());
            }
            else
            {
                MessageBox.Show("No item selected to copy.");
            }
        }
        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
          
            if (e.Key == Key.Enter)
            {


                searchstart();
                e.Handled = true;
            }
        }
        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.Key == Key.Enter)
            {


                searchstart();
                e.Handled = true;
            }
        }
        public async Task search(string dir, string fofsearch)
        {
            await Task.Run(async () =>
            {
                try
                {
                    string[] files = Directory.GetFiles(dir);
                    if (files.Length > 0)
                    {
                        foreach (string file in files)
                        {
                            if (cancel == 1)
                            {
                                break;
                            }
                            try
                            {
                                if (file.Contains(fofsearch))
                                {
                                    await this.Dispatcher.InvokeAsync(() =>
                                    {
                                        listBox1.Items.Add(file.ToString());
                                    });
                                     
                                    
                                }


                            }
                            catch (Exception)
                            { }

                        }
                    }
                    string[] directories = Directory.GetDirectories(dir);
                    foreach (string ddir in directories)
                    {
                        if (cancel == 1)
                        {
                            break;
                        }
                        _ = search(ddir, fofsearch);
                    }


                }
                catch (Exception)
                {

                }


            });

        }
        public void searchstart() {
            if (Directory.Exists(textBox1.Text))
            {
                if (textBox1.Text != "" && textBox2.Text != "")
                {
                    cancel = 0;
                    ButtonSearch.IsEnabled = false;
                    ButtonStop.IsEnabled = true;
                    _ = search(textBox1.Text, textBox2.Text);
                }
            }
            else
            {
                MessageBox.Show("Directory not exists");
            }

        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            searchstart();
      
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
             listBox1.Items.Clear();
        }

        private void ButtonStop_Click(object sender, RoutedEventArgs e)
        {
            ButtonSearch.IsEnabled = true;
            ButtonStop.IsEnabled = false;
            cancel = 1;
        }
    }
}