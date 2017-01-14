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
using System.Xml;

namespace MediaBox
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private PlayItemDataList m_playItemList = new PlayItemDataList();
        public MainWindow()
        {
            InitializeComponent();
            Binding binding = new Binding();
            binding.Source = m_playItemList;
            binding.Path = new PropertyPath("ListItems");
            m_playListView.SetBinding(ItemsControl.ItemsSourceProperty, binding);

        }

        private void OnOpen_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new System.Windows.Forms.FolderBrowserDialog();
            dialog.ShowDialog();
            var dir = dialog.SelectedPath;
            if (dir.Length == 0)
            {
                return;
            }
            m_playItemList.Clear();
 
            foreach (var file in Directory.GetFiles(dir, "*.swf", SearchOption.AllDirectories))
            {
                PlayItemData item = new PlayItemData(file);
                m_playItemList.AddItem(item);
            }

          
        }

        private void OnPlaylist_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            
            m_player.Source = new Uri(m_playItemList.GetItem((string)m_playListView.SelectedItem).Uri);
        }

        private void OnSave_PlayList(object sender, RoutedEventArgs e)
        {
            if(m_playItemList.Count == 0)
            {
                return;
            }

            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Encoding = Encoding.UTF8;
            settings.Indent = true;
            using (XmlWriter writer = XmlWriter.Create(".\\playlist.xml", settings))
            {
                writer.WriteStartElement("zf");
                foreach(var file in m_playItemList.Items)
                {
                    writer.WriteElementString("item", file.Uri);
                }
            }
        }

        private void OnOpenPlayList(object sender, RoutedEventArgs e)
        {            
            ICollection<string> files = new List<string>();
            using (XmlReader reader = XmlReader.Create(".\\playlist.xml"))
            {
                reader.MoveToContent();
                reader.ReadStartElement("zf");
                while(reader.Read())
                {
                    string s = reader.ReadInnerXml();
                    files.Add(s);
                }
            }
            if(files.Count == 0)
            {
                return;
            }
            m_playItemList.Clear();

            m_playItemList.AddItems(files);
        }
    }

}
