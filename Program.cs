using System;
using System.Runtime.InteropServices;
using ROBOTS;
using IKEMAMPUAN;


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
            // ini bisa dilakukan secara dinamis dengan menggunakan list

            // inisisasi kemampuan
            List<IKemampuan> listKemampuan = new List<IKemampuan>();
            listKemampuan.Add(new Repair(100));
            listKemampuan.Add(new ElectricShock(30));
            listKemampuan.Add(new PlasmaCannon(50));
            listKemampuan.Add(new SuperShield(40, 2));

            // game
            Console.WriteLine("PERTARUNGAN DIMULAI!");

            // menghitung turn ke berapa 
            int round = 1;
            // pertarungan berlangsung hingga boss mati
            while (morris.isAlive == true)
            {
                Console.WriteLine($"Round : {round}");

                lewis.serang(morris);
                if (morris.energi <= 0) { morris.mati(); break; }

                penny.serang(morris);
                if (morris.energi <= 0) { morris.mati(); break; }

                gus.serang(morris);
                if (morris.energi <= 0) { morris.mati(); break; }

                
                morris.gunakanKemampuanRandom(listKemampuan);
                lewis.gunakanKemampuanRandom(listKemampuan);
                penny.gunakanKemampuanRandom(listKemampuan);
                gus.gunakanKemampuanRandom(listKemampuan);

                // mekanisme kurangi cooldown
                foreach (var allRobot in new List<Robot> { morris, lewis, penny, gus})
                {
                    allRobot.kurangiCooldown(listKemampuan);
                    allRobot.updateAbilityAktif();
                }

                // mekanisme regain energi
                morris.energi += 10;
                lewis.energi += 10;
                penny.energi += 10;
                gus.energi += 10;


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