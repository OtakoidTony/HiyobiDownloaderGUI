﻿using System;
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
using System.Net;
using System.IO;

namespace HiyobiDownloader
{
    public partial class Form1 : Form
    {

        public static PictureBox[] pictureBoxes = new PictureBox[15];
        public static int page = 1;
        public static JObject loadedDatabase = null;
        public static int selectedIndex = -1;
        public static string selectedInfo = null;
        public static JToken selectedData = null;
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
                selectedIndex = -1;
                pageLabel.Text = page + "";
                start();
            }
        }

        private void start()
        {
            string tags = textBox1.Text;
            string[] tagsStringArray = Hiyobi.tagsToArray(tags);
            string res = Hiyobi.search(tagsStringArray, page);
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
            selectedIndex = -1;
            pageLabel.Text = page + "";
            start();
        }

        private void printSelectedData(int selectedIndex)
        {
            Form1.selectedIndex = selectedIndex;
            selectedInfo = "";
            try
            {
                selectedData = loadedDatabase["list"][selectedIndex];
            }
            catch (System.NullReferenceException e)
            {
                selectedData = -1;
                debugTextBox.Text = "NullReferenceException";
                return;
            }
            catch (System.ArgumentOutOfRangeException)
            {
                selectedData = -1;
                debugTextBox.Text = "ArgumentOutOfRangeException";
                return;
            }
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
            pageLabel.Text = page + "";
            start();
        }

        private void gotoNextPageButton_Click(object sender, EventArgs e)
        {
            page += 1;
            pageLabel.Text = page + "";
            start();
        }

        private void downloadButton_Click(object sender, EventArgs e)
        {
            if (selectedIndex == -1) return;
            using (WebClient webClient = new WebClient())
            {
                debugTextBox.Text = "다운로드 시작";
                string[] filenames = Hiyobi.getImageFileName((string)selectedData["id"]);
                string galleryId = selectedData["id"] + "";
                string baseUrl = "https://cdn.hiyobi.me/data/" + galleryId + "/";

                progressBar1.Style = ProgressBarStyle.Continuous;
                progressBar1.Minimum = 0;
                progressBar1.Maximum = filenames.Length - 1;
                progressBar1.Step = 1;
                progressBar1.Value = 0;
                string sDirPath = Application.StartupPath + "\\"+ ((string)selectedData["title"]).Replace("|","｜")+" (" + (string)selectedData["id"]+")";
                DirectoryInfo di = new DirectoryInfo(sDirPath);
                if (di.Exists == false)
                {
                    di.Create();
                }
                for (int i = 0; i < filenames.Length; i++)
                {
                    webClient.DownloadFile(baseUrl + filenames[i], sDirPath +"\\"+ filenames[i]);
                    progressBar1.PerformStep();
                }
                debugTextBox.Text = "다운로드 완료";
            }
        }

        private void debugTextBox_TextChanged(object sender, EventArgs e)
        {

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

        public static string getCoverImageUrl(int number)
        {
            return "https://cdn.hiyobi.me/tn/" + number + ".jpg";
        }

        public static string getCoverImageUrl(string number)
        {
            return "https://cdn.hiyobi.me/tn/" + number + ".jpg";
        }

        public static string[] getImageUrls(string number)
        {
            string requestUrl = "https://cdn.hiyobi.me/data/json/" + number + "_list.json";
            var client = new RestClient(requestUrl);
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);
            JArray parsedArr = JArray.Parse(response.Content);
            string[] res = new string[parsedArr.Count()];
            for (var i = 0; i < res.Length; i++)
            {
                res[i] = "https://cdn.hiyobi.me/data/" + number + "/" + parsedArr[i]["name"];
            }
            return res;
        }

        public static string[] getImageFileName(string number)
        {
            string requestUrl = "https://cdn.hiyobi.me/data/json/" + number + "_list.json";
            var client = new RestClient(requestUrl);
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);
            JArray parsedArr = JArray.Parse(response.Content);
            string[] res = new string[parsedArr.Count()];
            for (var i = 0; i < res.Length; i++)
            {
                res[i] = parsedArr[i]["name"] + "";
            }
            return res;
        }
    }
}
