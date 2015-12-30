using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZedGraph;

namespace Mamdani_Fuzzy
{
    public partial class Form1 : Form
    {
        int size = 300;
        double[] x, y, xL, yL;
        Membership m;
        FuzzySet fuzzy;
        DeFuzzySet deFuzzy;
        //string[] pressure = new string[] { "Low", "Medium", "High" };
        //int[] low, med, hi, hot, warm, cold;
        List<int[]> data;
        string[] keys;
        //string[] temperature = new string[] { "Hot", "Warm", "Cold" };
        string[] output = new string[] { "HP_HH_Death", "Comfort", "LP_LH_Death" };


        public Form1()
        {
            m = new Membership();
            data = new List<int[]>();
            data.Add(new int[] { 0, 0, 760 });
            data.Add(new int[] { 473, 760, 1520 });
            data.Add(new int[] { 1300, 3700, 6000 });
            data.Add(new int[] { -100, -100, 18 });
            data.Add(new int[] { -30, 28, 35 });
            data.Add(new int[] { 30, 60, 100 });

            keys = new string[] { "Low", "Medium", "High", "Cold","Warm", "Hot" };
            fuzzy = new FuzzySet();
            int ct = 0;
            foreach (string obj in keys)
            {
                fuzzy.Set.Add(obj, data[ct]);
                ct++;
            }
            x = new double[size];
            y = new double[size];
            xL = new double[size];
            yL = new double[size];
            
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            GraphPane pane1, pane2, pane3;
            dataGridView1.Rows.Clear();
            zedGraphControl1.GraphPane = new GraphPane(zedGraphControl1.GraphPane.Rect, "Pressure", "m(Pressure)", "Pressure");
            zedGraphControl2.GraphPane = new GraphPane(zedGraphControl2.GraphPane.Rect, "Temperature", "m(temp)", "Temperature");

            pane1 = zedGraphControl1.GraphPane;
            pane2 = zedGraphControl2.GraphPane;
            pane3 = zedGraphControl3.GraphPane;
            pane1.CurveList.Clear();
            pane2.CurveList.Clear();
            pane3.CurveList.Clear();

            PointPairList list1, list2, list3, list4, list5, list6;
            list1 = new PointPairList();
            list2 = new PointPairList();
            list3 = new PointPairList();
            list4 = new PointPairList();
            list5 = new PointPairList();
            list6 = new PointPairList();

            Random rn = new Random();
            dataGridView1.Columns.Add("X", "Pressure");
            dataGridView1.Columns.Add("Y", "Temperature");
            double[,] membershipX = new double[x.Length,3];
            double[,] membershipY = new double[x.Length,3];

            for (int i=0; i<size; i++)
            {
                x[i] = Math.Round(100 / rn.NextDouble(),0);// % 0.5;
                y[i] = rn.Next(-100, 100);
                if (y[i] < -273)
                    y[i] = -273;
                if (x[i] > 6000)
                    x[i] = 6000;// % 0.5;
                membershipX[i, 0] = m.trimf(x[i], fuzzy.Set[keys[0]][0], fuzzy.Set[keys[0]][1], fuzzy.Set[keys[0]][2]);
                membershipX[i, 1] = m.trimf(x[i], fuzzy.Set[keys[1]][0], fuzzy.Set[keys[1]][1], fuzzy.Set[keys[1]][2]);
                membershipX[i, 2] = m.trimf(x[i], fuzzy.Set[keys[2]][0], fuzzy.Set[keys[2]][1], fuzzy.Set[keys[2]][2]);
                list1.Add(x[i], membershipX[i, 0]);
                list2.Add(x[i], membershipX[i, 1]);
                list3.Add(x[i], membershipX[i, 2]);

                membershipY[i, 0] = m.trimf(y[i], fuzzy.Set[keys[3]][0], fuzzy.Set[keys[3]][1], fuzzy.Set[keys[3]][2]);
                membershipY[i, 1] = m.trimf(y[i], fuzzy.Set[keys[4]][0], fuzzy.Set[keys[4]][1], fuzzy.Set[keys[4]][2]);
                membershipY[i, 2] = m.trimf(y[i], fuzzy.Set[keys[5]][0], fuzzy.Set[keys[5]][1], fuzzy.Set[keys[5]][2]);
                list4.Add(y[i], membershipY[i, 0]);
                list5.Add(y[i], membershipY[i, 1]);
                list6.Add(y[i], membershipY[i, 2]);
                
                dataGridView1.Rows.Add(new string[] { x[i].ToString(), y[i].ToString() });
            }

            list1.Sort(SortType.XValues);
            list2.Sort(SortType.XValues);
            list3.Sort(SortType.XValues);
            list4.Sort(SortType.XValues);
            list5.Sort(SortType.XValues);
            list6.Sort(SortType.XValues);
            pane2.XAxis.Scale.Min = -100;
            pane2.XAxis.Scale.Max = 100;
            LineItem curve1 = pane1.AddCurve("", list1, Color.Red, SymbolType.None);
            curve1.Line.Width = 3;
            LineItem curve2 = pane1.AddCurve("", list2, Color.Blue, SymbolType.None);
            curve2.Line.Width = 3;
            LineItem curve3 = pane1.AddCurve("", list3, Color.Green, SymbolType.None);
            curve3.Line.Width = 3;

            LineItem curve4 = pane2.AddCurve("", list4, Color.Red, SymbolType.None);
            curve4.Line.Width = 3;
            LineItem curve5 = pane2.AddCurve("", list5, Color.Blue, SymbolType.None);
            curve5.Line.Width = 3;
            LineItem curve6 = pane2.AddCurve("", list6, Color.Green, SymbolType.None);
            curve6.Line.Width = 3;


            zedGraphControl1.AxisChange();
            zedGraphControl1.Invalidate();
            zedGraphControl2.AxisChange();
            zedGraphControl2.Invalidate();
        }
    }
}
