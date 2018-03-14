using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;

namespace POD_lab8
{
    class Program
    {
        static void LSB(string wiadomosc, Bitmap obraz, int ilosc_bitow, int wys, int szer)
        {
            byte[] wiadomosc_bajty = Encoding.ASCII.GetBytes(wiadomosc);
            int wartosc_piksela_czerwonego, wartosc_piksela_zielonego, wartosc_piksela_niebieskiego, x=szer, y=wys;
            String wiadomosc_bitowo = "";
            Bitmap wynik = new Bitmap(obraz.Width, obraz.Height);
            for (int i = 0; i < wiadomosc_bajty.Length; i++)
                wiadomosc_bitowo=wiadomosc_bitowo+Convert.ToString(wiadomosc_bajty[i], 2).PadLeft(8, '0');
            for (int z = 0; z < wiadomosc_bitowo.Length; z += 3*ilosc_bitow)
            {
                Color pixelColor = obraz.GetPixel(x, y);
                if (z + ilosc_bitow >= wiadomosc_bitowo.Length)
                {
                    wartosc_piksela_czerwonego = (pixelColor.R - pixelColor.R % (int)Math.Pow(2, ilosc_bitow)) + Convert.ToInt32(wiadomosc_bitowo.Substring(z, wiadomosc_bitowo.Length - z), 2);
                    obraz.SetPixel(x, y, Color.FromArgb(wartosc_piksela_czerwonego, pixelColor.G, pixelColor.B));
                    break;
                }
                else
                {
                    wartosc_piksela_czerwonego = (pixelColor.R - pixelColor.R % (int)Math.Pow(2, ilosc_bitow)) + Convert.ToInt32(wiadomosc_bitowo.Substring(z, ilosc_bitow), 2);
                    if (z + 2 * ilosc_bitow >= wiadomosc_bitowo.Length)
                    {
                        wartosc_piksela_zielonego = (pixelColor.G - pixelColor.G % (int)Math.Pow(2, ilosc_bitow)) + Convert.ToInt32(wiadomosc_bitowo.Substring(z + ilosc_bitow, (wiadomosc_bitowo.Length - z - ilosc_bitow)), 2);
                        obraz.SetPixel(x, y, Color.FromArgb(wartosc_piksela_czerwonego, wartosc_piksela_zielonego, pixelColor.B));
                        break;
                    }
                    else
                    {
                        wartosc_piksela_zielonego = (pixelColor.G - pixelColor.G % (int)Math.Pow(2, ilosc_bitow)) + Convert.ToInt32(wiadomosc_bitowo.Substring(z + ilosc_bitow, ilosc_bitow), 2);
                        if (z + 3 * ilosc_bitow >= wiadomosc_bitowo.Length)
                        {
                            wartosc_piksela_niebieskiego = (pixelColor.B - pixelColor.B % (int)Math.Pow(2, ilosc_bitow)) + Convert.ToInt32(wiadomosc_bitowo.Substring(z + 2 * ilosc_bitow, wiadomosc_bitowo.Length - z - 2 * ilosc_bitow), 2);
                            obraz.SetPixel(x, y, Color.FromArgb(wartosc_piksela_czerwonego, wartosc_piksela_zielonego, wartosc_piksela_niebieskiego));
                            break;
                        }
                        else
                        {
                            wartosc_piksela_niebieskiego = (pixelColor.B - pixelColor.B % (int)Math.Pow(2, ilosc_bitow)) + Convert.ToInt32(wiadomosc_bitowo.Substring(z + 2 * ilosc_bitow, ilosc_bitow), 2);
                        }
                    }
                }
                obraz.SetPixel(x, y, Color.FromArgb(wartosc_piksela_czerwonego,wartosc_piksela_zielonego,wartosc_piksela_niebieskiego));
                x++;
                if (x == wynik.Width)
                {
                    x = 0;
                    y++;
                }
            }
            obraz.Save("rezultat.jpg");
        }
        static string odszyfrujLSB(Bitmap obraz, int ilosc_bitow, int x, int y, int dlugosc)
        {
            string wiadomosc = "";
            for(int i=x;i<obraz.Height;i++)
                for(int j=y;j<obraz.Width;j++)
                {
                    Color pixelColor = obraz.GetPixel(j, i);
                    if (wiadomosc.Length < dlugosc * 8)
                        wiadomosc = wiadomosc + Convert.ToString(pixelColor.R % (int)Math.Pow(2, ilosc_bitow), 2).PadLeft(ilosc_bitow, '0');
                    else
                    {
                        break;
                    }
                    if (wiadomosc.Length < dlugosc * 8)
                        wiadomosc = wiadomosc + Convert.ToString(pixelColor.G % (int)Math.Pow(2, ilosc_bitow), 2).PadLeft(ilosc_bitow, '0');
                    else
                        break;
                    if (wiadomosc.Length < dlugosc * 8)
                        wiadomosc = wiadomosc + Convert.ToString(pixelColor.B % (int)Math.Pow(2, ilosc_bitow), 2).PadLeft(ilosc_bitow, '0');
                    else
                        break;
                }
            string wiadomosc_odszyfrowana = "";
            for(int i=0;i<wiadomosc.Length; i += 8)
            {
                if(i+8>wiadomosc.Length)
                {
                    
                    break;
                }
                else
                    wiadomosc_odszyfrowana = wiadomosc_odszyfrowana + (char)Convert.ToInt32(wiadomosc.Substring(i, 8),2);
            }
            return wiadomosc_odszyfrowana;
        }
        static void Main(string[] args)
        {
            Bitmap obraz = new Bitmap("obraz2.bmp");
            string wiadomosc= "abcdefghijkl";
            System.Random los = new Random(DateTime.Now.Millisecond);
            int x, y;
            do
            {
                x = los.Next(0, obraz.Height);
                y = los.Next(0, obraz.Width);
            } while ((wiadomosc.Length - (obraz.Height - x - 1) * obraz.Width + y) < 0);
            Console.WriteLine(x + " " + y);
            LSB(wiadomosc, obraz, 5,0,0);
            Bitmap rezultat = new Bitmap("rezultat.jpg");
            wiadomosc = odszyfrujLSB(rezultat, 5, 0, 0,wiadomosc.Length);
            Console.WriteLine(wiadomosc);
            Console.ReadKey();
        }
    }
}
