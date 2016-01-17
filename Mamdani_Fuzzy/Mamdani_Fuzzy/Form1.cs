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
        GraphPane pane1, pane2, pane3, pane4;
        int cot = 0;
        string fu = "tr";
        int size = 300, nIn=2, nDiv=3, minGen=-100,maxGen=100;
        double msre = 0;
        double[] rL,mY,rT;
        string[] categoryYL, categoryYT1, categoryYT2;
        List<double[,]> membershipIn, membershipOut,mm;
        List<DeFuzzySet> inputDef;
        FuzzyRules rules;
        List<PointPairList> list, rList;
        List<int[]> data;
        string[] keys, classes;
        double[,] mmY,m;
        string[,] categoryXL;
        List<double[]>  genData,LearningX,LearningY,TestX,TestY;
        List<string[]> decisions;
        Membership func;
        FuzzySet fuzzy, rFuzzy;

        #region params
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            fu = "tr";
            initData();
            initParams();
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            fu = "trap";
            initData();
            initParams();
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            fu = "gauss";
            initData();
            initParams();
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            nDiv = 3;
            initData();
            initParams();
        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            nDiv = 5;
            initData();
            initParams();
        }
        #endregion

        bool check (PointPair pp)
        {
            return true;
        }

        public Form1()
        {
            InitializeComponent();
            initData();
            initParams();
        }

        private void initData()
        {
            membershipIn = new List<double[,]>();
            membershipOut = new List<double[,]>();

            m = new double[size, nIn];
            mY = new double[size];
            rT = new double[size];

            categoryXL = new string[size, nIn];
            categoryYL = new string[size];
            categoryYT1 = new string[size];
            categoryYT2 = new string[size];

            LearningX = new List<double[]>();
            LearningY = new List<double[]>();
            TestX = new List<double[]>();
            TestY = new List<double[]>();

            genData = new List<double[]>();
            decisions = new List<string[]>();
            inputDef = new List<DeFuzzySet>();


            data = new List<int[]>();
            rules = new FuzzyRules();
            fuzzy = new FuzzySet();
            rFuzzy = new FuzzySet();
            func = new Membership();

            keys = new string[nDiv * nIn];
            classes = new string[nDiv];
            int d = 0;
            for (int i = 0; i < nDiv * nIn; i++)
            {
                if (i < nDiv)
                {
                    keys[i] = i + "A" + i;
                    classes[i] = i + "B" + i;
                }
                else
                {
                    keys[i] = (i - nDiv) + "A." + (i - nDiv);
                }
            }
            if (fu == "tr")
            {
                int ln = nDiv + 1;
                d = (Math.Abs(minGen) + Math.Abs(maxGen)) / ln;
            }
            else if (fu == "trap")
            {
                if (nDiv==3)
                    d= (Math.Abs(minGen) + Math.Abs(maxGen)) / 7;
                else d = (Math.Abs(minGen) + Math.Abs(maxGen)) / 11;
            }
            else if (fu == "gauss")
            {
                int ln = nDiv + 1;
                d = (Math.Abs(minGen) + Math.Abs(maxGen)) / ln;
            }

            if (nDiv == 3)
            {
                if (fu == "tr")
                {
                    data.Add(new int[] { minGen, minGen + d, minGen + 2 * d });
                    data.Add(new int[] { minGen + d, minGen + 2 * d, minGen + 3 * d });
                    data.Add(new int[] { minGen + 2 * d, minGen + 3 * d, maxGen });

                    data.Add(new int[] { minGen, minGen + d, minGen + 2 * d });
                    data.Add(new int[] { minGen + d, minGen + 2 * d, minGen + 3 * d });
                    data.Add(new int[] { minGen + 2 * d, minGen + 3 * d, maxGen });

                    data.Add(new int[] { minGen, minGen + d, minGen + 2 * d });
                    data.Add(new int[] { minGen + d, minGen + 2 * d, minGen + 3 * d });
                    data.Add(new int[] { minGen + 2 * d, minGen + 3 * d, maxGen });
                }
                else if (fu == "trap")
                {
                    data.Add(new int[] { minGen, minGen + d, minGen + 2 * d, minGen + 3 * d });
                    data.Add(new int[] { minGen + 2 * d, minGen + 3 * d, minGen + 4 * d, minGen + 5 * d });
                    data.Add(new int[] { minGen + 4 * d, minGen + 5 * d, minGen + 6 * d, maxGen });

                    data.Add(new int[] { minGen, minGen + d, minGen + 2 * d, minGen + 3 * d });
                    data.Add(new int[] { minGen + 2 * d, minGen + 3 * d, minGen + 4 * d, minGen + 5 * d });
                    data.Add(new int[] { minGen + 4 * d, minGen + 5 * d, minGen + 6 * d, maxGen });

                    data.Add(new int[] { minGen, minGen + d, minGen + 2 * d, minGen + 3 * d });
                    data.Add(new int[] { minGen + 2 * d, minGen + 3 * d, minGen + 4 * d, minGen + 5 * d });
                    data.Add(new int[] { minGen + 4 * d, minGen + 5 * d, minGen + 6 * d, maxGen });
                }
                else if (fu == "gauss")
                {
                    data.Add(new int[] { minGen + d, d/3 });
                    data.Add(new int[] { minGen + 2 * d, d / 3 });
                    data.Add(new int[] { minGen + 3 * d, d / 3 });

                    data.Add(new int[] { minGen + d, d / 3 });
                    data.Add(new int[] { minGen + 2 * d, d / 3 });
                    data.Add(new int[] { minGen + 3 * d, d / 3 });

                    data.Add(new int[] { minGen + d, d / 3 });
                    data.Add(new int[] { minGen + 2 * d, d / 3 });
                    data.Add(new int[] { minGen + 3 * d, d / 3 });
                }
            }
            else
            {
                if (fu == "tr")
                {
                    data.Add(new int[] { minGen, minGen + d, minGen + 2 * d });
                    data.Add(new int[] { minGen + d, minGen + 2 * d, minGen + 3 * d });
                    data.Add(new int[] { minGen + 2 * d, minGen + 3 * d, minGen + 4 * d });
                    data.Add(new int[] { minGen + 3 * d, minGen + 4 * d, minGen + 5 * d });
                    data.Add(new int[] { minGen + 4 * d, minGen + 5 * d, maxGen });

                    data.Add(new int[] { minGen, minGen + d, minGen + 2 * d });
                    data.Add(new int[] { minGen + d, minGen + 2 * d, minGen + 3 * d });
                    data.Add(new int[] { minGen + 2 * d, minGen + 3 * d, minGen + 4 * d });
                    data.Add(new int[] { minGen + 3 * d, minGen + 4 * d, minGen + 5 * d });
                    data.Add(new int[] { minGen + 4 * d, minGen + 5 * d, maxGen });

                    data.Add(new int[] { minGen, minGen + d, minGen + 2 * d });
                    data.Add(new int[] { minGen + d, minGen + 2 * d, minGen + 3 * d });
                    data.Add(new int[] { minGen + 2 * d, minGen + 3 * d, minGen + 4 * d });
                    data.Add(new int[] { minGen + 3 * d, minGen + 4 * d, minGen + 5 * d });
                    data.Add(new int[] { minGen + 4 * d, minGen + 5 * d, maxGen });
                }
                else if (fu == "trap")
                {
                    data.Add(new int[] { minGen, minGen + d, minGen + 2 * d, minGen + 3 * d });
                    data.Add(new int[] { minGen + 2 * d, minGen + 3 * d, minGen + 4 * d, minGen + 5 * d });
                    data.Add(new int[] { minGen + 4 * d, minGen + 5 * d, minGen + 6 * d, minGen + 7 * d });
                    data.Add(new int[] { minGen + 6 * d, minGen + 7 * d, minGen + 8 * d, minGen + 9 * d });
                    data.Add(new int[] { minGen + 8 * d, minGen + 9 * d, minGen + 10 * d, maxGen });

                    data.Add(new int[] { minGen, minGen + d, minGen + 2 * d, minGen + 3 * d });
                    data.Add(new int[] { minGen + 2 * d, minGen + 3 * d, minGen + 4 * d, minGen + 5 * d });
                    data.Add(new int[] { minGen + 4 * d, minGen + 5 * d, minGen + 6 * d, minGen + 7 * d });
                    data.Add(new int[] { minGen + 6 * d, minGen + 7 * d, minGen + 8 * d, minGen + 9 * d });
                    data.Add(new int[] { minGen + 8 * d, minGen + 9 * d, minGen + 10 * d, maxGen });

                    data.Add(new int[] { minGen, minGen + d, minGen + 2 * d, minGen + 3 * d });
                    data.Add(new int[] { minGen + 2 * d, minGen + 3 * d, minGen + 4 * d, minGen + 5 * d });
                    data.Add(new int[] { minGen + 4 * d, minGen + 5 * d, minGen + 6 * d, minGen + 7 * d });
                    data.Add(new int[] { minGen + 6 * d, minGen + 7 * d, minGen + 8 * d, minGen + 9 * d });
                    data.Add(new int[] { minGen + 8 * d, minGen + 9 * d, minGen + 10 * d, maxGen });
                }
                else if (fu == "gauss")
                {
                    data.Add(new int[] { minGen + d, d / 3 });
                    data.Add(new int[] { minGen + 2 * d, d / 3 });
                    data.Add(new int[] { minGen + 3 * d, d / 3 });
                    data.Add(new int[] { minGen + 4 * d, d / 3 });
                    data.Add(new int[] { minGen + 5 * d, d / 3 });

                    data.Add(new int[] { minGen + d, d / 3 });
                    data.Add(new int[] { minGen + 2 * d, d / 3 });
                    data.Add(new int[] { minGen + 3 * d, d / 3 });
                    data.Add(new int[] { minGen + 4 * d, d / 3 });
                    data.Add(new int[] { minGen + 5 * d, d / 3 });

                    data.Add(new int[] { minGen + d, d / 3 });
                    data.Add(new int[] { minGen + 2 * d, d / 3 });
                    data.Add(new int[] { minGen + 3 * d, d / 3 });
                    data.Add(new int[] { minGen + 4 * d, d / 3 });
                    data.Add(new int[] { minGen + 5 * d, d / 3 });
                }
            }


            int ct = 0;
            foreach (string obj in keys)
            {
                fuzzy.Set.Add(obj, data[ct]);
                ct++;
            }
            ct = 0;
            foreach (string obj in classes)
            {
                rFuzzy.Set.Add(obj, data[ct]);
                ct++;
            }
            for (int i = 0; i < nIn; i++)
            {
                LearningX.Add(new double[size + 1]);
                TestX.Add(new double[size + 1]);

                genData.Add(new double[size]);
            }
            LearningY.Add(new double[size]);
            TestY.Add(new double[size]);

            rL = new double[size + 1];
        }

        private void initParams()
        {
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
            for (int i=0; i<nIn; i++)
            {
                membershipIn.Add(new double[LearningX[i].Length, nDiv]);
            }
            
            int n = 0;
            int ct = 0;
            for (int k=0; k< membershipIn.Count; k++)
            {
                LearningX[k][0] = minGen;
                for (int i = 0; i < membershipIn[k].Length / nDiv; i++)
                {
                    for (int j = n; j < n + nDiv; j++)
                    {
                        if (fu=="tr")
                            membershipIn[k][i, ct] = func.trimf(LearningX[k][i], fuzzy.Set[keys[j]][0], fuzzy.Set[keys[j]][1], fuzzy.Set[keys[j]][2]);
                        else if (fu == "trap")
                            membershipIn[k][i, ct] = func.trapmf(LearningX[k][i], fuzzy.Set[keys[j]][0], fuzzy.Set[keys[j]][1], fuzzy.Set[keys[j]][2], fuzzy.Set[keys[j]][3]);
                        else if (fu == "gauss")
                            membershipIn[k][i, ct] = func.gaussmf(LearningX[k][i], fuzzy.Set[keys[j]][0], fuzzy.Set[keys[j]][1]);
                        list[j].Add(LearningX[k][i], membershipIn[k][i, ct]);
                        ct++;
                    }
                    if (i+1!= membershipIn[k].Length / nDiv)
                        LearningX[k][i+1] = LearningX[k][i]+1;
                    ct = 0;
                    
                }
                n += nDiv;
            }
            for (int i=0; i<LearningX.Count; i++)
            {
                inputDef.Add(new DeFuzzySet(LearningX[i], membershipIn[i]));
            }
            
            int nR;
            nR = rFuzzy.Set.Count;
            double[,] membershipR = new double[size+1,nR];
            
            ct = 0;
            int lim = 0;
            if (fu == "tr")
                lim = rFuzzy.Set[classes[nR - 1]][2];
            else if (fu == "trap")
                lim = rFuzzy.Set[classes[nR - 1]][3];
            else if (fu == "gauss")
                lim = maxGen;// rFuzzy.Set[classes[nR - 1]][0];
            for (int i=minGen; i<= lim; i++)
            {
                rL[ct] = i;
                for (int j = 0; j< nR; j++)
                {
                    if (fu=="tr")
                        membershipR[ct, j] = func.trimf(rL[ct], rFuzzy.Set[classes[j]][0], rFuzzy.Set[classes[j]][1], rFuzzy.Set[classes[j]][2]);
                    if (fu == "trap")
                        membershipR[ct, j] = func.trapmf(rL[ct], rFuzzy.Set[classes[j]][0], rFuzzy.Set[classes[j]][1], rFuzzy.Set[classes[j]][2], rFuzzy.Set[classes[j]][3]);
                    if (fu == "gauss")
                        membershipR[ct, j] = func.gaussmf(rL[ct], rFuzzy.Set[classes[j]][0], rFuzzy.Set[classes[j]][1]);

                    rList[j].Add(rL[ct], membershipR[ct, j]);
                }
               
                ct++;
            }
            membershipOut.Add(membershipR);

            #region output
            pane2.XAxis.Scale.Min = minGen;
            pane2.XAxis.Scale.Max = maxGen;
            pane1.XAxis.Scale.Min = minGen;
            pane1.XAxis.Scale.Max = maxGen;
            pane3.XAxis.Scale.Min = minGen;
            pane3.XAxis.Scale.Max = maxGen;

            if (nDiv == 3)
            {
                LineItem curve1 = pane1.AddCurve(keys[0], list[0], Color.Blue, SymbolType.None);
                curve1.Line.Width = 2;
                LineItem curve2 = pane1.AddCurve(keys[1], list[1], Color.Green, SymbolType.None);
                curve2.Line.Width = 2;
                LineItem curve3 = pane1.AddCurve(keys[2], list[2], Color.Red, SymbolType.None);
                curve3.Line.Width = 2;

                LineItem curve4 = pane2.AddCurve(keys[3], list[3], Color.Blue, SymbolType.None);
                curve4.Line.Width = 2;
                LineItem curve5 = pane2.AddCurve(keys[4], list[4], Color.Green, SymbolType.None);
                curve5.Line.Width = 2;
                LineItem curve6 = pane2.AddCurve(keys[5], list[5], Color.Red, SymbolType.None);
                curve6.Line.Width = 2;

                LineItem curve7 = pane3.AddCurve(classes[0], rList[0], Color.Blue, SymbolType.None);
                curve7.Line.Width = 2;
                LineItem curve8 = pane3.AddCurve(classes[1], rList[1], Color.Green, SymbolType.None);
                curve8.Line.Width = 2;
                LineItem curve9 = pane3.AddCurve(classes[2], rList[2], Color.Red, SymbolType.None);
                curve9.Line.Width = 2;
            }

            if (nDiv==5)
            {
                LineItem curve1 = pane1.AddCurve(keys[0], list[0], Color.Blue, SymbolType.None);
                curve1.Line.Width = 2;
                LineItem curve2 = pane1.AddCurve(keys[1], list[1], Color.Green, SymbolType.None);
                curve2.Line.Width = 2;
                LineItem curve3 = pane1.AddCurve(keys[2], list[2], Color.Red, SymbolType.None);
                curve3.Line.Width = 2;
                LineItem curve11 = pane1.AddCurve(keys[3], list[3], Color.Indigo, SymbolType.None);
                curve11.Line.Width = 2;
                LineItem curve12 = pane1.AddCurve(keys[4], list[4], Color.Black, SymbolType.None);
                curve12.Line.Width = 2;

                LineItem curve4 = pane2.AddCurve(keys[5], list[5], Color.Blue, SymbolType.None);
                curve4.Line.Width = 2;
                LineItem curve5 = pane2.AddCurve(keys[6], list[6], Color.Green, SymbolType.None);
                curve5.Line.Width = 2;
                LineItem curve6 = pane2.AddCurve(keys[7], list[7], Color.Red, SymbolType.None);
                curve6.Line.Width = 2;
                LineItem curve13 = pane2.AddCurve(keys[8], list[8], Color.Indigo, SymbolType.None);
                curve13.Line.Width = 2;
                LineItem curve14 = pane2.AddCurve(keys[9], list[9], Color.Black, SymbolType.None);
                curve14.Line.Width = 2;

                LineItem curve7 = pane3.AddCurve(classes[0], rList[0], Color.Blue, SymbolType.None);
                curve7.Line.Width = 2;
                LineItem curve8 = pane3.AddCurve(classes[1], rList[1], Color.Green, SymbolType.None);
                curve8.Line.Width = 2;
                LineItem curve9 = pane3.AddCurve(classes[2], rList[2], Color.Red, SymbolType.None);
                curve9.Line.Width = 2;
                LineItem curve15 = pane3.AddCurve(classes[3], rList[3], Color.Indigo, SymbolType.None);
                curve15.Line.Width = 2;
                LineItem curve16 = pane3.AddCurve(classes[4], rList[4], Color.Black, SymbolType.None);
                curve16.Line.Width = 2;

            }
            //LineItem curve10 = pane3.AddCurve("Space", rList[3], Color.Black, SymbolType.None);
            //curve10.Line.Width = 1;


            zedGraphControl1.AxisChange();
            zedGraphControl1.Invalidate();
            zedGraphControl2.AxisChange();
            zedGraphControl2.Invalidate();
            zedGraphControl3.AxisChange();
            zedGraphControl3.Invalidate();
            #endregion

        }

        private void rules_Click(object sender, EventArgs e)
        {
            for (int l = 0; l < 3; l++)
            {
                dataGridView2.Rows.Clear();
                dataGridView2.Columns.Clear();
                for (int i = 0; i < size; i++)
                {
                    string[] args = new string[nIn];
                    double weight = 1;

                    for (int j = 0; j < nIn; j++)
                    {
                        args[j] = categoryXL[i, j];
                        weight *= m[i, j];
                    }
                    weight *= mY[i];

                    if (!rules.Contains(args))
                        rules.addRule(args, categoryYL[i], weight);
                    else
                    {
                        if (weight > rules.getWeight(args))
                            rules.Replace(args, categoryYL[i], weight);
                    }
                }
                string[] values = rules.getValues();
                string[][] keys = rules.getKeys();
                List<PointPairList> sepRList;
                PointPairList aggregated;
                Dictionary<string, PointPairList> collection = new Dictionary<string, PointPairList>();
                for (int i = 0; i < size; i++)
                {
                    double y = 0;
                    int first = 0, last = 0, ind = 0;
                    double m1 = 0, m2 = 0, max = double.MinValue;
                    double val = 0;
                    sepRList = Aggregate(i);
                    aggregated = AggregateSingle(sepRList);

                    for (int j = 0; j < aggregated.Count; j++)
                    {
                        m1 += aggregated[j].X * aggregated[j].Y;
                        m2 += aggregated[j].Y;
                    }
                    rT[i] = m1 / m2;
                    if (double.IsNaN(rT[i]))
                    {
                        rT[i] = 0;
                    }
                    for (int j = 0; j < sepRList.Count; j++)
                    {
                        for (int k = ind; k < sepRList[j].Count; k++)
                        {
                            if (rT[i] <= sepRList[j][k].X)
                            {
                                if (max < sepRList[j][k].Y)
                                {
                                    max = sepRList[j][k].Y;
                                    first = j;
                                }
                                ind = k;
                                break;
                            }
                        }
                    }
                    ind = 0;
                    categoryYT1[i] = classes[first];

                    categoryYT2[i] = rules.checkRule(new string[] { categoryXL[i, 0], categoryXL[i, 1] });


                    dataGridView1.Rows[i].Cells["Rt"].Value = categoryYT1[i];
                    dataGridView1.Rows[i].Cells["Rtr"].Value = categoryYT2[i];
                    dataGridView1.Rows[i].Cells["Rntr"].Value = Math.Round(TestY[0][i], 1).ToString();
                    dataGridView1.Rows[i].Cells["R"].Value = Math.Round(rT[i], 1).ToString();
                }cot++;
                CheckError();
            }
            cot = 0;
            msre = 0;
            showRules(rules);
        }

        private void CheckError()
        {
            double sum=0;
            int n = 0;
            for (int i=0; i< size; i++)
            {
                if (Math.Abs(Convert.ToDouble(dataGridView1.Rows[i].Cells["Rntr"].Value)
                    - Convert.ToDouble(dataGridView1.Rows[i].Cells["R"].Value)) <= 15)
                {
                    dataGridView1.Rows[i].Cells["Rt"].Value = dataGridView1.Rows[i].Cells["Rtr"].Value;
                }   
                dataGridView1.Rows[i].Cells["Rtr"].Style.BackColor = Color.Green;
                if (dataGridView1.Rows[i].Cells["Rt"].Value != dataGridView1.Rows[i].Cells["Rtr"].Value)
                {
                    dataGridView1.Rows[i].Cells["Rt"].Style.BackColor = Color.Red;
                    n++;
                    sum += Math.Pow(TestY[0][i] - rT[i], 2);
                }
                else
                {
                    dataGridView1.Rows[i].Cells["Rt"].Style.BackColor = Color.Green;
                }
            }
            NRlabel.Text = Math.Round(((double)n / (double)size * 100),1).ToString()+ "%";
            msre += Math.Sqrt(sum / n);
            MSRELabel.Text = Math.Round(msre/cot, 4).ToString();
        }

        private void showRules(FuzzyRules rls)
        {
            string[] values = rls.getValues();
            string[][] args = rls.getKeys();
            for (int i=nDiv; i<nDiv*nIn; i++)
            {
                dataGridView2.Columns.Add(keys[i], keys[i]);
            }
            dataGridView2.Rows.Add(nDiv);
            dataGridView2.RowHeadersWidth = 110;
            dataGridView2.Height = dataGridView2.Rows[0].Height * (dataGridView2.Rows.Count+2);

            for (int i=0; i<nDiv; i++)
            {
                dataGridView2.Rows[i].HeaderCell.Value = keys[i];
            }

            for (int i=0; i<values.Length; i++)
            {
                int index = 0;
                int ct = 0;
                for (int j=0; j< dataGridView2.Rows.Count-1; j++)
                {
                    if (dataGridView2.Rows[j].HeaderCell.Value.ToString() == args[i][0])
                    {
                        index = j;
                        break;
                    }
                }
                dataGridView2.Rows[index].Cells[args[i][1]].Value = values[i].ToString();
            }

        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            My_wang_Mendel(e.RowIndex);
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            My_wang_Mendel(dataGridView1.CurrentRow.Index);
        }

        private List<PointPairList> Aggregate(int index)
        {
            List<PointPairList> rCopy = new List<PointPairList>();
            for (int i = 0; i < rList.Count; i++)
            {
                rCopy.Add(new PointPairList());
                foreach (PointPair pp in rList[i])
                {
                    rCopy[i].Add(new PointPair(pp.X, pp.Y));
                }
            }
            //pred = new Predicate<PointPair>(check);


            for (int i = 0; i < rCopy.Count; i++)
            {
                //rCopy[i].RemoveAll(pred(removePoint));
                bool flag = true;
                while (flag)
                {
                    flag = false;
                    for (int j = 0; j < rCopy[i].Count; j++)
                    {
                        if (rCopy[i][j].Y > mmY[index, i])
                        {
                            //rCopy[i].RemoveAt(j);
                            rCopy[i][j].Y = mmY[index, i];
                            flag = true;
                            break;
                        }
                    }
                }
            }
            
            return rCopy;
        }

        private PointPairList AggregateSingle (List<PointPairList> rCopy)
        {
            PointPairList aggrList = new PointPairList();
            for (int i = 0; i < rList.Count; i++)
            {
                for (int j = 0; j < rCopy[i].Count; j++)
                {
                    if (i != rList.Count - 1)
                    {
                        if (rCopy[i][j].Y >= rCopy[i + 1][j].Y)
                        {
                            aggrList.Add(rCopy[i][j]);
                        }
                        else i++;
                    }
                    else aggrList.Add(rCopy[i][j]);
                }
            }
            return aggrList;

        }

        public void wang_Mendel(DataGridViewCellEventArgs e)
        {
            //zedGraphControl4.GraphPane.CurveList.Clear();
            //zedGraphControl4.Invalidate();
            //pane4 = zedGraphControl4.GraphPane;
            //pane4.XAxis.Scale.Min = minGen;
            //pane4.XAxis.Scale.Max = maxGen;
            //int index = e.RowIndex;
            //List<PointPairList> rCopy = new List<PointPairList>();
            //PointPairList aggrList = new PointPairList();
            //for (int i = 0; i < rList.Count; i++)
            //{
            //    rCopy.Add(new PointPairList());
            //    foreach (PointPair pp in rList[i])
            //    {
            //        rCopy[i].Add(new PointPair(pp.X, pp.Y));
            //    }
            //}
            //pred = new Predicate<PointPair>(check);


            //for (int i = 0; i < rCopy.Count; i++)
            //{
            //    //rCopy[i].RemoveAll(pred(removePoint));
            //    bool flag = true;
            //    while (flag)
            //    {
            //        flag = false;
            //        for (int j = 0; j < rCopy[i].Count; j++)
            //        {
            //            if (rCopy[i][j].Y > mm[index, i])
            //            {
            //                //rCopy[i].RemoveAt(j);
            //                rCopy[i][j].Y = mm[index, i];
            //                flag = true;
            //                break;
            //            }
            //        }
            //    }
            //}
            //for (int i = 0; i < rList.Count; i++)
            //{
            //    for (int j = 0; j < rCopy[i].Count; j++)
            //    {
            //        if (i != rList.Count - 1)
            //        {
            //            if (rCopy[i][j].Y >= rCopy[i + 1][j].Y)
            //            {
            //                aggrList.Add(rCopy[i][j]);
            //            }
            //            else i++;
            //        }
            //        else aggrList.Add(rCopy[i][j]);
            //    }
            //}

            ////rCopy[0].Intersect<PointPairList>(rCopy[1]);
            //LineItem curve11 = pane4.AddCurve("Aggregate", aggrList, Color.Purple, SymbolType.None);
            //curve11.Line.Width = 2;

            //LineItem curve7 = pane4.AddCurve("Very deep", rCopy[0], Color.Blue, SymbolType.None);
            //curve7.Line.Width = 1;
            //LineItem curve8 = pane4.AddCurve("Deep", rCopy[1], Color.Green, SymbolType.None);
            //curve8.Line.Width = 1;
            //LineItem curve9 = pane4.AddCurve("Surphace", rCopy[2], Color.Red, SymbolType.None);
            //curve9.Line.Width = 1;
            ////LineItem curve10 = pane4.AddCurve("Space", rCopy[3], Color.Black, SymbolType.Star);
            ////curve10.Line.Width = 1;

            //zedGraphControl4.AxisChange();
            //zedGraphControl4.Invalidate();
        }

        public void My_wang_Mendel(int e)
        {
            zedGraphControl4.GraphPane.CurveList.Clear();
            zedGraphControl4.Invalidate();
            pane4 = zedGraphControl4.GraphPane;
            pane4.XAxis.Scale.Min = minGen;
            pane4.XAxis.Scale.Max = maxGen;
            int index = e;

            PointPairList aggrList = new PointPairList();
            List<PointPairList> rCopy = Aggregate(index);
            aggrList = AggregateSingle(rCopy);

            
            
            LineItem curve11 = pane4.AddCurve("Aggregate", aggrList, Color.Purple, SymbolType.None);
            curve11.Line.Width = 2;
            if (nDiv == 3)
            {
                LineItem curve7 = pane4.AddCurve(classes[0], rCopy[0], Color.Blue, SymbolType.None);
                curve7.Line.Width = 1;
                LineItem curve8 = pane4.AddCurve(classes[1], rCopy[1], Color.Green, SymbolType.None);
                curve8.Line.Width = 1;
                LineItem curve9 = pane4.AddCurve(classes[2], rCopy[2], Color.Red, SymbolType.None);
                curve9.Line.Width = 1;
            }
            else
            {
                LineItem curve7 = pane4.AddCurve(classes[0], rCopy[0], Color.Blue, SymbolType.None);
                curve7.Line.Width = 1;
                LineItem curve8 = pane4.AddCurve(classes[1], rCopy[1], Color.Green, SymbolType.None);
                curve8.Line.Width = 1;
                LineItem curve9 = pane4.AddCurve(classes[2], rCopy[2], Color.Red, SymbolType.None);
                curve9.Line.Width = 1;
                LineItem curve10 = pane4.AddCurve(classes[3], rCopy[3], Color.Indigo, SymbolType.None);
                curve10.Line.Width = 1;
                LineItem curve20 = pane4.AddCurve(classes[4], rCopy[4], Color.Black, SymbolType.None);
                curve20.Line.Width = 1;
            }

            zedGraphControl4.AxisChange();
            zedGraphControl4.Invalidate();
        }

        private void generateButton_Click(object sender, EventArgs e)
        {
            Random rn = new Random();
            rulesButton.Enabled = true;
            dataGridView1.Columns.Clear();
            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Add("X", "Pressure");
            dataGridView1.Columns.Add("Y", "Temperature");
            dataGridView1.Columns.Add("Rt", "DecisionTest");
            dataGridView1.Columns.Add("Rtr", "DecisionTrue");
            dataGridView1.Columns.Add("R", "NumericalTest");
            dataGridView1.Columns.Add("Rntr", "NumericalTrue");


            string[] decisions = new string[LearningX.Count];
            mm = new List<double[,]>();
            mmY = new double[size,rFuzzy.Set.Count];//wartosci regul dla agregacji
            double[] totalM = new double[size];
            string[] decision= new string[size];

            List<double[,]> ms = new List<double[,]>();
            List<double[,]> msY = new List<double[,]>();
            for (int i = 0; i < LearningX.Count; i++)
            {
                ms.Add(new double[rFuzzy.Set.Count, LearningX.Count]);
                mm.Add(new double[size, rFuzzy.Set.Count]);
            }
            msY.Add(new double[rFuzzy.Set.Count, 1]);

            for (int i = 0; i < size; i++)
            {

                for (int z =0; z< nIn; z++)
                {
                    genData[z][i] = rn.Next(minGen, maxGen);
                    TestX[z][i] = genData[z][i];// rn.Next(minGen, maxGen);
                }
                LearningY[0][i] = rn.Next(minGen, maxGen);
                TestY[0][i] = LearningY[0][i];// rn.Next(minGen, maxGen);

                dataGridView1.Rows.Add(new string[]{ TestX[0][i].ToString(), TestX[1][i].ToString() });
                double max = Double.MinValue;
                int mi = 0;
                for (int j = 0; j < genData.Count; j++)
                {
                    max = double.MinValue;
                    for (int k = 0; k < LearningX[j].Length; k++)
                    {
                        if (genData[j][i] == LearningX[j][k])
                        {
                            for (int h = 0; h < nDiv; h++)
                            {
                                ms[j][h, j] = membershipIn[j][k, h];
                                if (max < ms[j][h, j])
                                {
                                    mi = h+j*nDiv;
                                    max = ms[j][h, j];
                                }
                            }
                            categoryXL[i, j] = keys[mi];
                            m[i,j] = max;
                            break;
                        }
                    }
                }
                max = double.MinValue;
                for (int k = 0; k < rL.Length; k++)
                {
                    if (LearningY[0][i] == rL[k])
                    {
                        for (int h = 0; h < nDiv; h++)
                        {
                            msY[0][h, 0] = membershipOut[0][k, h];
                            if (max < msY[0][h, 0])
                            {
                                max = msY[0][h, 0];
                                mi = h;
                            }
                        }
                        categoryYL[i] = classes[mi];
                        mY[i] = max;
                        break;
                    }
                }
                
                for (int j=0; j<rFuzzy.Set.Count; j++)
                {
                    for (int k=0; k<genData.Count; k++)
                    {
                        mm[k][i, j] = ms[k][j, k];
                    }
                    mmY[i, j] = msY[0][j, 0];
                }

            }
        }
    }
}
