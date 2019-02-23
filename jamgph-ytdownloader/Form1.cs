using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VideoLibrary;

namespace jamgph_ytdownloader
{
    public partial class Form1 : Form
    {
        string link;
        public Form1()
        {
            InitializeComponent();
        }
        

        private void button1_Click(object sender, EventArgs e)
        {
            label1.Text = "Please Wait...";
            SaveVideoToDisk(link);
        }
        void SaveVideoToDisk(string link)
        {
            var youTube = YouTube.Default; // starting point for YouTube actions
            try
            {
                var video = youTube.GetVideo(link); // gets a Video object with info about the video
                string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                path = path + @"\dl\video";
                try
                {
                    File.WriteAllBytes($@"{path}\{video.FullName}", video.GetBytes());
                }
                catch (DirectoryNotFoundException)
                {
                    Directory.CreateDirectory(path);
                    File.WriteAllBytes($@"{path}\{video.FullName}", video.GetBytes());
                }
                label1.Text = $"Saved to {path}";
                //link = "";
                //textBox1.Text = "";
            }
            catch (ArgumentException)
            {
                label1.Text = "Invalid URL";
            }
            
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            link = textBox1.Text;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
