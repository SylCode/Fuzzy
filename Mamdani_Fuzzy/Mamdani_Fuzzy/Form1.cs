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
        int size = 300, nIn=2, nDiv=3;
        double[] x, y, xL, yL, rL;
        List<double[,]> membershipIn, membershipOut;
        List<double[]> inputData, genData;
        List<string[]> decisions;
        int maxPressure, maxTemp;
        Membership m;
        FuzzySet fuzzy, rFuzzy;
        List<DeFuzzySet> inputDef;
        FuzzyRules rules;
        List<PointPairList> list, rList;
        List<int[]> data;
        string[] keys, classes;
        string[] output = new string[] { "Very deep", "Deep", "Norm", "High" };


        public Form1()
        {

            membershipIn = new List<double[,]>();
            membershipOut = new List<double[,]>();
            inputData = new List<double[]>();
            genData = new List<double[]>();
            decisions = new List<string[]>();
            inputDef = new List<DeFuzzySet>();


            data = new List<int[]>();
            rules = new FuzzyRules();
            fuzzy = new FuzzySet();
            rFuzzy = new FuzzySet();
            m = new Membership();

            maxPressure = 3700;
            maxTemp = 60;

            keys = new string[] { "Low", "Medium", "High Pressure", "Cold","Warm", "Hot"};
            classes = new string[] { "Very deep", "Deep", "Norm", "Space" };

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

            rules.addRule(new string[] {"Low", "Cold" }, classes[3]);
            rules.addRule(new string[] { "Low", "Warm" }, classes[3]);
            rules.addRule(new string[] { "Low", "Hot" }, classes[3]);
            rules.addRule(new string[] { "Medium", "Cold" }, classes[2]);
            rules.addRule(new string[] { "Medium", "Warm" }, classes[2]);
            rules.addRule(new string[] { "Medium", "Hot" }, classes[2]);
            rules.addRule(new string[] { "High Pressure", "Cold" }, classes[1]);
            rules.addRule(new string[] { "High Pressure", "Warm" }, classes[1]);
            rules.addRule(new string[] { "High Pressure", "Hot" }, classes[0]);

            int ct = 0;
            foreach (string obj in keys)
            {
                fuzzy.Set.Add(obj, data[ct]);
                ct++;
            }
            foreach (string obj in classes)
            {
                rFuzzy.Set.Add(obj, data[ct]);
                ct++;
            }
            inputData.Add(new double[maxPressure+1]);
            inputData.Add(new double[100+maxTemp+1]);

            genData.Add(new double[size]);
            genData.Add(new double[size]);
            

            rL = new double[maxTemp+1];
            
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

            list = new List<PointPairList>();
            rList = new List<PointPairList>();

            for (int i = 0; i < fuzzy.Set.Count; i++)
                list.Add(new PointPairList());
            for (int i = 0; i < rFuzzy.Set.Count; i++)
                rList.Add(new PointPairList());

            membershipIn.Add(new double[inputData[0].Length, 3]);
            membershipIn.Add(new double[inputData[1].Length, 3]);
            int n = 0;
            int ct = 0;
            for (int k=0; k< membershipIn.Count; k++)
            {
                inputData[k][0] = fuzzy.Set[keys[n]][0];
                for (int i = 0; i < membershipIn[k].Length / 3; i++)
                {
                    for (int j = n; j < n + nDiv; j++)
                    {
                        membershipIn[k][i, ct] = m.trimf(inputData[k][i], fuzzy.Set[keys[j]][0], fuzzy.Set[keys[j]][1], fuzzy.Set[keys[j]][2]);
                        list[j].Add(inputData[k][i], membershipIn[k][i, ct]);
                        ct++;
                    }
                    if (i+1!= membershipIn[k].Length / 3)
                        inputData[k][i+1] = inputData[k][i]+1;
                    ct = 0;
                    
                }
                n += nDiv;
            }
            for (int i=0; i<inputData.Count; i++)
            {
                inputDef.Add(new DeFuzzySet(inputData[i], membershipIn[i]));
            }
            
            int nR, nD;
            nR = rFuzzy.Set.Count;
            double[,] membershipR = new double[maxTemp+1,nR];
            
            ct = 0;
            for (int i=rFuzzy.Set[classes[0]][0]; i<= rFuzzy.Set[classes[nR-1]][2]; i++)
            {
                rL[ct] = i;
                for (int j = 0; j< nR; j++)
                {
                    membershipR[ct, j] = m.trimf(rL[ct], rFuzzy.Set[classes[j]][0], rFuzzy.Set[classes[j]][1], rFuzzy.Set[classes[j]][2]);
                    rList[j].Add(rL[ct], membershipR[ct, j]);
                }
               
                ct++;
            }
            membershipOut.Add(membershipR);

            #region output
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

            LineItem curve7 = pane3.AddCurve("Very deep", rList[0], Color.Blue, SymbolType.None);
            curve7.Line.Width = 1;
            LineItem curve8 = pane3.AddCurve("Deep", rList[1], Color.Green, SymbolType.None);
            curve8.Line.Width = 1;
            LineItem curve9 = pane3.AddCurve("Surphace", rList[2], Color.Red, SymbolType.None);
            curve9.Line.Width = 1;
            LineItem curve10 = pane3.AddCurve("Space", rList[3], Color.Black, SymbolType.None);
            curve10.Line.Width = 1;


            zedGraphControl1.AxisChange();
            zedGraphControl1.Invalidate();
            zedGraphControl2.AxisChange();
            zedGraphControl2.Invalidate();
            zedGraphControl3.AxisChange();
            zedGraphControl3.Invalidate();
            #endregion

        }

        private void Aggregate(string decision, double aggregate)
        {

        }


        private void generateButton_Click(object sender, EventArgs e)
        {
            Random rn = new Random();
            GraphPane paneA = zedGraphControl4.GraphPane;
            dataGridView1.Columns.Add("X", "Pressure");
            dataGridView1.Columns.Add("Y", "Temperature");
            dataGridView1.Columns.Add("R", "Decision");
            string[] decisions = new string[inputData.Count];
            double[,] m = new double[size, rFuzzy.Set.Count];  //wartosci regul dla agregacji
            double[] totalM = new double[size];
            string[] decision= new string[size];

            List<double[,]> ms = new List<double[,]>();
            for (int i = 0; i < inputData.Count; i++)
                ms.Add(new double[rFuzzy.Set.Count, inputData.Count]);

            for (int i = 0; i < size; i++)
            {
                genData[0][i] = Math.Round(100 / rn.NextDouble(), 0);// % 0.5;
                genData[1][i] = rn.Next(-100, 60);
                if (genData[0][i] > 3699)
                    genData[0][i] = 3699;
                
                for (int j = 0; j < genData.Count; j++)
                {
                    for (int k = 0; k < inputData[j].Length; k++)
                    {
                        if (genData[j][i] == inputData[j][k])
                        {
                            ms[j][0, j] = membershipIn[j][k, 2];
                            ms[j][1, j] = membershipIn[j][k, 2];
                            ms[j][2, j] = membershipIn[j][k, 1];
                            ms[j][3, j] = membershipIn[j][k, 0];
                            break;
                        }
                    }
                }
                double[] max = new double[rFuzzy.Set.Count];
                for (int j=0; j<max.Length; j++)
                {
                    max[j] = double.MinValue;
                }
                for (int j=0; j<rFuzzy.Set.Count; j++)
                {
                    for (int k=0; k<genData.Count; k++)
                    {
                        if (max[j] < ms[k][j, k])
                            max[j] = ms[k][j, k];
                    }
                    m[i, j] = max[j];
                }

            }
        }
    }
}
