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
        FuzzyRules rules;
        //string[] pressure = new string[] { "Low", "Medium", "High" };
        //int[] low, med, hi, hot, warm, cold;
        List<int[]> data;
        string[] keys;
        //string[] temperature = new string[] { "Hot", "Warm", "Cold" };
        string[] output = new string[] { "Very deep", "Deep", "Norm", "High" };


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
            data.Add(new int[] { -36, -24, -12 });
            data.Add(new int[] { -24, -12, 0});
            data.Add(new int[] { -12, 0, 12 });
            data.Add(new int[] { 0, 12, 24 });

            keys = new string[] { "Low", "Medium", "High Pressure", "Cold","Warm", "Hot", "Very deep", "Deep", "Norm", "Space" };

            rules = new FuzzyRules();
            rules.addRule(new string[] {"Low", "Cold" }, "Space");
            rules.addRule(new string[] { "Low", "Warm" }, "Space");
            rules.addRule(new string[] { "Low", "Hot" }, "Space");
            rules.addRule(new string[] { "Medium", "Cold" }, "Surphace");
            rules.addRule(new string[] { "Medium", "Warm" }, "Surphace");
            rules.addRule(new string[] { "Medium", "Hot" }, "Surphace");
            rules.addRule(new string[] { "High Pressure", "Cold" }, "Deep");
            rules.addRule(new string[] { "High Pressure", "Warm" }, "Deep");
            rules.addRule(new string[] { "High Pressure", "Hot" }, "Very deep");

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
            rL = new double[6];
            
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

            List<PointPairList> list = new List<PointPairList>();
            for (int i = 0; i < 10; i++)
                list.Add(new PointPairList());

            membershipX = new double[xL.Length, 3];
            membershipY = new double[yL.Length, 3];

            for (int i=0; i<=maxPressure; i++)
            {
                xL[i] = i;
                membershipX[i, 0] = m.trimf(xL[i], fuzzy.Set[keys[0]][0], fuzzy.Set[keys[0]][1], fuzzy.Set[keys[0]][2]);
                membershipX[i, 1] = m.trimf(xL[i], fuzzy.Set[keys[1]][0], fuzzy.Set[keys[1]][1], fuzzy.Set[keys[1]][2]);
                membershipX[i, 2] = m.trimf(xL[i], fuzzy.Set[keys[2]][0], fuzzy.Set[keys[2]][1], fuzzy.Set[keys[2]][2]);
                list[0].Add(xL[i], membershipX[i, 0]);
                list[1].Add(xL[i], membershipX[i, 1]);
                list[2].Add(xL[i], membershipX[i, 2]);
            }

            int t = -101;
            for (int i=0; i<=100+maxTemp; i++)
            {
                yL[i] = t + i;
                membershipY[i, 0] = m.trimf(yL[i], fuzzy.Set[keys[3]][0], fuzzy.Set[keys[3]][1], fuzzy.Set[keys[3]][2]);
                membershipY[i, 1] = m.trimf(yL[i], fuzzy.Set[keys[4]][0], fuzzy.Set[keys[4]][1], fuzzy.Set[keys[4]][2]);
                membershipY[i, 2] = m.trimf(yL[i], fuzzy.Set[keys[5]][0], fuzzy.Set[keys[5]][1], fuzzy.Set[keys[5]][2]);
                list[3].Add(yL[i], membershipY[i, 0]);
                list[4].Add(yL[i], membershipY[i, 1]);
                list[5].Add(yL[i], membershipY[i, 2]);
            }
            double[,] membershipR = new double[6,4];
            rL[0] = fuzzy.Set[keys[6]][0];
            for (int i=0; i< 6; i++)
            {
                rL[i] = fuzzy.Set[keys[6]][0] + i * (Math.Abs(fuzzy.Set[keys[6]][0]) - Math.Abs(fuzzy.Set[keys[6]][1]));
                membershipR[i, 0] = m.trimf(rL[i], fuzzy.Set[keys[6]][0], fuzzy.Set[keys[6]][1], fuzzy.Set[keys[6]][2]);
                membershipR[i, 1] = m.trimf(rL[i], fuzzy.Set[keys[7]][0], fuzzy.Set[keys[7]][1], fuzzy.Set[keys[7]][2]);
                membershipR[i, 2] = m.trimf(rL[i], fuzzy.Set[keys[8]][0], fuzzy.Set[keys[8]][1], fuzzy.Set[keys[8]][2]);
                membershipR[i, 3] = m.trimf(rL[i], fuzzy.Set[keys[9]][0], fuzzy.Set[keys[9]][1], fuzzy.Set[keys[9]][2]);
                list[6].Add(rL[i], membershipR[i, 0]);
                list[7].Add(rL[i], membershipR[i, 1]);
                list[8].Add(rL[i], membershipR[i, 2]);
                list[9].Add(rL[i], membershipR[i, 3]);
            }

            pane2.XAxis.Scale.Min = -100;
            pane2.XAxis.Scale.Max = maxTemp;
            pane1.XAxis.Scale.Max = maxPressure;
            

            LineItem curve1 = pane1.AddCurve("Low", list[0], Color.Blue, SymbolType.None);
            curve1.Line.Width = 1;
            LineItem curve2 = pane1.AddCurve("Medium", list[1],Color.Green, SymbolType.None);
            curve2.Line.Width = 1;
            LineItem curve3 = pane1.AddCurve("High", list[2], Color.Red, SymbolType.None);
            curve3.Line.Width = 1;

            LineItem curve4 = pane2.AddCurve("Cold", list[3], Color.Blue, SymbolType.None);
            curve4.Line.Width = 1;
            LineItem curve5 = pane2.AddCurve("Warm", list[4], Color.Green, SymbolType.None);
            curve5.Line.Width = 1;
            LineItem curve6 = pane2.AddCurve("Hot", list[5], Color.Red, SymbolType.None);
            curve6.Line.Width = 1;

            LineItem curve7 = pane3.AddCurve("Very deep", list[6], Color.Blue, SymbolType.None);
            curve7.Line.Width = 1;
            LineItem curve8 = pane3.AddCurve("Deep", list[7], Color.Green, SymbolType.None);
            curve8.Line.Width = 1;
            LineItem curve9 = pane3.AddCurve("Surphace", list[8], Color.Red, SymbolType.None);
            curve9.Line.Width = 1;
            LineItem curve10 = pane3.AddCurve("Space", list[9], Color.Black, SymbolType.None);
            curve10.Line.Width = 1;


            zedGraphControl1.AxisChange();
            zedGraphControl1.Invalidate();
            zedGraphControl2.AxisChange();
            zedGraphControl2.Invalidate();
            zedGraphControl3.AxisChange();
            zedGraphControl3.Invalidate();

        }

        private void Aggregate(string decision, double aggregate)
        {

        }

        
        private void button1_Click(object sender, EventArgs e)
        {
            Random rn = new Random();
            GraphPane paneA = zedGraphControl4.GraphPane;
            dataGridView1.Columns.Add("X", "Pressure");
            dataGridView1.Columns.Add("Y", "Temperature");
            dataGridView1.Columns.Add("R", "Decision");

            for (int i=0; i<size; i++)
            {
                x[i] = Math.Round(100 / rn.NextDouble(),0);// % 0.5;
                y[i] = rn.Next(-100, 60);
                if (x[i] > 3699)
                    x[i] = 3699;

                double totalM, m1=0,m2=0;
                string decision1, decision2, decision;
                decision = decision1 = decision2 = "";
                for (int j=0; j< xL.Length-1; j++)
                {
                    if (x[i]==xL[j])
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
                if (decision1 == "")
                {
                    int a = 0;
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
                m1 = 0; m2 = 0;
                decision = rules.checkRule(new string[] {decision1, decision2 });
                decision1 = ""; decision2 = "";
                dataGridView1.Rows.Add(new string[] { x[i].ToString(), y[i].ToString(), decision });
            }
            
        }
    }
}
