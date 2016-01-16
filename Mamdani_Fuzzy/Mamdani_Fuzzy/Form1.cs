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
        int size = 300, nIn=2, nDiv=3, minGen=-100,maxGen=100;
        double[] rL,mY;
        string[] categoryYL, categoryYT;
        List<double[,]> membershipIn, membershipOut,mm;
        double[,] mmY,m;
        string[,] categoryXL;
        List<double[]>  genData,LearningX,LearningY,TestX,TestY;
        List<string[]> decisions;
        Membership func;
        FuzzySet fuzzy, rFuzzy;
        List<DeFuzzySet> inputDef;

        private void button1_Click(object sender, EventArgs e)
        {
            for (int i=0; i< size; i++)
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
                    rules.addRule(args, categoryYL[i],weight);
                else
                {
                    if (weight > rules.getWeight(args))
                        rules.Replace(args, categoryYL[i], weight);
                }
            }
            string[] values = rules.getValues();
            string[][] keys = rules.getKeys();
            for (int i=0; i<size; i++)
            {
                double y = 0;
                double m1 = 0, m2 = 0;
                for (int j=0; j<values.Length; j++)
                {
                    //m2+=              //by keys we get the intervals from fuzzyset.Set and calculate trimf. Trimf for x1,x2, y For EACH rule in rule set
                }
                //dataGridView1.Rows[i].Cells[2].Value=rules.checkRule()
            }
        }

        FuzzyRules rules;
        List<PointPairList> list, rList;


        List<int[]> data;
        string[] keys, classes;
        string[] output = new string[] { "Very deep", "Deep", "Norm", "High" };

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            My_wang_Mendel(e);
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

        public void My_wang_Mendel(DataGridViewCellEventArgs e)
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

        bool check (PointPair pp)
        {
            return true;
        }

        public Form1()
        {

            membershipIn = new List<double[,]>();
            membershipOut = new List<double[,]>();

            m = new double[size, nIn];
            mY = new double[size];

            categoryXL = new string[size, nIn];
            categoryYL = new string[size];
            categoryYT = new string[size];

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


            keys = new string[] { "Low", "Medium", "High Pressure", "Cold","Warm", "Hot"};
            classes = new string[] { "Space", "Norm", "Deep" };


            int d = (Math.Abs(minGen) + Math.Abs(maxGen))/4;

            data.Add(new int[] { minGen, minGen, minGen + 2 * d });
            data.Add(new int[] { minGen + d, minGen + 2 * d, minGen + 3 * d });
            data.Add(new int[] { minGen + 2 * d, maxGen, maxGen });

            data.Add(new int[] { minGen, minGen, minGen + 2 * d });
            data.Add(new int[] { minGen + d, minGen + 2 * d, minGen + 3 * d });
            data.Add(new int[] { minGen + 2 * d, maxGen, maxGen });

            data.Add(new int[] { minGen, minGen + d, minGen + 2 * d });
            data.Add(new int[] { minGen + d, minGen + 2 * d, minGen + 3 * d });
            data.Add(new int[] { minGen + 2 * d, minGen + 3 * d, maxGen });

            //rules.addRule(new string[] {"Low", "Cold" }, classes[2]);
            //rules.addRule(new string[] { "Low", "Warm" }, classes[2]);
            //rules.addRule(new string[] { "Low", "Hot" }, classes[2]);
            //rules.addRule(new string[] { "Medium", "Cold" }, classes[1]);
            //rules.addRule(new string[] { "Medium", "Warm" }, classes[1]);
            //rules.addRule(new string[] { "Medium", "Hot" }, classes[1]);
            //rules.addRule(new string[] { "High Pressure", "Cold" }, classes[1]);
            //rules.addRule(new string[] { "High Pressure", "Warm" }, classes[1]);
            //rules.addRule(new string[] { "High Pressure", "Hot" }, classes[0]);

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
            for (int i=0; i< nIn; i++)
            {
                LearningX.Add(new double[size+1]);
                TestX.Add(new double[size + 1]);

                genData.Add(new double[size]);
            }
            LearningY.Add(new double[size]);
            TestY.Add(new double[size]);

            rL = new double[size+1];
            
            InitializeComponent();
            initParams();
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
                LearningX[k][0] = fuzzy.Set[keys[n]][0];
                for (int i = 0; i < membershipIn[k].Length / 3; i++)
                {
                    for (int j = n; j < n + nDiv; j++)
                    {
                        membershipIn[k][i, ct] = func.trimf(LearningX[k][i], fuzzy.Set[keys[j]][0], fuzzy.Set[keys[j]][1], fuzzy.Set[keys[j]][2]);
                        list[j].Add(LearningX[k][i], membershipIn[k][i, ct]);
                        ct++;
                    }
                    if (i+1!= membershipIn[k].Length / 3)
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
            for (int i=rFuzzy.Set[classes[0]][0]; i<= rFuzzy.Set[classes[nR-1]][2]; i++)
            {
                rL[ct] = i;
                for (int j = 0; j< nR; j++)
                {
                    membershipR[ct, j] = func.trimf(rL[ct], rFuzzy.Set[classes[j]][0], rFuzzy.Set[classes[j]][1], rFuzzy.Set[classes[j]][2]);
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

            LineItem curve7 = pane3.AddCurve("Deep", rList[0], Color.Blue, SymbolType.None);
            curve7.Line.Width = 1;
            LineItem curve8 = pane3.AddCurve("Surphace", rList[1], Color.Green, SymbolType.None);
            curve8.Line.Width = 1;
            LineItem curve9 = pane3.AddCurve("Space", rList[2], Color.Red, SymbolType.None);
            curve9.Line.Width = 1;
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

        private void Aggregate(string decision, double aggregate)
        {

        }


        private void generateButton_Click(object sender, EventArgs e)
        {
            Random rn = new Random();
            dataGridView1.Columns.Add("X", "Pressure");
            dataGridView1.Columns.Add("Y", "Temperature");
            dataGridView1.Columns.Add("R", "Decision");
            
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
                    TestX[z][i] = rn.Next(minGen, maxGen);
                }
                LearningY[0][i] = rn.Next(minGen, maxGen);
                TestY[0][i] = rn.Next(minGen, maxGen);

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
                                    mi = h;
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
