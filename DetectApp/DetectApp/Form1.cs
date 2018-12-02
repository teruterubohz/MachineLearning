using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace DetectApp
{
    public partial class Form1 : Form
    {
        NeuralNetwork nn;
        double[] pixel;
        string[] labelName;
        // コンストラクタ
        public Form1()
        {
            InitializeComponent();
            // 入力層に入力するデータを格納する配列
            pixel = new double[28 * 28];
            nn = new NeuralNetwork();
            // weight.datを読み込んでニューラルネットワークを構築
            nn.LoadWeight();
            // label.txt (正解の種類)を読み込む
            labelName = LoadLabelName();
            // 初期化
            GraphInit();
        }

        // マウスのイベントハンドラ:マウスが動いた時に呼び出される
        private void pictureBox1_MouseMove( object sender, MouseEventArgs e)
        {
            // 左ボタンが押されていたらマウスの軌跡を描画
            if(( Control.MouseButtons & MouseButtons.Left) == MouseButtons.Left)
            {
                // マウスの軌跡を描画
                Point pos = pictureBox1.PointToClient(Cursor.Position);
                GraphPlot(pos.X, pos.Y);
            }
        }

        // マウスのイベントハンドラ：マウスのボタンが離されたら呼び出される
        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            // ニューラルネットワークの計算(画像認識)を行う
            nn.CalcForward(pixel);
            // 結果を表示する
            setText1(labelName[nn.GetMaxOutput()]);
        }

        // ボタンのイベントハンドラ
        private void button1_Click(object sender, EventArgs e)
        {
            GraphInit();
            // Pixelをクリア
            for( int i = 0; i < pixel.Length; i++)
            {
                pixel[i] = 0.0;
            }
        }

        // テキストを表示する
        delegate void setTextDelegate(string s);
        void setText1(string s )
        {
            if (InvokeRequired)
                Invoke(new setTextDelegate(setText1), new object[] { s });
            else
                label1.Text = s;
        }

        // label.txtを読み込む
        private string[] LoadLabelName()
        {
            string[] names;
            using (StreamReader reader = new StreamReader(@"label.txt"))
            {
                string line = reader.ReadLine();    // ファイルから1行読み込む
                names = line.Split(',');
            }
            return names;
        }

        // ピクチャーボックスを初期化する関数
        private void GraphInit()
        {
            Graphics graphic = pictureBox1.CreateGraphics();
            graphic.Clear(Color.White);
            graphic.Dispose();
        }

        // マウスの座標に円を描画した後、マウスの座標を28x28の座標に変換する関数
        private void GraphPlot( int x , int y)
        {
            if ((x < 0) || x >= pictureBox1.Width) return;
            if ((y < 0) || y >= pictureBox1.Height) return;
            Graphics graphics = pictureBox1.CreateGraphics();
            graphics.FillEllipse(Brushes.Black, x, y, 25, 25);// 円を描画
            graphics.Dispose();
            int px = x * 28 / pictureBox1.Width;
            int py = y * 28 / pictureBox1.Height;
            pixel[py * 28 + px] = 1.0;
        }
    }
}
