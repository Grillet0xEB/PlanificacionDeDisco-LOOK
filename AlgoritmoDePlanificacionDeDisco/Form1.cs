using System;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using ZedGraph;
using System.Collections;

namespace AlgoritmoDePlanificacionDeDisco
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            iniciar();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            label2.Text = "Tira: |";
            Graficar();
            zedGraphControl1.Refresh();
        }

        private void iniciar()
        {
            GraphPane Grafico = zedGraphControl1.GraphPane;
            Grafico.Title = "Algoritmo LOOK de Planificacion de Disco";
            Grafico.FontSpec.FontColor = Color.Black;
            Grafico.FontSpec.IsBold = true;
            Grafico.Legend.IsVisible = false;
            //Configuracion del panel
            Grafico.PaneGap = 5;
            Grafico.PaneFill = new Fill(Color.White, Color.LightBlue, 90f);
            Grafico.PaneBorder = new Border(true, Color.RoyalBlue, 1.2f);
            //Configuracion de los ejes
            Grafico.AxisFill = new Fill(Color.White);
            Grafico.AxisBorder = new Border(true, Color.Black, 1f);
            //Configuracion del eje X
            Grafico.XAxis.Title = "Pasos";
            Grafico.XAxis.TitleFontSpec.FontColor = Color.Black;
            Grafico.XAxis.TitleFontSpec.IsBold = false;
            Grafico.XAxis.IsShowGrid = false;
            Grafico.XAxis.IsShowMinorGrid = false;
            Grafico.XAxis.MinorStep = 0.2f;
            Grafico.XAxis.Min = 0;
            Grafico.XAxis.Max = 16;
            Grafico.XAxis.Step = 1;
            //Configuracion del eje Y
            Grafico.YAxis.Title = "Posicion del cabezal";
            Grafico.XAxis.TitleFontSpec.FontColor = Color.Black;
            Grafico.YAxis.TitleFontSpec.IsBold = false;
            Grafico.YAxis.IsShowGrid = false;
            Grafico.YAxis.IsShowMinorGrid = false;
            Grafico.YAxis.MinorStep = 2f;
            Grafico.YAxis.Min = 0;
            Grafico.YAxis.Max = 99;
            
            zedGraphControl1.IsShowPointValues = false;
            zedGraphControl1.Size = new Size(660, 360);
            zedGraphControl1.AxisChange();
        }

        private void Graficar()
        {
            GraphPane Grafico = zedGraphControl1.GraphPane;
            Grafico.GraphItemList.Clear();
            Grafico.CurveList.Clear();
            Grafico.Legend.IsVisible = checkBox1.Checked ? true : false;

            PointPairList curvaOriginal = new PointPairList();
            PointPairList curvaOrdenada = new PointPairList();
            Llenar(curvaOriginal, curvaOrdenada); //Funcion que ordena la lista
            
            

            if (checkBox1.Checked)
            {
                LineItem Movimientos = Grafico.AddCurve("FCFS",
                                                    curvaOriginal,
                                                    Color.Red,
                                                    SymbolType.Circle);
                Movimientos.Line.Width = 1.5f;
                Movimientos.Line.IsSmooth = true;
                Movimientos.Line.SmoothTension = 0.01f;
                Movimientos.Symbol.Border.Color = Color.Black;

                LineItem Movimientos2 = Grafico.AddCurve("LOOK",
                                                    curvaOrdenada,
                                                    Color.LightBlue,
                                                    SymbolType.Circle);
                Movimientos2.Line.Width = 1.5f;
                Movimientos2.Line.IsSmooth = true;
                Movimientos2.Line.SmoothTension = 0.01f;
                Movimientos2.Symbol.Border.Color = Color.Black;
            }
            else
            {
                LineItem Movimientos = Grafico.AddCurve("LOOK",
                                                 curvaOrdenada,
                                                 Color.LightBlue,
                                                 SymbolType.Circle);
                Movimientos.Line.Width = 1.5f;
                Movimientos.Line.IsSmooth = true;
                Movimientos.Line.SmoothTension = 0.01f;
                Movimientos.Symbol.Border.Color = Color.Black;
            }

        }

        public PointPairList Llenar(PointPairList FCFS, PointPairList LOOK)
        {
            double[] puntosY = new double[17];
            double[] puntosX = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17 };
            double[] curvaDesendente = new double[17];
            double[] curvaAscendente = new double[17];
            puntosY[0] = (int)numericUpDown1.Value;
            curvaDesendente[0] = (int)numericUpDown1.Value;
            FCFS.Add(0, puntosY[0]);
            int puntoMayor = 0;
            int puntoMenor = 99;
            int movimientos = 0;
            Random rnd = new Random();
            
            for (int i = 1; i <= 16; i++)
            {
                int aleatorio = rnd.Next(0, 100);
                label2.Text += String.Format(" {0} |", aleatorio);
                FCFS.Add(i, aleatorio);

                if (aleatorio <= puntosY[0])
                {
                    curvaDesendente[i] = aleatorio;
                }
                else
                {
                    curvaAscendente[i] = aleatorio;
                }
            }
            
            Array.Sort(curvaDesendente);
            Array.Reverse(curvaDesendente);
            Array.Sort(curvaAscendente);
            for (int i = 1; i <= 16; i++)
            {
                puntosY[i] = curvaDesendente[i] + curvaAscendente[i];
            }
            for (int i = 0; i < puntosY.Length; i++)
            {
                if (puntosY[i] > puntoMayor)
                {
                    puntoMayor = (int)puntosY[i];
                }

                if (puntosY[i] < puntoMenor)
                {
                    puntoMenor = (int)puntosY[i];
                }
            }
            LOOK.Add(puntosX, puntosY);
            movimientos = ((int)puntosY[0] - puntoMenor) + (puntoMayor - puntoMenor);
            label3.Text = String.Format("Movimientos = ({0} - {1}) + ({2} - {1}) = {3}", puntosY[0], puntoMenor, puntoMayor, movimientos);
            
            return LOOK;
        }
    }
}