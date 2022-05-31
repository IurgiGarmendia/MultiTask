using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MultiTask
{
    public partial class Form1 : Form
    {
        int i = 0;
        string mezua = "";
        CancellationTokenSource cancellationToken;//=new CancellationTokenSource();
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SomeTask(i);
            i++;
        }

        //https://docs.microsoft.com/en-us/answers/questions/186037/taskrun-without-wait.html
        public Task SomeTask(int i)
        {
            return  Task.Run( () =>
            {
                //await Task.Delay(4000);
                Thread.Sleep(2000);
                string message = "Simple MessageBox "+i;
                MessageBox.Show(message);
                //return true;
            });

        }

        private async void button2_ClickAsync(object sender, EventArgs e)
        {
            //button2.Enabled = false;
            //int j = listView1.SelectedItems[0].Index;
            int j = dataGridView1.SelectedRows[0].Index;
            //string textua = listView1.SelectedItems[0].SubItems[0].Text;
            //listBox1.SelectedIndex;

            dataGridView1.Rows[j].DefaultCellStyle.BackColor = Color.Bisque;

            //listView1.Items[j].BackColor = Color.Bisque;
            //listView1.BeginInvoke(new Action(()=>
            //{
            //    listView1.Items[j].BackColor = Color.Bisque;
            //}));
            //Application.DoEvents();
            cancellationToken = new CancellationTokenSource();

            //var allTasks = new List<Task>();
            //var throttler = new SemaphoreSlim(initialCount: 20);


            bool er = true;
            try
            {
                er = await SomeTaskAwait("textua", j);
            }
            catch (OperationCanceledException)
            {
                er = false;

            }
            //catch (ObjectDisposedException ode)
            //{
            //    //er = false;
            //}
            //catch (Exception ez)
            //{
            //    MessageBox.Show("big ezception " + j + "valor i: " + i);
            //}
            finally
            {
                if (cancellationToken != null)
                    cancellationToken.Dispose();
            }

            if (er)
                dataGridView1.Rows[j].DefaultCellStyle.BackColor = Color.Brown;
            else
                dataGridView1.Rows[j].DefaultCellStyle.BackColor = Color.White;

            //if(er)
            //    MessageBox.Show(mezua);
            //listView1.Items[j].BackColor = Color.Brown;
            //Application.DoEvents();
            i++;
            //if (cancellationToken != null)
            //    cancellationToken = null;
            //MessageBox.Show("tarea finalizada "+j+"valor i: "+i);
            //button2.Enabled = true;
        }


        public async Task<bool> SomeTaskAwait(string textua, int j)
        {
            //https://csharp.hotexamples.com/es/examples/System.Windows.Forms/ListView/BeginInvoke/php-listview-begininvoke-method-examples.html
            //https://social.msdn.microsoft.com/Forums/en-US/58fbf608-255c-4410-80c9-fe91a380ef5d/how-to-update-ui-from-async-routine-without-begininvoke-or-invoke?forum=csharpgeneral
            //https://stephenhaunts.com/2014/10/14/using-async-and-await-to-update-the-ui-thread/comment-page-1/

            return await Task.Run(async () =>
            //return await Task.Factory.StartNew(() =>
            {
                //listView1.Items[j].BackColor = Color.Bisque;

                //listView1.BeginInvoke(new Action(() =>
                //{
                //    listView1.Items[j].BackColor = Color.Bisque;
                //}));

                //label1.Invoke(new Action(() =>
                //{
                //    label1.Text="hasi";
                //}));

                //dataGridView1.BeginInvoke(new Action(() =>
                //{
                //    dataGridView1.Rows[j].DefaultCellStyle.BackColor = Color.Bisque;
                //}));


                //await Task.Delay(8000);
                Thread.Sleep(8000);
                //https://www.codeproject.com/Questions/1243163/Csharp-how-to-cancle-task-run-by-click-on-another
                if(cancellationToken.Token.IsCancellationRequested)
                    cancellationToken.Token.ThrowIfCancellationRequested();
                //Console.WriteLine("hello");
                //string message = "Simple MessageBox " + i;
                //MessageBox.Show(textua);
                //mezua = "asycketik dator";
                //listView1.Items[j].BackColor = Color.Brown;

                //listView1.BeginInvoke(new Action(() =>
                //{
                //    listView1.Items[j].BackColor = Color.Brown;
                //}));
                label1.Invoke(new Action(() =>
                {
                    label1.Text = "bukatu";
                }));

                //dataGridView1.BeginInvoke(new Action(() =>
                //{
                //    dataGridView1.Rows[j].DefaultCellStyle.BackColor = Color.Brown;
                //}));

                return true;
            } );

        }

        private void Form1_Load(object sender, EventArgs e)
        {
          
            listBox1.Items.Add("tori bat");
            listBox1.Items.Add("tori bi");
            listBox1.Items.Add("tori hiru");
            listBox1.Items.Add("tori lau");


            listView1.Items.Add("tori bat");
            listView1.Items.Add("tori bi");
            listView1.Items.Add("tori hiru");
            listView1.Items.Add("tori lau");

            dataGridView1.DataSource = GetEmpList();
        }


        private void listBox1_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();

            bool isItemSelected = ((e.State & DrawItemState.Selected) == DrawItemState.Selected);
            int itemIndex = e.Index;
            if (itemIndex >= 0 && itemIndex < listBox1.Items.Count)
            {
                Graphics g = e.Graphics;

                // Background Color
                SolidBrush backgroundColorBrush = new SolidBrush((isItemSelected) ? Color.Red : Color.White);
                g.FillRectangle(backgroundColorBrush, e.Bounds);

                // Set text color
                string itemText = listBox1.Items[itemIndex].ToString();

                SolidBrush itemTextColorBrush = (isItemSelected) ? new SolidBrush(Color.White) : new SolidBrush(Color.Black);
                g.DrawString(itemText, e.Font, itemTextColorBrush, listBox1.GetItemRectangle(itemIndex).Location);

                // Clean up
                backgroundColorBrush.Dispose();
                itemTextColorBrush.Dispose();
            }

            e.DrawFocusRectangle();
        }



        protected List<Emp> GetEmpList()
        {
            List<Emp> lEmp = new List<Emp>();
            Emp oemp = new Emp(1234, "Devesh Omar", "GZB");
            lEmp.Add(oemp);
            oemp = new Emp(1234, "ROLI", "GZB");
            lEmp.Add(oemp);
            oemp = new Emp(1235, "ROLI", "MainPuri");
            lEmp.Add(oemp);
            oemp = new Emp(1236, "ROLI", "Kanpur");
            lEmp.Add(oemp);
            oemp = new Emp(1237, "Manish Omar", "GZB");
            lEmp.Add(oemp);
            oemp = new Emp(1238, "ROLI1", "MainPuri");
            lEmp.Add(oemp);
            oemp = new Emp(1239, "ROLI2", "MainPuri");
            lEmp.Add(oemp);
            oemp = new Emp(1230, "ROLI3", "CNB");
            lEmp.Add(oemp);
            oemp = new Emp(1231, "ROLI4", "CNB-UP");
            lEmp.Add(oemp);
            oemp = new Emp(1232, "ROLI5", "GHAZIABAD");
            lEmp.Add(oemp);
            oemp = new Emp(1233, "ROLI6", "UP");
            lEmp.Add(oemp);
            return lEmp;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (cancellationToken != null)
                cancellationToken.Cancel();
        }
    }


    public class Emp
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public Emp(int id, string name, string city)
        {
            this.ID = id;
            this.Name = name;
            this.City = city;
        }
    }
}
