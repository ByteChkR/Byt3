using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Xml.Serialization;

namespace BenchmarkResultViewer
{
    public partial class Form1 : Form
    {
        public class PerformanceResult
        {
            public bool Matched => (!LowerIsBetter || TargetAndVariance >= Result) &&
                                   (LowerIsBetter || TargetAndVariance <= Result);
            public decimal TargetAndVariance => LowerIsBetter ? Target + Variance : Target - Variance;
            public decimal DeltaFromTarget => Target - Result;
            public decimal Percentage => Math.Round(Result / Target * 100, 4);

            [XmlIgnore]
            public int Version;
            [XmlIgnore]
            public string PerformanceFolder;

            public bool LowerIsBetter;
            public string TestName;
            public int N;
            public decimal Result;
            public decimal Target;
            public decimal Variance;

            public override string ToString()
            {
                return
                    $"{TestName}: {Result}ms ({Percentage}%); Matched: {Matched}; Target: {Target}ms; Delta: {DeltaFromTarget}ms";
            }
        }

        private string folder;

        public Form1(string[] args)
        {
            InitializeComponent();
            if (args.Length != 0)
            {
                folder = args[0];
            }
            else
            {
                folder = null;
            }


        }

        private Chart CreateChart(string name)
        {
            Chart chart = new Chart();
            chart.Name = name;
            chart.Text = name;
            ChartArea area = new ChartArea();
            area.Name = "MainArea";
            chart.ChartAreas.Add(area);

            Legend legend = new Legend("Legend1");
            chart.Legends.Add(legend);

            return chart;
        }

        private Series CreateSeries(Chart chart, string name)
        {
            Series series = new Series
            {
                ChartArea = "MainArea",
                Legend = "Legend1",
                Name = name,
                ChartType = SeriesChartType.Line,
                BorderWidth = 3,
                Enabled = false
            };
            chart.Series.Add(series);
            return series;
        }

        private void VersionSelectionChanged(CheckedListBox list, Chart chart, ItemCheckEventArgs args)
        {
            string s = list.Items[args.Index].ToString();
            Series series = chart.Series.FindByName(s);
            series.Enabled = args.NewValue == CheckState.Checked;
        }

        private Dictionary<string, List<PerformanceResult>> GetTests(string performanceFolder, string[] versions)
        {
            List<int> vs = versions.Select(x => int.Parse(Path.GetFileNameWithoutExtension(x))).ToList();
            vs.Sort();

            Dictionary<string, List<PerformanceResult>> ret = new Dictionary<string, List<PerformanceResult>>();
            foreach (int version in vs)
            {
                PerformanceResult[] results = LoadResultsFromVersionFolder(performanceFolder, Path.Combine(performanceFolder, version.ToString()));

                foreach (PerformanceResult performanceResult in results)
                {
                    if (ret.ContainsKey(performanceResult.TestName))
                    {
                        ret[performanceResult.TestName].Add(performanceResult);
                    }
                    else
                    {
                        ret.Add(performanceResult.TestName, new List<PerformanceResult> { performanceResult });
                    }
                }
            }

            return ret;
        }

        private PerformanceResult[] LoadResultsFromVersionFolder(string performanceFolder, string path)
        {
            int v = int.Parse(Path.GetFileName(path));
            string[] files = Directory.GetFiles(path, "*.xml", SearchOption.TopDirectoryOnly);
            PerformanceResult[] ret = new PerformanceResult[files.Length];
            XmlSerializer xs = new XmlSerializer(typeof(PerformanceResult));
            for (int i = 0; i < files.Length; i++)
            {
                Stream s = File.OpenRead(files[i]);
                ret[i] = (PerformanceResult)xs.Deserialize(s);
                ret[i].Version = v;
                ret[i].PerformanceFolder = performanceFolder;
                s.Close();
            }

            return ret;
        }

        private void chart1_Click(object sender, EventArgs e)
        {

        }

        private List<Chart> charts = new List<Chart>();
        private void Form1_Load(object sender, EventArgs e)
        {
            tabControl1.SelectedIndexChanged += TabControl1OnSelectedIndexChanged;


            if (folder == null)
            {
                if (fbdSelectDir.ShowDialog() == DialogResult.OK)
                    folder = fbdSelectDir.SelectedPath;
                else
                {
                    Application.Exit();
                    return;
                }
            }

            Text = "Viewing: " + folder;
            charts = new List<Chart> { chart1 };
            string[] versions = Directory.GetDirectories(folder, "*", SearchOption.TopDirectoryOnly);

            Dictionary<string, List<PerformanceResult>> tests = GetTests(folder, versions);

            foreach (KeyValuePair<string, List<PerformanceResult>> test in tests)
            {
                TabPage page = new TabPage(test.Key);
                Chart chart = CreateChart(test.Key);
                charts.Add(chart);
                page.Controls.Add(chart);
                chart.Dock = DockStyle.Fill;

                allList.Items.Add(test.Key);

                Series series = CreateSeries(chart, test.Key);
                series.Enabled = true;
                Series seriesAllChart = CreateSeries(chart1, test.Key);

                Series target = CreateSeries(chart, "Target");
                target.Enabled = true;

                int i = 1;
                foreach (PerformanceResult performanceResult in test.Value)
                {
                    CustomLabel lbl = new CustomLabel(i - 0.5f, i + 0.5f, performanceResult.Version.ToString(), 2, LabelMarkStyle.None);

                    chart.ChartAreas[0].AxisX.CustomLabels.Add(lbl);
                    chart1.ChartAreas[0].AxisX.CustomLabels.Add(lbl);

                    target.Points.AddY(performanceResult.Target);
                    series.Points.AddY(performanceResult.Result);
                    seriesAllChart.Points.AddY(performanceResult.Result);
                    i++;
                }



                //chart.Series.Add(series);
                //chart1.Series.Add(seriesAllChart);

                //CheckedListBox list = new CheckedListBox();
                //page.Controls.Add(list);
                //list.Dock = DockStyle.Right;


                //list.ItemCheck += (sender, args) => VersionSelectionChanged(series, list, args);

                tabControl1.TabPages.Add(page);

            }

            allList.ItemCheck += (s, args) => VersionSelectionChanged(allList, chart1, args);

            allList.CheckOnClick = true;

            Enumerable.Range(0, allList.Items.Count).ToList().ForEach(x => allList.SetItemChecked(x, true));


            //Directory.CreateDirectory("images");
            //Size originalSize = Size;
            //foreach (Chart chart in charts)
            //{
            //    Size = new Size(1920, 1080);
            //    Bitmap bmp = new Bitmap(chart.Width, chart.Height);
            //    chart.DrawToBitmap(bmp, new Rectangle(0, 0, bmp.Width, bmp.Height));
            //    bmp.Save(Path.Combine("images", chart.Name + ".png"), ImageFormat.Png);
            //    bmp.Dispose();
            //    tabControl1.SelectedIndex++;
            //    Application.DoEvents();
            //}
            //tabControl1.SelectedIndex = 0;

            //Size = originalSize;
            Show();
        }

        private void TabControl1OnSelectedIndexChanged(object sender, EventArgs e)
        {
            btnScreen.Parent = tabControl1.SelectedTab;
        }

        private void btnScreen_Click(object sender, EventArgs e)
        {
            Chart chart = charts.First(x=>x.Text== tabControl1.SelectedTab.Text);

            if (sfdScreen.ShowDialog() == DialogResult.OK)
            {
                Directory.CreateDirectory("images");
                Size originalSize = Size;
                Size = new Size(1920, 1080);
                Bitmap bmp = new Bitmap(chart.Width, chart.Height);
                chart.DrawToBitmap(bmp, new Rectangle(0, 0, bmp.Width, bmp.Height));
                bmp.Save(sfdScreen.FileName, ImageFormat.Png);
                bmp.Dispose();
                tabControl1.SelectedIndex++;
                Application.DoEvents();

                tabControl1.SelectedIndex = 0;

                Size = originalSize;
            }
        }
    }
}
