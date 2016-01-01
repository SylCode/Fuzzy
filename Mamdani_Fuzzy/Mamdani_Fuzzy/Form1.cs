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
        double[] x, y, xL, yL, rL;
        double[,] membershipX;
        double[,] membershipY;
        int maxPressure, maxTemp;
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
            maxPressure = 3700;
            maxTemp = 60;
            m = new Membership();
            data = new List<int[]>();
            data.Add(new int[] { 0, 0, 760 });
            data.Add(new int[] { 473, 760, 1520 });
            data.Add(new int[] { 1300, maxPressure, maxPressure });
            data.Add(new int[] { -100, -100, 18 });
            data.Add(new int[] { -30, 28, 35 });
            data.Add(new int[] { 30, maxTemp, maxTemp });

            data.Add(new int[] { -1, 0, 1 });
            data.Add(new int[] { 0, 1, 2 });
            data.Add(new int[] { 1, 2, 3 });

            keys = new string[] { "Low", "Medium", "High", "Cold","Warm", "Hot", "HP_HH_Death", "Comfort", "LP_LH_Death" };
            fuzzy = new FuzzySet();
            int ct = 0;
            foreach (string obj in keys)
            {
                fuzzy.Set.Add(obj, data[ct]);
                ct++;
            }
            x = new double[size];
            y = new double[size];
            xL = new double[maxPressure+1];
            yL = new double[100+maxTemp+1];
            rL = new double[5];
            
            InitializeComponent();
            initParams();
        }

        private void initParams()
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

            PointPairList list1, list2, list3, list4, list5, list6, list7, list8, list9;
            list1 = new PointPairList();
            list2 = new PointPairList();
            list3 = new PointPairList();
            list4 = new PointPairList();
            list5 = new PointPairList();
            list6 = new PointPairList();
            list7 = new PointPairList();
            list8 = new PointPairList();
            list9 = new PointPairList();

            membershipX = new double[xL.Length, 3];
            membershipY = new double[yL.Length, 3];

            for (int i=0; i<=maxPressure; i++)
            {
                xL[i] = i;
                membershipX[i, 0] = m.trimf(xL[i], fuzzy.Set[keys[0]][0], fuzzy.Set[keys[0]][1], fuzzy.Set[keys[0]][2]);
                membershipX[i, 1] = m.trimf(xL[i], fuzzy.Set[keys[1]][0], fuzzy.Set[keys[1]][1], fuzzy.Set[keys[1]][2]);
                membershipX[i, 2] = m.trimf(xL[i], fuzzy.Set[keys[2]][0], fuzzy.Set[keys[2]][1], fuzzy.Set[keys[2]][2]);
                list1.Add(xL[i], membershipX[i, 0]);
                list2.Add(xL[i], membershipX[i, 1]);
                list3.Add(xL[i], membershipX[i, 2]);
            }

            int t = -101;
            for (int i=0; i<=100+maxTemp; i++)
            {
                yL[i] = t + i;
                membershipY[i, 0] = m.trimf(yL[i], fuzzy.Set[keys[3]][0], fuzzy.Set[keys[3]][1], fuzzy.Set[keys[3]][2]);
                membershipY[i, 1] = m.trimf(yL[i], fuzzy.Set[keys[4]][0], fuzzy.Set[keys[4]][1], fuzzy.Set[keys[4]][2]);
                membershipY[i, 2] = m.trimf(yL[i], fuzzy.Set[keys[5]][0], fuzzy.Set[keys[5]][1], fuzzy.Set[keys[5]][2]);
                list4.Add(yL[i], membershipY[i, 0]);
                list5.Add(yL[i], membershipY[i, 1]);
                list6.Add(yL[i], membershipY[i, 2]);
            }
            double[,] membershipR = new double[5,3];
            for (int i=0; i< 5; i++)
            {
                rL[i] = i - 1;
                membershipR[i, 0] = m.trimf(rL[i], fuzzy.Set[keys[6]][0], fuzzy.Set[keys[6]][1], fuzzy.Set[keys[6]][2]);
                membershipR[i, 1] = m.trimf(rL[i], fuzzy.Set[keys[7]][0], fuzzy.Set[keys[7]][1], fuzzy.Set[keys[7]][2]);
                membershipR[i, 2] = m.trimf(rL[i], fuzzy.Set[keys[8]][0], fuzzy.Set[keys[8]][1], fuzzy.Set[keys[8]][2]);
                list7.Add(yL[i], membershipR[i, 0]);
                list8.Add(yL[i], membershipR[i, 1]);
                list9.Add(yL[i], membershipR[i, 2]);
            }

            //list1.Sort(SortType.XValues);
            //list2.Sort(SortType.XValues);
            //list3.Sort(SortType.XValues);
            //list4.Sort(SortType.XValues);
            //list5.Sort(SortType.XValues);
            //list6.Sort(SortType.XValues);
            pane2.XAxis.Scale.Min = -100;
            pane2.XAxis.Scale.Max = maxTemp;
            pane1.XAxis.Scale.Max = maxPressure;
            

            LineItem curve1 = pane1.AddCurve("Low", list1, Color.Blue, SymbolType.None);
            curve1.Line.Width = 1;
            LineItem curve2 = pane1.AddCurve("Medium", list2,Color.Green, SymbolType.None);
            curve2.Line.Width = 1;
            LineItem curve3 = pane1.AddCurve("High", list3, Color.Red, SymbolType.None);
            curve3.Line.Width = 1;

            LineItem curve4 = pane2.AddCurve("Cold", list4, Color.Blue, SymbolType.None);
            curve4.Line.Width = 1;
            LineItem curve5 = pane2.AddCurve("Warm", list5, Color.Green, SymbolType.None);
            curve5.Line.Width = 1;
            LineItem curve6 = pane2.AddCurve("Hot", list6, Color.Red, SymbolType.None);
            curve6.Line.Width = 1;

            LineItem curve7 = pane3.AddCurve("DeadL", list7, Color.Blue, SymbolType.None);
            curve4.Line.Width = 1;
            LineItem curve8 = pane3.AddCurve("Norm", list8, Color.Green, SymbolType.None);
            curve5.Line.Width = 1;
            LineItem curve9 = pane3.AddCurve("DeadH", list9, Color.Red, SymbolType.None);
            curve6.Line.Width = 1;


            zedGraphControl1.AxisChange();
            zedGraphControl1.Invalidate();
            zedGraphControl2.AxisChange();
            zedGraphControl2.Invalidate();
            zedGraphControl3.AxisChange();
            zedGraphControl3.Invalidate();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //GraphPane pane1, pane2, pane3;
            //dataGridView1.Rows.Clear();
            //zedGraphControl1.GraphPane = new GraphPane(zedGraphControl1.GraphPane.Rect, "Pressure", "m(Pressure)", "Pressure");
            //zedGraphControl2.GraphPane = new GraphPane(zedGraphControl2.GraphPane.Rect, "Temperature", "m(temp)", "Temperature");

            //pane1 = zedGraphControl1.GraphPane;
            //pane2 = zedGraphControl2.GraphPane;
            //pane3 = zedGraphControl3.GraphPane;
            //pane1.CurveList.Clear();
            //pane2.CurveList.Clear();
            //pane3.CurveList.Clear();

            //PointPairList list1, list2, list3, list4, list5, list6;
            //list1 = new PointPairList();
            //list2 = new PointPairList();
            //list3 = new PointPairList();
            //list4 = new PointPairList();
            //list5 = new PointPairList();
            //list6 = new PointPairList();

            Random rn = new Random();
            dataGridView1.Columns.Add("X", "Pressure");
            dataGridView1.Columns.Add("Y", "Temperature");
            dataGridView1.Columns.Add("R", "Decision");
            //double[,] membershipX = new double[x.Length,3];
            //double[,] membershipY = new double[x.Length,3];

            for (int i=0; i<size; i++)
            {
                x[i] = Math.Round(100 / rn.NextDouble(),0);// % 0.5;
                y[i] = rn.Next(-100, 60);
                if (y[i] < -273)
                    y[i] = -273;
                if (x[i] > 6000)
                    x[i] = 6000;

                double totalM, m1=0,m2=0;
                string decision1, decision2, decision;
                decision = decision1 = decision2 = "";
                for (int j=0; j< xL.Length-1; j++)
                {
                    if (x[i]>=xL[j]&&x[i]<= xL[j+1])
                    {
                        if (membershipX[j, 0] <= membershipX[j, 1])
                        {
                            m1 = membershipX[j, 1];
                            decision1 = keys[1];
                        }
                        else
                        {
                            m1 = membershipX[j, 0];
                            decision1 = keys[0];
                        }
                        if (m1 <= membershipX[j, 2])
                        {
                            m1 = membershipX[j, 2];
                            decision1 = keys[2];
                        }
                        break;
                    }
                }
                for (int j = 0; j < yL.Length - 1; j++)
                {
                    if (y[i] >= yL[j] && y[i] <= yL[j + 1])
                    {
                        if (membershipY[j, 0] <= membershipY[j, 1])
                        {
                            m2 = membershipY[j, 1];
                            decision2 = keys[4];
                        }
                        else
                        {
                            m2 = membershipY[j, 0];
                            decision2 = keys[3];
                        }
                        if (m2 <= membershipX[j, 2])
                        {
                            m2 = membershipY[j, 2];
                            decision2 = keys[5];
                        }
                        break;
                    }
                }
                totalM = Math.Min(m1,m2);
                if (decision1 == "Medium" && decision2 == "Warm" || decision1 == "Medium" && decision2 == "Warm")
                    decision = "Norm";
                else decision = "Dead";
                // % 0.5;
                //membershipX[i, 0] = m.trimf(x[i], fuzzy.Set[keys[0]][0], fuzzy.Set[keys[0]][1], fuzzy.Set[keys[0]][2]);
                //membershipX[i, 1] = m.trimf(x[i], fuzzy.Set[keys[1]][0], fuzzy.Set[keys[1]][1], fuzzy.Set[keys[1]][2]);
                //membershipX[i, 2] = m.trimf(x[i], fuzzy.Set[keys[2]][0], fuzzy.Set[keys[2]][1], fuzzy.Set[keys[2]][2]);
                //list1.Add(x[i], membershipX[i, 0]);
                //list2.Add(x[i], membershipX[i, 1]);
                //list3.Add(x[i], membershipX[i, 2]);

                //membershipY[i, 0] = m.trimf(y[i], fuzzy.Set[keys[3]][0], fuzzy.Set[keys[3]][1], fuzzy.Set[keys[3]][2]);
                //membershipY[i, 1] = m.trimf(y[i], fuzzy.Set[keys[4]][0], fuzzy.Set[keys[4]][1], fuzzy.Set[keys[4]][2]);
                //membershipY[i, 2] = m.trimf(y[i], fuzzy.Set[keys[5]][0], fuzzy.Set[keys[5]][1], fuzzy.Set[keys[5]][2]);
                //list4.Add(y[i], membershipY[i, 0]);
                //list5.Add(y[i], membershipY[i, 1]);
                //list6.Add(y[i], membershipY[i, 2]);

                dataGridView1.Rows.Add(new string[] { x[i].ToString(), y[i].ToString(), decision });
            }

            //list1.Sort(SortType.XValues);
            //list2.Sort(SortType.XValues);
            //list3.Sort(SortType.XValues);
            //list4.Sort(SortType.XValues);
            //list5.Sort(SortType.XValues);
            //list6.Sort(SortType.XValues);
            //pane2.XAxis.Scale.Min = -100;
            //pane2.XAxis.Scale.Max = 100;
            //LineItem curve1 = pane1.AddCurve("", list1, Color.Red, SymbolType.None);
            //curve1.Line.Width = 3;
            //LineItem curve2 = pane1.AddCurve("", list2, Color.Blue, SymbolType.None);
            //curve2.Line.Width = 3;
            //LineItem curve3 = pane1.AddCurve("", list3, Color.Green, SymbolType.None);
            //curve3.Line.Width = 3;

            //LineItem curve4 = pane2.AddCurve("", list4, Color.Red, SymbolType.None);
            //curve4.Line.Width = 3;
            //LineItem curve5 = pane2.AddCurve("", list5, Color.Blue, SymbolType.None);
            //curve5.Line.Width = 3;
            //LineItem curve6 = pane2.AddCurve("", list6, Color.Green, SymbolType.None);
            //curve6.Line.Width = 3;


            //zedGraphControl1.AxisChange();
            //zedGraphControl1.Invalidate();
            //zedGraphControl2.AxisChange();
            //zedGraphControl2.Invalidate();
        }
    }
}
