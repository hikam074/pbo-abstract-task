using System;
using System.Runtime.InteropServices;
using TUGAS2.Classes;

namespace MAIN
{
    class Program
    {
        static void Main(string[] args)
        {
            // inisisasi robot
            // nama, energi, armor, serangan
            BosRobot morris = new BosRobot("Morris", 1000, 80, 100, 20);
            NormalRobot lewis = new NormalRobot("Lewis", 300, 100, 120);
            NormalRobot penny = new NormalRobot("Penny", 200, 30, 200);
            NormalRobot gus = new NormalRobot("Gus", 450, 70, 110);


            // game
            Console.WriteLine("PERTARUNGAN DIMULAI!");

            int round = 1;
            while (morris.isAlive == true)
            {
                Console.WriteLine($"Round : {round}");

                lewis.serang(morris);
                if (morris.energi <= 0) { morris.mati(); break; }

                penny.serang(morris);
                if (morris.energi <= 0) { morris.mati(); break; }

                gus.serang(morris);
                if (morris.energi <= 0) { morris.mati(); break; }

                morris.cetakInformasi();

                round++;
                Console.WriteLine();
            }



            Console.WriteLine("GAME COMPLETED!");
            Console.ReadLine();
            
            // maaf kak kode yang saya tulis tidak sempurna karena filenya tidak bisa diselamatkan di storage yang lama pada H-5 jam pengumpulan :(
        }
    }
}