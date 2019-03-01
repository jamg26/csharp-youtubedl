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
using System.Data.SqlClient;
using System.Data.SqlTypes;
using youtube_crawler;

namespace jamgph_ytdownloader
{
    public partial class Form1 : Form
    {
        string link;
        SqlConnection conn = ClassDb.getConnection();
        public Form1()
        {
            InitializeComponent();
        }
        

        private void button1_Click(object sender, EventArgs e)
        {
            label1.Text = "Starting Please wait...";
            string type = convSelect.Text;
            button1.Hide();
            Task.Factory.StartNew(() => SaveVideoToDisk(link, type));
        }
        void SaveVideoToDisk(string link, string type)
        {
            var youTube = YouTube.Default; // starting point for YouTube actions
            try
            {
                var video = youTube.GetVideo(link); // gets a Video object with info about the video
                string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                string path2 = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                path = path + @"\dl\video";
                path2 = path2 + @"\dl\music";
                Directory.CreateDirectory(path);
                Directory.CreateDirectory(path2);
                try
                {
                    File.WriteAllBytes($@"{path}\{video.FullName}", video.GetBytes());
                    if(type == "MP3")
                    {
                        string source_filename = $@"{path}\{video.FullName}";
                        string target_filename = $@"{path2}\{video.FullName}.mp3";
                        cs_ffmpeg_mp3_converter.FFMpeg.Convert2Mp3(source_filename, target_filename);
                        File.Delete($@"{path}\{video.FullName}");
                    }
                }
                catch (DirectoryNotFoundException)
                {
                    Invoke(new Action(() => {
                        label1.Text = $"Folder is missing!";
                        button1.Show();
                    }));
                }
                Invoke(new Action(() => {
                    label1.Text = $"Saved to desktop 'dl' folder";
                    button1.Show();
                }));
            }
            catch (ArgumentException)
            {
                Invoke(new Action(() => {
                    label1.Text = "Invalid URL";
                    button1.Show();
                }));
                
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            link = textBox1.Text;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Created by: JAMG\nhttps://jamgph.com", "About", MessageBoxButtons.OK, MessageBoxIcon.Question);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            conn.Open();
            SqlCommand cmd = new SqlCommand("SELECT status FROM ytcrawler WHERE Id = 1", conn);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            conn.Close();
            string stat = $"{dt.Rows[0]["status"]}";
            if(stat != "true")
            {
                MessageBox.Show("Something went wrong!");
                this.Close();
            }

        }

    }
}
