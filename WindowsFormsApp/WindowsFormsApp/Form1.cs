using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Diagnostics;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp
{
    public partial class Form1 : Form
    {
        dbWorkersEntities db = new dbWorkersEntities();
        DbSet<Workers> workers;
        Workers worker;
        static string indat;
        public Form1()
        {
            InitializeComponent();
            label2.Text = "הכנס.י טביעת אצבע ולאחר הצפצוף לחץ.י על כפתור כניסה\\יציאה";
            this.button2.Cursor = System.Windows.Forms.Cursors.No;
            this.button3.Cursor = System.Windows.Forms.Cursors.No;

            workers = db.Workers;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            label4.Text = "";
            int fP = 0;
            try
            {
                fP = int.Parse(indat);
            }
            catch (Exception)
            {

            }
            finally
            {
                worker = workers.FirstOrDefault(x => fP == x.Fingerprint);
                indat = "0";
                if (worker == null)
                {
                    label1.Text = "לא זוהתה טביעת האצבע";
                    label2.Text = "הכנס.י טביעת אצבע ולאחר הצפצוף לחץ.י על כפתור כניסה\\יציאה";
                    this.button3.Enabled = false;
                }
                else
                {
                    if (worker.NumWorker == 9)
                    {
                        this.button2.Enabled = true;
                    }
                    else
                    {
                        this.button2.Enabled = false;
                        dataGridView1.DataSource = null;
                    }
                    this.button3.Enabled = true;
                    if (worker.IsEnter == false)
                    {

                        worker.IsEnter = true;
                        label1.Text = " ברוך.ה הבא.ה " + worker.Name;
                        label2.Text = "גש.י בבקשה לעמדה " + worker.Place;

                        worker.TimeEnter = DateTime.Now;
                        worker.TimeExit = null;
                    }
                    else
                    {
                        label2.Text = "שעות עבודתך נקלטו במערכת";
                        label1.Text = " צאתך לשלום " + worker.Name;
                        worker.TimeExit = DateTime.Now;
                        worker.IsEnter = false;
                        worker.NunHours = worker.NunHours == null ? 0 : worker.NunHours;
                        worker.NunHours += Math.Abs(worker.TimeExit.Value.Hour - worker.TimeEnter.Value.Hour);
                    }
                }
            }
            
            if (DateTime.Now.Day == 10)
            {
                Salery();
            }
            if (DateTime.Now.Day == 11)
            {
                ZeroSalery();
            }
            db.SaveChanges();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = workers.ToList();
            label4.Text = "";
        }
        private void button3_Click(object sender, EventArgs e)
        {
            Salery(worker);
            dataGridView1.DataSource = null;
            label4.Text = "משכורתך עד ליום זה עומדת על סך: "+worker.Salery;
        }
        public static void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort sp = (SerialPort)sender;
            string indata = sp.ReadExisting();
            Debug.Print("Data Received:");
            //Debug.Print(indata);
            Console.WriteLine(indata);
            indat = indata;
        }
        public void ZeroSalery()
        {
            workers.ToList().ForEach(x =>
            {
                x.Salery = 0;
                x.NunHours = 0;
            }
            );
        }
        private void Salery()
        {
            workers.ToList().ForEach(x => Salery(x));
        }
        private void Salery(Workers worker)
        {
            worker.Salery = 35 * worker.NunHours;
            db.SaveChanges();
        }
    }
}
