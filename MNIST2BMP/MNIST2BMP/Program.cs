using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;

namespace MNIST2BMP
{
    class Program
    {
        static void Main(string[] args)
        {
            byte[] pixel = new byte[28 * 28];
            Bitmap bitmap = new Bitmap(28, 28);
            // フォルダ作成
            System.IO.Directory.CreateDirectory(@"bmp_data");
            // ファイルを読み込む
            using( FileStream fsPixel =
                new FileStream(@"train-images-idx3-ubyte\train-images.idx3-ubyte", FileMode.Open, FileAccess.Read))
            {
                using (BinaryReader brPixel = new BinaryReader(fsPixel))
                {
                    brPixel.ReadInt32(); // ファイルヘッダを読み飛ばす
                    brPixel.ReadInt32();
                    brPixel.ReadInt32();
                    brPixel.ReadInt32();
                    // ピクセルデータを読み込んでBMP形式に変換
                    for ( int i = 0; i < 60000; i++ )
                    {
                        for( int j = 0; j < 28 * 28; j++)
                        {
                            pixel[j] = brPixel.ReadByte();
                        }
                        for( int y = 0; y < 28; y++)
                        {
                            for( int x = 0; x < 28; x++)
                            {
                                bitmap.SetPixel(x, y, Color.FromArgb(
                                    (int)pixel[y * 28 + x],
                                    (int)pixel[y * 28 + x],
                                    (int)pixel[y * 28 + x]));
                            }
                        }
                        bitmap.Save(@"bmp_data\image" + i + ".bmp", ImageFormat.Bmp);
                        System.Console.WriteLine(@"bmp_data\image" + i + ".bmp");
                    }
                }
            }
         }
    }
}
