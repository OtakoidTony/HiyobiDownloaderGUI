using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RestSharp;
using Newtonsoft.Json.Linq;

namespace HiyobiDownloader
{
    public partial class Form1 : Form
    {
        
        public static PictureBox[] pictureBoxes = new PictureBox[15];
        public static int page = 1;
        public static JObject loadedDatabase = null;
        public static int selectedIndex = 0;
        public static string selectedInfo = null;


        public Form1()
        {
            InitializeComponent();
            pictureBoxes[0] = pictureBox1;
            pictureBoxes[1] = pictureBox2;
            pictureBoxes[2] = pictureBox3;
            pictureBoxes[3] = pictureBox4;
            pictureBoxes[4] = pictureBox5;
            pictureBoxes[5] = pictureBox6;
            pictureBoxes[6] = pictureBox7;
            pictureBoxes[7] = pictureBox8;
            pictureBoxes[8] = pictureBox9;
            pictureBoxes[9] = pictureBox10;
            pictureBoxes[10] = pictureBox11;
            pictureBoxes[11] = pictureBox12;
            pictureBoxes[12] = pictureBox13;
            pictureBoxes[13] = pictureBox14;
            pictureBoxes[14] = pictureBox15;

            foreach (PictureBox i in pictureBoxes) i.SizeMode = PictureBoxSizeMode.Zoom;
            
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                page = 1;
                pageLabel.Text = page + "";
                start();
            }
        }

        private void start()
        {
            String tags = textBox1.Text;
            String[] tagsStringArray = Hiyobi.tagsToArray(tags);
            String res = Hiyobi.search(tagsStringArray, page);
            debugTextBox.Text = res;

            loadedDatabase = JObject.Parse(res);
            var test = "";
            if (loadedDatabase["errorMsg"] != null)
            {
                debugTextBox.Text = (string)loadedDatabase["errorMsg"];
                return;
            }
            foreach (var i in loadedDatabase["list"])
            {
                test += i["id"];
                test += Environment.NewLine;
            }
            for (var i = 0; i < pictureBoxes.Length; i++)
            {
                pictureBoxes[i].SizeMode = PictureBoxSizeMode.Zoom;
                try
                {
                    pictureBoxes[i].Load(Hiyobi.getCoverImageUrl((string)loadedDatabase["list"][i]["id"]));
                }
                catch (Exception)
                {
                    pictureBoxes[i].Image = null;
                }

            }
        }
        

        private void button1_Click(object sender, EventArgs e)
        {
            page = 1;
            pageLabel.Text = page + "";
            start();
        }

        private void printSelectedData(int selectedIndex)
        {
            selectedInfo = "";
            JToken selectedData = loadedDatabase["list"][selectedIndex];
            selectedInfo += "[id]" + Environment.NewLine;
            selectedInfo += selectedData["id"] + Environment.NewLine + Environment.NewLine;
            selectedInfo += "[title]" + Environment.NewLine;
            selectedInfo += ((string)selectedData["title"]).Replace("&amp;", "&") + Environment.NewLine + Environment.NewLine;
            selectedInfo += "[artists]" + Environment.NewLine;
            foreach (var i in selectedData["artists"])
            {
                selectedInfo += i["value"] + Environment.NewLine;
            }
            selectedInfo += Environment.NewLine + "[groups]" + Environment.NewLine;
            foreach (var i in selectedData["groups"])
            {
                selectedInfo += i["value"] + Environment.NewLine;
            }
            selectedInfo += Environment.NewLine + "[parodys]" + Environment.NewLine;
            foreach (var i in selectedData["parodys"])
            {
                selectedInfo += i["value"] + Environment.NewLine;
            }
            selectedInfo += Environment.NewLine + "[characters]" + Environment.NewLine;
            foreach (var i in selectedData["characters"])
            {
                selectedInfo += i["value"] + Environment.NewLine;
            }
            selectedInfo += Environment.NewLine + "[tags]" + Environment.NewLine;
            foreach (var i in selectedData["tags"])
            {
                selectedInfo += i["value"] + Environment.NewLine;
            }
            debugTextBox.Text = selectedInfo;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            printSelectedData(0);
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            printSelectedData(1);
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            printSelectedData(2);
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            printSelectedData(3);
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            printSelectedData(4);
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            printSelectedData(5);
        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {
            printSelectedData(6);
        }

        private void pictureBox8_Click(object sender, EventArgs e)
        {
            printSelectedData(7);
        }

        private void pictureBox9_Click(object sender, EventArgs e)
        {
            printSelectedData(8);
        }

        private void pictureBox10_Click(object sender, EventArgs e)
        {
            printSelectedData(9);
        }

        private void pictureBox11_Click(object sender, EventArgs e)
        {
            printSelectedData(10);
        }

        private void pictureBox12_Click(object sender, EventArgs e)
        {
            printSelectedData(11);
        }

        private void pictureBox13_Click(object sender, EventArgs e)
        {
            printSelectedData(12);
        }

        private void pictureBox14_Click(object sender, EventArgs e)
        {
            printSelectedData(13);
        }

        private void pictureBox15_Click(object sender, EventArgs e)
        {
            printSelectedData(14);
        }

        private void pageLabel_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void gotoPrevPageButton_Click(object sender, EventArgs e)
        {
            if (page == 1) return;
            page -= 1;
            pageLabel.Text = page+"";
            start();
        }

        private void gotoNextPageButton_Click(object sender, EventArgs e)
        {
            page += 1;
            pageLabel.Text = page + "";
            start();
        }
    }

    class Hiyobi
    {
        public static string[] tagsToArray(string tags)
        {
            return tags.Split(' ').Except(new List<string> { string.Empty }).ToArray();
        }

        private static string makeBody(string[] tags, int page)
        {
            var body = new JObject();
            body.Add("search", JArray.FromObject(tags));
            body.Add("paging", page);
            return body.ToString();
        }

        public static string search(string[] tags, int page)
        {
            var client = new RestClient("https://api.hiyobi.me/search");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);

            var body = makeBody(tags, page);
            request.AddParameter("application/json", body, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            return response.Content;
        }

        public static String getCoverImageUrl(int number)
        {
            return "https://cdn.hiyobi.me/tn/" + number + ".jpg";
        }

        public static String getCoverImageUrl(string number)
        {
            return "https://cdn.hiyobi.me/tn/" + number + ".jpg";
        }
    }
}
